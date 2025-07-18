using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{

    internal class HeaderManager
    {
        //想一下為何value放int 資料對位

        public static Dictionary<string, int> GetHeaders(string filePath)
        {
            Dictionary<string, int> header = new Dictionary<string, int>();
            if (header.Count == 0)
            {
                StreamReader streamReader = new StreamReader(filePath);
                string data = streamReader.ReadLine();
                streamReader.Close();

                if (string.IsNullOrEmpty(data))
                    return null;


                string[] headers = data.Split(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    header[headers[i]] = i;
                }
            }
            return header;
        }

        public static string HeadersCheck(string filePath, Type type)
        {
            if (!File.Exists(filePath))
                return "";
            StreamReader streamReader = new StreamReader(filePath);
            string data = streamReader.ReadLine();
            string[] headers = type.GetProperties().Select(x => x.Name).ToArray();
            streamReader.Close();

            if (string.IsNullOrEmpty(data))
                return "";

            if (data != String.Join(",", headers))
                return data;

            return String.Join(",", headers);
        }


        public static FileWriteInStrategy CheckWriteInType<T>(string filePath) where T : class, new()
        {

            // 取得第一筆資料
            string headerInRawData = HeadersCheck(filePath, typeof(T));
            //當沒有資料就代表檔案是空白的
            if (String.IsNullOrEmpty(headerInRawData))
            {
                return FileWriteInStrategy.WriteHeaderThenNewData;
            }

            //取得原始Header長相
            string header = String.Join(",", typeof(T).GetProperties().Select(p => p.Name));
            //比對資料,檢查第一筆資料是否是Header
            if (header != headerInRawData)
            {
                return FileWriteInStrategy.WriteHeaderThenOldAndNewData;
            }
            else
            {
                return FileWriteInStrategy.AppendNewData;
            }
        }
    }
}

// 程序再理順一點
// 上傳github