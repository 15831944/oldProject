using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CYZFramework.Log;
using System.Threading;

namespace CYZFramework.Thread
{
    public class CyzEventWaitHandle
    {
        public static string CyzEventPath = System.Configuration.ConfigurationSettings.AppSettings["CyzEventPath"].ToString();
        private string eventfullname = "";
        private string eventname = "";

        public CyzEventWaitHandle(string name)
        {
            try
            {
                if (!Directory.Exists(CyzEventPath + "CyzEvent\\")) {
                    Directory.CreateDirectory(CyzEventPath + "CyzEvent\\");
                }
                eventname = name;
                eventfullname = CyzEventPath + "CyzEvent\\" + name;                
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
        }

        public static CyzEventWaitHandle OpenExisting(string name)
        {
            CyzEventWaitHandle mcewh = null;
            try
            {

                if (File.Exists(CyzEventPath + name))
                {
                    mcewh = new CyzEventWaitHandle(name);
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            return mcewh;
        }

        public bool Reset()
        {
            bool reval = false;
            try
            {
                bool bb = false;
                using (Mutex mm = new Mutex(false, eventname, out bb))
                {
                    if (mm != null)
                    {
                        bool waitbb = mm.WaitOne();
                        if (waitbb)
                        {
                            try
                            {
                                if (File.Exists(eventfullname))
                                {
                                    File.Delete(eventfullname);
                                    reval = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                CYZLog.writeLog(ex.ToString());
                            }
                            finally
                            {
                                mm.ReleaseMutex();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            return reval;
        }

        public bool Set()
        {
            bool reval = false;
            try
            {
                bool bb = false;
                using (Mutex mm = new Mutex(false, eventname, out bb))
                {
                    if (mm != null)
                    {
                        bool waitbb = mm.WaitOne();
                        if (waitbb)
                        {
                            try
                            {
                                if (!File.Exists(eventfullname))
                                {
                                    File.Create(eventfullname);
                                }
                                reval = true;
                            }
                            catch (Exception ex)
                            {
                                CYZLog.writeLog(ex.ToString());
                            }
                            finally {
                                mm.ReleaseMutex();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            return reval;
        }
        public bool WaitOne()
        {
            bool reval = false;
            try
            {
                if (File.Exists(eventfullname))
                {
                    reval = true;
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
            }
            return reval;
        }
      
    }





}
