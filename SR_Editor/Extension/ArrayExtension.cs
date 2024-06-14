using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SR_Editor.Core
{
	public static class ArrayExtension
	{
		public static string ConvertToString(this byte[] array)
		{
			return Encoding.GetEncoding("utf-8").GetString(array);
		}

		public static void SaveFile(this byte[] fileBytes, string fileName)
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
			fileStream.Write(fileBytes, 0, (int)fileBytes.Length);
			fileStream.Close();
		}

		public static Bitmap ToImage(this byte[] array)
		{
			Bitmap bitmap;
			if (!array.IsNull())
			{
				bitmap = (Bitmap)Image.FromStream(new MemoryStream(array));
			}
			else
			{
				bitmap = null;
			}
			return bitmap;
		}
	}
}