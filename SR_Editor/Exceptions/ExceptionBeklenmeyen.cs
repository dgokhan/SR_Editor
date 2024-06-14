using SR_Editor.Core;
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;

namespace SR_Editor.Core.Exceptions
{
	public class ExceptionBeklenmeyen : ExceptionBase
	{
		private string aciklama;

		public override string Baslik
		{
			get
			{
				return "Beklenmeyen Hata".CeviriYap();
			}
		}

		public override string Mesaj
		{
			get
			{
				string str;
				str = this.Message;
				return str;
				if (this.aciklama == null)
				{
					Exception innerException = base.InnerException;
					if ((!base.InnerException.Message.Contains("See the inner exception") ? false : base.InnerException.InnerException != null))
					{
						innerException = base.InnerException.InnerException;
					}
					if (innerException is OutOfMemoryException)
					{
						GC.Collect();
						this.aciklama = "UYARI: Hafızada yeterli yer yok !\r\n\r\n Bellek kullanımı tekrar düzenlendi.";
						/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
						{
							this.aciklama = "There is not enough free memory!\r\n\r\n Memory usage has been edited again";
						}*/
					}
					else if (!(!(innerException is InvalidOperationException) ? true : !innerException.Source.Equals("System.Web.Services")))
					{
						this.aciklama = "Medula Bağlantı Hatası!\r\n\r\nDaha sonra tekrar deneyiniz!";
						/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
						{
							this.aciklama = " Web services connection error!\r\n\r\nPlease try again later!";
						}*/
					}
					else if (innerException is InvalidPrinterException)
					{
						this.aciklama = "Bilgisayarınıza Yüklenmiş bir Yazıcı bulunamadı!\r\nİşlem Durduruldu";
						/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
						{
							this.aciklama = "Printer not found";
						}*/
					}
					else if (innerException is IOException)
					{
						if ((!innerException.Message.StartsWith("The process cannot access the file") ? true : !innerException.Message.EndsWith("because it is being used by another process.")))
						{
							this.aciklama = "Beklenemeyen bir hata gerçekleşti. Dosyaya erişirken hata oluştu...";
						}
						else
						{
							int num = innerException.Message.LastIndexOf("\\");
							int num1 = innerException.Message.LastIndexOf("'");
							string str1 = innerException.Message.Substring(num + 1, num1 - num - 1);
							this.aciklama = string.Format("{0} dosyası başka bir program tarafından kullanılıyor!", str1);
							/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
							{
								this.aciklama = "The process cannot access the file";
							}*/
						}
						/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
						{
							this.aciklama = "The process cannot access the file";
						}*/
					}
					else if (innerException is SqlException)
					{
						SqlException sqlException = innerException as SqlException;
						int number = sqlException.Number;
						if (number == -2)
						{
							this.aciklama = "Veritabanı Zaman aşımı Hatası!\r\n\r\nLütfen Daha Sonra Tekrar Deneyiniz!";
							/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
							{
								this.aciklama = "Database Timeout Error!\r\n\r\nPlease try again later";
							}*/
						}
						else if (number == 8152)
						{
							this.aciklama = "Veritabanına Kayıt Sırasında Hata Oluştu!\r\nGirilen metin, veritabanında ayrılan alana göre çok büyük\r\n\r\nBilgi Sistemlerine Başvurunuz!";
							/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
							{
								this.aciklama = "Registration Error Occurred During Database!\r\nThe text entered is too big for the space allocated in the database";
							}*/
						}
						else
						{
							this.aciklama = string.Concat("Veritabanı hatası:\n", sqlException.Message);
							/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
							{
								this.aciklama = "Database error";
							}*/
						}
					}
					else if (!(innerException is SqlTypeException))
					{
						this.aciklama = base.Mesaj;
					}
					else
					{
						SqlTypeException sqlTypeException = innerException as SqlTypeException;
						/*if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 3)
						{
							this.aciklama = sqlTypeException.Message;
						}
						else */if (sqlTypeException.Message.Contains("chosen as the deadlock victim"))
						{
							this.aciklama = "Veritabanı Yoğunluğundan Dolayı İşlem Tamamlanamadı!\r\n\r\nLütfen Tekrar Deneyiniz!";
						}
						else if (sqlTypeException.Message.Contains("aktarım düzeyi hatası"))
						{
							this.aciklama = "Veritabanı Bağlantısı Kurulamadı!\r\n\r\nLütfen Daha Sonra Tekrar Deneyiniz!";
						}
						else if (sqlTypeException.Message.Contains("transport-level error"))
						{
							this.aciklama = "Veritabanı Bağlantısı Kurulamadı!\r\n\r\nLütfen Daha Sonra Tekrar Deneyiniz!";
						}
						else if (!sqlTypeException.Message.Contains("SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM."))
						{
							this.aciklama = "Veritabanına Kayıt Sırasında hata oluştu!\r\nLütfen Daha Sonra Tekrar Deneyiniz";
						}
						else
						{
							this.aciklama = "Veritabanına Kayıt Sırasında hata oluştu!\r\nGeçersiz tarih hatası!";
						}
					}
					if (this.aciklama != null)
					{
						this.aciklama = this.aciklama.CeviriYap();
					}
					this.aciklama = string.Format("{0}", this.aciklama);
					str = this.aciklama;
				}
				else
				{
					str = this.aciklama;
				}
				return str;
			}
		}

		public ExceptionBeklenmeyen(string message) : base(message)
		{
			base.HataTipi = ExceptionType.Error;
		}

		public ExceptionBeklenmeyen(string message, Exception innerException) : base(message, innerException)
		{
			base.HataTipi = ExceptionType.Error;
		}

		public ExceptionBeklenmeyen(string message, Exception innerException, Bitmap screen) : base(message, innerException, null, screen)
		{
			base.HataTipi = ExceptionType.Error;
		}

		public ExceptionBeklenmeyen(string message, Exception innerException, int? loginId, Bitmap screen) : base(message, innerException, loginId, screen)
		{
			base.HataTipi = ExceptionType.Error;
		}

		public override void HataOlusuncaGerceklestir()
		{
			base.HataOlusuncaGerceklestir();
			if (base.InnerException is OutOfMemoryException)
			{
				GC.Collect();
			}
		}
	}
}