using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary.Strategies
{
    internal class WriteHeaderThenNewData : AWriteData
    {
        public override void Write<T>(string filePath, List<T> t)
        {
            StreamWriter SW = new StreamWriter(filePath, false);
            PropertyInfo[] Tproperties = typeof(T).GetProperties();
            string writeIn = String.Join(",", Tproperties.Select(x => x.Name));
            SW.WriteLine(String.Join(",", writeIn));
            foreach (T minT in t)
            {
                string data = string.Join(",", minT.GetType().GetProperties().Select(x => x.GetValue(minT)));
                SW.WriteLine(data);
            }
            SW.Flush();
            SW.Close();
        }
    }
}
