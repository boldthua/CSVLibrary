using CSVLibrary.Strategies;
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
    public class CSVHelper
    {
        //C:\Users\User\source\repos\CSVLibrary\data.exe
        public static List<T> Read<T>(string filePath, int start, int count) where T : class, new()
        {             // 這裡的T是新創的類別，只拿你要的資料
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            string fExtension = filePath.Split('.').Last();

            if (fExtension != "csv")
            {
                throw new ArgumentException("指定的檔案不是CSV檔。");
            }
            int currentLine = 0;

            // 先拿到東西
            StreamReader streamReader = new StreamReader(filePath);
            string dataLine = streamReader.ReadLine();

            //  先檢查RawData的第一行是否是header
            if (String.IsNullOrEmpty(dataLine))
                return new List<T>();
            // 取得Raw Header
            Dictionary<string, int> headers = HeaderManager.GetHeaders(filePath);
            string header = string.Join(",", typeof(T).GetProperties().Select(x => x.Name));
            int countRemain = count;
            List<T> list = new List<T>();
            while (!streamReader.EndOfStream)
            {
                if (dataLine == header)
                    dataLine = streamReader.ReadLine();//想一下 
                if (currentLine < start)
                {
                    currentLine++;
                    continue;
                }


                T data = new T();
                PropertyInfo[] properties = data.GetType().GetProperties();
                string[] lineDatas = dataLine.Split(',');


                foreach (PropertyInfo property in properties)
                {
                    if (headers.ContainsKey(property.Name))
                    {
                        property.SetValue(data, lineDatas[headers[property.Name]]);
                    }
                }
                list.Add(data);
                if (countRemain == 0)
                {
                    countRemain = count;
                }

            }
            streamReader.Close();
            return list;
        }
        //"C:\Users\User\source\repos\CSVLibrary123\data.csv"
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        /// <param name="append">true為在舊資料後寫上新資料，false為覆蓋舊資料</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Write<T>(string filePath, T t, bool append) where T : class, new()
        {
            string[] fileNames = filePath.Split('\\');
            string currentFilePath = String.Join("\\", fileNames.Take(fileNames.Length - 1));
            // 找不到資料夾時，創建路徑相應檔名的資料夾後，寫入資料。
            if (!Directory.Exists(currentFilePath))
            {
                Directory.CreateDirectory(currentFilePath);
                return;
            }
            string fExtension = filePath.Split('.').Last();
            if (fExtension != "csv")
            {
                throw new ArgumentException("只接受寫入副檔名為CSV的檔案。");
            }
            List<T> list = new List<T>() { t };
            Write(filePath, list, append);
        }
        public static void Write<T>(string filePath, List<T> datas, bool append) where T : class, new()
        {
            string[] fileNames = filePath.Split('\\');
            string currentFilePath = String.Join("\\", fileNames.Take(fileNames.Length - 1));
            // 找不到資料夾時，創建路徑相應檔名的資料夾後，寫入資料。
            if (!Directory.Exists(currentFilePath))
            {
                Directory.CreateDirectory(currentFilePath);
                return;
            }
            string fExtension = filePath.Split('.').Last();
            if (fExtension != "csv")
            {
                throw new ArgumentException("只接受寫入副檔名為CSV的檔案。");
            }

            AWriteData awriteData = null;
            if (!append)
            {
                awriteData = new WriteHeaderThenNewData();
            }
            else
            {
                FileWriteInStrategy writeType = HeaderManager.CheckWriteInType<T>(filePath);
                // 檢查目的地檔案 1.原本沒header加上header 2.直接重寫 3.接著最後繼續寫
                Type type = Type.GetType("CSVLibrary.Strategies." + writeType.ToString());
                awriteData = (AWriteData)Activator.CreateInstance(type);
            }

            awriteData.Write(filePath, datas);
        }

    }
}
