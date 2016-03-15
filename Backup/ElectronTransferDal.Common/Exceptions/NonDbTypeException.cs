using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferDal.Common.Exceptions
{
    [global::System.Serializable]
    public class NonDbTypeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        readonly Type _type;
        public NonDbTypeException() { }
        public NonDbTypeException(Type type)
        {
            _type = type;
        }

        public Type Type
        {
            get { return _type; }
        }

        public override string ToString()
        {
            return string.Format("{0}不是DBEntity", this.Type.Name);
        }
    }
}
