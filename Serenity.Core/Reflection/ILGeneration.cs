﻿using System;
using System.Reflection;
#if !COREFX
using System.Reflection.Emit;
#endif

namespace Serenity.Reflection
{
    /// <summary>
    /// IL Generation helpers
    /// </summary>
    public static class ILGeneration
    {
        /// <summary>
        /// Generates the field getter.
        /// </summary>
        /// <param name="fieldInfo">The field information.</param>
        /// <returns></returns>
        public static Func<object, object> GenerateGetter(FieldInfo fieldInfo)
        {
#if COREFX
            return x => fieldInfo.GetValue(x);
#else
            // create a method without a name, object as result type and one parameter of type object
            // the last parameter is very import for accessing private fields
            var method = new DynamicMethod(string.Empty, typeof(object), new[] { typeof(object) }, fieldInfo.DeclaringType, true);
            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // load the first argument onto the stack (source of type object)
            il.Emit(OpCodes.Castclass, fieldInfo.DeclaringType); // cast the parameter of type object to the type containing the field
            il.Emit(OpCodes.Ldfld, fieldInfo); // store the value of the given field on the stack. The casted version of source is used as instance

            if (fieldInfo.FieldType.IsValueType)
                il.Emit(OpCodes.Box, fieldInfo.FieldType); // box the value type, so you will have an object on the stack

            il.Emit(OpCodes.Ret); // return the value on the stack

            return (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
#endif
        }

        /// <summary>
        /// Generates the field setter.
        /// </summary>
        /// <param name="fieldInfo">The field information.</param>
        /// <returns></returns>
        public static Action<object, object> GenerateSetter(FieldInfo fieldInfo)
        {
#if COREFX
            return (x, v) => fieldInfo.SetValue(x, v);
#else
            var method = new DynamicMethod(string.Empty, null, new[] { typeof(object), typeof(object) }, fieldInfo.DeclaringType, true);
            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0); // load the first argument onto the stack (source of type object)
            il.Emit(OpCodes.Castclass, fieldInfo.DeclaringType); // cast the parameter of type object to the type containing the field
            il.Emit(OpCodes.Ldarg_1); // push the second argument onto the stack (this is the value)

            if (fieldInfo.FieldType.IsValueType)
                il.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType); // unbox the value parameter to the value-type
            else
                il.Emit(OpCodes.Castclass, fieldInfo.FieldType); // cast the value on the stack to the field type

            il.Emit(OpCodes.Stfld, fieldInfo); // store the value on stack in the field
            il.Emit(OpCodes.Ret); // return

            return (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
#endif
        }
    }
}