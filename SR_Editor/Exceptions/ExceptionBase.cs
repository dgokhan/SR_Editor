using SR_Editor.Core;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SR_Editor.Core.Exceptions
{
	public class ExceptionBase : Exception, IDisposable
	{
		public string AktifFormBaslik
		{
			get
			{
				string text;
				if (Form.ActiveForm != null)
				{
					text = Form.ActiveForm.Text;
				}
				else
				{
					text = null;
				}
				return text;
			}
		}

		public virtual string Baslik
		{
			get
			{
				return "Hata";
			}
		}

		public virtual string BilgisayarAdi
		{
			get
			{
				return Environment.MachineName;
			}
		}

		public Bitmap EkranResmi
		{
			get;
			set;
		}

		public virtual DateTime ExceptinDate
		{
			get
			{
				return UtilDateTime.Instance.Now;
			}
		}

		public ExceptionType HataTipi
		{
			get;
			set;
		}

		public int? LoginId
		{
			get;
			set;
		}

		public virtual string Mesaj
		{
			get
			{
				return this.Message;
			}
		}

		public virtual string WindowsKullaniciAdi
		{
			get
			{
				return Environment.UserName;
			}
		}

		public ExceptionBase(string message) : base(message)
		{
			this.HataTipi = ExceptionType.Error;
		}

		public ExceptionBase(string message, Exception innerException) : base(message, innerException)
		{
			this.HataTipi = ExceptionType.Error;
		}

		public ExceptionBase(string message, Exception innerException, int? loginId = null, Bitmap ekranResmi = null) : base(message, innerException)
		{
			this.EkranResmi = ekranResmi;
			this.LoginId = loginId;
			this.HataTipi = ExceptionType.Error;
		}

		public void Dispose()
		{
			if (this.EkranResmi != null)
			{
				this.EkranResmi.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		public virtual void HataOlusuncaGerceklestir()
		{
		}

		public override string ToString()
		{
			string str = string.Format(string.Concat("{0} ", Environment.NewLine, Environment.NewLine, " {1}"), this.Mesaj, this.StackTrace);
			return str;
		}
	}
}