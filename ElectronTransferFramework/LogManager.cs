using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net.Config;

namespace ElectronTransferFramework
{


    public class LogManager :Singleton<LogManager>, ILogManager 
    {
        ILogManager _logManager = new LogManagerImplement();
        #region ILogManager 成员

        public void Debug(object message)
        {
            _logManager.Debug(message);
        }

        public void Error(object message)
        {
            _logManager.Error(message);
        }

        public void Fatal(object message)
        {
            _logManager.Fatal(message);
        }

        public void Info(object message)
        {
            _logManager.Info(message);
        }

        #endregion
    }
    /// <summary>
    /// 日志管理器
    /// </summary>
    internal class LogManagerImplement : ElectronTransferFramework.ILogManager
    {
        private const string LOGFILENAME = "log.config";
        private const string LOGNAME = "ElectronTransfer";
        private log4net.ILog _log;

        public LogManagerImplement()
        {
#if DEBUG
            NativeMethods.AllocConsole();
#endif
            var path =Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), LOGFILENAME);
            XmlConfigurator.Configure(new FileInfo(path));
            _log = log4net.LogManager.GetLogger(LOGNAME);
        }

        /// <summary>
        /// 输出调试消息
        /// </summary>
        /// <param name="message">消息</param>
        public void Debug(object message)
        {
            _log.Debug(message);
        }

        /// <summary>
        /// 输出错误消息
        /// </summary>
        /// <param name="message">消息</param>
        public void Error(object message)
        {
            _log.Error(message);
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message">消息</param>
        public void Info(object message)
        {
            _log.Info(message);
        }

        /// <summary>
        /// 输出致命消息
        /// </summary>
        /// <param name="message">消息</param>
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }
    }
}
