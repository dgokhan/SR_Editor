/*
<copyright file="BGMetaObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class for all meta objects, such as table (meta) and table's field
    /// This class has unique id and unique name
    /// </summary>
    public abstract partial class BGMetaObject : BGObject, BGConfigurableI, BGConfigurableBinaryI, BGObjectWithNameI, BGIndexableI
    {
        public static readonly HashSet<string> ReservedWords = new HashSet<string>(new[]
        {
            //C# 1.1 RESERVED
            "abstract", "as", "base", "bool", "break", "byte",
            "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double",
            "else", "enum", "event", "explicit", "extern", "finally",
            "fixed", "float", "for", "foreach", "goto", "if",
            "implicit", "in", "int", "interface", "internal", "is",
            "lock", "long", "namespace", "new", "null", "object",
            "operator", "out", "override", "params", "private",
            "public", "readonly", "ref", "return", "sbyte", "sealed",
            "short", "sizeof", "stackalloc", "static", "string", "struct",
            "switch", "this", "throw", "try", "typeof", "unit",
            "ulong", "unchecked", "unsafe", "ushort", "using", "virtual",
            "void", "volatile", "while", "FALSE", "TRUE",

            //C# 2.0 RESERVED
            "yield",

            //C# 3.0 RESERVED
            "by", "descending", "from", "group", "into", "orderby",
            "select", "var", "where",
        });

        //this is for new objects only
        public static readonly HashSet<string> ReservedWordsForNewObjects = new HashSet<string>(new[]
        {
            // !!! We can not add new reserved words to ReservedWords, cause it may breaks existing databases
            //missing keywords
            "false", "protected", "true", "uint",

            //missing BGDatabase reserved
            "Index"
        });

        private string name;

        /// <inheritdoc/>
        public virtual string Name
        {
            get => name;
            set => SetName(value);
        }

        private bool system;

        /// <summary>
        /// is this meta is system? System means it's used by database itself, rather than by a user
        /// It also means this meta object can not be removed by a user in the Editor.
        /// </summary>
        public virtual bool System
        {
            get => system;
            set => system = value;
        }

        private string addon;

        /// <summary>
        /// Addons can create their own meta objects
        /// It also means this meta object can not be removed by a user in the Editor.
        /// </summary>
        public string Addon
        {
            get => addon;
            set => addon = value;
        }


        /*
        /// <summary>
        /// Do not call setter
        /// </summary>
        public new bool IsDeleted
        {
            set => deleted = value;
            get => deleted;
        }
        */

        /// <summary>
        /// Physical objects's index (position)
        /// </summary>
        public abstract int Index { get; }


        private string comment;

        /// <summary>
        /// comment
        /// </summary>
        public virtual string Comment
        {
            get => comment;
            set => comment = value;
        }

        private object controller;
        private string controllerType;
        private bool controllerLoadTried;

        /// <summary>
        /// C# controller type
        /// </summary>
        public virtual string ControllerType
        {
            get => controllerType;
            set
            {
                if (Equals(controllerType, value)) return;
                controllerType = value;
                controller = null;
                controllerLoadTried = false;
            }
        }

        /// <summary>
        /// C# controller
        /// </summary>
        public object Controller
        {
            get
            {
                if (controller != null) return controller;
                if (controllerLoadTried || string.IsNullOrEmpty(controllerType)) return null;
                controllerLoadTried = true;
                try
                {
                    var type = BGUtil.GetType(controllerType);
                    if (type == null) throw new Exception($"Can not find a C# type with name {controllerType}");
                    controller = Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                }

                return controller;
            }
            set
            {
                if (Equals(controllerType, value)) return;
                controller = value;
                if (controller != null)
                {
                    controllerLoadTried = true;
                    controllerType = controller.GetType().FullName;
                }
            }
        }

        protected BGMetaObject(BGId id, string name) : base(id)
        {
            SetName(name);
        }

        private void SetName(string value)
        {
            var nameError = CheckName(value);
            if (nameError != null) throw new BGException("Error in name ($): $", value, nameError);
            name = value;
        }

        /// <summary>
        /// Check if name is valid.
        /// names should comply to several conditions to be valid names
        /// </summary>
        public static string CheckName(string name)
        {
            //is it faster than regexp?
            if (string.IsNullOrEmpty(name)) return "Name can not be empty";
            if (name.Length > 31) return "Name is not valid (31 characters max, no more)";
            if (!char.IsLetter(name[0])) return "Name should start with a letter";
            if (ReservedWords.Contains(name)) return "This name (" + name + ") is reserved for system needs. Please, choose another name.";

            for (var i = 1; i < name.Length; i++)
            {
                var c = name[i];
                if (!char.IsLetterOrDigit(c) && c != '_') return "Name should contain letters, digits or underscore only";
            }

            return null;
        }

        //=================================================================================================================
        //                      Config
        //=================================================================================================================

        /// <inheritdoc />
        public abstract string ConfigToString();

        /// <inheritdoc />
        public abstract void ConfigFromString(string config);

        /// <inheritdoc />
        public abstract byte[] ConfigToBytes();

        /// <inheritdoc />
        public abstract void ConfigFromBytes(ArraySegment<byte> config);
    }
}