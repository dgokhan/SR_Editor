/*
<copyright file="BGLoaderForRepoStreamingAssets.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
// using Object = UnityEngine.Object;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Loader for loading database from StreamingAssets folder
    /// </summary>
    public class BGLoaderForRepoStreamingAssets : BGLoaderForRepo
    {
        public const string LoaderName = "StreamingAssets";

        public const string FileName = "bansheegz_database.bytes";
        public const string FolderName = "StreamingAssets";

        private static MethodInfo assignDatabaseInstanceIdMethod;

        /// <inheritdoc/>
        public override string Name => LoaderName;

        public static string FilePath => GetFilePath(FileName);

        public static string AssetPath => Path.Combine(Path.Combine("Assets", FolderName), FileName);

        private static MethodInfo AssignDatabaseInstanceIdMethod
        {
            get
            {
                if (assignDatabaseInstanceIdMethod != null) return assignDatabaseInstanceIdMethod;
                var editorUtilityType = BGUtil.GetType("BansheeGz.BGDatabase.Editor.BGEditorUtility");
                if (editorUtilityType == null) return null;
                assignDatabaseInstanceIdMethod = editorUtilityType.GetMethod("AssignDatabaseInstanceId",
                    BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
                return assignDatabaseInstanceIdMethod;
            }
        }

        /// <inheritdoc/>
        public override byte[] Load(LoadRequest request)
        {
            byte[] content;
            if (IsEmpty(request))
                //main
                content = Load(FileName, true);
            else
            {
                //sub asset
                var filePath = ToPath(request);
                content = Load(filePath, false);
            }

            return content;
        }

        /// <inheritdoc/>
        protected override string ToPath(LoadRequest request)
        {
            if (IsEmpty(request)) return FileName;
            var baseKeyNoExt = Path.GetFileNameWithoutExtension(Path.GetFileName(request.basePath));
            var fullKeyNoExt = AppendPaths(baseKeyNoExt, request.paths);
            var filePath = Path.ChangeExtension(fullKeyNoExt, "bytes");
            return filePath;
        }

        /*
        public override string GetPath(string basePath, params string[] paths)
        {
            if (paths == null) return basePath;
            var result = Path.GetFileNameWithoutExtension(Path.GetFileName(basePath));
            result = AppendPaths(result, paths);
            return Path.ChangeExtension(result, "bytes");
        }
        */

        //load database content
        private byte[] Load(string fileName, bool mainDatabase)
        {
            byte[] result = null;
            switch (Application.platform)
            {
                case RuntimePlatform.WebGLPlayer:
                {
                    //it's not possible to load
                    break;
                }

                case RuntimePlatform.Android:
                {
                    //custom loading is required
                    result = LoadAndroid(fileName, mainDatabase);
                    break;
                }

                default:
                {
                    result = LoadDefault(fileName, mainDatabase);
                    break;
                }
            }

            return result;
        }

        private byte[] LoadDefault(string fileName, bool mainDatabase)
        {
            var file = GetFilePath(fileName);
            if (!File.Exists(file)) return null;

            if (mainDatabase)
            {
                BGRepo.DefaultRepoAssetPath = file;
                if (Application.isEditor) TryToAssignInstanceId();
            }

            return File.ReadAllBytes(file);
        }

        private void TryToAssignInstanceId()
        {
            BGRepo.DefaultRepoAssetId = 0;
            var method = AssignDatabaseInstanceIdMethod;
            if (method == null) return;
            method.Invoke(null, new object[] { AssetPath });
        }

        public static byte[] TestMeIfYouDare(string file) => new BGLoaderForRepoStreamingAssets().TryToLoadAndroid(file, FileName);

        private byte[] LoadAndroid(string fileName, bool mainDatabase)
        {
            var apkPath = Application.dataPath;
            if (!File.Exists(apkPath)) return null;

            try
            {
                var result = TryToLoadAndroid(apkPath, fileName);
                if (result == null && !Application.isEditor && Path.GetFileName(apkPath) != "base.apk")
                {
                    // maybe split?
                    var newDataPath = Path.GetDirectoryName(apkPath) + "/base.apk";
                    if (File.Exists(newDataPath)) result = TryToLoadAndroid(newDataPath, fileName);
                }

                if (result != null && mainDatabase) BGRepo.DefaultRepoAssetPath = fileName;


                return result;
            }
            catch (Exception e)
            {
                Debug.Log("Can not load BGDatabase from apk archive!");
                Debug.LogException(e);
                return null;
            }
        }

        private byte[] TryToLoadAndroid(string apkPath, string fileName)
        {
            using (var stream = File.OpenRead(apkPath))
            {
                using (var reader = new BinaryReader(stream))
                {
                    if (!stream.CanRead) return null;
                    if (!stream.CanSeek) return null;

                    ZipArchiveUtils.ReadEndOfCentralDirectory(stream, reader, out _, out var centralDirectoryStart);

                    var targetFileName = "assets/" + fileName;
                    try
                    {
                        stream.Seek(centralDirectoryStart, SeekOrigin.Begin);

                        while (ZipCentralDirectoryFileHeader.TryReadBlock(reader, out var header))
                        {
                            var size = header.UncompressedSize;
                            if (header.CompressedSize != size) continue;

                            if (!targetFileName.Equals(Encoding.UTF8.GetString(header.Filename))) continue;

                            stream.Seek(header.RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
                            if (!ZipLocalFileHeader.TrySkipBlock(reader)) throw new ZipArchiveException("Local file header corrupt");
//                            var offset = stream.Position;
                            if (size > (long)int.MaxValue) throw new IOException("invalid size " + (long)int.MaxValue);

                            var count = (int)size;
                            var offset = 0;

                            var buffer = new byte[count];
                            while (count > 0)
                            {
                                var num = stream.Read(buffer, offset, count);
                                if (num == 0) throw new EndOfStreamException();
                                offset += num;
                                count -= num;
                            }

                            return buffer;
                        }
                    }
                    catch (EndOfStreamException ex)
                    {
                        throw new ZipArchiveException("CentralDirectoryInvalid", ex);
                    }
                }
            }

            return null;
        }

        private static string GetFilePath(string fileName)
        {
            return Path.Combine(Application.streamingAssetsPath, fileName);
        }
    }
}