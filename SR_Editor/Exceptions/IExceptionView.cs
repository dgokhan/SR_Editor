using System;

namespace SR_Editor.Core.Exceptions
{
	public interface IExceptionView
	{
		string DetayMesaj
		{
			get;
			set;
		}

		bool TumDetayGosterilsinMi
		{
			get;
			set;
		}

		void ExceptionGoster(ExceptionBase exception);

		void ExceptionGoster(Exception exception, string detayMesaj);
	}
}