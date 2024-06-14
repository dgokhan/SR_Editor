/*
<copyright file="BGSyncUtil.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// sync jobs utils class
    /// </summary>
    public static partial class BGSyncUtil
    {
        /// <summary>
        /// Read the file and calls the action callback with file content
        /// </summary>
        public static void ReadFile(BGLogger logger, string path, Action<byte[]> action)
        {
            logger.AppendLine("Trying to read file at ($)..", path);
//            var content = File.ReadAllBytes(path);
            byte[] content;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                content = new byte[stream.Length];
                stream.Read(content, 0, content.Length);
            }

            if (logger.AppendLine(content.Length == 0, "Content of file is empty")) return;
            logger.AppendLine("File is read successfully. ($) bytes", content.Length);
            action(content);
        }

        /// <summary>
        /// Read the file content
        /// </summary>
        public static byte[] ReadFile(BGLogger logger, string path)
        {
            logger.AppendLine("Trying to read file at ($)..", path);
//            var content = File.ReadAllBytes(path);
            byte[] content;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                content = new byte[stream.Length];
                stream.Read(content, 0, content.Length);
            }

            if (logger.AppendLine(content.Length == 0, "Content of file is empty")) return null;
            logger.AppendLine("File is read successfully. $ bytes", content.Length);
            return content;
        }

        public static bool AppendWarning(BGLogger logger, bool print, bool condition, string message, params object[] parameters)
        {
            if (condition) AppendWarning(logger, print, message, parameters);
            return condition;
        }

        public static void AppendWarning(BGLogger logger, bool print, string message, params object[] parameters)
        {
            try
            {
                logger?.AppendWarning(message, parameters);
            }
            catch (Exception e)
            {
            }
        }
    }
}