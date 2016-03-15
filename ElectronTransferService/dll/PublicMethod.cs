using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CYZFramework.Log;
using System.Runtime.InteropServices;
using System.Reflection;
//using System.Web.Compilation;
using System.Reflection.Emit;
using ElectronTransferFramework;

namespace ElectronTransferServiceDll
{
    public class PublicMethod
    {
        //static object oo = new object();
        public static void write_state(string s, object d)
        {
            try
            {
                string msg = "";
                if (d is double)
                {
                    int d1 = (int)((double)d * 100);
                    int d2 = (int)(
                        (int)((double)d * 10000) - (int)((double)d * 100) * 100
                        );

                    msg = d1.ToString();
                }
                else 
                {
                    msg = d.ToString();
                }

                using (StreamWriter sw = new StreamWriter(s + "state.log", false))
                {
                    sw.WriteLine(msg);
                    sw.Close();
                }

                CYZLog.writeLog2("downPro  " + msg, "", s);
            }
            catch (Exception ex) { }
            finally
            { }
        }
        public static string read_state(string s,string session_id)
        {
            string d = "0.0";
            //lock (oo)
            //{
            FileStream fs = null;
            try
            {
                string dirs = Directory.GetDirectories(s).Where(o =>o.IndexOf(session_id)>=0).First();
                
                fs = new FileStream(dirs + "\\state.log", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                d = sr.ReadLine().Replace(" ","").Replace("\n","");
                sr.Close();
            }
            catch { }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            //}
            return d;
        }


       // static string ExportConnectivity_dll = Assembly.GetExecutingAssembly().GetPhysicalDirectory();

        ///// <summary>
        ///// c++写的导出连接关系
        ///// </summary>
        ///// <param name="kxmcs">%10kV沙富线F727<C>%10kV新丰线F725<C>10kV龙沙联线F716<C>10kV永盛线F728</param>
        ///// <param name="session">12345678</param>
        ///// <param name="initFileName">配置文件全路径</param>
        ///// <param name="ftpPath">存储文件的路径(目录"E:\work\cad\jm_cad\test\")</param>
        ///// <returns></returns>
        ////[DllImport("ExportConnectivity.dll")]
        ////public static extern bool exportconnectivity(string kxmcs, string session, string initFileName, string ftpPath);
        //public static bool exportconnectivity(string kxmcs, string session, string initFileName, string ftpPath)
        //{
        //    bool reval = false;
        //    try
        //    {
        //        string dllname = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "ExportConnectivity.dll");
        //        DllCaller invMethod = new DllCaller(dllname, "exportconnectivity", reval, kxmcs, session, initFileName, ftpPath);
        //        reval = (bool)invMethod.Call(kxmcs, session, initFileName, ftpPath);

        //    }
        //    catch (Exception ex) {
        //        CYZLog.writeLog(ex.Message);                
        //    }
        //    return reval;
        //}
    }



}
