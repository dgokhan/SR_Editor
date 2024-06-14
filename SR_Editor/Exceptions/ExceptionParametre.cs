using System;

namespace SR_Editor.Core.Exceptions
{
	public class ExceptionParametre : ExceptionBase
	{
		public override string Baslik
		{
			get
			{
				return "Parametre bulunamadı";
			}
		}

		public ExceptionParametre(string message) : base(message)
		{
		}

		public ExceptionParametre(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}