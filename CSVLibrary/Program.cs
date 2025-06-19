using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //ORM(Object Relaction Mapping) => 物件關係對應 的開發/呼叫方式
            //所有的行為操作全部都是使用物件導向(類別與物件)來完成的
            //透過CSV Library 可以在不需要了解 StreamReader/Writer 的情況下來去完成CSV 讀寫操作



            //EntityFreamwork => LINQ 操作資料庫

            //在架構面前 資料庫只是其中一種資料來源
            //資料來源: json/csv/api/database ...etc
            //DAO => Data Access Object 資料來源存取
            //DTO => Data Transfer Object 資料傳輸


            //讀取
            //List<RecordModel> datas = CSVHelper.Read<RecordModel>(filePath);
            Student student1 = new Student();
            Student student2 = new Student();
            student1.Id = "001";
            student1.Name = "學生1號";
            student1.Gender = "男";
            student1.Age = "0";
            student1.Weight = "100";
            student1.Height = "100";
            student1.Grade = "S";

            // CSVHelper.Write<Student>(@"C:\Users\User\source\repos\CSVLibrary\data.csv", student1, true);

            List<StudentPhysicalData> students = CSVHelper.Read<StudentPhysicalData>(@"C:\Users\User\source\repos\CSVLibrary\data.csv");
            foreach (StudentPhysicalData student in students)
            {
                Console.WriteLine(JsonConvert.SerializeObject(student));
            }

            Console.ReadKey();
            // Console.WriteLine(CSVHelper.Read<StudentPhysicalData>(@"C:\Users\User\source\repos\CSVLibrary\data.csv"));
            //寫入
            //RecordModel model = new RecordModel(xx,xxx,xxxx);
            //CSVHelper.Write<RecordModel>(model);

            //作業：檢查網址格式
            //File Directory

        }
    }
}
