using System;

namespace SR_Editor.Core.Utility
{
	public class FormTasarimComponent
	{
		private string formKodu;

		private string userControlAdi;

		private byte? tipiId;

		private string componentAdi;

		private object component;

		private byte[] deger;

		public object Component
		{
			get
			{
				return this.component;
			}
			set
			{
				this.component = value;
			}
		}

		public string ComponentAdi
		{
			get
			{
				return this.componentAdi;
			}
			set
			{
				this.componentAdi = value;
			}
		}

		public byte[] Deger
		{
			get
			{
				return this.deger;
			}
			set
			{
				this.deger = value;
			}
		}

		public string FormKodu
		{
			get
			{
				return this.formKodu;
			}
			set
			{
				this.formKodu = value;
			}
		}

		public byte? TipiId
		{
			get
			{
				return this.tipiId;
			}
			set
			{
				this.tipiId = value;
			}
		}

		public string UserControlAdi
		{
			get
			{
				return this.userControlAdi;
			}
			set
			{
				this.userControlAdi = value;
			}
		}

		public FormTasarimComponent()
		{
		}
	}
}