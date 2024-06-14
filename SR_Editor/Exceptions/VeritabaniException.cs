using System;
using System.Net;

namespace SR_Editor.Core.Exceptions
{
	public class VeritabaniException : ExceptionBase
	{
		public override string Baslik
		{
			get
			{
				return "Web İşlemi Hatası";
			}
		}

		public override string Mesaj
		{
			get
			{
				string mesaj;
				if (!(base.InnerException is WebException))
				{
					mesaj = base.Mesaj;
				}
				else
				{
					WebExceptionStatus status = (base.InnerException as WebException).Status;
					string str = "";
					switch (status)
					{
						case WebExceptionStatus.Success:
						{
							str = "Başarılı";
							break;
						}
						case WebExceptionStatus.NameResolutionFailure:
						{
							str = "Adres çözümleme hatası";
							break;
						}
						case WebExceptionStatus.ConnectFailure:
						{
							str = "Bağlantı hatası";
							break;
						}
						case WebExceptionStatus.ReceiveFailure:
						{
							str = "Web Cevap alma hatası";
							break;
						}
						case WebExceptionStatus.SendFailure:
						{
							str = "Web gönderim hatası";
							break;
						}
						case WebExceptionStatus.PipelineFailure:
						{
							str = "İş hattı hatası";
							break;
						}
						case WebExceptionStatus.RequestCanceled:
						{
							str = "Talep iptali";
							break;
						}
						case WebExceptionStatus.ProtocolError:
						{
							str = "HTTP protokol hatası";
							break;
						}
						case WebExceptionStatus.ConnectionClosed:
						{
							str = "Bağlantı kapalı";
							break;
						}
						case WebExceptionStatus.TrustFailure:
						{
							str = "Güvenlik seviyesi hatası";
							break;
						}
						case WebExceptionStatus.ServerProtocolViolation:
						{
							break;
						}
						case WebExceptionStatus.KeepAliveFailure:
						{
							str = "Bağlantı açık tutulamadı";
							break;
						}
						case WebExceptionStatus.Pending:
						{
							str = "Bekleme hatası";
							break;
						}
						case WebExceptionStatus.Timeout:
						{
							str = "Zaman aşımına uğrandı";
							break;
						}
						case WebExceptionStatus.ProxyNameResolutionFailure:
						{
							str = "HTTP protokol hatası";
							break;
						}
						case WebExceptionStatus.UnknownError:
						{
							str = "Bilinmeyen Web hatası";
							break;
						}
						case WebExceptionStatus.MessageLengthLimitExceeded:
						{
							str = "Mesaj boyutu çok büyük";
							break;
						}
						case WebExceptionStatus.CacheEntryNotFound:
						{
							break;
						}
						case WebExceptionStatus.RequestProhibitedByCachePolicy:
						{
							break;
						}
						case WebExceptionStatus.RequestProhibitedByProxy:
						{
							str = "Proxy engelleme hatası";
							break;
						}
					}
					str = string.Concat(str, string.Format(" ({0})", status));
					mesaj = str;
				}
				return mesaj;
			}
		}

		public VeritabaniException(string message) : base(message)
		{
		}

		public VeritabaniException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}