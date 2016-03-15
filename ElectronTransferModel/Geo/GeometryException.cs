using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel.Geo
{
    [global::System.Serializable]
    public class GeometryException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GeometryException() { }
        //public GeometryException(string message) : base(message) { }
        public GeometryException(string text, Exception inner) : base("文本格式不正确:"+text, inner) 
        {
            
        }
        protected GeometryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
