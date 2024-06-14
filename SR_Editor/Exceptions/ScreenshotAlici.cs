using SR_Editor.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SR_Editor.Core.Exceptions
{
	internal static class ScreenshotAlici
	{
		private const int ExceptionResimKaliteOrani = 20;

		public static Bitmap ScreenshotAl()
		{
			Screen[] allScreens = Screen.AllScreens;
			Rectangle empty = Rectangle.Empty;
			Rectangle rectangle = ((IEnumerable<Screen>)allScreens).Aggregate<Screen, Rectangle>(empty, (Rectangle current, Screen screen) => Rectangle.Union(current, screen.Bounds));
			Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);
			Graphics graphic = Graphics.FromImage(bitmap);
			try
			{
				graphic.CopyFromScreen(rectangle.X, rectangle.Y, 0, 0, rectangle.Size, CopyPixelOperation.SourceCopy);
			}
			finally
			{
				if (graphic != null)
				{
					((IDisposable)graphic).Dispose();
				}
			}
			Bitmap image = bitmap.ToStreamWithQuality(20).ToByte().ToImage();
			return image;
		}
	}
}