using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace CYZFramework.Log
{
    public class CYZLog
    {
        public static Mutex gmm = new Mutex(false, @"Global\CYZFrameworkLog");
        public static string oName = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        public static void writeLog(string msg1)
        {
            bool bb = false;
            //using (Mutex mm = new Mutex(false, @"Global\CYZFrameworkLog", out bb))
            Mutex mm = gmm;
            {
                if (mm != null)
                {
                    bool waitbb = mm.WaitOne(1000);
                    if (waitbb)
                    {
                        StreamWriter sw = null;
                        try
                        {
                            string filepath = System.AppDomain.CurrentDomain.BaseDirectory;

                            filepath += "CYZLog\\";
                            string fileName = filepath + oName + DateTime.Now.ToString("yyyy-MM-dd") + ".txlog";

                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }

                            if (!File.Exists(fileName))
                            {
                                //File.Create(fileName);   
                                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                fs.Close();
                            }

                            sw = new StreamWriter(fileName, true, Encoding.UTF8);

                            string dtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                            sw.WriteLine("--" + dtime + " :");

                            sw.WriteLine("  msg1:" + msg1);


                            Console.WriteLine(dtime);
                            Console.WriteLine("  msg1:" + msg1);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            mm.ReleaseMutex();
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        public static void writeLog(string msg1, string msg2)
        {
            bool bb = false;
            //using (Mutex mm = new Mutex(false, @"Global\CYZFrameworkLog", out bb))
            Mutex mm = gmm;
            {
                if (mm != null)
                {
                    bool waitbb = mm.WaitOne(1000);
                    if (waitbb)
                    {
                        StreamWriter sw = null;
                        try
                        {
                            string filepath = System.AppDomain.CurrentDomain.BaseDirectory;

                            filepath += "CYZLog\\";
                            string fileName = filepath + oName + DateTime.Now.ToString("yyyy-MM-dd") + ".txlog";

                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }

                            if (!File.Exists(fileName))
                            {
                                //File.Create(fileName);   
                                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                fs.Close();
                            }

                            sw = new StreamWriter(fileName, true, Encoding.UTF8);

                            string dtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                            sw.WriteLine("--" + dtime + " :");

                            sw.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                sw.WriteLine("  msg2:" + msg2);

                            Console.WriteLine(dtime);
                            Console.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                Console.WriteLine("  msg2:" + msg2);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            mm.ReleaseMutex();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        public static void writeLog(string msg1, string msg2, string msg3)
        {
            bool bb = false;
            //using (Mutex mm = new Mutex(false, @"Global\CYZFrameworkLog", out bb))
            Mutex mm = gmm;
            {
                if (mm != null)
                {
                    bool waitbb = mm.WaitOne(1000);
                    if (waitbb)
                    {
                        StreamWriter sw = null;
                        try
                        {
                            string filepath = System.AppDomain.CurrentDomain.BaseDirectory;

                            filepath += "CYZLog\\";
                            string fileName = filepath + oName + DateTime.Now.ToString("yyyy-MM-dd") + ".txlog";

                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }

                            if (!File.Exists(fileName))
                            {
                                //File.Create(fileName);   
                                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                fs.Close();
                            }

                            sw = new StreamWriter(fileName, true, Encoding.UTF8);

                            string dtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                            sw.WriteLine("--" + dtime + " :");

                            sw.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                sw.WriteLine("  msg2:" + msg2);
                            if (msg3 != null && msg3 != "")
                                sw.WriteLine("  msg3:" + msg3);

                            Console.WriteLine(dtime);
                            Console.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                Console.WriteLine("  msg2:" + msg2);
                            if (msg3 != null && msg3 != "")
                                Console.WriteLine("  msg3:" + msg3);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            mm.ReleaseMutex();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="msg3"></param>
        public static void writeLog1(string msg1, string msg2, string msg3)
        {
            bool bb = false;
            //using (Mutex mm = new Mutex(false, @"Global\CYZFrameworkLog", out bb))
            Mutex mm = gmm;
            {
                if (mm != null)
                {
                    bool waitbb = mm.WaitOne(1000);
                    if (waitbb)
                    {
                        StreamWriter sw = null;
                        try
                        {
                            string filepath = System.AppDomain.CurrentDomain.BaseDirectory;

                            filepath += "CYZLog\\";
                            string fileName = filepath + oName + DateTime.Now.ToString("yyyy-MM-dd") + ".applog";

                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }

                            if (!File.Exists(fileName))
                            {
                                //File.Create(fileName);   
                                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                fs.Close();
                            }

                            sw = new StreamWriter(fileName, true, Encoding.UTF8);

                            string dtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                            sw.WriteLine("--" + dtime + " :");

                            sw.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                sw.WriteLine("  msg2:" + msg2);
                            if (msg3 != null && msg3 != "")
                                sw.WriteLine("  msg3:" + msg3);

                            Console.WriteLine(dtime);
                            Console.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                Console.WriteLine("  msg2:" + msg2);
                            if (msg3 != null && msg3 != "")
                                Console.WriteLine("  msg3:" + msg3);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            mm.ReleaseMutex();
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="filepath"></param>
        public static void writeLog2(string msg1, string filepath)
        {
            bool bb = false;
            //using (Mutex mm = new Mutex(false, @"Global\CYZFrameworkLog", out bb))
            Mutex mm = gmm;
            {
                if (mm != null)
                {
                    bool waitbb = mm.WaitOne(1000);
                    if (waitbb)
                    {
                        StreamWriter sw = null;
                        try
                        {
                            if (string.IsNullOrEmpty(filepath))
                                filepath = System.AppDomain.CurrentDomain.BaseDirectory;

                            filepath += "CYZLog\\";
                            string fileName = filepath + oName + DateTime.Now.ToString("yyyy-MM-dd") + ".txlog";

                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }

                            if (!File.Exists(fileName))
                            {
                                //File.Create(fileName);   
                                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                fs.Close();
                            }

                            sw = new StreamWriter(fileName, true, Encoding.UTF8);

                            string dtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                            sw.WriteLine(dtime + " :");

                            sw.WriteLine("  msg1:" + msg1);

                            Console.WriteLine(dtime);
                            Console.WriteLine("  msg1:" + msg1);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            mm.ReleaseMutex();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <param name="filepath"></param>
        public static void writeLog2(string msg1, string msg2, string filepath)
        {
            bool bb = false;

            //using (Mutex mm = new Mutex(false, @"Global\CYZFrameworkLog", out bb))
            Mutex mm = gmm;
            {
                if (mm != null)
                {
                    bool waitbb = mm.WaitOne(1000);
                    if (waitbb)
                    {
                        StreamWriter sw = null;
                        try
                        {
                            if (string.IsNullOrEmpty(filepath))
                                filepath = System.AppDomain.CurrentDomain.BaseDirectory;

                            filepath += "CYZLog\\";
                            string fileName = filepath + oName + DateTime.Now.ToString("yyyy-MM-dd") + ".txlog";

                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }

                            if (!File.Exists(fileName))
                            {
                                //File.Create(fileName);   
                                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                fs.Close();
                            }

                            sw = new StreamWriter(fileName, true, Encoding.UTF8);

                            string dtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                            sw.WriteLine(dtime + " :");

                            sw.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                sw.WriteLine("  msg2:" + msg2);

                            Console.WriteLine(dtime);
                            Console.WriteLine("  msg1:" + msg1);
                            if (msg2 != null && msg2 != "")
                                Console.WriteLine("  msg2:" + msg2);

                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            mm.ReleaseMutex();
                        }
                    }
                }
            }

        }

    }
}
