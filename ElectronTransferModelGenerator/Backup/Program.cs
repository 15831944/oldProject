using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.IO;

namespace ElectronTransferModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("请输入储存的目录");
            string dir = Console.ReadLine();
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            Console.WriteLine("请输入命名空间");
            string nameOfnamespace = Console.ReadLine();

            string database = Console.ReadLine();
            ClassGenerator g = new OracleClassGenerator(dir, nameOfnamespace);
            Console.WriteLine("正在生成...");
            g.GenerateAll();
            Console.WriteLine("完成，按回车键退出。");
            Console.ReadLine();
        }
    }
}
