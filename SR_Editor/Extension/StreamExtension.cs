using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public static class StreamExtension
	{
		public static byte[] ToByte(this Stream stream)
		{
			byte[] array;
			if (!(stream is MemoryStream))
			{
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					stream.CopyTo(memoryStream);
					array = memoryStream.ToArray();
				}
				finally
				{
					if (memoryStream != null)
					{
						((IDisposable)memoryStream).Dispose();
					}
				}
			}
			else
			{
				array = ((MemoryStream)stream).ToArray();
			}
			return array;
		}
	}
}