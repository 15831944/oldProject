using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;


namespace ElectronTransferFramework
{
    public delegate object FastInvokeHandler(object target, object[] paramters);
    public static class EmitUtils
    {
        class PropInfo
        {
            public string Name { get; set; }
            public MethodInfo Getter { get; set; }
            public MethodInfo Setter { get; set; }
            public Type Type { get; set; }
        }

        
        internal static FastInvokeHandler GetMethodInvoker(this MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(il, methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
        }

        private static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }

        static List<PropInfo> GetPublicProps(Type t)
        {
            return t
                    .GetProperties()
                    .Select(p => new PropInfo
                    {
                        Name = p.Name,
                        Getter = p.DeclaringType == t ? p.GetGetMethod(true) : p.DeclaringType.GetProperty(p.Name).GetGetMethod(true),
                        Setter = p.DeclaringType == t ? p.GetSetMethod(true) : p.DeclaringType.GetProperty(p.Name).GetSetMethod(true),
                        Type = p.PropertyType
                    })
                    .Where(info => info.Getter != null && info.Setter != null)
                    .ToList();
        }
        static List<FieldInfo> GetFields(Type type) 
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        }
        public static Func<object, object> CreateCloneMethod(Type type) 
        {
            var fields = GetFields(type);
            var method = new DynamicMethod(string.Format("Clone{0}", Guid.NewGuid()), type, new[] { typeof(object) }, true);
            var il = method.GetILGenerator();
            il.DeclareLocal(type);
            il.DeclareLocal(type);
            il.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Stloc_1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Stloc_0);
            foreach (var field in fields)
            {
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldfld, field);
                il.Emit(OpCodes.Stfld, field);
            }
            il.Emit(OpCodes.Ret);
 
            return (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
        }
    }
    public static class ObjectCopy
    {
        struct Identity
        {
            int _hashcode;
            RuntimeTypeHandle _type;

            public Identity(int hashcode, RuntimeTypeHandle type)
            {
                _hashcode = hashcode;
                _type = type;
            }
        }
        //缓存对象复制的方法。
        static Dictionary<Type, Func<object, Dictionary<Identity, object>, object>> methods1 = new Dictionary<Type, Func<object, Dictionary<Identity, object>, object>>();
        static Dictionary<Type, Action<object, Dictionary<Identity, object>, object>> methods2 = new Dictionary<Type, Action<object, Dictionary<Identity, object>, object>>();
        static Dictionary<Type, Action<object, Dictionary<Identity, object>, object>> methods3 = new Dictionary<Type, Action<object, Dictionary<Identity, object>, object>>();

        static List<FieldInfo> GetSettableFields(Type t)
        {
            return t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        }

        class PropInfo
        {
            public string Name { get; set; }
            public MethodInfo Getter { get; set; }
            public MethodInfo Setter { get; set; }
            public Type Type { get; set; }
        }

        static List<PropInfo> GetPublicProps(Type t)
        {
            return t
                    .GetProperties()
                    .Select(p => new PropInfo
                    {
                        Name = p.Name,
                        Getter = p.DeclaringType == t ? p.GetGetMethod(true) : p.DeclaringType.GetProperty(p.Name).GetGetMethod(true),
                        Setter = p.DeclaringType == t ? p.GetSetMethod(true) : p.DeclaringType.GetProperty(p.Name).GetSetMethod(true),
                        Type = p.PropertyType
                    })
                    .Where(info => info.Getter != null && info.Setter != null)
                    .ToList();
        }

        static Func<object, Dictionary<Identity, object>, object> CreateCloneMethod1(Type type, Dictionary<Identity, object> objects)
        {
            Type tmptype;
            var fields = GetSettableFields(type);
            var dm = new DynamicMethod(string.Format("Clone{0}", Guid.NewGuid()), typeof(object), new[] { typeof(object), typeof(Dictionary<Identity, object>) }, true);
            var il = dm.GetILGenerator();
            il.DeclareLocal(type);
            il.DeclareLocal(type);
            il.DeclareLocal(typeof(Identity));
            if (!type.IsArray)
            {
                il.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc_1);
                il.Emit(OpCodes.Ldloca_S, 2);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("GetHashCode"));
                il.Emit(OpCodes.Ldtoken, type);
                il.Emit(OpCodes.Call, typeof(Identity).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int), typeof(RuntimeTypeHandle) }, null));
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldloc_2);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Callvirt, typeof(Dictionary<Identity, object>).GetMethod("Add"));
                foreach (var field in fields)
                {
                    if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
                    {
                        //不符合条件的字段，直接忽略，避免报错。
                        if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
                            (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
                            break;
                        il.Emit(OpCodes.Ldloc_1);
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Ldfld, field);
                        il.Emit(OpCodes.Ldarg_1);
                        il.EmitCall(OpCodes.Call, typeof(ObjectCopy).GetMethod("CopyImpl1", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
                        il.Emit(OpCodes.Stfld, field);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldloc_1);
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Ldfld, field);
                        il.Emit(OpCodes.Stfld, field);
                    }
                }
                for (type = type.BaseType; type != null && type != typeof(object); type = type.BaseType)
                {
                    //只需要查找基类的私有成员，共有或受保护的在派生类中直接被复制过了。
                    fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
                    foreach (var field in fields)
                    {
                        if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
                        {
                            //不符合条件的字段，直接忽略，避免报错。
                            if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
                                (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
                                break;
                            il.Emit(OpCodes.Ldloc_1);
                            il.Emit(OpCodes.Ldloc_0);
                            il.Emit(OpCodes.Ldfld, field);
                            il.Emit(OpCodes.Ldarg_1);
                            il.EmitCall(OpCodes.Call, typeof(ObjectCopy).GetMethod("CopyImpl1", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
                            il.Emit(OpCodes.Stfld, field);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldloc_1);
                            il.Emit(OpCodes.Ldloc_0);
                            il.Emit(OpCodes.Ldfld, field);
                            il.Emit(OpCodes.Stfld, field);
                        }
                    }
                }
            }
            else
            {
                Type arraytype = type.GetElementType();
                var i = il.DeclareLocal(typeof(int));
                var lb1 = il.DefineLabel();
                var lb2 = il.DefineLabel();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldlen);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Stloc, i);
                il.Emit(OpCodes.Newarr, arraytype);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc_1);
                il.Emit(OpCodes.Ldloca_S, 2);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("GetHashCode"));
                il.Emit(OpCodes.Ldtoken, type);
                il.Emit(OpCodes.Call, typeof(Identity).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int), typeof(RuntimeTypeHandle) }, null));
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldloc_2);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Callvirt, typeof(Dictionary<Identity, object>).GetMethod("Add"));
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Br, lb1);
                il.MarkLabel(lb2);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Ldelem, arraytype);
                if (!arraytype.IsValueType && arraytype != typeof(String))
                {
                    il.EmitCall(OpCodes.Call, typeof(ObjectCopy).GetMethod("CopyImpl1", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(arraytype), null);
                }
                il.Emit(OpCodes.Stelem, arraytype);
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, i);
                il.MarkLabel(lb1);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Clt);
                il.Emit(OpCodes.Brfalse, lb2);
            }
            il.Emit(OpCodes.Ret);

            return (Func<object, Dictionary<Identity, object>, object>)dm.CreateDelegate(typeof(Func<object, Dictionary<Identity, object>, object>));
        }

        static Action<object, Dictionary<Identity, object>, object> CreateCloneMethod2(Type type, Dictionary<Identity, object> objects)
        {
            Type tmptype;
            var fields = GetSettableFields(type);
            var dm = new DynamicMethod(string.Format("Copy{0}", Guid.NewGuid()), null, new[] { typeof(object), typeof(Dictionary<Identity, object>), typeof(object) }, true);
            var il = dm.GetILGenerator();
            il.DeclareLocal(type);
            il.DeclareLocal(type);
            il.DeclareLocal(typeof(Identity));
            if (!type.IsArray)
            {
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Stloc_1);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Stloc_0);
                foreach (var field in fields)
                {
                    if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
                    {
                        //不符合条件的字段，直接忽略，避免报错。
                        if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
                            (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
                            break;
                        il.Emit(OpCodes.Ldloc_1);
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Ldfld, field);
                        il.Emit(OpCodes.Ldarg_1);
                        il.EmitCall(OpCodes.Call, typeof(ObjectCopy).GetMethod("CopyImpl1", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
                        il.Emit(OpCodes.Stfld, field);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldloc_1);
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Ldfld, field);
                        il.Emit(OpCodes.Stfld, field);
                    }
                }
                for (type = type.BaseType; type != null && type != typeof(object); type = type.BaseType)
                {
                    //只需要查找基类的私有成员，共有或受保护的在派生类中直接被复制过了。
                    fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
                    foreach (var field in fields)
                    {
                        if (!field.FieldType.IsValueType && field.FieldType != typeof(String))
                        {
                            //不符合条件的字段，直接忽略，避免报错。
                            if ((field.FieldType.IsArray && (field.FieldType.GetArrayRank() > 1 || (!(tmptype = field.FieldType.GetElementType()).IsValueType && tmptype != typeof(String) && tmptype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))) ||
                                (!field.FieldType.IsArray && field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) == null))
                                break;
                            il.Emit(OpCodes.Ldloc_1);
                            il.Emit(OpCodes.Ldloc_0);
                            il.Emit(OpCodes.Ldfld, field);
                            il.Emit(OpCodes.Ldarg_1);
                            il.EmitCall(OpCodes.Call, typeof(ObjectCopy).GetMethod("CopyImpl1", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(field.FieldType), null);
                            il.Emit(OpCodes.Stfld, field);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldloc_1);
                            il.Emit(OpCodes.Ldloc_0);
                            il.Emit(OpCodes.Ldfld, field);
                            il.Emit(OpCodes.Stfld, field);
                        }
                    }
                }
            }
            else
            {
                Type arraytype = type.GetElementType();
                var i = il.DeclareLocal(typeof(int));
                var lb1 = il.DefineLabel();
                var lb2 = il.DefineLabel();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldlen);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Stloc, i);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc_1);
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Br, lb1);
                il.MarkLabel(lb2);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Ldelem, arraytype);
                if (!arraytype.IsValueType && arraytype != typeof(String))
                {
                    il.EmitCall(OpCodes.Call, typeof(ObjectCopy).GetMethod("CopyImpl1", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(arraytype), null);
                }
                il.Emit(OpCodes.Stelem, arraytype);
                il.Emit(OpCodes.Ldloc, i);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Sub);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, i);
                il.MarkLabel(lb1);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Clt);
                il.Emit(OpCodes.Brfalse, lb2);
            }
            il.Emit(OpCodes.Ret);

            return (Action<object, Dictionary<Identity, object>, object>)dm.CreateDelegate(typeof(Action<object, Dictionary<Identity, object>, object>));
        }

        /// <summary>
        /// 对属性进行复制，不查找父类，用于当前实体对象的属性拷贝。
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="objects">复制链</param>
        /// <returns></returns>
        static Action<object, Dictionary<Identity, object>, object> CreateCloneMethod3(Type type, Dictionary<Identity, object> objects)
        {
            var props = GetPublicProps(type);
            var dm = new DynamicMethod(string.Format("Copy{0}", Guid.NewGuid()), null, new[] { typeof(object), typeof(Dictionary<Identity, object>), typeof(object) }, true);
            var il = dm.GetILGenerator();
            il.DeclareLocal(type);//存放源对象
            il.DeclareLocal(type);//存放目标对象
            il.DeclareLocal(typeof(Identity));//存放标识
            if (!type.IsArray)
            {
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Stloc_1);//将参数中获取的目标对象存放到局部变量1中
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, type);
                il.Emit(OpCodes.Stloc_0);//将参数中获取的源对象存放到局部变量0中
                foreach (var prop in props)
                {
                    if (!prop.Type.IsValueType && prop.Type != typeof(String))
                    {
                        //不符合条件的属性，直接忽略。
                        if (prop.Type.IsArray)
                            break;
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Call, prop.Getter);//获取局部变量0中暂存的源对象的属性
                        il.Emit(OpCodes.Ldarg_1);//加载参数中的复制链对象
                        il.Emit(OpCodes.Ldloc_1);
                        il.Emit(OpCodes.Call, prop.Getter);//获取局部变量1中暂存的目标对象的属性
                        il.EmitCall(OpCodes.Call, typeof(ObjectCopy).GetMethod("CopyImpl2", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(prop.Type), null);

                    }
                    else
                    {
                        il.Emit(OpCodes.Ldloc_1);
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Call, prop.Getter);
                        il.Emit(OpCodes.Call, prop.Setter);
                    }
                }
            }
            il.Emit(OpCodes.Ret);

            return (Action<object, Dictionary<Identity, object>, object>)dm.CreateDelegate(typeof(Action<object, Dictionary<Identity, object>, object>));
        }

        static T CopyImpl1<T>(T source, Dictionary<Identity, object> objects) where T : class
        {
            //为空则直接返回null
            if (source == null)
                return null;

            Type type = source.GetType();
            Identity id = new Identity(source.GetHashCode(), type.TypeHandle);
            object result;
            //如果发现曾经复制过，用之前的，从而停止递归复制。
            if (!objects.TryGetValue(id, out result))
            {
                //最后查找对象的复制方法，如果不存在，创建新的。
                Func<object, Dictionary<Identity, object>, object> method;
                if (!methods1.TryGetValue(type, out method))
                {
                    method = CreateCloneMethod1(type, objects);
                    methods1.Add(type, method);
                }
                result = method(source, objects);
            }
            return (T)result;
        }

        static void CopyImpl2<T>(T source, T target, Dictionary<Identity, object> objects) where T : class
        {
            Type type = source.GetType();
            Identity id = new Identity(source.GetHashCode(), type.TypeHandle);
            object result;
            //如果发现曾经复制过，用之前的，从而停止递归复制。
            if (!objects.TryGetValue(id, out result))
            {
                objects.Add(new Identity(source.GetHashCode(), type.TypeHandle), source);
                //最后查找对象的复制方法，如果不存在，创建新的。
                Action<object, Dictionary<Identity, object>, object> method;
                if (!methods3.TryGetValue(type, out method))
                {
                    method = CreateCloneMethod3(type, objects);
                    methods3.Add(type, method);
                }
                method(source, objects, target);
            }
        }


        /// <summary>
        /// 创建对象深度复制的副本
        /// </summary>
        public static T ToObjectCopy<T>(this T source) where T : class
        {
            Type type = source.GetType();
            Dictionary<Identity, object> objects = new Dictionary<Identity, object>();//存放内嵌引用类型的复制链，避免构成一个环。
            Func<object, Dictionary<Identity, object>, object> method;
            if (!methods1.TryGetValue(type, out method))
            {
                method = CreateCloneMethod1(type, objects);
                methods1.Add(type, method);
            }
            return (T)method(source, objects);
        }


        /// <summary>
        /// 将source对象的所有属性复制到target对象中，深度复制
        /// </summary>
        public static void ObjectCopyTo<T>(this T source, T target) where T : class
        {
            if (target == null)
                throw new Exception("将要复制的目标未初始化");
            Type type = source.GetType();
            if (type != target.GetType())
                throw new Exception("要复制的对象类型不同，无法复制");
            Dictionary<Identity, object> objects = new Dictionary<Identity, object>();//存放内嵌引用类型的复制链，避免构成一个环。
            objects.Add(new Identity(source.GetHashCode(), type.TypeHandle), source);
            Action<object, Dictionary<Identity, object>, object> method;
            if (!methods2.TryGetValue(type, out method))
            {
                method = CreateCloneMethod2(type, objects);
                methods2.Add(type, method);
            }
            method(source, objects, target);
        }

        /// <summary>
        /// 将source对象的所有属性复制到target对象中，仅对公有属性复制，不查找父类，不创建引用属性，但复制引用属性内部的属性。
        /// </summary>
        public static void PropCopyTo<T>(this T source, T target) where T : class
        {
            if (target == null)
                throw new Exception("将要复制的目标未初始化");
            Type type = source.GetType();
            Dictionary<Identity, object> objects = new Dictionary<Identity, object>();//存放内嵌引用类型的复制链，避免构成一个环。
            objects.Add(new Identity(source.GetHashCode(), type.TypeHandle), source);
            Action<object, Dictionary<Identity, object>, object> method;
            if (!methods3.TryGetValue(type, out method))
            {
                method = CreateCloneMethod3(type, objects);
                methods3.Add(type, method);
            }
            method(source, objects, target);
        }
    }
}
