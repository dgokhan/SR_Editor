using SR_Editor.Core;
//using SR_Editor.Core.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SR_Editor.Core.Exceptions
{
	public class ExceptionManager
	{
		public ExceptionManager()
		{
		}

		private static string BuildErrorMessage(ICollection<Exception> exceptions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (exceptions.Count > 1)
			{
				stringBuilder.Append("Sistemde bazı hatalar meydana geldi. Hata mesajları aşağıda listelenmiştir".CeviriYap());
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(Environment.NewLine);
			}
			foreach (Exception exception1 in 
				from exception in exceptions
				where exception != null
				select exception)
			{
				if (!exception1.Message.Contains("See the inner exception"))
				{
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(string.Concat("-----------------", "Mesaj".CeviriYap(), "---------------------"));
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(exception1.Message);
					stringBuilder.Append(Environment.NewLine);
					if (exception1.GetType().FullName.IsNotNull())
					{
						stringBuilder.Append(string.Concat("-----------------", "Hata Tipi".CeviriYap(), "------------"));
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(exception1.GetType().FullName);
						stringBuilder.Append(Environment.NewLine);
					}
					if (exception1.TargetSite.IsNotNull())
					{
						stringBuilder.Append("-----------------Exception Target Site-----");
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(exception1.TargetSite);
						stringBuilder.Append(Environment.NewLine);
					}
					if (exception1.StackTrace.IsNotNull())
					{
						stringBuilder.Append("-----------------Stack Trace---------------");
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(exception1.StackTrace);
					}
					else if (exception1.InnerException.IsNotNull())
					{
						if (exception1.InnerException.StackTrace.IsNotNull())
						{
							stringBuilder.Append("-----------------Stack Trace---------------");
							stringBuilder.Append(Environment.NewLine);
							stringBuilder.Append(exception1.InnerException.StackTrace);
						}
					}
					stringBuilder.Append("-------------------------------------------");
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(Environment.NewLine);
				}
			}
			exceptions.Clear();
			return stringBuilder.ToString();
		}

		private static string BuildErrorTitleMessage(ICollection<Exception> exceptions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (exceptions.Count > 1)
			{
				stringBuilder.Append("Sistemde bazı hatalar meydana geldi. Hata mesajları aşağıda listelenmiştir".CeviriYap());
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(Environment.NewLine);
			}
			foreach (Exception exception1 in 
				from exception in exceptions
				where exception != null
				select exception)
			{
				if (!exception1.Message.Contains("See the inner exception"))
				{
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(string.Concat("Mesaj".CeviriYap(), ": "));
					stringBuilder.Append(exception1.Message);
					if ((!exception1.InnerException.IsNotNull() ? false : exception1.Message.IsNull()))
					{
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(exception1.InnerException.Message);
					}
					stringBuilder.Append(Environment.NewLine);
				}
			}
			return stringBuilder.ToString();
		}

		private static List<Exception> GetExceptionList(Exception pException)
		{
			List<Exception> exceptions = new List<Exception>();
			if (pException.InnerException != null)
			{
				if (pException.InnerException.InnerException != null)
				{
					if (pException.InnerException.InnerException.InnerException != null)
					{
						exceptions.Add(pException.InnerException.InnerException.InnerException);
					}
					exceptions.Add(pException.InnerException.InnerException);
				}
				exceptions.Add(pException.InnerException);
			}
			exceptions.Add(pException);
			return exceptions;
		}
        
		public static void MesajGoster(string mesaj, Exception exception/*, LogTip logTip = 1*/)
		{
			IExceptionView exceptionView;
			List<Exception> exceptions = new List<Exception>();
			if (exception != null)
			{
				exceptions = ExceptionManager.GetExceptionList(exception);
				try
				{
					try
					{
						Bitmap bitmap = null;
						int? nullable = null;
						/*if (Login.AktifLogin != null)
						{
							nullable = new int?(Login.AktifLogin.Id);
						}
						ILogger logger = LogFactory.CreateLogger(LogerTip.PusulaLogger);*/
						if (!(exception is ExceptionBase))
						{
							/*logTip = LogTip.Error;
							logger.Log(logTip, exception, nullable, bitmap);*/
							exceptionView = ExceptionViewFactory.CreateViewer(ExceptionType.Error);
							exceptionView.ExceptionGoster(exception, ExceptionManager.BuildErrorMessage(exceptions));
						}
						else
						{
							ExceptionBase exceptionBase = exception as ExceptionBase;
							exceptionBase.EkranResmi = bitmap;
							exceptionBase.LoginId = nullable;
							exceptionView = ExceptionViewFactory.CreateViewer(exceptionBase.HataTipi);
							bool flag = false;
							/*if (exceptionBase.HataTipi == ExceptionType.Error)
							{
								logTip = LogTip.Error;
								flag = true;
							}
							if ((logTip == LogTip.Error ? true : logTip == LogTip.Fatal))
							{
								logger.Log(logTip, exceptionBase);
							}*/
							exceptionView.TumDetayGosterilsinMi = flag;
							exceptionBase.HataOlusuncaGerceklestir();
							exceptionView.ExceptionGoster(exceptionBase, ExceptionManager.BuildErrorMessage(exceptions));
						}
					}
					catch (Exception exception2)
					{
						Exception exception1 = exception2;
						exceptions.Add(exception1);
						exceptions.Add(exception1.InnerException);
						exceptionView = ExceptionViewFactory.CreateViewer(ExceptionType.Error);
						exceptionView.TumDetayGosterilsinMi = true;
						exceptionView.ExceptionGoster(new ExceptionBase(ExceptionManager.BuildErrorTitleMessage(exceptions)), ExceptionManager.BuildErrorMessage(exceptions));
					}
				}
				finally
				{
					exceptions.Clear();
				}
			}
		}
        
		public static void MesajGoster(Exception exception/*, LogTip logTip*/)
		{
			ExceptionManager.MesajGoster(exception.Message, exception/*, logTip*/);
		}
	}
}