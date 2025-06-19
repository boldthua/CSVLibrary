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
    internal class CSVHelper
    {
        //C:\Users\User\source\repos\CSVLibrary\data.exe
        public static List<T> Read<T>(string filePath) where T : class, new()
        {             // 這裡的T是新創的類別，只拿你要的資料
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
            string aLineShouldBeHeader = streamReader.ReadLine();
            //  先檢查RawData的第一行是否是header
            if (String.IsNullOrEmpty(aLineShouldBeHeader))
                return list;
            // 取得Raw Header
            Dictionary<string, int> headers = HeaderManager.GetHeaders(filePath);
            string header = string.Join(",", typeof(T).GetProperties().Select(x => x.Name));

            while (!streamReader.EndOfStream)
            {
                // 先把第一筆挑掉
                if (streamReader.ReadLine() == header)
                    continue;

                T data = new T();
                PropertyInfo[] properties = data.GetType().GetProperties();
                string[] lineDatas = streamReader.ReadLine().Split(',');


                foreach (PropertyInfo property in properties)
                {
                    if (headers.ContainsKey(property.Name))
                    {
                        property.SetValue(data, lineDatas[headers[property.Name]]);
                    }
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

            // 開始檢查檔案決定寫入方式
            FileWriteInStrategy writeType = HeaderManager.CheckWriteInType<T>(filePath);
            Type type = Type.GetType("CSVLibrary.Strategies." + writeType.ToString());
            AWriteData aWriteData = (AWriteData)Activator.CreateInstance(type);
            aWriteData.Write(filePath, t);
        }
    }
}
