using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InitLock
{
    class Program
    {
        static void Main(string[] args)
        {
            //批量
            foreach( var senseLock in SenseLock.GetAll())
            {
                bool ret;
                senseLock.Open();
                if (senseLock.VerifyDevPin("123456781234567812345678") ||
                    senseLock.VerifyDevPin("cadf581a5f976c771535cbc1"))
                {
                }
                ret = senseLock.EraserDirectory(null);
                ret = senseLock.CreateDirectory(@"\");
                ret = senseLock.ChangeDirectory(@"\");
                senseLock.VerifyDevPin("123456781234567812345678");
                ret = senseLock.ChangeDevPin("123456781234567812345678", "cadf581a5f976c771535cbc1");
                
                ret = senseLock.VerifyDevPin("cadf581a5f976c771535cbc1");;
                ret = senseLock.DownloadFile("Code.bin", "581a", true);
                ret = senseLock.DownloadFile("Time.txt", "1234", false);
                ret = senseLock.DownloadFile("ReadTime.bin", "3456", true);
                senseLock.ChangeUserPin("12345678", "d6465065");
                ret = senseLock.VerifyUserPin("d6465065");
                Console.WriteLine(senseLock.GetKey());
                Console.WriteLine(senseLock.GetTime());
                
            }
            Console.ReadLine();
        }
    }
}
