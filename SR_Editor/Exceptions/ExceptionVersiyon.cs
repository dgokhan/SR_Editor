using System;
using System.Drawing;

namespace SR_Editor.Core.Exceptions
{
	public class ExceptionVersiyon : ExceptionBase
	{
		public override string Baslik
		{
			get
			{
				return "Versiyon Hatası";
			}
		}

		public ExceptionVersiyon(string message) : base(message)
		{
		}

		public ExceptionVersiyon(string message, Exception innerException) : base(message, innerException)
		{
		}

		public ExceptionVersiyon(string message, Exception innerException, Bitmap screen) : base(message, innerException, null, screen)
		{
		}

		public ExceptionVersiyon(string message, Exception innerException, int? loginId, Bitmap screen) : base(message, innerException, loginId, screen)
		{
		}
	}
}