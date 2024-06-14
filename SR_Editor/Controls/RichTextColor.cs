using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTab;
using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SR_Editor.Core.Controls
{
	public class RichTextColor
	{
		private XtraTabControl tabControl;

		private Color resultColor;

		private BarItem itemFontColor;

		private PopupControlContainer container;

		private RichTextBox rtPad;

		private ColorCellsControl CellsControl
		{
			get
			{
				ColorCellsControl item = (ColorCellsControl)this.TabControl.TabPages[0].Controls[0];
				return item;
			}
		}

		private string CustomColorsName
		{
			get
			{
				return "CustomColors";
			}
		}

		public Color ResultColor
		{
			get
			{
				return this.resultColor;
			}
		}

		public XtraTabControl TabControl
		{
			get
			{
				return this.tabControl;
			}
		}

		public RichTextColor(PopupControlContainer container, BarItem item, RichTextBox rtPad)
		{
			this.container = container;
			this.itemFontColor = item;
			this.rtPad = rtPad;
			this.resultColor = Color.Empty;
			this.tabControl = this.CreateTabControl();
			this.TabControl.TabStop = false;
			XtraTabPageCollection tabPages = this.TabControl.TabPages;
			XtraTabPage[] xtraTabPage = new XtraTabPage[] { new XtraTabPage(), new XtraTabPage(), new XtraTabPage() };
			tabPages.AddRange(xtraTabPage);
			for (int i = 0; i < this.TabControl.TabPages.Count; i++)
			{
				this.SetTabPageProperties(i);
			}
			this.TabControl.Dock = DockStyle.Fill;
			this.container.Controls.AddRange(new Control[] { this.TabControl });
			this.container.Size = this.CalcFormSize();
		}

		public Size CalcFormSize()
		{
			Size bestSize = ColorCellsControlViewInfo.BestSize;
			bestSize.Height = this.GetNearestBestClientHeight(bestSize.Height, this.TabControl.TabPages[2].Controls[0] as ColorListBox);
			return this.TabControl.CalcSizeByPageClient(bestSize);
		}

		private ColorListBox CreateColorListBox()
		{
			return new ColorListBox();
		}

		private XtraTabControl CreateTabControl()
		{
			return new XtraTabControl();
		}

		private Size GetBestListBoxSize(ColorListBox colorBox)
		{
			int width = ColorCellsControlViewInfo.BestSize.Width;
			Size bestSize = ColorCellsControlViewInfo.BestSize;
			Size size = new Size(width, this.GetNearestBestClientHeight(bestSize.Height, colorBox));
			return size;
		}

		private int GetNearestBestClientHeight(int height, ColorListBox OwnerControl)
		{
			int num;
			MethodInfo method = typeof(BaseListBoxControl).GetMethod("CheckMinItemSize", BindingFlags.Instance | BindingFlags.NonPublic);
			object[] objArray = new object[] { 0 };
			int num1 = (int)method.Invoke(OwnerControl, objArray);
			int num2 = height / num1;
			num = (num2 * num1 != height ? (num2 + 1) * num1 : height);
			return num;
		}

		private void OnEnterColor(object sender, EnterColorEventArgs e)
		{
			this.resultColor = e.Color;
			this.OnEnterColor(e.Color);
		}

		private void OnEnterColor(Color clr)
		{
			this.container.HidePopup();
			this.rtPad.SelectionColor = clr;
			int imageIndex = this.itemFontColor.ImageIndex;
			ImageList images = this.itemFontColor.Images as ImageList;
			Bitmap item = (Bitmap)images.Images[imageIndex];
			Graphics graphic = Graphics.FromImage(item);
			Rectangle rectangle = new Rectangle(7, 7, 8, 8);
			graphic.FillRectangle(new SolidBrush(clr), rectangle);
			if (imageIndex == images.Images.Count - 1)
			{
				images.Images.RemoveAt(imageIndex);
			}
			images.Images.Add(item);
			this.itemFontColor.ImageIndex = images.Images.Count - 1;
		}

		private void SetTabPageProperties(int pageIndex)
		{
			XtraTabPage item = this.TabControl.TabPages[pageIndex];
			ColorListBox bestListBoxSize = null;
			BaseControl colorCellsControl = null;
			switch (pageIndex)
			{
				case 0:
				{
					item.Text = Localizer.Active.GetLocalizedString(StringId.ColorTabCustom);
					colorCellsControl = new ColorCellsControl(null);
					(colorCellsControl as ColorCellsControl).Properties = new RepositoryItemColorEdit()
					{
						ShowColorDialog = false
					};
					(colorCellsControl as ColorCellsControl).EnterColor += new EnterColorEventHandler(this.OnEnterColor);
					colorCellsControl.Size = ColorCellsControlViewInfo.BestSize;
					break;
				}
				case 1:
				{
					item.Text = Localizer.Active.GetLocalizedString(StringId.ColorTabWeb);
					bestListBoxSize = this.CreateColorListBox();
					bestListBoxSize.Items.AddRange(ColorListBoxViewInfo.WebColors);
					bestListBoxSize.EnterColor += new EnterColorEventHandler(this.OnEnterColor);
					bestListBoxSize.Size = this.GetBestListBoxSize(bestListBoxSize);
					colorCellsControl = bestListBoxSize;
					break;
				}
				case 2:
				{
					item.Text = Localizer.Active.GetLocalizedString(StringId.ColorTabSystem);
					bestListBoxSize = this.CreateColorListBox();
					bestListBoxSize.Items.AddRange(ColorListBoxViewInfo.SystemColors);
					bestListBoxSize.EnterColor += new EnterColorEventHandler(this.OnEnterColor);
					bestListBoxSize.Size = this.GetBestListBoxSize(bestListBoxSize);
					colorCellsControl = bestListBoxSize;
					break;
				}
			}
			colorCellsControl.Dock = DockStyle.Fill;
			colorCellsControl.BorderStyle = BorderStyles.NoBorder;
			colorCellsControl.LookAndFeel.ParentLookAndFeel = this.itemFontColor.Manager.GetController().LookAndFeel;
			item.Controls.Add(colorCellsControl);
		}
	}
}