using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SR_Editor.Core
{
	public static class DirectoryExtension
	{
		public static IEnumerable<FileInfo> GetFileList(this DirectoryInfo rootDirectory, string exntension)
		{
			if (!rootDirectory.Exists)
			{
				throw new DirectoryNotFoundException("Directory sınıfına verilen kök dizin boş olamaz");
			}
			foreach (FileInfo fileInfo in 
				from dosya in rootDirectory.GetFiles()
				where dosya.Extension == exntension
				select dosya)
			{
				yield return fileInfo;
			}
			foreach (FileInfo fileInfo1 in rootDirectory.GetDirectories().SelectMany<DirectoryInfo, FileInfo>((DirectoryInfo d) => d.GetFileList(exntension)))
			{
				yield return fileInfo1;
			}
		}

		public static IEnumerable<FileInfo> GetFileList(this DirectoryInfo rootDirectory, string exntension, string partOfFileName)
		{
			if (!rootDirectory.Exists)
			{
				throw new DirectoryNotFoundException("Directory sınıfına verilen kök dizin boş olamaz");
			}
			foreach (FileInfo fileInfo in 
				from dosya in rootDirectory.GetFiles()
				where (dosya.Extension != exntension ? false : dosya.Name.Contains(partOfFileName))
				select dosya)
			{
				yield return fileInfo;
			}
			DirectoryInfo[] directories = rootDirectory.GetDirectories();
			for (int i = 0; i < (int)directories.Length; i++)
			{
				foreach (FileInfo fileList in directories[i].GetFileList(exntension, partOfFileName))
				{
					yield return fileList;
				}
			}
		}
	}
}