using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Reflection;
using System.IO;
using CYZFramework.Log;
using System.Reflection.Emit;
using ElectronTransferFramework;

namespace CYZFramework
{
    public class WApi
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();


        

        [DllImport("kernel32", EntryPoint = "CreateSemaphore", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint CreateSemaphore(SecurityAttribute auth, int initialCount, int maximumCount, string name);

        [DllImport("kernel32", EntryPoint = "WaitForSingleObject", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint WaitForSingleObject(uint hHandle, uint dwMilliseconds);

        [DllImport("kernel32", EntryPoint = "ReleaseSemaphore", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        public static extern bool ReleaseSemaphore(uint hHandle, int lReleaseCount, out int lpPreviousCount);

        [DllImport("kernel32", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.VariantBool)]
        public static extern bool CloseHandle(uint hHandle);


        /// <summary>
        /// c++写的导出连接关系
        /// </summary>
        /// <param name="kxmcs">%10kV沙富线F727<C>%10kV新丰线F725<C>10kV龙沙联线F716<C>10kV永盛线F728</param>
        /// <param name="session">12345678</param>
        /// <param name="initFileName">配置文件全路径</param>
        /// <param name="ftpPath">存储文件的路径(目录"E:\work\cad\jm_cad\test\")</param>
        /// <returns></returns>
        [DllImport("ExportConnectivity.dll", CharSet = CharSet.Ansi)]
        public static extern bool exportconnectivity(string kxmcs, string session,string ltt_id,string initFileName, string ftpPath,string connstring);
        //public static bool exportconnectivity(string kxmcs, string session, string initFileName, string ftpPath)
        //{
        //    bool reval = false;
        //    try
        //    {
        //        string dllname = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), "ExportConnectivity.dll");
        //        DllCaller invMethod = new DllCaller(dllname, "exportconnectivity", reval, kxmcs, session, initFileName, ftpPath);
        //        reval = (bool)invMethod.Call(kxmcs, session, initFileName, ftpPath);

        //    }
        //    catch (Exception ex)
        //    {
        //        CYZLog.writeLog(ex.Message);
        //    }
        //    return reval;
        //}    

    }



    public class DllCaller : IDisposable
    {
        [DllImport("Kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("Kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("Kernel32.dll")]
        static extern bool FreeLibrary(IntPtr hModule);

        private IntPtr libPtr;
        private MethodInfo method;
        /// <param name="dllFile">DLL文件的位置</param>
        /// <param name="functionName">函数名，注意带字符串的函数分W版跟A版</param>
        /// <param name="result">返回的数类型，如果返回void，使用typeof(void)</param>
        /// <param name="args">参数列表</param>
        /// <remarks>
        /// 注意:为了方便使用，返回类型跟参数类型都用一个实例表示，实例值没影响。
        /// 例如:用true,false表示bool; 0表示int; (byte)0表示byte, IntPtr.Zero表示指针等
        /// </remarks>
        /// <example>
        /// int MessageBox(
        /// HWND hWnd, 
        /// LPCTSTR lpText, 
        /// LPCTSTR lpCaption, 
        /// UINT uType
        /// ); 
        /// DllCaller MessageBox = new DllCaller(
        /// "user32.dll", "MessageBoxW", IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)0
        /// );
        /// </example>
        public DllCaller(string dllFile, string functionName, object result, params object[] args)
        {
            if (dllFile == null) throw new ArgumentNullException();
            if (functionName == null) throw new ArgumentNullException();

            this.libPtr = LoadLibrary(dllFile);
            if (this.libPtr == IntPtr.Zero) throw new DllNotFoundException(dllFile);

            IntPtr procPtr = GetProcAddress(this.libPtr, functionName);
            if (procPtr == IntPtr.Zero) throw new EntryPointNotFoundException(functionName);

            AssemblyName asmName = new AssemblyName();
            asmName.Name = "DynamicAssembly";

            AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule("DynamicModule");

            Type resultType = (result == typeof(void) ? typeof(void) : result.GetType());
            Type[] argTypes = new Type[args.Length];
            for (int i = 0; i < args.Length; i++)
                argTypes[i] = args[i].GetType();

            MethodBuilder funBuilder = modBuilder.DefineGlobalMethod(
                functionName,
                MethodAttributes.Public | MethodAttributes.Static,
                resultType,
                argTypes
                );

            ILGenerator ilGen = funBuilder.GetILGenerator();
            for (int i = 0; i < args.Length; i++)
                ilGen.Emit(OpCodes.Ldarg, i);

            if (IntPtr.Size == 4)
                ilGen.Emit(OpCodes.Ldc_I4, (int)procPtr);
            else if (IntPtr.Size == 8)
                ilGen.Emit(OpCodes.Ldc_I8, (long)procPtr);

            ilGen.EmitCalli(OpCodes.Calli, CallingConvention.StdCall, resultType, argTypes);
            ilGen.Emit(OpCodes.Ret);
            modBuilder.CreateGlobalFunctions();
            this.method = modBuilder.GetMethod(functionName);
        }

        public object Call(params object[] args)
        {
            return this.method.Invoke(null, args);
        }
        public void Dispose()
        {
            if (this.method != null)
            {
                if (this.libPtr != IntPtr.Zero)
                    FreeLibrary(this.libPtr);
                this.libPtr = IntPtr.Zero;
                this.method = null;
                GC.SuppressFinalize(this);
            }
        }
        ~DllCaller()
        {
            this.Dispose();
        }
    }
}
