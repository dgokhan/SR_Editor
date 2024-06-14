/*
<copyright file="BGUtil.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
#if !BG_SA
using Debug = UnityEngine.Debug;
#endif


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Misc utilities
    /// </summary>
    public static partial class BGUtil
    {
        public const string NoDatabaseFoundError = "Can not load database from all possible locations. More info: http://www.bansheegz.com/BGDatabase/Setup/";

        public static bool IsAboutToStartInEditor;

        //this is to indicate tests are running
        public static bool TestIsRunning { get; private set; }

        //================================================================================================
        //                                              Object Factory
        //================================================================================================
        /// <summary>
        /// Creates an object using provided type name
        /// </summary>
        public static T Create<T>(string typeName, bool includePrivateConstructors, params object[] parameters)
        {
            var type = GetType(typeName);

            if (type != null) return Create<T>(type, includePrivateConstructors, parameters);

            if (string.IsNullOrEmpty(typeName)) throw new BGException("Type name is not defined");
            throw new BGException("Can not find type ($)", typeName);
        }

        /// <summary>
        /// Creates an object using provided type
        /// </summary>
        public static T Create<T>(Type type, bool includePrivateConstructors, params object[] parameters)
        {
            if (!includePrivateConstructors) return (T)Activator.CreateInstance(type, parameters);

            //private & public 
            var types = parameters == null ? Type.EmptyTypes : new Type[parameters.Length];
            if (parameters != null)
                for (var i = 0; i < parameters.Length; i++)
                    types[i] = parameters[i].GetType();
            var constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, types, null);
            return (T)constructor.Invoke(parameters);
        }

        /// <summary>
        /// Get a type by its name
        /// </summary>
        public static Type GetType(string typeName)
        {
            return GetType(typeName, null);
        }

        /// <summary>
        /// Get a type by its name 
        /// </summary>
        public static Type GetType(string typeName, bool publicOnly)
        {
            return GetType(typeName, publicOnly ? t => t.IsPublic || t.IsNestedPublic : (Predicate<Type>)null);
        }

        /// <summary>
        /// Get a type by its name and filter
        /// </summary>
        public static Type GetType(string typeName, Predicate<Type> filter)
        {
            var type = Type.GetType(typeName);
            if (type != null && filter != null && !filter(type)) type = null;
            if (type == null)
            {
                if (string.IsNullOrEmpty(typeName)) throw new BGException("Type name is not defined");

                var commaIndex = typeName.IndexOf(',', 0, typeName.Length);
                if (commaIndex >= 0)
                    //it could be AssemblyQualifiedName, if it's- we'll search in all assemblies for full type name
                    TryToExtractFullTypeName(ref typeName);

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (var i = 0; i < assemblies.Length; i++)
                {
                    try
                    {
                        type = assemblies[i].GetType(typeName, false, false);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    if (type != null && filter != null && !filter(type)) type = null;
                    if (type != null) break;
                }
            }

            return type;
        }

        //try to extract type name from full-qualified type name
        private static void TryToExtractFullTypeName(ref string typeNameCandidate)
        {
            var index = -1;
            var bracketLevel = 0;
            for (var i = 0; i < typeNameCandidate.Length; i++)
                switch (typeNameCandidate[i])
                {
                    case '[':
                    {
                        bracketLevel++;
                        break;
                    }

                    case ']':
                    {
                        bracketLevel--;
                        break;
                    }

                    case ',':
                    {
                        if (bracketLevel == 0)
                        {
                            index = i;
                            goto LetMeOut;
                        }

                        break;
                    }
                }

            LetMeOut:
            if (index < 0) return;

            typeNameCandidate = typeNameCandidate.Substring(0, index).Trim();
        }


        //================================================================================================
        //                                              Collections
        //================================================================================================
        /// <summary>
        /// is collection empty
        /// </summary>
        public static bool IsEmpty<T>(ICollection<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// is list empty
        /// </summary>
        public static bool IsEmpty<T>(T[] list)
        {
            return list == null || list.Length == 0;
        }

        /// <summary>
        /// make sure the list is exists/added to the dictionary
        /// </summary>
        public static List<TV> EnsureList<TK, TV>(IDictionary<TK, List<TV>> key2Value, TK key)
        {
            //do not use Ensure for performance and GC reasons
            key2Value.TryGetValue(key, out var result);
            if (result != null) return result;

            result = new List<TV>();
            key2Value[key] = result;
            return result;
//            return Ensure(key2Value, key, () => new List<TV>());
        }

        /// <summary>
        /// make sure the key is exists/added to the dictionary
        /// </summary>
        public static TV Ensure<TK, TV>(Dictionary<TK, TV> key2Value, TK key, Func<TV> newValue) where TV : class
        {
            key2Value.TryGetValue(key, out var result);
            if (result != null) return result;

            result = newValue();
            key2Value[key] = result;
            return result;
        }

        /// <summary>
        /// Get value from the dictionary
        /// </summary>
        public static TV Get<TK, TV>(IDictionary<TK, TV> key2Value, TK key)
        {
            key2Value.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Get value from the dictionary, return defaultValue if value not present 
        /// </summary>
        public static TV Get<TK, TV>(Dictionary<TK, TV> key2Value, TK key, TV defaultValue)
        {
            if (!key2Value.TryGetValue(key, out var value)) return defaultValue;
            return value;
        }


        /// <summary>
        /// Get the value from dictionary if it's not null
        /// </summary>
        public static TV GetNullable<TK, TV>(Dictionary<TK, TV> key2Value, TK key)
        {
            return key2Value == null ? default : Get(key2Value, key);
        }

        /// <summary>
        /// Iterate the list
        /// </summary>
        public static void ForEach<T>(List<T> list, Action<T> action)
        {
            if (list == null) return;
            for (var i = 0; i < list.Count; i++) action(list[i]);
        }

        /// <summary>
        /// If list values are equal
        /// </summary>
        public static bool ListsValuesEqual<T>(List<T> value1, List<T> value2)
        {
            if (value1 == null && value2 == null) return true;

            if (value1 == value2 || value1 == null || value2 == null) return false;

            if (value1.Count != value2.Count) return false;

            if (value1.Count == 0) return true;

            var comparer = EqualityComparer<T>.Default;
            var equal = true;
            for (var i = 0; i < value1.Count; i++)
            {
                var val = value1[i];
                if (comparer.Equals(value2[i], val)) continue;
                equal = false;
                break;
            }

            return equal;
        }

        /// <summary>
        /// If array values are equal
        /// </summary>
        public static bool ArraysValuesEqual<T>(T[] value1, T[] value2)
        {
            if (value1 == null && value2 == null) return true;

            if (value1 == value2 || value1 == null || value2 == null) return false;

            if (value1.Length != value2.Length) return false;

            if (value1.Length == 0) return true;

            var comparer = EqualityComparer<T>.Default;
            var equal = true;
            for (var i = 0; i < value1.Length; i++)
            {
                var val = value1[i];
                if (comparer.Equals(value2[i], val)) continue;
                equal = false;
                break;
            }

            return equal;
        }

        /// <summary>
        /// Is provided type a list?
        /// </summary>
        public static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        /// <summary>
        /// Convert ArraySegment to byte array
        /// </summary>
        public static byte[] ToArray(ArraySegment<byte> segment)
        {
            if (segment.Count == 0) return Array.Empty<byte>();
            var result = new byte[segment.Count];
            Buffer.BlockCopy(segment.Array, segment.Offset, result, 0, segment.Count);
            return result;
        }

        //================================================================================================
        //                                              Convert
        //================================================================================================
        /// <summary>
        /// Convert string value to int, exception if value can not be converted
        /// </summary>
        public static int ToInt(string value, int @default = 0, bool throwException = false)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                if (throwException) throw new BGException("Can not convert to int, value=$", value);
                return @default;
            }
        }


        /// <summary>
        /// Convert string to enum value
        /// </summary>
        public static T ToEnum<T>(string data) where T : struct
        {
            return (T)Enum.ToObject(typeof(T), ToInt(data));
        }

        //================================================================================================
        //                                              Format
        //================================================================================================
        /// <summary>
        /// Format message by replacing $ chars to provided arguments
        /// </summary>
        public static string Format(string message, params object[] args)
        {
            if (args == null || args.Length == 0) return message;
            try
            {
                var indexOf = message.IndexOf('$');
                if (indexOf == -1) return message;

                for (var i = 0; i < 100 && indexOf >= 0; i++)
                {
                    var toReplace = "{" + i + "}";
                    message = message.Substring(0, indexOf) + toReplace + message.Substring(indexOf + 1);
                    indexOf += toReplace.Length;
                    if (indexOf >= message.Length) indexOf = -1;
                    else indexOf = message.IndexOf('$', indexOf);
                }

                return string.Format(message, args);
            }
            catch (Exception)
            {
                return message;
            }
        }

        //================================================================================================
        //                                              Reflection
        //================================================================================================
        /// <summary>
        /// Get type's T attribute 
        /// </summary>
        public static T GetAttribute<T>(Type type, bool inherit = false) where T : Attribute
        {
            var attributeType = typeof(T);
            if (!type.IsDefined(attributeType, inherit)) return null;
            return (T)Attribute.GetCustomAttribute(type, attributeType);
        }

        /// <summary>
        /// Does provided type has T attribute 
        /// </summary>
        public static bool HasAttribute<T>(Type type, bool inherit)
        {
            return type.IsDefined(typeof(T), inherit);
        }

        /// <summary>
        /// Get all types, using predicate as a filter action 
        /// </summary>
        public static List<Type> GetTypes(Predicate<Type> filter = null)
        {
            var result = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (IsSystem(assembly)) continue;

                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    continue;
                }

                if (filter == null)
                    for (var i = 0; i < types.Length; i++)
                        result.Add(types[i]);
                else
                    for (var i = 0; i < types.Length; i++)
                    {
                        var type = types[i];
                        if (filter(type)) result.Add(type);
                    }
            }

            return result;
        }

        /// <summary>
        /// Is assembly is system? System means Unity assemblies
        /// </summary>
        private static bool IsSystem(Assembly assembly)
        {
            var assemblyFullName = assembly.FullName;
            if (assemblyFullName.StartsWith("Unity", StringComparison.Ordinal)
                || assemblyFullName.StartsWith("System", StringComparison.Ordinal)
                || assemblyFullName.StartsWith("Mono", StringComparison.Ordinal)
                || assemblyFullName.StartsWith("Accessibility", StringComparison.Ordinal)
                || assemblyFullName.StartsWith("mscorlib", StringComparison.Ordinal)
//                || assemblyFullName.StartsWith("Boo")
//                || assemblyFullName.StartsWith("ExCSS")
//                || assemblyFullName.StartsWith("I18N")
//                || assemblyFullName.StartsWith("nunit.framework")
//                || assemblyFullName.StartsWith("ICSharpCode.SharpZipLib")
            ) return true;

            return false;
        }

        /// <summary>
        /// Find all filtered subtypes of the provided targetType  
        /// </summary>
        public static List<Type> GetAllSubTypes(Type targetType, Predicate<Type> filter = null)
        {
            bool SubclassFilter(Type type) => type.IsClass && !type.IsAbstract && type.IsSubclassOf(targetType);
            return filter == null ? GetTypes(SubclassFilter) : GetTypes(type => SubclassFilter(type) && filter(type));
        }

        /// <summary>
        /// Clone the object
        /// </summary>
        public static T Clone<T>(T @object) where T : class
        {
            if (@object == null) return null;
            var method = @object.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null) return null;
            return (T)method.Invoke(@object, null);
        }


        /// <summary>
        /// Benchmark  action execution time
        /// </summary>
        public static long Measure(string operation, Action action, bool printResult = true)
        {
            var watch = Stopwatch.StartNew();

            action();
            watch.Stop();

            var result = watch.ElapsedMilliseconds;

            if (printResult) Debug.Log(operation + ": " + result);
            return result;
        }

        /*
    private static readonly List<long> measures = new List<long>();  
    public static long Measure(string operation, int times, Action action)
    {
        var watch = Stopwatch.StartNew();

        action();
        watch.Stop();

        var result = watch.ElapsedMilliseconds;

        if (measures.Count < times)
        {
            measures.Add(result);
        }
        else
        {
            var overall = measures.Average();
            Debug.Log(operation +'=' + overall);
            measures.Clear();
        }
        
        return result;
    }
    */


        /// <summary>
        /// Get all implementation for provided interface type
        /// </summary>
        public static List<Type> GetAllImplementations(Type interfaceType, Predicate<Type> filter = null)
        {
            Predicate<Type> subclassFilter = type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type);
            return filter == null ? GetTypes(subclassFilter) : GetTypes(type => subclassFilter(type) && filter(type));
        }

        /// <summary>
        /// Catch exception if it's thrown by action and optionally call finallyAction 
        /// </summary>
        public static void Catch(ref Exception exception, Action action, Action finallyAction = null)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (exception == null) exception = e;
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        /// <summary>
        /// Catch exception if it's thrown by action and optionally call callbacks actions 
        /// </summary>
        public static void Catch(Action action, Action<Exception> exceptionAction = null, Action finallyAction = null)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                exceptionAction?.Invoke(e);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        /// <summary>
        /// Calls field.FromCustomString if CustomStringFormatSupported is true and field.FromString otherwise
        /// </summary>
        public static void FromString(BGField field, int entityIndex, string value)
        {
            if (field.CustomStringFormatSupported) field.FromCustomString(entityIndex, value);
            else field.FromString(entityIndex, value);
        }

        /// <summary>
        /// Calls field.ToCustomString if CustomStringFormatSupported is true and field.ToString otherwise
        /// </summary>
        public static string ToString(BGField field, int entityIndex)
        {
            return field.CustomStringFormatSupported ? field.ToCustomString(entityIndex) : field.ToString(entityIndex);
        }

        /// <summary>
        /// Finds safe duplicate meta name (it should be unique within repo)
        /// </summary>
        public static string DuplicateMetaName(BGMetaEntity meta, Func<string, bool> isValidName = null)
        {
            var counter = 2;
            var newTableName = GetBaseName(meta.Name, counter);
            while (meta.Repo.HasMeta(newTableName) || isValidName != null && !isValidName(newTableName))
            {
                newTableName = GetBaseName(meta.Name, ++counter);
                if (counter > 100000) throw new Exception("Can not generate new name");
            }

            return newTableName;
        }

        //get base table name 
        private static string GetBaseName(string metaName, int counter)
        {
            var counterString = "" + counter;
            var newName = metaName + counterString;
            if (newName.Length > 31) newName = metaName.Substring(0, 31 - counterString.Length) + counterString;

            return newName;
        }

        /// <summary>
        /// Identify that some test is running
        /// </summary>
        public static void RunTest(Action action)
        {
            TestIsRunning = true;
            try
            {
                action();
            }
            finally
            {
                TestIsRunning = false;
            }
        }

        /// <summary>
        /// concatenate 2 dimensional array into one dimensional one
        /// </summary>
        public static T[] Concat<T>(params T[][] arrays)
        {
            var count = 0;
            for (var i = 0; i < arrays.Length; i++) count += arrays[i].Length;

            var result = new T[count];
            var offset = 0;
            for (var i = 0; i < arrays.Length; i++)
            {
                var array = arrays[i];
                var arrayLength = array.Length;
                Array.Copy(array, 0, result, offset, arrayLength);
                offset += arrayLength;
            }

            return result;
        }

        /// <summary>
        /// Identify that BGEncryptor implementation class should be skipped in Unity Editor GUI
        /// </summary>
        public interface SkipMeInEditor
        {
        }

        /// <summary>
        ///  if target type is assignable from fieldType
        /// </summary>
        public static bool IsAssignable(Type fieldType, Type targetType)
        {
            return targetType.IsAssignableFrom(fieldType);
            //WTF????
            // if (targetType == typeof(UnityEngine.Object)) return targetType.IsAssignableFrom(fieldType);
            // var isAssignableFrom = fieldType.IsAssignableFrom(targetType);
            // return isAssignableFrom;
        }

        /// <summary>
        /// if 2 string values are equal(ignoring the difference between null values and empty values)
        /// </summary>
        public static bool AreEqual(string value1, string value2)
        {
            var v1 = string.IsNullOrEmpty(value1);
            var v2 = string.IsNullOrEmpty(value2);
            if (v1 && v2) return true;
            if (v1 || v2) return false;
            return value1.Equals(value2);
        }

        public static bool IsPrefab(GameObject go) => go.scene.rootCount == 0;

        /// <summary>
        /// Save the database. This method works only inside Unity Editor!
        /// Do not remove it, it's used by reflection
        /// </summary>
        public static void SaveDatabaseInUnityEditor()
        {
            try
            {
                var typeName = "BansheeGz.BGDatabase.Editor.BGRepoSaver";
                var repoSaverType = GetType(typeName);
                if (repoSaverType == null) throw new Exception($"Can not save database: {typeName} type is not found!");
                var methodName = "SaveAndMarkAsSaved";
                var method = repoSaverType.GetMethod(methodName);
                if (method == null) throw new Exception($"Can not save database: method {methodName} method is not found at type {typeName}!");
                method.Invoke(null, null);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static string CheckNameForNewMetaObject(string newName)
        {
            if (newName == null) return "Name can not be empty";
            if (BGMetaObject.ReservedWordsForNewObjects.Contains(newName)) return $"This name [{newName}] is reserved for system needs. Please, choose another name.";
            return null;
        }
        public static void CheckNameForNewMetaObjectWithException(string newName)
        {
            var error = CheckNameForNewMetaObject(newName);
            if (!string.IsNullOrEmpty(error)) throw new Exception(error);
        }
    }
}