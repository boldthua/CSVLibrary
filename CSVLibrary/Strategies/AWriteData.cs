using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary.Strategies
{
    internal abstract class AWriteData
    {
        public abstract void Write<T>(string filePath, T t);
    }
}
