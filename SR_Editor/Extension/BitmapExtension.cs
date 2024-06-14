using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public static class BitmapExtension
	{
		private static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo imageCodecInfo = imageEncoders.FirstOrDefault<ImageCodecInfo>((ImageCodecInfo t) => t.MimeType == mimeType);
			return imageCodecInfo;
		}

		public static void Save(this Bitmap resim, string yol, int kaliteOrani)
		{
			if ((kaliteOrani < 0 ? true : kaliteOrani > 100))
			{
				throw new ArgumentOutOfRangeException("Kalite oranı 0 ile 100 arasında olmalı!!!");
			}
			EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, (long)kaliteOrani);
			ImageCodecInfo encoderInfo = BitmapExtension.GetEncoderInfo("image/jpeg");
			EncoderParameters encoderParameter1 = new EncoderParameters(1);
			encoderParameter1.Param[0] = encoderParameter;
			resim.Save(yol, encoderInfo, encoderParameter1);
		}

		public static byte[] ToArray(this Bitmap bitmap)
		{
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Gif);
			return memoryStream.ToArray();
		}

		public static Stream ToStreamWithQuality(this Bitmap resim, int kaliteOrani)
		{
			if ((kaliteOrani < 0 ? true : kaliteOrani > 100))
			{
				throw new ArgumentOutOfRangeException("Kalite oranı 0 ile 100 arasında olmalı!!!");
			}
			EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, (long)kaliteOrani);
			ImageCodecInfo encoderInfo = BitmapExtension.GetEncoderInfo("image/jpeg");
			EncoderParameters encoderParameter1 = new EncoderParameters(1);
			encoderParameter1.Param[0] = encoderParameter;
			Stream memoryStream = new MemoryStream();
			resim.Save(memoryStream, encoderInfo, encoderParameter1);
			return memoryStream;
		}
	}
}