using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;


using System.Reflection;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferDal.OracleDal;
using ElectronTransferModel.V9_4;
using System.Configuration;


using ElectronTransferServiceDll;
using System.Threading;
using CYZFramework.Log;


namespace ElectronTransferService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
           // new UploadExecutionFactory();
           // new ExportExecutionFactory();

           //var currentAssembly = Assembly.GetExecutingAssembly();
           //var assemblies = ReflectionUtils.GetReferencedAssemblies(currentAssembly).ToList();
           //assemblies.Add(currentAssembly);
           //assemblies.ForEach(ExecutionManager.Instance.EnsureInitialize);

            Application["dataappendpath"] = ConfigurationManager.AppSettings["dataappendpath"];
            Application["datapackagepath"] = ConfigurationManager.AppSettings["datapackagepath"];
            Application["dataftppath"] = ConfigurationManager.AppSettings["dataftppath"];
            Application["QJConfig"] = ConfigurationManager.AppSettings["QJConfig"];
            Application["cadEventName"] = ConfigurationManager.AppSettings["cadEventName"];
            Application["spatialEventName"] = ConfigurationManager.AppSettings["spatialEventName"];

            Application["NewTime"] = DateTime.Today.ToString("yyyy-MM-dd");

            ElectronTransferServicePro.dataappendpath = Application["dataappendpath"].ToString();
            ElectronTransferServicePro.datapackagepath = Application["datapackagepath"].ToString();
            ElectronTransferServicePro.dataftppath = Application["dataftppath"].ToString();
            ElectronTransferServicePro.QJConfig = Application["QJConfig"].ToString();
            ElectronTransferServicePro.cadEventName = Application["cadEventName"].ToString();
            ElectronTransferServicePro.spatialEventName = Application["spatialEventName"].ToString();            

            try
            {
                bool bbEvent1 = false;
                using (EventWaitHandle mmEvent = new EventWaitHandle(false, EventResetMode.AutoReset, @"Global\spatialSyncService", out bbEvent1))
                {
                    if (mmEvent != null)
                    {
                        if (mmEvent.Reset())
                        {
                            CYZLog.writeLog("Start event spatialSyncService");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CYZLog.writeLog("ex=" + ex.ToString());
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}