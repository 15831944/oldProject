using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using ElectronTransferFramework.TextProcess;
using System.Diagnostics;

namespace ElectronTransferFramework.Serialize
{
    public static class XmlSerializeUtils
    {
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="obj">的预想</param>
        /// <param name="path">路径</param>
        /// <param name="extraTypes">额外的类型，若对象中存在自定义类型的对象，则必须将这些对象输入</param>
        /// <param name="inverse">反码保存</param>
        public static void SaveAs(this ICanSerialize obj, string path, Type[] extraTypes,string password)

        {
            Save(path,obj,extraTypes,password);
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="obj">的预想</param>
        /// <param name="path">路径</param>
        /// <param name="extraTypes">额外的类型，若对象中存在自定义类型的对象，则必须将这些对象输入</param>
        public static void SaveAs(this ICanSerialize obj, string path, Type[] extraTypes)
        {
            Save(path, obj, extraTypes, string.Empty);
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="obj">对象</param>
        /// <param name="extraTypes">额外的类型，若对象中存在自定义类型的对象，则必须将这些对象输入</param>
        public static void Save(string path, ICanSerialize obj, Type[] extraTypes, string password)
        {
            //using (FileStream outputStream = new FileStream(path,FileMode.Create))
            //{
            //    using (MemoryStream memoryStream = new MemoryStream())
            //    {
            //        XmlSerializer serializer = XmlSerializerFactory.Instance.Create(obj.GetType(), extraTypes);//new XmlSerializer(obj.GetType(), extraTypes);
            //        serializer.Serialize(memoryStream, obj);                    
                    
            //        memoryStream.Seek(0, SeekOrigin.Begin);
            //        if (!string.IsNullOrEmpty(password))
            //        {

            //            CompressUtils.Compress(memoryStream, outputStream, password);
            //        }
            //        else 
            //        {
            //            memoryStream.WriteTo(outputStream);
            //        }
                    

            //    }
                
                
            //}
            var tempFilePath = Path.GetTempFileName();
            using (var tempFileWriter = new StreamWriter(tempFilePath, false, Encoding.UTF8))
            {

                XmlSerializer serializer = XmlSerializerFactory.Instance.Create(obj.GetType(), extraTypes);
                serializer.Serialize(tempFileWriter, obj);
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (!string.IsNullOrEmpty(password))
            {
                CompressUtils.Compress(tempFilePath, path, password);
                File.Delete(tempFilePath);
            }
            else
            {
                File.Move(tempFilePath, path);
            }
        }
        public static void Save(string path, ICanSerialize obj, Type[] extraTypes) 
        {
            Save(path, obj, extraTypes, string.Empty);
        }

        //public static void Save<T>(string path,object obj,Type[] extraTypes)
        //{
        //    using (StreamWriter writer = new StreamWriter(path))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes );
        //        serializer.Serialize(writer, obj);
        //    }
        //}
        /// <summary>
        /// 加载对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">路径</param>
        /// <param name="extraTypes">额外类型</param>
        /// <returns></returns>
        public static T Load<T>(string path,Type[] extraTypes,string password)
        {
            //var bytes = File.ReadAllBytes(path);
            using (FileStream fileStream= new FileStream(path, FileMode.Open))
            {
                return Load<T>(fileStream, extraTypes, password);
            }
        }

        public static T Load<T>(Stream input, Type[] extraTypes, string password)
        {
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    bool compress = !string.IsNullOrEmpty(password);
            //    if (compress)
            //    {
            //        CompressUtils.Decompress(input, memoryStream, password);
            //        memoryStream.Seek(0, SeekOrigin.Begin);
            //    }
            //    var rawStream = compress ? memoryStream : input;

            //    XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
            //    return (T)serializer.Deserialize(rawStream);
            //}
            bool compress = !string.IsNullOrEmpty(password);
            var tempFilePath = Path.GetTempFileName();
            if (compress)
            {
                using ( FileStream output = new FileStream( tempFilePath,FileMode.Open ) )
                {
                    CompressUtils.Decompress(input, output, password);
                    output.Seek(0, SeekOrigin.Begin);
                }
            }
            Stream rawStream = compress ? new FileStream(tempFilePath,FileMode.Open) : input;
            XmlSerializer serializer = new XmlSerializer(typeof(T), extraTypes);
            return (T)serializer.Deserialize(rawStream);
            

        }

        public static T Load<T>(string path, Type[] extraTypes) 
        {
            return Load<T>(path, extraTypes,string.Empty);
        }

        private static void ForceCollect(int size)
        {
            try
            {
                using (var memFailPoint = new MemoryFailPoint(size))
                {
                }
            }
            catch (InsufficientMemoryException)
            {

                //throw;
            }
            catch (OutOfMemoryException)
            {


            }

            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }


        }


    }
}
