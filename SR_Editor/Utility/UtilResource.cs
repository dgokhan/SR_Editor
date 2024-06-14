using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;

namespace SR_Editor.Core
{
	public class UtilResource
	{
		private ResourceManager stringResManager = null;

		private static UtilResource instance;

		private Dictionary<string, string> stringDictionary = new Dictionary<string, string>();

		private System.Reflection.Assembly assembly = null;

		private ResourceManager resManager = null;

		private Dictionary<string, Image> resimDictionary = new Dictionary<string, Image>();

		private System.Reflection.Assembly Assembly
		{
			get
			{
				if (this.assembly == null)
				{
					this.assembly = System.Reflection.Assembly.Load("Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
				}
				return this.assembly;
			}
		}

		public static UtilResource Instance
		{
			get
			{
				if (UtilResource.instance == null)
				{
					UtilResource.instance = new UtilResource();
				}
				return UtilResource.instance;
			}
		}

		private ResourceManager ResManager
		{
			get
			{
				if (this.resManager == null)
				{
					this.resManager = new ResourceManager("Editor.Properties.Resources", this.Assembly);
				}
				return this.resManager;
			}
		}

		static UtilResource()
		{
			UtilResource.instance = null;
		}

		public UtilResource()
		{
		}

		public Image ResimGetir(string adi)
		{
			Image image;
			Image obj = null;
			if (!this.resimDictionary.ContainsKey(adi))
			{
				string.Concat("Editor.Resources.", adi);
				obj = (Image)this.ResManager.GetObject(adi);
				if (obj == null)
				{
					if (!(adi != "yardim_16"))
					{
						goto Label1;
					}
					image = this.ResimGetir("yardim_16");
					return image;
				}
				else
				{
					this.resimDictionary.Add(adi, obj);
				}
			}
			else
			{
				obj = this.resimDictionary[adi];
            }
            Label1:
            image = obj;
			return image;
		}

		public Stream StreamGetir(string anahtar)
		{
			Stream manifestResourceStream;
			try
			{
				manifestResourceStream = this.Assembly.GetManifestResourceStream(anahtar);
				return manifestResourceStream;
			}
			catch (MissingManifestResourceException missingManifestResourceException)
			{
			}
			manifestResourceStream = null;
			return manifestResourceStream;
		}

		public string StringGetir(string anahtar)
		{
			string str = null;
			if (this.stringResManager == null)
			{
				this.stringResManager = new ResourceManager("Editor.LocalizationRes", this.Assembly);
			}
			if (!this.stringDictionary.TryGetValue(anahtar, out str))
			{
				lock (this)
				{
					if (str == null)
					{
						try
						{
							str = this.stringResManager.GetString(anahtar);
						}
						catch (MissingManifestResourceException missingManifestResourceException)
						{
						}
						if (this.stringDictionary.ContainsKey(anahtar))
						{
						}
						this.stringDictionary.Add(anahtar, str);
					}
				}
			}
			return str;
		}
	}
}