using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace InitLock
{
    internal class SenseLock : IDisposable
    {
        //Sense4 API

        // ctlCode definition for S4Control
        static public uint S4_LED_UP = 0x00000004;  // LED up
        static public uint S4_LED_DOWN = 0x00000008;  // LED down
        static public uint S4_LED_WINK = 0x00000028;  // LED wink
        static public uint S4_GET_DEVICE_TYPE = 0x00000025;	//get device type
        static public uint S4_GET_SERIAL_NUMBER = 0x00000026;	//get device serial
        static public uint S4_GET_VM_TYPE = 0x00000027;  // get VM type
        static public uint S4_GET_DEVICE_USABLE_SPACE = 0x00000029;  // get total space
        static public uint S4_SET_DEVICE_ID = 0x0000002a;  // set device ID
        static public uint S4_SET_USB_MODE = 0x00000041;  /** set the device as a normal usb device*/
        static public uint S4_SET_HID_MODE = 0x00000042;                      /** set the device as a HID device*/
        // device type definition 
        static public uint S4_LOCAL_DEVICE = 0x00;		// local device 
        static public uint S4_MASTER_DEVICE = 0x80;		// net master device
        static public uint S4_SLAVE_DEVICE = 0xc0;		// net slave device

        // vm type definiton 
        static public uint S4_VM_51 = 0x00;		// VM51
        static public uint S4_VM_251_BINARY = 0x01;		// VM251 binary mode
        static public uint S4_VM_251_SOURCE = 0x02;		// VM251 source mode


        // PIN type definition 
        static public uint S4_USER_PIN = 0x000000a1;		// user PIN
        static public uint S4_DEV_PIN = 0x000000a2;		// dev PIN
        static public uint S4_AUTHEN_PIN = 0x000000a3;		// autheticate Key


        // file type definition 
        static public uint S4_RSA_PUBLIC_FILE = 0x00000006;		// RSA public file
        static public uint S4_RSA_PRIVATE_FILE = 0x00000007;		// RSA private file 
        static public uint S4_EXE_FILE = 0x00000008;		// VM file
        static public uint S4_DATA_FILE = 0x00000009;		// data file

        // dwFlag definition for S4WriteFile
        static public uint S4_CREATE_NEW = 0x000000a5;		// create new file
        static public uint S4_UPDATE_FILE = 0x000000a6;		// update file
        static public uint S4_KEY_GEN_RSA_FILE = 0x000000a7;		// produce RSA key pair
        static public uint S4_SET_LICENCES = 0x000000a8;		// set the license number for modle,available for net device only
        static public uint S4_CREATE_ROOT_DIR = 0x000000ab;		// create root directory, available for empty device only
        static public uint S4_CREATE_SUB_DIR = 0x000000ac;		// create child directory
        static public uint S4_CREATE_MODULE = 0x000000ad;		// create modle, available for net device only

        // the three parameters below must be bitwise-inclusive-or with S4_CREATE_NEW, only for executive file
        static public uint S4_FILE_READ_WRITE = 0x00000000;      // can be read and written in executive file,default
        static public uint S4_FILE_EXECUTE_ONLY = 0x00000100;      // can NOT be read or written in executive file
        static public uint S4_CREATE_PEDDING_FILE = 0x00002000;		// create padding file


        /* return value*/
        static public uint S4_SUCCESS = 0x00000000;		// succeed
        static public uint S4_UNPOWERED = 0x00000001;
        static public uint S4_INVALID_PARAMETER = 0x00000002;
        static public uint S4_COMM_ERROR = 0x00000003;
        static public uint S4_PROTOCOL_ERROR = 0x00000004;
        static public uint S4_DEVICE_BUSY = 0x00000005;
        static public uint S4_KEY_REMOVED = 0x00000006;
        static public uint S4_INSUFFICIENT_BUFFER = 0x00000011;
        static public uint S4_NO_LIST = 0x00000012;
        static public uint S4_GENERAL_ERROR = 0x00000013;
        static public uint S4_UNSUPPORTED = 0x00000014;
        static public uint S4_DEVICE_TYPE_MISMATCH = 0x00000020;
        static public uint S4_FILE_SIZE_CROSS_7FFF = 0x00000021;
        static public uint S4_DEVICE_UNSUPPORTED = 0x00006a81;
        static public uint S4_FILE_NOT_FOUND = 0x00006a82;
        static public uint S4_INSUFFICIENT_SECU_STATE = 0x00006982;
        static public uint S4_DIRECTORY_EXIST = 0x00006901;
        static public uint S4_FILE_EXIST = 0x00006a80;
        static public uint S4_INSUFFICIENT_SPACE = 0x00006a84;
        static public uint S4_OFFSET_BEYOND = 0x00006B00;
        static public uint S4_PIN_BLOCK = 0x00006983;
        static public uint S4_FILE_TYPE_MISMATCH = 0x00006981;
        static public uint S4_CRYPTO_KEY_NOT_FOUND = 0x00009403;
        static public uint S4_APPLICATION_TEMP_BLOCK = 0x00006985;
        static public uint S4_APPLICATION_PERM_BLOCK = 0x00009303;
        static public int S4_DATA_BUFFER_LENGTH_ERROR = 0x00006700;
        static public uint S4_CODE_RANGE = 0x00010000;
        static public uint S4_CODE_RESERVED_INST = 0x00020000;
        static public uint S4_CODE_RAM_RANGE = 0x00040000;
        static public uint S4_CODE_BIT_RANGE = 0x00080000;
        static public uint S4_CODE_SFR_RANGE = 0x00100000;
        static public uint S4_CODE_XRAM_RANGE = 0x00200000;
        static public uint S4_ERROR_UNKNOWN = 0xffffffff;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SENSE4_CONTEXT
        {
            public int dwIndex;		//device index
            public int dwVersion;		//version		
            public int hLock;			//device handle
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] reserve;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
            public byte[] bAtr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] bID;
            public uint dwAtrLen;
        }

        //Assume that Sense4user.dll in , if not, modify the lines below
        [DllImport(@"Sense4.dll")]
        private static extern uint S4Enum([MarshalAs(UnmanagedType.LPArray), Out] SENSE4_CONTEXT[] s4_context, ref uint size);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4Open(ref SENSE4_CONTEXT s4_context);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4Close(ref SENSE4_CONTEXT s4_context);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4Control(ref SENSE4_CONTEXT s4Ctx, uint ctlCode, byte[] inBuff,
            uint inBuffLen, byte[] outBuff, uint outBuffLen, ref uint BytesReturned);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4CreateDir(ref SENSE4_CONTEXT s4Ctx, string DirID, uint DirSize, uint Flags);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4ChangeDir(ref SENSE4_CONTEXT s4Ctx, string Path);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4EraseDir(ref SENSE4_CONTEXT s4Ctx, string DirID);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4VerifyPin(ref SENSE4_CONTEXT s4Ctx, byte[] Pin, uint PinLen, uint PinType);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4ChangePin(ref SENSE4_CONTEXT s4Ctx, byte[] OldPin, uint OldPinLen,
            byte[] NewPin, uint NewPinLen, uint PinType);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4WriteFile(ref SENSE4_CONTEXT s4Ctx, string FileID, uint Offset,
            byte[] Buffer, uint BufferSize, uint FileSize, ref uint BytesWritten, uint Flags,
            uint FileType);
        [DllImport(@"Sense4.dll")]
        private static extern uint S4Execute(ref SENSE4_CONTEXT s4Ctx, string FileID, byte[] InBuffer,
            uint InbufferSize, byte[] OutBuffer, uint OutBufferSize, ref uint BytesReturned);

        public SenseLock(int index)
        {
            Initialize(index);
        }
        public bool IsUserVerified { set; get; }

        public bool IsOpened { get; set; }

        public bool VerifyUserPin(string pin) 
        {
            return VerifyPin(pin, S4_USER_PIN);
        }

        public bool VerifyDevPin(string pin) 
        {
            return VerifyPin(pin, S4_DEV_PIN);
        }

        bool VerifyPin(string pin,uint pinType)
        {
            uint ret = S4ChangeDir(ref si[_index], "\\");
            var pinBytes = Encoding.ASCII.GetBytes(pin);
            return 0 == S4VerifyPin(ref si[_index], pinBytes, (uint)pinBytes.Length, pinType);
        }

        public string GetKey()
        {
            uint BytesReturn = 0;   
            byte[] outBuffer = new byte[32];
            if (0 == S4Execute(ref si[_index], "581a", null, 0, outBuffer, 32, ref BytesReturn))
            {
                return Encoding.ASCII.GetString(outBuffer);
            }
            else
                return string.Empty;
        }

        public DateTime GetTime()
        {
            uint BytesReturn = 0;
            byte[] outBuffer = new byte[32];
            if (0 == S4Execute(ref si[_index], "3456", null, 0, outBuffer, 32, ref BytesReturn))
            {
                return DateTime.Parse( Encoding.ASCII.GetString(outBuffer));
            }
            else
                return DateTime.MinValue;
        }
        public static int Count { get; set; }
        int _index;
        static SENSE4_CONTEXT[] si;
        public static IEnumerable<SenseLock> GetAll() 
        {
            for (int index = 0; index < Count; index++)
                yield return new SenseLock(index);
        }
        static SenseLock() 
        {
            uint size = 0;
            uint ret = S4Enum(null, ref size);
            Debug.Assert(0x11 == ret);
            si = new SENSE4_CONTEXT[size / Marshal.SizeOf(typeof(SENSE4_CONTEXT))];
            ret = S4Enum(si, ref size);
            Debug.Assert(0x00 == ret);
            Count = si.Length;
        }
        private void Initialize(int index)
        {
            _index = index;
        }

        public void Open()
        {
            uint ret = S4Open(ref si[_index]);
            IsOpened = 0x00 == ret;
            Debug.Assert(0x00 == ret);
        }

        public void Close()
        {
            uint ret = S4Close(ref si[_index]);
            if (IsOpened)
            {
                IsOpened = 0x00 == ret;
            }
            Debug.Assert(0x00 == ret);

        }

        public void Dispose()
        {
            Close();
        }

        public bool ChangeDevPin(string oldPin, string newPin) 
        {
            return ChangePin(oldPin, newPin, S4_DEV_PIN);
        }

        public bool ChangeUserPin(string oldPin, string newPin) 
        {
            return ChangePin(oldPin, newPin, S4_USER_PIN);
        }
        bool ChangePin(string oldPin, string newPin,uint pinType) 
        {
            var oldPinBytes = Encoding.ASCII.GetBytes(oldPin);
            var newPinBytes = Encoding.ASCII.GetBytes(newPin);
            return 0 == S4ChangePin(ref si[_index], oldPinBytes, (uint)oldPinBytes.Length, newPinBytes, (uint)newPinBytes.Length, pinType);
        }
        public bool DownloadFile(string path, string filename,bool execute) 
        {
            byte[] inBuffer = new byte[4096];

            try
            {
                //Assume that demov20.bin in c:\, if not, modify the line below
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    BinaryReader sr = new BinaryReader(fs);
                    sr.Read(inBuffer, 0, (int)fs.Length);
                    uint BytesWritten = 0;
                    return 0 == S4WriteFile(ref si[_index], filename, 0, inBuffer, (uint)fs.Length, (uint)fs.Length, ref BytesWritten, S4_CREATE_NEW, execute? S4_EXE_FILE:S4_DATA_FILE);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in ReadFile: {0}", e);
                return false;
            }
        }
        public bool ChangeDirectory(string dir) 
        {
            return 0 == S4ChangeDir(ref si[_index], dir);
        }
        public bool EraserDirectory(string dir) 
        {
            return 0==S4EraseDir(ref si[_index], dir);
        }
        public bool CreateDirectory(string dir) 
        {
            return 0 == S4CreateDir(ref si[_index], dir, 0, S4_CREATE_ROOT_DIR);
        }

        //SENSE4_CONTEXT[] si;



    }
}
