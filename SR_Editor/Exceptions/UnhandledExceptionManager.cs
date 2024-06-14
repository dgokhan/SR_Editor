using System;
using System.Threading;
using System.Windows.Forms;
using RoyaleSupport;

namespace SR_Editor.Core.Exceptions
{
	public class UnhandledExceptionManager
	{
		public UnhandledExceptionManager()
		{
		}

		private static void ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			if(e.Exception is ApiException<RemoteServiceErrorResponse>)
			{
				var ex = e.Exception as ApiException<RemoteServiceErrorResponse>;
				UnhandledExceptionManager.UnhandledMesajGoster(new ExceptionBeklenmeyen(ex.Result.Error.Message, e.Exception));
			}
			else if(e.Exception is ApiException)
			{
				UnhandledExceptionManager.UnhandledMesajGoster(new ExceptionBeklenmeyen(e.Exception.Message, e.Exception));
			}
			else if (!(e.Exception is ExceptionBase))
			{
				UnhandledExceptionManager.UnhandledMesajGoster(new ExceptionBeklenmeyen(e.Exception.Message, e.Exception));
			}
			else
			{
				UnhandledExceptionManager.UnhandledMesajGoster(e.Exception);
			}
		}

		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			UnhandledExceptionManager.UnhandledMesajGoster((Exception)e.ExceptionObject);
		}

		private static void UnhandledMesajGoster(Exception ex)
		{
			if (ex != null)
			{
				ExceptionManager.MesajGoster(ex.Message, ex/*, LogTip.Information*/);
			}
		}

		public static void UnhandledYakala()
		{
			Application.ThreadException += new ThreadExceptionEventHandler(UnhandledExceptionManager.ThreadException);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
		}
	}
}