using System;

namespace SR_Editor.Core.Exceptions
{
	public class ExceptionViewFactory
	{
		private readonly static object obj;

		private static IExceptionView _instance;

		static ExceptionViewFactory()
		{
			ExceptionViewFactory.obj = new object();
		}

		public ExceptionViewFactory()
		{
		}

		public static IExceptionView CreateViewer(ExceptionType hataTipi)
		{
			IExceptionView formExceptionView;
			switch (hataTipi)
			{
				case ExceptionType.Error:
				{
					formExceptionView = new FormExceptionView();
					break;
				}
				case ExceptionType.Warning:
				{
					formExceptionView = new FormWarningView();
					break;
				}
				default:
				{
					formExceptionView = new FormExceptionView();
					break;
				}
			}
			return formExceptionView;
		}
	}
}