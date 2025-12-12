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
        public override void Write<T>(string filePath, List<T> t)
        {
            StreamWriter sw = new StreamWriter(filePath);

            foreach (T minT in t)
            {
                string data = string.Join(",", minT.GetType().GetProperties().Select(x => x.GetValue(minT)));
                sw.WriteLine(data);
            }
            sw.Flush();
            sw.Close();
        }

    }
}
