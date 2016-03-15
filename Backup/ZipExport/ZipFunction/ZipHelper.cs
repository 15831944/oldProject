using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace ElectronTransferDal.ZipFunction
{
    public class ZipHelper
    {

        /// <summary>解压文件</summary>
        /// <param name="file">压缩文件的名称，如：C:\123\123.zip</param>
        /// <param name="dir">dir要解压的文件夹路径</param>
        /// <returns></returns>
        public static bool UnpackFiles(string file, string dir)
        {
            try
            {
                if (!File.Exists(file))
                    return false;

                dir = dir.Replace("/", "\\");
                if (!dir.EndsWith("\\"))
                    dir += "\\";

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                ZipInputStream s = new ZipInputStream(File.OpenRead(file));
                //s.Password
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    string directoryName = Path.GetDirectoryName(theEntry.FileName);//.Name);
                    string fileName = Path.GetFileName(theEntry.FileName);//.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(dir + directoryName);

                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(dir + theEntry.FileName);//.Name);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 创建压缩包
        /// </summary>
        /// <param name="path"></param>
        /// <param name="zipPath"></param>
        public static void CreateZip(string path,string zipPath)
        {
            //ZipFile实例化一个对象zip
            using (ZipFile zip = new ZipFile(Encoding.Default))
            {
                //加密压缩
                //zip.Password = "123456";
                //将要压缩的文件夹添加到zip对象中去(要压缩的文件夹路径和名称)
                zip.AddDirectory(path);
                zip.Save(zipPath);
            }
        }

        /// <summary>
        /// 创建压缩包
        /// </summary>
        /// <param name="files"></param>
        /// <param name="zipPath"></param>
        public static void CreateZip(IEnumerable<string> files, string zipPath)
        {
            //ZipFile实例化一个对象zip
            using (ZipFile zip = new ZipFile(Encoding.Default))
            {
                //加密压缩
                //zip.Password = "123456";
                zip.AddFiles(files);
                zip.Save(zipPath);
            }
        }
    }
}
