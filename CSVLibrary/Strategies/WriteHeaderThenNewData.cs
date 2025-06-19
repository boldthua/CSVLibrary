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
        public override void Write<T>(string filePath, T t)
        {
            PropertyInfo[] Tproperties = typeof(T).GetProperties();
            string writeIn = String.Join(",", Tproperties.Select(x => x.Name));

            string data = string.Join(",", t.GetType().GetProperties().Select(x => x.GetValue(t)));

            StreamWriter SW = new StreamWriter(filePath, false);
            SW.WriteLine(String.Join(",", writeIn));

            SW.WriteLine(data);
            SW.Flush();
            SW.Close();
        }
    }
}
