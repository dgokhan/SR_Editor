using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraNavBar;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Menu;
using DevExpress.XtraTreeList.Nodes;
using SR_Editor.Core.Utility;
using SR_Editor.Framework;
using SR_Editor.LookUp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class FormBase : XtraForm
	{
		private UtilParameters formParams;

		private DXValidationProvider dxValidationProvider;

		private bool isSaveOnClose = false;

		private bool isModified = false;

		private bool immediateClose = true;

		private bool isEditMode = false;

		private bool isDialog = false;

		private IContainer components;

		private List<FormTasarimComponent> listFormTasarimComponent = new List<FormTasarimComponent>();

		private Bar barTasarim;

		private ModuleInfo moduleInfo;

        //private CoreEntities coreEntities;

        public static SocketClient client;

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UtilParameters FormParams
		{
			get
			{
				if (this.formParams == null)
				{
					this.formParams = new UtilParameters();
				}
				return this.formParams;
			}
			set
			{
				this.formParams = value;
				this.FormParamsChanged();
			}
		}

		[Category("Editor")]
		[DefaultValue(false)]
		public bool ImmediateClose
		{
			get
			{
				return this.immediateClose;
			}
			set
			{
				this.immediateClose = value;
			}
		}

		[Category("Editor")]
		[DefaultValue(false)]
		public bool IsDialog
		{
			get
			{
				return this.isDialog;
			}
			set
			{
				this.isDialog = value;
			}
		}

		public bool IsEditMode
		{
			get
			{
				return this.isEditMode;
			}
			set
			{
				this.isEditMode = value;
			}
		}

		public bool IsModified
		{
			get
			{
				return this.isModified;
			}
			set
			{
				this.isModified = value;
			}
		}

		public bool IsSaveOnClose
		{
			get
			{
				return this.isSaveOnClose;
			}
			set
			{
				this.isSaveOnClose = value;
			}
		}

		public List<FormTasarimComponent> ListFormTasarimComponent
		{
			get
			{
				return this.listFormTasarimComponent;
			}
		}

		public ModuleInfo Module
		{
			get
			{
				return this.moduleInfo;
			}
			set
			{
				this.moduleInfo = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DXValidationProvider ValidationProvider
		{
			get
			{
				return this.dxValidationProvider;
			}
		}

		public FormBase()
		{
			this.InitializeComponent();
		}

		private void barButtonItemTasarimKaydet_ItemClick(object sender, ItemClickEventArgs e)
		{
			this.TasarimKaydet();
		}

		private void barButtonItemTasarimOrjinaleDon_ItemClick(object sender, ItemClickEventArgs e)
		{
			/*if (UtilMessage.Show(EnumUtilMessage.OrjinalTasarimaDonOnayKontrol, null, "Yapılan tasarım silinecek?\rDevam etmek istiyormusunuz?", "Orjinale Dön", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.Yes)
			{
				this.OrjinalTasarimaDon();
				UtilMessage.Show(EnumUtilMessage.TasarimYuklendiIslemSonuc, null, "Orjinal tasarım yüklendi lütfen ekranı kapatıp tekrar açınız!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}*/
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (System.Windows.Forms.Control control in base.Controls)
				{
					control.Dispose();
				}
				this.Module = null;
			}
			base.Dispose(disposing);
		}

		private void FindAndFocusNode(TreeList treeList, string sFind)
		{
			bool str;
			int num = 0;
			TreeListNode treeListNode = (treeList.FocusedNode.NextVisibleNode == null ? treeList.Nodes[0] : treeList.FocusedNode.NextVisibleNode);
			object obj = (treeList.FocusedColumn != null ? treeList.FocusedColumn : treeList.Columns[0]);
			while (num < treeList.VisibleNodesCount)
			{
				object value = treeListNode.GetValue(obj);
				if (value != null)
				{
					string str1 = value.ToString();
					if (string.IsNullOrEmpty(str1))
					{
						str = true;
					}
					else
					{
						char chr = str1[0];
						str = !(chr.ToString() == sFind);
					}
					if (!str)
					{
						treeListNode.Selected = true;
						treeList.FocusedNode = treeListNode;
						break;
					}
				}
				num++;
				treeListNode = (treeListNode.NextVisibleNode == null ? treeList.Nodes[0] : treeListNode.NextVisibleNode);
			}
		}

		public virtual void FormParamsChanged()
		{
            if(client != null)
            {
                //client.onReceiveHandlers += Client_onReceive;
            }
		}

        public virtual void Client_onReceive(Packet p)
        {

        }

        private void FormEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			bool closeReason = e.CloseReason == System.Windows.Forms.CloseReason.UserClosing | e.CloseReason == System.Windows.Forms.CloseReason.None;
			if ((!this.isModified || !closeReason ? false : this.isSaveOnClose))
			{
				System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Yes;
				/*dialogResult = UtilMessage.Show(EnumUtilMessage.DegisikliklerKaydetOnayKontrol, null, "Değişiklikleri kaydetmek istiyor musunuz?", this.Text, MessageBoxButtons.YesNoCancel);
				if (dialogResult != System.Windows.Forms.DialogResult.Yes)
				{
					if (dialogResult != System.Windows.Forms.DialogResult.Cancel)
					{
						goto Label1;
					}
					e.Cancel = true;
					return;
				}
				else
				{
					try
					{
						if (!this.Kaydet())
						{
							e.Cancel = true;
						}
					}
					catch (Exception exception)
					{
						throw exception;
					}
				}*/
            }
            Label1:
            if (this.FormParams != null)
			{
				this.FormParams.DialogResult = base.DialogResult;
			}
			this.Dispose(true);
			GC.Collect();
		}

		private void FormEditor_FormShown(object sender, EventArgs e)
		{
			//this.TasarimButtonuEkle();
			//this.TasarimYukle();
			/*if ((Editor.Core.EditorApplication.EditorApplication.LanguageId != 1 ? true : Editor.Core.EditorApplication.EditorApplication.SecondLanguageId != 1))
			{
				UtilLanguage.FormCeviriYap(this);
			}*/
		}

		private void FormEditor_Load(object sender, EventArgs e)
		{

		}

		protected void InitControl(GridView pGridView, PopupMenu pPopUpMenu)
		{
			pGridView.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.PopupMenuShowing);
			pGridView.Tag = pPopUpMenu;
		}

		protected void InitControl(TreeList pTreeList, PopupMenu pPopUpMenu)
		{
			pTreeList.PopupMenuShowing += new DevExpress.XtraTreeList.PopupMenuShowingEventHandler(this.pTreeList_PopupMenuShowing);
			pTreeList.Tag = pPopUpMenu;
		}

		private void InitializeComponent()
		{
			this.dxValidationProvider = new DXValidationProvider();
			((ISupportInitialize)this.dxValidationProvider).BeginInit();
			base.SuspendLayout();
			this.dxValidationProvider.ValidateHiddenControls = false;
			this.dxValidationProvider.ValidationMode = ValidationMode.Manual;
			base.ClientSize = new System.Drawing.Size(609, 514);
			base.Name = "FormBase";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Editor Form";
			base.FormClosing += new FormClosingEventHandler(this.FormEditor_FormClosing);
			base.Load += new EventHandler(this.FormEditor_Load);
			base.Shown += new EventHandler(this.FormEditor_FormShown);
			((ISupportInitialize)this.dxValidationProvider).EndInit();
			base.ResumeLayout(false);
		}

		public void InitNavBarGroup(NavBarGroup navBarGroup)
		{
			FormBase formBase = this;
			navBarGroup.ControlContainer.Controls[0].VisibleChanged += new EventHandler(formBase.SetNavBarFocus);
		}

		public virtual void InitTasarimBar(Bar pBar)
		{
			if (this.barTasarim.IsBos())
			{
				this.barTasarim = pBar;
			}
		}

		public virtual void InitTasarimsalCompenent(object pControl, string pUserControlAdi)
		{
			if ((!this.listFormTasarimComponent.IsDolu() || !pControl.IsDolu() || !this.Module.IsDolu() ? false : this.Module.FullKey.IsDolu()))
			{
				if (pControl is LayoutControl)
				{
					this.TasarimsalLayoutControlEkle(pControl as LayoutControl, pUserControlAdi);
				}
				else if (pControl is List<object>)
				{
					foreach (object obj in pControl as List<object>)
					{
						if (obj is LayoutControl)
						{
							this.TasarimsalLayoutControlEkle(obj as LayoutControl, pUserControlAdi);
						}
					}
				}
			}
		}

		protected virtual bool Kaydet()
		{
			return true;
		}

		protected virtual void Listele()
		{
		}

		private void OrjinalTasarimaDon()
		{
			/*if (this.coreEntities.IsBos())
			{
				this.coreEntities = CoreEntities.Yeni();
			}
			byte? nullable = null;
			this.coreEntities.FormTasarimQuery.AktifFormTasarimlariniIptalEt(this.Module.FullKey, null, nullable);
			foreach (FormTasarimGrubu byFormKodu in this.coreEntities.FormTasarimGrubuQuery.GetByFormKodu(this.Module.FullKey))
			{
				this.coreEntities.FormTasarimGrubuAdapter.Iptal(byFormKodu);
			}
			foreach (FormTasarimKullanici byFormTasarimsalKullaniciYonetim in this.coreEntities.FormTasarimKullaniciQuery.GetByFormTasarimsalKullaniciYonetim(this.Module.FullKey))
			{
				this.coreEntities.FormTasarimKullaniciAdapter.Iptal(byFormTasarimsalKullaniciYonetim);
			}
			this.coreEntities.SaveChanges();*/
		}

		private void PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
		{
			this.PopupMenuShowing(sender, e, (PopupMenu)(sender as GridView).Tag);
		}

		private void PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e, PopupMenu pPopupMenu)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				//(new TurkceGridMenuIslemleri(e)).MenuHazirla();
			}
			else if (!(e.MenuType != GridMenuType.Row ? true : !(sender as GridView).IsDataRow(e.HitInfo.RowHandle)))
			{
				if (pPopupMenu != null)
				{
					pPopupMenu.ShowPopup(System.Windows.Forms.Control.MousePosition);
				}
			}
			else if (e.MenuType == GridMenuType.User)
			{
				if (pPopupMenu != null)
				{
					pPopupMenu.ShowPopup(System.Windows.Forms.Control.MousePosition);
				}
			}
		}

		private void PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e, PopupMenu pPopupMenu)
		{
			if (e.Menu.MenuType == TreeListMenuType.Column)
			{
				//(new TurkceTreeListMenuIslemleri(e)).MenuHazirla();
			}
			else if (!(e.Menu.MenuType != TreeListMenuType.Node ? true : (sender as TreeList).FocusedNode == null))
			{
				if (pPopupMenu != null)
				{
					pPopupMenu.ShowPopup(System.Windows.Forms.Control.MousePosition);
				}
			}
			else if (e.Menu.MenuType == TreeListMenuType.User)
			{
				if (pPopupMenu != null)
				{
					pPopupMenu.ShowPopup(System.Windows.Forms.Control.MousePosition);
				}
			}
		}

		private void pTreeList_PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
		{
			this.PopupMenuShowing(sender, e, (PopupMenu)(sender as TreeList).Tag);
		}

		private void ReadOnly_PropertiesChanged(object sender, EventArgs e)
		{
			if (!(!((RepositoryItem)sender).ReadOnly ? true : !(((RepositoryItem)sender).Appearance.BackColor != Color.LightGray)))
			{
				((RepositoryItem)sender).Appearance.BackColor2 = ((RepositoryItem)sender).AppearanceFocused.BackColor2;
				((RepositoryItem)sender).Appearance.BackColor = Color.LightGray;
			}
			else if ((((RepositoryItem)sender).ReadOnly ? false : ((RepositoryItem)sender).Appearance.BackColor == Color.LightGray))
			{
				((RepositoryItem)sender).Appearance.BackColor = ((RepositoryItem)sender).AppearanceDisabled.BackColor2;
			}
		}

		public virtual EditorIslemSonuc SaveChanges()
		{
			return new EditorIslemSonuc(EnumSonucTipi.IslemBasarili);
		}

		protected void SetDefaultHandlers(System.Windows.Forms.Control control)
		{
			if (control is BaseEdit)
			{
				if (!(control is CheckEdit))
				{
					(control as BaseEdit).Properties.AppearanceFocused.BackColor = Color.LightSeaGreen;
					(control as BaseEdit).Properties.AppearanceFocused.BackColor2 = (control as BaseEdit).BackColor;
					(control as BaseEdit).Properties.AppearanceDisabled.BackColor = Color.LightGray;
				}
				(control as BaseEdit).PropertiesChanged += new EventHandler(this.ReadOnly_PropertiesChanged);
			}
			else if (control is TreeList)
			{
				(control as TreeList).KeyPress += new KeyPressEventHandler(this.TreeList_KeyPress);
			}
			else if (control is LayoutControl)
			{
				for (int i = 0; i < ((LayoutControl)control).Items.Count; i++)
				{
					if (((LayoutControl)control).Items[i] is LayoutControlItem && ((LayoutControlItem)((LayoutControl)control).Items[i]).Control != null)
					{
						this.SetDefaultHandlers(((LayoutControlItem)((LayoutControl)control).Items[i]).Control);
					}
				}
			}
			foreach (System.Windows.Forms.Control control1 in control.Controls)
			{
				this.SetDefaultHandlers(control1);
			}
		}

		protected void SetDefaultHandlers()
		{
			this.SetDefaultHandlers(this);
		}

		public void SetMerkezModulInfo(System.Windows.Forms.Control control)
		{
			if (control is BaseEdit)
			{
				BaseEdit lightSeaGreen = control as BaseEdit;
				if ((lightSeaGreen.DataBindings == null ? false : lightSeaGreen.DataBindings.Count > 0))
				{
					/*if (BaseEntityAdapter.ListMerkezAktarimConfig.Find((MerkezAktarimConfig t) => (t.TabloAdi != this.Module.TableName ? false : t.TabloKolonAdi == lightSeaGreen.DataBindings[0].BindingMemberInfo.BindingField)) != null)
					{
						lightSeaGreen.Properties.ReadOnly = true;
						lightSeaGreen.Properties.AppearanceFocused.BackColor = Color.LightSeaGreen;
						lightSeaGreen.Properties.AppearanceFocused.BackColor2 = (control as BaseEdit).BackColor;
						lightSeaGreen.Properties.AppearanceDisabled.BackColor = Color.LightGray;
					}*/
				}
				lightSeaGreen = null;
			}
			else if (control is LayoutControl)
			{
				for (int i = 0; i < ((LayoutControl)control).Items.Count; i++)
				{
					if (((LayoutControl)control).Items[i] is LayoutControlItem && ((LayoutControlItem)((LayoutControl)control).Items[i]).Control != null)
					{
						this.SetMerkezModulInfo(((LayoutControlItem)((LayoutControl)control).Items[i]).Control);
					}
				}
			}
			else if (control is TextBoxMaskBox)
			{
				this.SetMerkezModulInfo(control.Parent);
			}
		}

		protected virtual void SetNavBarFocus(object sender, EventArgs e)
		{
		}

		private void simpleButtonTasarimYap_Click(object sender, EventArgs e)
		{
			foreach (FormTasarimComponent formTasarimComponent in this.listFormTasarimComponent)
			{
				if ((formTasarimComponent.TipiId.IsBosIse((byte)0) != 1 || !formTasarimComponent.Component.IsDolu() ? false : formTasarimComponent.Component is LayoutControl))
				{
					(formTasarimComponent.Component as LayoutControl).ShowCustomizationForm();
				}
			}
		}

		private void TasarimButtonuEkle()
		{
			//if ((!this.barTasarim.IsDolu() || !this.listFormTasarimComponent.IsDolu() || this.listFormTasarimComponent.Count <= 0 || this.barTasarim.Manager == null || Editor.Core.EditorApplication.EditorApplication.Yetki == null ? false : Editor.Core.EditorApplication.EditorApplication.Yetki.Sistem.EkranTasarimiYapabilir))
			{
				BarSubItem barSubItem = new BarSubItem(this.barTasarim.Manager, "Tasarim");
				barSubItem.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 162);
				barSubItem.Appearance.ForeColor = Color.Red;
				barSubItem.Appearance.Options.UseFont = true;
				barSubItem.Appearance.Options.UseForeColor = true;
				BarButtonItem barButtonItem = new BarButtonItem()
				{
					Caption = "Kaydet"
				};
				barButtonItem.ItemClick += new ItemClickEventHandler(this.barButtonItemTasarimKaydet_ItemClick);
				barButtonItem.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 162);
				barButtonItem.Appearance.ForeColor = Color.Red;
				barButtonItem.Appearance.Options.UseFont = true;
				barButtonItem.Appearance.Options.UseForeColor = true;
				BarButtonItem font = new BarButtonItem()
				{
					Caption = "Orjinale Dön"
				};
				font.ItemClick += new ItemClickEventHandler(this.barButtonItemTasarimOrjinaleDon_ItemClick);
				font.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8f, FontStyle.Bold, GraphicsUnit.Point, 162);
				font.Appearance.ForeColor = Color.Red;
				font.Appearance.Options.UseFont = true;
				font.Appearance.Options.UseForeColor = true;
				//barSubItem.AddItem(barButtonItem);
				//barSubItem.AddItem(font);
				this.barTasarim.AddItem(barSubItem);
			}
		}

		private void TasarimKaydet()
		{
			List<FormTasarimComponent> formTasarimComponents = this.listFormTasarimComponent.FindAll((FormTasarimComponent p) => (p.TipiId.IsBosIse((byte)0) != 1 || !p.Component.IsDolu() ? false : p.Component is LayoutControl));
			foreach (FormTasarimComponent array in formTasarimComponents)
			{
				LayoutControl component = array.Component as LayoutControl;
				MemoryStream memoryStream = new MemoryStream();
				component.SaveLayoutToStream(memoryStream);
				memoryStream.Position = (long)0;
				array.Deger = memoryStream.ToArray();
			}
			if (formTasarimComponents.Count > 0)
			{
				UtilParameters utilParameter = new UtilParameters();
				utilParameter.Add("FormKodu", this.Module.FullKey);
				System.Drawing.Size size = base.Size;
				utilParameter.Add("BoyutX", size.Width);
				size = base.Size;
				utilParameter.Add("BoyutY", size.Height);
				utilParameter.Add("ListFormTasarimComponent", formTasarimComponents);
				//Editor.Core.EditorApplication.EditorApplication.Module.Sistem.FormTasarim.TasarimsalKullaniciYonetim.Show(utilParameter);
			}
		}

		private void TasarimsalLayoutControlEkle(LayoutControl pLayoutControl, string pUserControlAdi)
		{
			if (this.listFormTasarimComponent.FindAll((FormTasarimComponent p) => (!pUserControlAdi.IsBos() || !(p.ComponentAdi == pLayoutControl.Name) ? (!pUserControlAdi.IsDolu() || !(p.UserControlAdi == pUserControlAdi) ? false : p.ComponentAdi == pLayoutControl.Name) : true)).Count == 0)
			{
				List<FormTasarimComponent> formTasarimComponents = this.listFormTasarimComponent;
				FormTasarimComponent formTasarimComponent = new FormTasarimComponent()
				{
					FormKodu = this.Module.FullKey,
					UserControlAdi = pUserControlAdi,
					TipiId = new byte?(1),
					ComponentAdi = pLayoutControl.Name,
					Component = pLayoutControl
				};
				formTasarimComponents.Add(formTasarimComponent);
			}
		}

		private void TasarimYukle()
		{
			/*try
			{
				if ((!this.listFormTasarimComponent.IsDolu() || this.listFormTasarimComponent.Count <= 0 || !this.Module.IsDolu() ? false : this.Module.FullKey.IsDolu()))
				{
					if (this.coreEntities.IsBos())
					{
						this.coreEntities = CoreEntities.Yeni();
					}
					List<Editor.Core.EF.FormTasarim> byFormBase = this.coreEntities.FormTasarimQuery.GetByFormBase(this.Module.FullKey);
					foreach (FormTasarimComponent formTasarimComponent in this.listFormTasarimComponent)
					{
						LayoutControl component = formTasarimComponent.Component as LayoutControl;
						Editor.Core.EF.FormTasarim formTasarim = byFormBase.Find((Editor.Core.EF.FormTasarim p) => (!(p.ComponentAdi == formTasarimComponent.ComponentAdi) || !formTasarimComponent.UserControlAdi.IsBos() && !(formTasarimComponent.UserControlAdi == p.UserControlAdi) ? false : p.EkIsKullaniciTasarimi.IsBosIse(false)));
						if (formTasarim.IsBos())
						{
							formTasarim = byFormBase.Find((Editor.Core.EF.FormTasarim p) => (p.ComponentAdi != formTasarimComponent.ComponentAdi ? false : (formTasarimComponent.UserControlAdi.IsBos() ? true : formTasarimComponent.UserControlAdi == p.UserControlAdi)));
						}
						if (formTasarim.IsDolu())
						{
							MemoryStream memoryStream = new MemoryStream(formTasarim.Deger)
							{
								Position = (long)0
							};
							component.RestoreLayoutFromStream(memoryStream);
						}
					}
					if ((byFormBase.Count <= 0 || byFormBase[0].EkFormBoyutX.IsBosIse((short)50) <= 50 ? false : byFormBase[0].EkFormBoyutY.IsBosIse((short)50) > 50))
					{
						base.Size = new System.Drawing.Size(Convert.ToInt32(byFormBase[0].EkFormBoyutX), Convert.ToInt32(byFormBase[0].EkFormBoyutY));
					}
				}
			}
			catch
			{
			}*/
		}

		private void TreeList_KeyPress(object sender, KeyPressEventArgs e)
		{
			string upper = e.KeyChar.ToString().ToUpper();
			if ((((TreeList)sender).FocusedNode == null || ((TreeList)sender).OptionsBehavior.Editable ? false : ((TreeList)sender).Columns.Count != 0))
			{
				if (!string.IsNullOrEmpty(upper))
				{
					this.FindAndFocusNode((TreeList)sender, upper);
				}
			}
		}

		protected virtual new bool Validate()
		{
			this.ValidateActiveControl(base.ActiveControl);
			return this.dxValidationProvider.Validate();
		}

		protected virtual bool ValidateActiveControl(System.Windows.Forms.Control control)
		{
			bool flag = false;
			System.Windows.Forms.Control activeControl = base.ActiveControl;
			while (activeControl is ContainerControl)
			{
				activeControl = ((ContainerControl)activeControl).ActiveControl;
			}
			if (activeControl is TextBoxMaskBox)
			{
				activeControl = activeControl.Parent;
				if (activeControl.Parent is TreeList)
				{
					activeControl = activeControl.Parent;
				}
			}
			if (activeControl is BaseEdit)
			{
				((BaseEdit)activeControl).DoValidate();
				if (activeControl.Parent is GridControl)
				{
					((GridControl)activeControl.Parent).FocusedView.PostEditor();
					((GridControl)activeControl.Parent).FocusedView.UpdateCurrentRow();
					flag = true;
				}
				else if (activeControl.Parent is TreeList)
				{
					((TreeList)activeControl.Parent).PostEditor();
					flag = true;
				}
				/*else if (activeControl.Parent is VGridControl)
				{
					((VGridControl)activeControl.Parent).PostEditor();
					flag = true;
				}*/
			}
			else if (activeControl is GridControl)
			{
				((GridControl)activeControl).FocusedView.PostEditor();
				((GridControl)activeControl).FocusedView.UpdateCurrentRow();
				flag = true;
			}
			else if (activeControl is TreeList)
			{
				((TreeList)activeControl).PostEditor();
				flag = true;
			}
			/*else if (activeControl is VGridControl)
			{
				((VGridControl)activeControl).PostEditor();
				flag = true;
			}*/
			return flag;
		}
	}
}