using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary.Strategies
{
    internal class AppendNewData : AWriteData
    {
        public override void Write<T>(string filePath, T t)
        {
            string data = string.Join(",", t.GetType().GetProperties().Select(x => x.GetValue(t)));
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine(data);
            sw.Close();
        }
    }
}
