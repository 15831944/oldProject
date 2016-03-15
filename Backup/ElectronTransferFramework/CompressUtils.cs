using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

namespace ElectronTransferFramework
{
    public class CompressUtils
    {
        public static void Compress(Stream input, Stream output,string password) 
        {
            using (ZipFile zipFile = new ZipFile { Password = password, CompressionLevel = Ionic.Zlib.CompressionLevel.None, Encryption= EncryptionAlgorithm.WinZipAes256 })
            {
                zipFile.AddEntry("Data.xml", input);
               
                zipFile.Save(output);
            }
            
        }

        public static void Compress(string input, string output, string password) 
        {
            using (FileStream inputStream = new FileStream(input, FileMode.Open)) 
            {
                using (FileStream outputStream = new FileStream(output, FileMode.Create)) 
                {
                    Compress(inputStream, outputStream, password);
                }
            }
        }

        public static void Decompress(Stream input, Stream output, string password) 
        {
            //add exception
            using (ZipFile zipFile = ZipFile.Read(input)) 
            {
                zipFile.Entries.Single(o=>o.FileName=="Data.xml").ExtractWithPassword(output,password);
            }
        }

        public static void Decompress(string input, string output, string password) 
        {
            using (FileStream inputStream = new FileStream(input, FileMode.Open))
            {
                using (FileStream outputStream = new FileStream(output, FileMode.Create))
                {
                    Decompress(inputStream, outputStream, password);
                }
            }
        }

        public static bool CheckFile(string path) 
        {
            try
            {
                using (ZipFile zipFile = ZipFile.Read(path))
                {
                    zipFile.Entries.Single(o => o.FileName == "Data.xml");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckZip(string path) 
        {
            return ZipFile.CheckZip(path);
        }

        public static bool CheckZip(string path,string password) 
        {
            try
            {
                return ZipFile.CheckZipPassword(path, password);
            }
            catch(ZipException)
            {
                return false;
            }
        }
    }
}
