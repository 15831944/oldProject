using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Spring.Expressions;

namespace ElectronTransferFramework
{
    public class TypeUtility
    {
        public delegate object MemberGetDelegate(object obj);

        public static MemberGetDelegate
            GetMemberGetDelegate(Type type, string memberName)
        {
            Type objectType = type;

            PropertyInfo pi = objectType.GetProperty(memberName);
            FieldInfo fi = objectType.GetField(memberName);
            if (pi != null)
            {
                // Member is a Property...

                MethodInfo mi = pi.GetGetMethod();
                if (mi != null)
                {
                    // NOTE:  As reader J. Dunlap pointed out...
                    //  Delegate.CreateDelegate is faster/cleaner than Reflection.Emit
                    //  for calling a property's get accessor.
                    
                    return (MemberGetDelegate)
                        Delegate.CreateDelegate(typeof(MemberGetDelegate), mi);
                }
                else
                    throw new Exception(String.Format(
                        "Property: '{0}' of Type: '{1}' does not have a Public Get accessor",
                        memberName, objectType.Name));
            }
            else if (fi != null)
            {
                // Member is a Field...

                DynamicMethod dm = new DynamicMethod("Get" + memberName,
                    objectType, new Type[] { objectType }, objectType);
                ILGenerator il = dm.GetILGenerator();
                // Load the instance of the object (argument 0) onto the stack
                il.Emit(OpCodes.Ldarg_0);
                // Load the value of the object's field (fi) onto the stack
                il.Emit(OpCodes.Ldfld, fi);
                // return the value on the top of the stack
                il.Emit(OpCodes.Ret);

                return (MemberGetDelegate)
                    dm.CreateDelegate(typeof(MemberGetDelegate));
            }
            else
                throw new Exception(String.Format(
                    "Member: '{0}' is not a Public Property or Field of Type: '{1}'",
                    memberName, objectType.Name));
        }


        private static Dictionary<string, Delegate> _memberGetDelegates = new Dictionary<string, Delegate>();

        public static MemberGetDelegate
            GetCachedMemberGetDelegate(Type type, string memberName)
        {
            if (_memberGetDelegates.ContainsKey(memberName))
                return (MemberGetDelegate)_memberGetDelegates[memberName];

            MemberGetDelegate returnValue = GetMemberGetDelegate( type, memberName);
            lock (_memberGetDelegates)
            {
                _memberGetDelegates[memberName] = returnValue;
            }
            return returnValue;
        }

    }
}
