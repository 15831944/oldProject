using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AopAlliance.Aop;
using ElectronTransferDal.Common;
using ElectronTransferDal.OracleDal;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework.Aspect;
using ElectronTransferModel.V9_4;
using ElectronTransferFramework;
//using Oracle.DataAccess.Client;
using Spring.Aop.Framework;
using Spring.Aop.Support;
using ElectronTransferModel.Geo;
using ElectronTransferModel.Base;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferFramework.TextProcess;
using System.Threading;
using System.Diagnostics;
using System.Linq.Expressions;
using System.IO;

namespace ElectronTransferConsoleTest
{
    public class Program 
    {
        public class Student
        {
            public string Name { get; set; }
        }
        private static void Main(string[] args)
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            var path=Path.Combine(@"..\..\..\..\..\文档\数据包", "dbcad_201308093.xml");
            var newPath = Path.ChangeExtension(path, "inv");
            File.WriteAllBytes(newPath, File.ReadAllBytes(path).Inverse());
            w.Stop();
            Expression<Func<Student, string>> expr = o => o.Name;
            var parameterExpr = Expression.Parameter(typeof(Common_n), "test");
            var propertyExpr = Expression.Property(parameterExpr, "G3E_ID");
            var f0 = Expression.Lambda(propertyExpr, parameterExpr);
            //var i = Expression.Call(parameterExpr, typeof(Student).GetProperty("Name").GetGetMethod(true));
            var f1 = Expression.Lambda<Func<Common_n, Int64>>(propertyExpr, parameterExpr);
            var func = expr.Compile();
            var result = func(new Student { Name = "jack" });
            var method = f1.Compile();

            Common_n n = new Common_n { G3E_ID = 1L };
            Stopwatch watch1 = new Stopwatch();
            Stopwatch watch2 = new Stopwatch();
            Stopwatch watch3 = new Stopwatch();
            watch1.Start();

            for (int index = 0; index < 1000000; index++)
            {
                var result1 = n.G3E_ID;//ExpressionEvaluator.GetValue(n, "G3E_ID");
            }
            watch1.Stop();
            watch2.Start();
            for (int index = 0; index < 1000000; index++)
            {
                var result2 = n.GetValue("G3E_ID");
            }
            watch2.Stop();
            var student = new Student { Name = "jack" };
            watch3.Start();
            for (int index = 0; index < 1000000; index++)
            {
                var result2 = method(n);
            }
            watch3.Stop();
            var result3 = n.GetValue("G3E_FID");
            
            Console.WriteLine(watch1.Elapsed);
            Console.WriteLine(watch2.Elapsed);
            Console.WriteLine(watch3.Elapsed);
            Console.ReadLine();
            
        }
    }
}