/*
<copyright file="BGAddonCodeGen.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

using UnityEngine;


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Code generation addon. With this addon enabled, custom classes, inherited from BGEntity will be used as tables rows.
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/CodeGeneration/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "CodeGen", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerCodeGen")]
    public partial class BGAddonCodeGen : BGAddon
    {
        /// <summary>
        /// Default class generator class
        /// </summary>
        public const string DefaultExtensionClassesGenerator = "BansheeGz.BGDatabase.Editor.BGExtensionClassesGenerator";

        private string generatorClass;
        private string sourceFile;
        private string classesNamePrefix;
        private string fieldsNamePrefix;
        private string package;
        private string entitiesPrefix;

        private string referenceClassPostfix;
        private string referenceListClassPostfix;
        
        private string fieldReferenceClassPostfix;
        private bool readOnly;

        /// <summary>
        /// C# class to be used as class generator
        /// </summary>
        public string GeneratorClass
        {
            get => generatorClass;
            set
            {
                if (string.Equals(generatorClass, value)) return;
                generatorClass = value;
                FireChange();
            }
        }

        /// <summary>
        /// c# source file to be used for generated classes
        /// </summary>
        public string SourceFile
        {
            get => sourceFile;
            set
            {
                if (string.Equals(sourceFile, value)) return;
                sourceFile = value;
                FireChange();
            }
        }

        /// <summary>
        /// Name prefix to be used to prepend to classes names
        /// </summary>
        /// <example>
        /// Class for Meta with name Table and name prefix E_ will be named E_Table  
        /// </example>
        public string ClassesNamePrefix
        {
            get => classesNamePrefix;
            set
            {
                if (string.Equals(classesNamePrefix, value)) return;
                classesNamePrefix = value;
                FireChange();
            }
        }

        /// <summary>
        /// Name prefix to be used to prepend to fields names
        /// </summary>
        /// <example>
        /// Field with name myField and name prefix f_ will be named f_myField
        /// </example>
        public string FieldsNamePrefix
        {
            get => fieldsNamePrefix;
            set
            {
                if (string.Equals(fieldsNamePrefix, value)) return;
                fieldsNamePrefix = value;
                FireChange();
            }
        }

        /// <summary>
        /// C# namespace to be used for generated classes
        /// </summary>
        public string Package
        {
            get => package;
            set
            {
                if (string.Equals(package, value)) return;
                package = value;
                FireChange();
            }
        }

        /// <summary>
        /// Generate static getters for each entity (assuming each entity has unique name)
        /// </summary>
        public string EntitiesPrefix
        {
            get => entitiesPrefix;
            set
            {
                if (entitiesPrefix == value) return;
                entitiesPrefix = value;
                FireChange();
            }
        }

        /// <summary>
        /// If assigned, C# class will be generated with supplied postfix, which will be used to reference table row 
        /// </summary>
        public string ReferenceClassPostfix
        {
            get => referenceClassPostfix;
            set
            {
                if (string.Equals(referenceClassPostfix, value)) return;
                referenceClassPostfix = value;
                FireChange();
            }
        }

        /// <summary>
        /// If assigned, C# class will be generated with supplied postfix, which will be used to reference several table rows
        /// </summary>
        public string ReferenceListClassPostfix
        {
            get => referenceListClassPostfix;
            set
            {
                if (string.Equals(referenceListClassPostfix, value)) return;
                referenceListClassPostfix = value;
                FireChange();
            }
        }
        
        /// <summary>
        /// If assigned, C# class will be generated with supplied postfix, which will be used to reference table field 
        /// </summary>
        public string FieldReferenceClassPostfix
        {
            get => fieldReferenceClassPostfix;
            set
            {
                if (string.Equals(fieldReferenceClassPostfix, value)) return;
                fieldReferenceClassPostfix = value;
                FireChange();
            }
        }

        /// <summary>
        /// all setter/newEntity/delete methods will be slipped to ensure database in readonly mode
        /// </summary>
        public bool ReadOnly
        {
            get => readOnly;
            set
            {
                if (readOnly== value) return;
                readOnly = value;
                FireChange();
            }
        }
        //================================================================================================
        //                                              Config
        //================================================================================================

        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig
            {
                GeneratorClass = generatorClass,
                SourceFile = sourceFile,
                ClassesNamePrefix = classesNamePrefix,
                FieldsNamePrefix = fieldsNamePrefix,
                Package = package,
                EntitiesPrefix = entitiesPrefix,
                ReferenceClassPostfix = referenceClassPostfix,
                ReferenceListClassPostfix = referenceListClassPostfix,
                FieldReferenceClassPostfix = fieldReferenceClassPostfix,
                ReadOnly = readOnly
            });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var json = JsonUtility.FromJson<JsonConfig>(config);
            generatorClass = json.GeneratorClass;
            sourceFile = json.SourceFile;
            classesNamePrefix = json.ClassesNamePrefix;
            fieldsNamePrefix = json.FieldsNamePrefix;
            package = json.Package;
            entitiesPrefix = json.EntitiesPrefix;
            referenceClassPostfix = json.ReferenceClassPostfix;
            referenceListClassPostfix = json.ReferenceListClassPostfix;
            fieldReferenceClassPostfix = json.FieldReferenceClassPostfix;
            readOnly = json.ReadOnly;
        }

        [Serializable]
        private class JsonConfig
        {
            public string GeneratorClass;
            public string SourceFile;
            public string ClassesNamePrefix;
            public string FieldsNamePrefix;
            public string Package;
            public string EntitiesPrefix;
            public string ReferenceClassPostfix;
            public string ReferenceListClassPostfix;
            public string FieldReferenceClassPostfix;
            public bool ReadOnly;
        }

        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(512);

            //version
            writer.AddInt(4);

            //fields
            writer.AddString(generatorClass);
            writer.AddString(sourceFile);
            writer.AddString(classesNamePrefix);
            writer.AddString(fieldsNamePrefix);
            writer.AddString(package);
            writer.AddString(entitiesPrefix);

            //version 2
            writer.AddString(referenceClassPostfix);
            writer.AddString(referenceListClassPostfix);

            //version 3
            writer.AddString(fieldReferenceClassPostfix);
            
            //version 4
            writer.AddBool(readOnly);
            
            return writer.ToArray();
        }

        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    generatorClass = reader.ReadString();
                    sourceFile = reader.ReadString();
                    classesNamePrefix = reader.ReadString();
                    fieldsNamePrefix = reader.ReadString();
                    package = reader.ReadString();
                    entitiesPrefix = reader.ReadString();
                    break;
                }
                case 2:
                {
                    generatorClass = reader.ReadString();
                    sourceFile = reader.ReadString();
                    classesNamePrefix = reader.ReadString();
                    fieldsNamePrefix = reader.ReadString();
                    package = reader.ReadString();
                    entitiesPrefix = reader.ReadString();

                    //version 2
                    referenceClassPostfix = reader.ReadString();
                    referenceListClassPostfix = reader.ReadString();
                    break;
                }
                case 3:
                {
                    generatorClass = reader.ReadString();
                    sourceFile = reader.ReadString();
                    classesNamePrefix = reader.ReadString();
                    fieldsNamePrefix = reader.ReadString();
                    package = reader.ReadString();
                    entitiesPrefix = reader.ReadString();

                    //version 2
                    referenceClassPostfix = reader.ReadString();
                    referenceListClassPostfix = reader.ReadString();
                    
                    //version 3
                    fieldReferenceClassPostfix = reader.ReadString();
                    break;
                }
                case 4:
                {
                    generatorClass = reader.ReadString();
                    sourceFile = reader.ReadString();
                    classesNamePrefix = reader.ReadString();
                    fieldsNamePrefix = reader.ReadString();
                    package = reader.ReadString();
                    entitiesPrefix = reader.ReadString();

                    //version 2
                    referenceClassPostfix = reader.ReadString();
                    referenceListClassPostfix = reader.ReadString();
                    
                    //version 3
                    fieldReferenceClassPostfix = reader.ReadString();
                    
                    //version 4
                    readOnly = reader.ReadBool();
                    
                    break;
                }

                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================

        /// <inheritdoc />
        public override BGAddon CloneTo(BGRepo repo)
        {
            return new BGAddonCodeGen
            {
                Repo = repo,
                generatorClass = generatorClass,
                sourceFile = sourceFile,
                classesNamePrefix = classesNamePrefix,
                fieldsNamePrefix = fieldsNamePrefix,
                package = package,
                entitiesPrefix = entitiesPrefix,
                referenceClassPostfix = referenceClassPostfix,
                referenceListClassPostfix = referenceListClassPostfix,
                fieldReferenceClassPostfix = fieldReferenceClassPostfix,
                readOnly = readOnly,
            };
        }
        //================================================================================================
        //                                              Helper methods
        //================================================================================================

        /// <summary>
        /// full name (including namespace) for type to use for specified meta 
        /// </summary>
        public string GetMetaTypeWithPackage(string metaName) => (string.IsNullOrEmpty(package) ? "" : package + ".") + GetMetaType(metaName);

        /// <summary>
        /// full name (including namespace) for type to use for specified meta 
        /// </summary>
        public string GetEntityFactoryTypeWithPackage(string metaName) => GetMetaTypeWithPackage(metaName) + "+Factory";

        /// <summary>
        /// full name (with prefix) for specified field  
        /// </summary>
        public string GetFieldName(string fieldName) => (fieldsNamePrefix ?? "") + fieldName;

        /// <summary>
        /// full name (with prefix) for specified meta  
        /// </summary>
        public string GetMetaType(string metaName) => (classesNamePrefix ?? "") + metaName;
    }
}