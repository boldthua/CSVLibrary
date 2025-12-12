using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary.Strategies
{
    internal class WriteHeaderThenOldAndNewData : AWriteData
    {
        public override void Write<T>(string filePath, List<T> t)
        {
            //先取出舊資料
            StreamReader sr = new StreamReader(filePath);
            string oldData = sr.ReadToEnd(); // 每一次會讀取一行資料
            sr.Close();
            StreamWriter sw = new StreamWriter(filePath, false);
            PropertyInfo[] properties = typeof(T).GetProperties();
            string writeIn = String.Join(",", properties.Select(x => x.Name));
            sw.WriteLine(writeIn);
            sw.Write(oldData);

            foreach (T minT in t)
            {
                string data = string.Join(",", minT.GetType().GetProperties().Select(x => x.GetValue(minT)));
                sw.WriteLine(data);
            }
            sw.Close();
        }
    }
}
