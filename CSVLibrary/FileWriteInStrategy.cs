using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{
    internal enum FileWriteInStrategy
    {
        /// <summary>
        /// 當找不到檔案或 append = false 時，先寫入表頭，再寫入新資料。
        /// </summary>
        WriteHeaderThenNewData,

        /// <summary>
        /// 當舊資料的第一筆與表頭相同時，在舊資料後追加新資料。
        /// </summary>
        AppendNewData,

        /// <summary>
        /// 當舊資料的第一筆與表頭不同時，暫存舊資料，先寫入表頭，然後寫入舊資料和新資料。
        /// </summary>
        WriteHeaderThenOldAndNewData
    }
}
