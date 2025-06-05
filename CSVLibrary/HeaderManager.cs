using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{

    internal class HeaderManager
    {
        public static string header { get; set; }

        public static FileWriteInStrategy CheckWriteInType<T>(string filePath) where T : class, new()
        {

            // 取得第一筆資料
            List<T> oldDatas = CSVHelper.Read<T>(filePath);
            //當沒有資料就代表檔案是空白的
            if (oldDatas.Count == 0)
            {
                return FileWriteInStrategy.WriteHeaderThenNewData;
            }

            //取得原始Header長相
            string header = String.Join(",", typeof(T).GetProperties().Select(p => p.Name));
            //檢查第一筆資料是否是Header
            string firstData = CSVHelper.GetTasLine<T>(oldDatas[0]);
            // 比對資料
            if (header != firstData)
            {
                return FileWriteInStrategy.WriteHeaderThenOldAndNewData;
            }
            else
            {
                return FileWriteInStrategy.AppendNewData;
            }
        }

        public static void GetHeader<T>() where T : class, new()
        {
            PropertyInfo[] dataProperties = typeof(T).GetProperties();
            foreach (PropertyInfo property in dataProperties)
            {
                header += property.Name + ',';
            }
            header = header.TrimEnd(',');
        }

        public static void WriteHeaderThenNewData<T>(string filePath, T t) where T : class, new()
        {
            CSVHelper.CreateFile<T>(filePath, t);
        }

        public static void AppendNewData<T>(string filePath, T t)
        {
            StreamWriter sw = new StreamWriter(filePath, true);
            string data = CSVHelper.GetTasLine(t);
            sw.WriteLine(data);
            sw.Close();
        }

        public static void WriteHeaderThenOldAndNewData<T>(string filePath, T t)
        {
            //先取出舊資料
            StreamReader sr = new StreamReader(filePath);
            string oldData = sr.ReadToEnd(); // 每一次會讀取一行資料
            sr.Close();
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.WriteLine(header);
            sw.Write(oldData);
            string newData = CSVHelper.GetTasLine(t);
            sw.WriteLine(newData);
            sw.Close();
        }
    }
}

// 程序再理順一點
// 上傳github