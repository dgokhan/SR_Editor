using System;
using System.IO;
using System.Text;

namespace SR_Editor.Core.Utility
{
	public static class UtilFile
	{
		public static string ReadFile(string path)
		{
			string end;
			if (!File.Exists(path))
			{
				throw new FileNotFoundException("Verilen yolda bir dosya bulunmuyor");
			}
			StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("iso-8859-9"));
			try
			{
				end = streamReader.ReadToEnd();
			}
			finally
			{
				if (streamReader != null)
				{
					((IDisposable)streamReader).Dispose();
				}
			}
			return end;
		}
	}
}