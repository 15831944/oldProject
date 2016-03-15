using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ArxMap;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using ElectronTransferBll;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Factory;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferFramework.Serialize;
using ElectronTransferModel.Config;
using ElectronTransferView.ViewManager;
using acApp = Autodesk.AutoCAD.ApplicationServices.Application;
using CADException = Autodesk.AutoCAD.Runtime.Exception;
using Exception = System.Exception;


namespace ElectronTransferView
{
    public class NetLoad : IExtensionApplication
    {
        private static bool IsInstall;
        //程序集物理路径
        private static readonly string dirPath = Assembly.GetExecutingAssembly().GetPhysicalDirectory();

        private static ContextMenuExtension m_ContextMenu;


        #region IExtensionApplication 成员
        
        public void Initialize()
        {
            try
            {

#if EnableLock
                SenseLock.Instance.Open();
                if (SenseLock.Instance.VerifyUserPin("d6465065"))
                {
                    LockDaemon.Instance.OnUnplug =
                        () => MessageBox.Show("请使用加密狗！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LockDaemon.Instance.Start();
                    XmlPassword.Instance.Password = SenseLock.Instance.GetKey();
                    if (SenseLock.Instance.GetTime() < DateTime.Now)
                    {
                        MessageBox.Show("加密狗已过期，请联系管理员！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
#else
                XmlPassword.Instance.Password = "f2445aad4c0afdc0120d30aa1803a758";
#endif
                InitExceptionLogger();
                InitiationConfig();
                if (!string.IsNullOrEmpty(MapConfig.Instance.TimeLock))
                {
                    var currentTime = DateTime.Now.ToShortDateString();
                    var lastTime = DateTime.Parse(MapConfig.Instance.TimeLock);
                    var ts = DateTime.Now.Subtract(lastTime);
                    if (ts.Days < 0)
                    {
                        MessageBox.Show("插件已过期", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    var pastdueTime = DateTime.Parse("2015/6/30 00:00:00");
                    var pastdue = pastdueTime.Subtract(DateTime.Now);
                    if (pastdue.Days < 0)
                    {
                        MessageBox.Show("插件已过期", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    MapConfig.Instance.TimeLock = currentTime;
                    PublicMethod.Instance.SaveLocalMapConfig();
                }
                ChangeAutoCADWindow();
                EnsureInitialize();
                CADTypes.CreateLineType();
                CADTypes.CreateStyle();
                InstallCADPlugin();

                //启动台帐服务
                WindowsServices.TomcatServiceStart();
#if EnableLock
                }
                else
                {
                    SenseLock.Instance.Close();
                }
#endif
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        public void EnsureInitialize()
        {
            new SymbolExecutionFactory();
            new ViewExecutionFactory();
            new InsertSymbolExecutionFactory();
            new InsertToXmlExecutionFactory();
            new VerifyRuleExecutionFactory();

            var currentAssembly = Assembly.GetExecutingAssembly();
            var assemblies = ReflectionUtils.GetReferencedAssemblies(currentAssembly).ToList();
            assemblies.Add(currentAssembly);
            assemblies.ForEach(Try);
        }
        private void Try(Assembly assembly)
        {
            try
            {
                ExecutionManager.Instance.EnsureInitialize(assembly);
            }catch(Exception ex)
            {
                LogManager.Instance.Info(assembly);
                LogManager.Instance.Error(ex);
            }
        }

        public void InitExceptionLogger()
        {
            Application.ThreadException += Application_ThreadException;
        }

        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogManager.Instance.Error(e.Exception);
        }

        public void Terminate()
        {
#if EnableLock
            if (SenseLock.Instance.IsOpened)
            {
                SenseLock.Instance.Close();
            }
#endif
        }
        #endregion


        #region 配置
        /// <summary>
        /// 配置文件
        /// </summary>
        private void InitiationConfig()
        {
            var sdir = Path.Combine(dirPath, "MapConfig.xml");
            var attributedir = Path.Combine(dirPath, "DeviceAttributeConfig.xml");
            var dbsymbolsdir = Path.Combine(dirPath, "DBSymbolsConfig.xml");
            try
            {
                //初始化配置文件
                MapConfig.Instance = XmlSerializeUtils.Load<MapConfig>(sdir, new Type[] { });
                MapConfig.Instance.GCID = string.Empty;
                //初始化功能位置属性配置
                DeviceAttributeConfig.Instance = XmlSerializeUtils.Load<DeviceAttributeConfig>(attributedir, new Type[] { });
                //初始化设备参数配置
                SimpleDBSymbolsConfig.Instance = XmlSerializeUtils.Load<SimpleDBSymbolsConfig>(dbsymbolsdir, new Type[] { });
                //初始化标注
                MapConfig.Instance.labels = MapConfig.Instance.ListLabelShow.Split(',');
                //保存插件的根目录路径
                MapConfig.Instance.ClientXmlPath = dirPath;
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 安装插件
        /// <summary>
        /// 安装插件
        /// </summary>
        [CommandMethod("az")]
        public void InstallCADPlugin()
        {
            if (!IsInstall)
            {
                ViewHelper.AddToolBar();
                ViewHelper.AddMenuPanel();
                ViewHelper.AddSymbolPanel();
                ObjectContextMenu.AddContextMenu();
                IsInstall = true;
            }
        }
        #endregion
        #region 卸载插件
        /// <summary>
        /// 卸载插件
        /// </summary>
        [CommandMethod("xz")]
        public void UninstallCADPlugin()
        {
            if (IsInstall)
            {
                RemoveContextMenu();
                ViewHelper.RemoveSymbolPanel();
                ViewHelper.RemoveMenuPanel();
                ObjectContextMenu.RemoveContextMenu();
                IsInstall = false;
            }
        }
        /// <summary>
        /// 卸载右键菜单
        /// </summary>
        private void RemoveContextMenu()
        {
            try
            {
                if (m_ContextMenu != null)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.RemoveObjectContextMenuExtension(RXObject.GetClass(typeof (Entity)),m_ContextMenu);
                    m_ContextMenu.Dispose();
                    m_ContextMenu = null;
                }
            }
            catch(CADException ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
        }

      
        #endregion

        #region 更改AutoCAD窗口的标题和图标

        /// <summary>
        /// 更改AutoCAD窗口的标题和图标
        /// </summary>
        private static void ChangeAutoCADWindow()
        {
            SetIcon();
            SetTitle();
        }
        /// <summary>
        /// 更改AutoCAD窗口的标题名称
        /// </summary>
        private static void SetTitle()
        {
            //var acadApp = acApp.AcadApplication as AcadApplication;
            try
            {
                //if (acadApp != null)
                //{
                //    int AcadHwnd = (int)acadApp.HWND; //获取AutoCAD应用程序的窗口句柄
                //    WinAPI.SetWindowTextA(AcadHwnd, "北京中合实创电力科技有限公司 AutoCAD 1.0");
                //}
                var mWindow = Autodesk.AutoCAD.ApplicationServices.Application.MainWindow;
                mWindow.Text = "北京中合实创电力科技有限公司 电子化移交CAD系统 1.0";
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>.
        /// 更改AutoCAD窗口的图标
        /// </summary>
        private static void SetIcon()
        {
            try
            {
                var fileName = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(),
                                              @"Resource\zhsc.ico");
                var mWindow = Autodesk.AutoCAD.ApplicationServices.Application.MainWindow;
                mWindow.Icon = new Icon(fileName);
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        #endregion
    }
    public class WinAPI
    {
        #region  宏定义

        public const int WM_SETICON = 0x80;
        public const int IMAGW_ICON = 1;
        public const int LR_LOADFROMFILE = 0x10;

        #endregion

        #region  WinAPI定义

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            int hWnd, // handle to destination window 
            int Msg, // message 
            int wParam, // first message parameter 
            int lParam // second message parameter 
            );

        [DllImport("user32", EntryPoint = "LoadImage")]
        public static extern int LoadImageA(int hInst, string lpsz, int un1, int n1, int n2, int un2);

        [DllImport("user32", EntryPoint = "SetWindowText")]
        public static extern int SetWindowTextA(int hwnd, string lpString);

        #endregion
    }
}
