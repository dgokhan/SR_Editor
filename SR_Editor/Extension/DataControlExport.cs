using DevExpress.DXperience.Demos;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class DataControlExport
	{
		private PopupMenu popUpMenu;

		private Editor.Core.QBaseEntity qBaseEntity;

		private BaseView exportView;

		private string registryKey;

		public BaseView ExportView
		{
			get
			{
				return this.exportView;
			}
			set
			{
				this.exportView = value;
			}
		}

		public PopupMenu PopUpMenu
		{
			get
			{
				return this.popUpMenu;
			}
			set
			{
				this.popUpMenu = value;
			}
		}

		public Editor.Core.QBaseEntity QBaseEntity
		{
			get
			{
				return this.qBaseEntity;
			}
			set
			{
				this.qBaseEntity = value;
			}
		}

		public string RegistryKey
		{
			get
			{
				return this.registryKey;
			}
			set
			{
				this.registryKey = value;
			}
		}

		public DataControlExport(Editor.Core.QBaseEntity pQBaseEntity, PopupMenu pPopUpMenu, BaseView pBaseView, string pRegistryKeyName)
		{
			this.ExportView = pBaseView;
			this.qBaseEntity = pQBaseEntity;
			if (pPopUpMenu != null)
			{
				this.PopUpMenu = pPopUpMenu;
			}
			else
			{
				this.PopUpMenu = new PopupMenu(new BarManager());
			}
			this.InitExportMenuItems();
		}

		private void AddExportMenuItem(BarSubItem pParentItem, DataControlExport.DataExportItem pDataExportItem)
		{
			BarButtonItem barButtonItem = new BarButtonItem()
			{
				Id = this.PopUpMenu.Manager.GetNewItemId(),
				Hint = pDataExportItem.Caption,
				Caption = pDataExportItem.Caption,
				Tag = pDataExportItem,
				PaintStyle = BarItemPaintStyle.Caption
			};
			pParentItem.Manager.Items.Add(barButtonItem);
			pParentItem.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItem));
			barButtonItem.ItemShortcut = pDataExportItem.Shortcut;
			barButtonItem.ItemClick += new ItemClickEventHandler(this.MenuItem_ItemClick);
		}

		private void AddRegisterMenuItem(BarSubItem pParentItem, string pCaption)
		{
			BarButtonItem barButtonItem = new BarButtonItem()
			{
				Id = this.PopUpMenu.Manager.GetNewItemId(),
				Hint = pCaption,
				Caption = pCaption,
				PaintStyle = BarItemPaintStyle.Caption
			};
			pParentItem.Manager.Items.Add(barButtonItem);
			pParentItem.LinksPersistInfo.Add(new LinkPersistInfo(barButtonItem));
			barButtonItem.ItemClick += new ItemClickEventHandler(this.RegistryMenuItem_ItemClick);
		}

		private void ExportTo(string ext, string filter)
		{
			string fileName = MainFormHelper.GetFileName(string.Format("*.{0}", ext), filter);
			if (!string.IsNullOrEmpty(fileName))
			{
				try
				{
					this.ExportToCore(fileName, ext);
					MainFormHelper.OpenExportedFile(fileName);
				}
				catch
				{
					MainFormHelper.ShowExportErrorMessage();
				}
			}
		}

		private void ExportToCore(string filename, string ext)
		{
			if (this.ExportView != null)
			{
				Cursor current = Cursor.Current;
				Cursor.Current = Cursors.WaitCursor;
				if (ext == "rtf")
				{
					this.ExportView.ExportToRtf(filename);
				}
				if (ext == "pdf")
				{
					this.ExportView.ExportToPdf(filename);
				}
				if (ext == "mht")
				{
					this.ExportView.ExportToMht(filename);
				}
				if (ext == "html")
				{
					this.ExportView.ExportToHtml(filename);
				}
				if (ext == "txt")
				{
					this.ExportView.ExportToText(filename);
				}
				if (ext == "xls")
				{
					this.ExportView.ExportToXls(filename);
				}
				if (ext == "xlsx")
				{
					this.ExportView.ExportToXlsx(filename);
				}
				Cursor.Current = current;
			}
		}

		private void ExportToHTML()
		{
			this.ExportTo("html", "HTML document (*.html)|*.html");
		}

		private void ExportToMHT()
		{
			this.ExportTo("mht", "MHT document (*.mht)|*.mht");
		}

		private void ExportToPDF()
		{
			this.ExportTo("pdf", "PDF document (*.pdf)|*.pdf");
		}

		private void ExportToRTF()
		{
			this.ExportTo("rtf", "RTF document (*.rtf)|*.rtf");
		}

		private void ExportToText()
		{
			this.ExportTo("txt", "Text document (*.txt)|*.txt");
		}

		private void ExportToXLS()
		{
			this.ExportTo("xls", "XLS document (*.xls)|*.xls");
		}

		private void ExportToXLSX()
		{
			this.ExportTo("xlsx", "XLSX document (*.xlsx)|*.xlsx");
		}

		public void InitExportMenuItems()
		{
			BarSubItem barSubItem = new BarSubItem()
			{
				Id = this.PopUpMenu.Manager.GetNewItemId(),
				Hint = "Veri Aktar",
				Caption = "Veri Aktar",
				PaintStyle = BarItemPaintStyle.Caption
			};
			this.PopUpMenu.Manager.Items.Add(barSubItem);
			this.PopUpMenu.LinksPersistInfo.Add(new LinkPersistInfo(barSubItem));
			DataControlExport.DataExportItem dataExportItem = new DataControlExport.DataExportItem()
			{
				Caption = "Excel(xls)",
				Extension = "xls"
			};
			this.AddExportMenuItem(barSubItem, dataExportItem);
			DataControlExport.DataExportItem dataExportItem1 = new DataControlExport.DataExportItem()
			{
				Caption = "Excel(xlsx)",
				Extension = "xlsx"
			};
			this.AddExportMenuItem(barSubItem, dataExportItem1);
			DataControlExport.DataExportItem dataExportItem2 = new DataControlExport.DataExportItem()
			{
				Caption = "Pdf",
				Extension = "pdf"
			};
			this.AddExportMenuItem(barSubItem, dataExportItem2);
			DataControlExport.DataExportItem dataExportItem3 = new DataControlExport.DataExportItem()
			{
				Caption = "Html",
				Extension = "html"
			};
			this.AddExportMenuItem(barSubItem, dataExportItem3);
			DataControlExport.DataExportItem dataExportItem4 = new DataControlExport.DataExportItem()
			{
				Caption = "Mht",
				Extension = "mht"
			};
			this.AddExportMenuItem(barSubItem, dataExportItem4);
			DataControlExport.DataExportItem dataExportItem5 = new DataControlExport.DataExportItem()
			{
				Caption = "Rtf",
				Extension = "rtf"
			};
			this.AddExportMenuItem(barSubItem, dataExportItem5);
			DataControlExport.DataExportItem dataExportItem6 = new DataControlExport.DataExportItem()
			{
				Caption = "Txt",
				Extension = "txt"
			};
			this.AddExportMenuItem(barSubItem, dataExportItem6);
		}

		public void InitRegisterMenuItems(string pRegistryKeyName)
		{
			BarSubItem barSubItem = new BarSubItem()
			{
				Id = this.PopUpMenu.Manager.GetNewItemId(),
				Hint = "Şablon İşlemleri",
				Caption = "Şablon İşlemleri",
				PaintStyle = BarItemPaintStyle.Caption
			};
			this.PopUpMenu.Manager.Items.Add(barSubItem);
			this.PopUpMenu.LinksPersistInfo.Add(new LinkPersistInfo(barSubItem));
		}

		private void MenuItem_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.Item.Tag is DataControlExport.DataExportItem)
			{
				string extension = (e.Item.Tag as DataControlExport.DataExportItem).Extension;
				if (extension != null)
				{
					switch (extension)
					{
						case "pdf":
						{
							this.ExportToPDF();
							break;
						}
						case "html":
						{
							this.ExportToHTML();
							break;
						}
						case "mht":
						{
							this.ExportToMHT();
							break;
						}
						case "xls":
						{
							this.ExportToXLS();
							break;
						}
						case "xlsx":
						{
							this.ExportToXLSX();
							break;
						}
						case "rtf":
						{
							this.ExportToRTF();
							break;
						}
						case "txt":
						{
							this.ExportToText();
							break;
						}
					}
				}
			}
		}

		private void RegistryMenuItem_ItemClick(object sender, ItemClickEventArgs e)
		{
		}

		private class DataExportItem
		{
			private string extension;

			private string caption;

			private BarShortcut shortcut;

			public string Caption
			{
				get
				{
					return this.caption;
				}
				set
				{
					this.caption = value;
				}
			}

			public string Extension
			{
				get
				{
					return this.extension;
				}
				set
				{
					this.extension = value;
				}
			}

			public BarShortcut Shortcut
			{
				get
				{
					return this.shortcut;
				}
				set
				{
					this.shortcut = value;
				}
			}

			public DataExportItem()
			{
			}
		}
	}
}