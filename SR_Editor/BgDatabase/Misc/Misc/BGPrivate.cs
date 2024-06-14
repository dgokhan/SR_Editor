/*
<copyright file="BGPrivate.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Reflection;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// access private fields, methods and properties
    /// </summary>
    public static partial class BGPrivate
    {
        //================================================================================================
        //                                              Fields
        //================================================================================================
        /// <summary>
        /// Return objects field value by its name
        /// </summary>
        public static T GetField<T>(object obj, string name)
        {
            return (T)GetField(obj, name).GetValue(obj);
        }

        /// <summary>
        /// set objects field value by its name
        /// </summary>
        public static void SetField<T>(object obj, string name, T value)
        {
            GetField(obj, name).SetValue(obj, value);
        }

        /// <summary>
        /// Get field info by its name
        /// </summary>
        public static FieldInfo GetField(object obj, string name)
        {
            var isStatic = obj is Type;
            var type = isStatic ? (Type)obj : obj.GetType();

            return GetField(type, name, isStatic);
        }

        /// <summary>
        /// Get field info by its name
        /// </summary>
        public static FieldInfo GetField(Type type, string name, bool isStatic, bool includeBaseTypes = true)
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (isStatic) bindingFlags |= BindingFlags.Static;

            var targetField = type.GetField(name, bindingFlags);

            if (targetField != null) return targetField;

            if (!includeBaseTypes) return null;

            var basetype = type.BaseType;
            var typeOfObject = typeof(object);
            while (targetField == null && basetype != null && basetype != typeOfObject)
            {
                targetField = basetype.GetField(name, bindingFlags);
                basetype = basetype.BaseType;
            }

            return targetField;
        }

        //================================================================================================
        //                                              Properties
        //================================================================================================
        /// <summary>
        /// Get property value by its name/type
        /// </summary>
        public static T GetProperty<T>(object obj, string name)
        {
            return (T)GetProperty(obj, name).GetValue(obj, null);
        }

        /// <summary>
        /// set property value by its name
        /// </summary>
        public static void SetProperty<T>(object obj, string name, T value)
        {
            GetProperty(obj, name).SetValue(obj, value, null);
        }

        /// <summary>
        /// Get property info by its name
        /// </summary>
        public static PropertyInfo GetProperty(object obj, string name)
        {
            var isStatic = obj is Type;
            var type = isStatic ? (Type)obj : obj.GetType();

            return GetProperty(type, name, isStatic);
        }

        /// <summary>
        /// Get property info by its name
        /// </summary>
        public static PropertyInfo GetProperty(Type type, string name, bool isStatic, bool includeBaseTypes = true)
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (isStatic) bindingFlags |= BindingFlags.Static;

            var targetField = type.GetProperty(name, bindingFlags);

            if (targetField != null) return targetField;
            if (!includeBaseTypes) return null;

            var basetype = type.BaseType;
            while (targetField == null && basetype != null && basetype != typeof(object))
            {
                targetField = basetype.GetProperty(name, bindingFlags);
                basetype = basetype.BaseType;
            }

            return targetField;
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// Invoke method by its name
        /// </summary>
        public static object Invoke(object obj, string methodName, params object[] parameters)
        {
            return GetMethod(obj, methodName).Invoke(obj, parameters);
        }

        /// <summary>
        /// Invoke method by its name
        /// </summary>
        public static object Invoke(object obj, string methodName, Type[] types, params object[] parameters)
        {
            return GetMethod(obj, methodName, types).Invoke(obj, parameters);
        }

        /// <summary>
        /// Get method info by its name
        /// </summary>
        public static MethodInfo GetMethod(object obj, string name, Type[] types = null)
        {
            var isStatic = obj is Type;
            var type = isStatic ? (Type)obj : obj.GetType();

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            if (isStatic) bindingFlags |= BindingFlags.Static;

            var targetField = types == null ? type.GetMethod(name, bindingFlags) : type.GetMethod(name, bindingFlags, null, types, null);

            if (targetField != null) return targetField;


            var basetype = type.BaseType;
            while (targetField == null && basetype != null && basetype != typeof(object))
            {
                targetField = types == null ? basetype.GetMethod(name, bindingFlags) : basetype.GetMethod(name, bindingFlags, null, types, null);
                basetype = basetype.BaseType;
            }

            return targetField;
        }
    }
}