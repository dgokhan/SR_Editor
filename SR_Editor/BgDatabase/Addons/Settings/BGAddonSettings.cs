/*
<copyright file="BGAddonSettings.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Let you to fine-tune some parameters. 
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/Settings/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "Settings", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerSettings")]
    public partial class BGAddonSettings : BGAddon
    {
        private bool multiThreadedLoading;
        private bool zippedContent;

        private string encryptorType;
        private string encryptorConfig;
        private bool encryptSaveLoadAddon;

        private BGEncryptor encryptor;

        /// <summary>
        /// Enables multi-threaded database loading. If your database is very small, this wont give you any advantages, but otherwise it may result in big performance boost
        /// </summary>
        public bool MultiThreadedLoading
        {
            get => multiThreadedLoading;
            set
            {
                if (multiThreadedLoading == value) return;
                multiThreadedLoading = value;
                FireChange();
            }
        }

        /// <summary>
        /// Enables zipping content. This reduce the size of the database, but increase the loading/saving time 
        /// </summary>
        public bool ZippedContent
        {
            get => zippedContent;
            set
            {
                if (zippedContent == value) return;
                zippedContent = value;
                FireChange();
            }
        }

        /// <summary>
        /// C# class for encrypting/decrypting data 
        /// </summary>
        public string EncryptorType
        {
            get => encryptorType;
            set
            {
                if (encryptorType == value) return;
                if (!string.IsNullOrEmpty(value)) encryptor = GetEncryptor(value);
                else encryptor = null;

                encryptorType = value;
                FireChange();
            }
        }

        /// <summary>
        /// C# class for encrypting/decrypting data 
        /// </summary>
        public string EncryptorConfig
        {
            get => encryptorConfig;
            set
            {
                if (encryptorConfig == value) return;
                encryptorConfig = value;
                FireChange();
            }
        }

        /// <summary>
        /// Encrypt SaveLoad addon result  
        /// </summary>
        public bool EncryptSaveLoadAddon
        {
            get => encryptSaveLoadAddon;
            set
            {
                if (encryptSaveLoadAddon == value) return;
                encryptSaveLoadAddon = value;
                FireChange();
            }
        }

        /// <summary>
        /// Creates an encryptor object
        /// </summary>
        public BGEncryptor Encryptor
        {
            get
            {
                if (string.IsNullOrEmpty(encryptorType)) return null;
                if (encryptor != null) return encryptor;
                encryptor = GetEncryptor(encryptorType);
                return encryptor;
            }
        }
        //================================================================================================
        //                                              Configuration
        //================================================================================================

        /// <inheritdoc />
        public override string ConfigToString()
        {
            var configToString = JsonUtility.ToJson(new Settings
            {
                MultiThreadedLoading = multiThreadedLoading,
                ZippedContent = zippedContent,
                EncryptorType = encryptorType,
                EncryptorConfig = encryptorConfig,
                EncryptSaveLoadAddon = encryptSaveLoadAddon
            });
            return configToString;
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var fromJson = JsonUtility.FromJson<Settings>(config);
            multiThreadedLoading = fromJson.MultiThreadedLoading;
            zippedContent = fromJson.ZippedContent;
            encryptorType = fromJson.EncryptorType;
            encryptorConfig = fromJson.EncryptorConfig;
            encryptSaveLoadAddon = fromJson.EncryptSaveLoadAddon;
        }

        [Serializable]
        private class Settings
        {
            public bool MultiThreadedLoading;
            public bool ZippedContent;
            public string EncryptorType;
            public string EncryptorConfig;
            public bool EncryptSaveLoadAddon;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(32);

            //version
            writer.AddInt(2);

            //fields
            writer.AddBool(multiThreadedLoading);
            writer.AddBool(zippedContent);
            writer.AddString(encryptorType);
            writer.AddString(encryptorConfig);
            writer.AddBool(encryptSaveLoadAddon);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    multiThreadedLoading = reader.ReadBool();
                    zippedContent = reader.ReadBool();
                    break;
                }
                case 2:
                {
                    multiThreadedLoading = reader.ReadBool();
                    zippedContent = reader.ReadBool();
                    encryptorType = reader.ReadString();
                    encryptorConfig = reader.ReadString();
                    encryptSaveLoadAddon = reader.ReadBool();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        /// <inheritdoc />
        public override BGAddon CloneTo(BGRepo repo)
        {
            var clone = new BGAddonSettings
            {
                Repo = repo,
                multiThreadedLoading = multiThreadedLoading,
                zippedContent = zippedContent,
                encryptorType = encryptorType,
                encryptorConfig = encryptorConfig,
                encryptSaveLoadAddon = encryptSaveLoadAddon
            };
            return clone;
        }

        //create encryptor using it's type name
        private static BGEncryptor GetEncryptor(string type)
        {
            var encryptorClass = BGUtil.GetType(type);
            if (encryptorClass == null) throw new BGException("Can not find encryptor type $", type);
            if (!typeof(BGEncryptor).IsAssignableFrom(encryptorClass)) throw new BGException("Encryptor type $ does not implement BGEncryptor interface ", type);
            return (BGEncryptor)Activator.CreateInstance(encryptorClass);
        }
    }
}