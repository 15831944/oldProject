using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace ZipExport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                DTZipPro(args[0]);
            }
        }


        static void DTZipPro(object oo)
        {

            try
            {

                Console.WriteLine(oo);
                Console.WriteLine("正在解压缩地图瓦片。。。"); 

                string frompath = oo.ToString().Replace("<space>", " ") ;
                string topath = frompath.Substring(0, frompath.Length - 8);
                if (Directory.Exists(frompath.Substring(0, frompath.Length - 3)))
                {
                    return;
                }

                ElectronTransferDal.ZipFunction.ZipHelper.UnpackFiles(frompath, topath);

             
            }
            catch (Exception ex) { }
        }
    }
}
