using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


// ORM object relection mapping
namespace CSVLibrary
{
    internal class CSVHelper
    {
        //C:\Users\User\source\repos\CSVLibrary\data.exe
        public static List<T> Read<T>(string filePath) where T : class, new()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("路徑不存在，找不到指定的檔案。");
            }

            string fExtension = filePath.Split('.').Last();

            if (fExtension != "csv")
            {
                throw new ArgumentException("指定的檔案不是CSV檔。");
            }

            List<T> list = new List<T>();
            // 先拿到東西
            StreamReader streamReader = new StreamReader(filePath);
            while (!streamReader.EndOfStream)
            {
                T data = new T();
                string line = streamReader.ReadLine(); // 一筆資料
                PropertyInfo[] Tproperties = typeof(T).GetProperties(); //一筆資料的properties
                string[] propertyline = line.Split(',');
                for (int i = 0; i < propertyline.Length; i++)
                {
                    Tproperties[i].SetValue(data, propertyline[i]);
                }
                list.Add(data);
            }
            streamReader.Close();
            return list;
        }
        //"C:\Users\User\source\repos\CSVLibrary123\
        //data.csv"
        public static void Write<T>(string filePath, T t, bool append) where T : class, new()
        {
            HeaderManager.GetHeader<T>();
            string header = HeaderManager.header;
            //\r \n \t
            string[] fileNames = filePath.Split('\\');

            string currentFilePath = String.Join("\\", fileNames.Take(fileNames.Length - 1));

            // 找不到資料夾時，創建路徑相應檔名的資料夾後，寫入資料。
            if (!Directory.Exists(currentFilePath) || !File.Exists(filePath))
            {
                Directory.CreateDirectory(currentFilePath);
                CreateFile<T>(filePath, t);
                return;
            }

            string fExtension = filePath.Split('.').Last();
            if (fExtension != "csv")
            {
                throw new ArgumentException("只接受寫入副檔名為CSV的檔案。");
            }

            // 開始檢查檔案決定寫入方式
            FileWriteInStrategy writeType = HeaderManager.CheckWriteInType<T>(filePath);

            switch (writeType)
            {
                case FileWriteInStrategy.WriteHeaderThenNewData:
                    HeaderManager.WriteHeaderThenNewData<T>(filePath, t);
                    break;
                case FileWriteInStrategy.AppendNewData:
                    HeaderManager.AppendNewData<T>(filePath, t);
                    break;
                case FileWriteInStrategy.WriteHeaderThenOldAndNewData:
                    HeaderManager.WriteHeaderThenOldAndNewData<T>(filePath, t);
                    break;
            }


        }

        public static void CreateFile<T>(string filePath, T t) where T : class, new()
        {
            PropertyInfo[] Tproperties = typeof(T).GetProperties();

            string header = HeaderManager.header;

            StreamWriter SW = new StreamWriter(filePath, false);
            SW.WriteLine(header);

            string data = GetTasLine<T>(t);
            SW.WriteLine(data);
            SW.Flush();
            SW.Close();
        }

        public static string GetTasLine<T>(T t)
        {
            string data = "";
            PropertyInfo[] dataProperties = typeof(T).GetProperties();
            foreach (PropertyInfo property in dataProperties)
            {
                string propertyValue = property.GetValue(t) as string;
                data += propertyValue + ",";
            }
            data = data.TrimEnd(',');
            return data;
        }
    }
}
