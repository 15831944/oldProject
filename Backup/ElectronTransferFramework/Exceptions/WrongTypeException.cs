﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferFramework.Exceptions
{
    [global::System.Serializable]
    public class WrongTypeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public WrongTypeException() { }
        public WrongTypeException(string message) : base(message) { }
        public WrongTypeException(string message, Exception inner) : base(message, inner) { }
        protected WrongTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
