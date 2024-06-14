using DevExpress.Utils;
using DevExpress.XtraBars;
using SR_Editor.LookUp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UtilEditorImage
	{
		private static UtilEditorImage instance;

		private static object lockObject;

		private ImageList imageList;

		private ImageList smallImageList;

		private ImageList mediumImageList;

		private ImageList largeImageList;

		public Image Add
		{
			get
			{
				return this.imageList.Images["Add.png"];
			}
		}

		public Image ApacheIIScore
		{
			get
			{
				return this.imageList.Images["ApacheIIScore.png"];
			}
		}

		public Image Cancel
		{
			get
			{
				return this.imageList.Images["Cancel.png"];
			}
		}

		public Image Delete
		{
			get
			{
				return this.imageList.Images["Delete.png"];
			}
		}

		public Image Finans
		{
			get
			{
				return this.imageList.Images["MainModule.Finans.png"];
			}
		}

		public Image HastaKabul
		{
			get
			{
				return this.imageList.Images["MainModule.HastaKabul.png"];
			}
		}

		public Image IK
		{
			get
			{
				return this.imageList.Images["MainModule.IK.png"];
			}
		}

		public Image Information
		{
			get
			{
				return this.imageList.Images["Information.png"];
			}
		}

		public static UtilEditorImage Instance
		{
			get
			{
				if (UtilEditorImage.instance == null)
				{
					lock (UtilEditorImage.lockObject)
					{
						if (UtilEditorImage.instance == null)
						{
							UtilEditorImage.instance = new UtilEditorImage();
						}
					}
				}
				return UtilEditorImage.instance;
			}
		}

		public Image Laboratuar
		{
			get
			{
				return this.imageList.Images["MainModule.Laboratuar.png"];
			}
		}

		public Image MedikalFinans
		{
			get
			{
				return this.imageList.Images["MainModule.MedikalFinans.png"];
			}
		}

		public Image Pazarlama
		{
			get
			{
				return this.imageList.Images["MainModule.Pazarlama.png"];
			}
		}

		public Image Radyoloji
		{
			get
			{
				return this.imageList.Images["MainModule.Radyoloji.png"];
			}
		}

		public Image Randevu
		{
			get
			{
				return this.imageList.Images["MainModule.Randevu.png"];
			}
		}

		public Image Rapor
		{
			get
			{
				return this.imageList.Images["MainModule.Rapor.png"];
			}
		}

		public Image Refresh
		{
			get
			{
				return this.imageList.Images["Refresh.png"];
			}
		}

		public Image SatinAlma
		{
			get
			{
				return this.imageList.Images["MainModule.Satinalma.png"];
			}
		}

		public Image Save
		{
			get
			{
				return this.imageList.Images["Save.png"];
			}
		}

		public Image Search
		{
			get
			{
				return this.imageList.Images["SearchSmall.png"];
			}
		}

		public Image Sistem
		{
			get
			{
				return this.imageList.Images["MainModule.Sistem.png"];
			}
		}

		public Image Stok
		{
			get
			{
				return this.imageList.Images["MainModule.Stok.png"];
			}
		}

		public Image Tedavi
		{
			get
			{
				return this.imageList.Images["MainModule.Tedavi.png"];
			}
		}

		public Image Warning
		{
			get
			{
				return this.imageList.Images["Warning.png"];
			}
		}

		static UtilEditorImage()
		{
			UtilEditorImage.lockObject = new object();
		}

		private UtilEditorImage()
		{
			this.imageList = new ImageList()
			{
				ImageSize = new Size(32, 32),
				ColorDepth = ColorDepth.Depth32Bit,
				TransparentColor = Color.Transparent
			};
			this.smallImageList = new ImageList()
			{
				ImageSize = new Size(16, 16),
				ColorDepth = ColorDepth.Depth32Bit,

                TransparentColor = Color.Transparent
            };
			this.mediumImageList = new ImageList()
			{
				ImageSize = new Size(24, 24),
				ColorDepth = ColorDepth.Depth32Bit,

                TransparentColor = Color.Transparent
            };
			this.largeImageList = new ImageList()
			{
				ImageSize = new Size(32, 32),
				ColorDepth = ColorDepth.Depth32Bit,

                TransparentColor = Color.Transparent
            };
			this.Load();
		}

		public Image GetDefaultImage(string pImageName)
		{
			Image item = null;
			if (!this.imageList.Images.ContainsKey(pImageName))
			{
				//UtilMessage.Show(EnumUtilMessage.ResimBulunamadiKontrol, null, "Resim bulunamadı.");
			}
			else
			{
				item = this.imageList.Images[pImageName];
			}
			return item;
		}

		public Image GetLargeImage(string pImageName)
		{
			Image item = null;
			if (!this.largeImageList.Images.ContainsKey(pImageName))
			{
				if (this.imageList.Images.ContainsKey(pImageName))
				{
					item = this.imageList.Images[pImageName];
					this.largeImageList.Images.Add(pImageName, this.GetScaledImage(item, 32, 32));
				}
			}
			if (!this.largeImageList.Images.ContainsKey(pImageName))
			{
				//UtilMessage.Show(EnumUtilMessage.ResimBulunamadiKontrol, null, "Resim bulunamadı.");
			}
			else
			{
				item = this.largeImageList.Images[pImageName];
			}
			return item;
		}

		public Image GetMainModuleImage(string pImageName)
		{
			return this.GetMediumImage(string.Concat("MainModule.", pImageName));
		}

        public Image GetSubModuleImage(string pImageName)
        {
            return GetResourceImage(string.Concat("SubModule.", pImageName));//this.GetMediumImage(string.Concat("SubModule.", pImageName));
        }

        public Image GetMediumImage(string pImageName)
		{
			Image item = null;
			if (!this.mediumImageList.Images.ContainsKey(pImageName))
			{
				if (this.imageList.Images.ContainsKey(pImageName))
				{
					item = this.imageList.Images[pImageName];
					this.mediumImageList.Images.Add(pImageName, /*this.GetScaledImage(item, 24, 24)*/item);
				}
			}
			if (!this.mediumImageList.Images.ContainsKey(pImageName))
			{
				//UtilMessage.Show(EnumUtilMessage.ResimBulunamadiKontrol, null, "Resim bulunamadı.");
			}
			else
			{
				item = this.mediumImageList.Images[pImageName];
			}
			return item;
		}

		public Image GetResourceImage(string pImageName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			Image image = ResourceImageHelper.CreateImageFromResources(string.Concat("Editor.Core.Resources.", pImageName), executingAssembly);
			return image;
		}

		private Image GetScaledImage(Image image, int width, int height)
		{
            return image;
			Image image1;
			bool flag;
			if (image == null)
			{
				flag = true;
			}
			else
			{
				Size size = image.Size;
				flag = size.Equals(new Size(width, height));
			}
			if (flag)
			{
				image1 = image;
			}
			else
			{
				Bitmap bitmap = new Bitmap(width, height);
				Graphics graphic = Graphics.FromImage(bitmap);
				graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphic.DrawImage(image, 0, 0, width, height);
				bitmap.MakeTransparent();
				image1 = bitmap;
			}
			return image1;
		}

		public Image GetSmallImage(string pImageName)
		{
			Image item = null;
			if (!this.smallImageList.Images.ContainsKey(pImageName))
			{
				if (this.imageList.Images.ContainsKey(pImageName))
				{
					item = this.imageList.Images[pImageName];
					this.smallImageList.Images.Add(pImageName, this.GetScaledImage(item, 16, 16));
				}
			}
			if (!this.smallImageList.Images.ContainsKey(pImageName))
			{
				//UtilMessage.Show(EnumUtilMessage.ResimBulunamadiKontrol, null, "Resim bulunamadı.");
			}
			else
			{
				item = this.smallImageList.Images[pImageName];
			}
			return item;
		}

		private void Load()
		{
			/*this.LoadImage("StandartButton.Add.png");
			this.LoadImage("StandartButton.Cancel.png");
			this.LoadImage("StandartButton.Delete.png");
			this.LoadImage("StandartButton.Information.png");
			this.LoadImage("StandartButton.Refresh.png");
			this.LoadImage("StandartButton.Save.png");
			this.LoadImage("StandartButton.Warning.png");
			this.LoadImage("StandartButton.SearchSmall.png");
			this.LoadImage("StandartButton.Folder1Documents.png");
			this.LoadImage("StandartButton.Folder1.png");*/
			//this.LoadImage("MainModule.Finans.png");
			this.LoadImage("MainModule.HastaKabul.png");
			this.LoadImage("MainModule.IK.png");
			//this.LoadImage("MainModule.Laboratuar.png");
			//this.LoadImage("MainModule.MedikalFinans.png");
			//this.LoadImage("MainModule.Pazarlama.png");
			//this.LoadImage("MainModule.Radyoloji.png");
			//this.LoadImage("MainModule.Randevu.png");
			//this.LoadImage("MainModule.Rapor.png");
			//this.LoadImage("MainModule.Satinalma.png");
			this.LoadImage("MainModule.Sistem.png");
			//this.LoadImage("MainModule.Stok.png");
			this.LoadImage("MainModule.Tedavi.png");
		}

		public void LoadImage(string pImageName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (!this.imageList.Images.ContainsKey(pImageName))
			{
				Image image = ResourceImageHelper.CreateImageFromResources(string.Concat("Editor.Core.Resources.", pImageName), executingAssembly);
				this.imageList.Images.Add(pImageName, image);
			}
		}

		public void SetImage(BarItem pBarItem, string pImageName)
		{
			pBarItem.Glyph = this.GetSmallImage(pImageName);
			pBarItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
		}
	}
}