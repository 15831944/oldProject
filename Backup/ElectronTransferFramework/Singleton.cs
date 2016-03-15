using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework
{
    /// <summary>
    /// 单件
    /// </summary>
    /// <typeparam name="T">要单件化的类型</typeparam>
    public class Singleton<T> where T : new()
    {
        static T _instance;
        static object _lockObj=new object();
        /// <summary>
        /// 当前对象
        /// </summary>
        public static T Instance 
        {
            get{
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
