using DevExpress.XtraBars;
//using SR_Editor.Core.EF;
using SR_Editor.LookUp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SR_Editor.EditorApplication;

namespace SR_Editor.Core
{
    public class ModuleInfo : ModuleInfoGroup
    {
        private int maxInstance = 10;

        private bool isDialog = false;

        private bool isMerkezModul = false;

        private BarShortcut itemShortcut = BarShortcut.Empty;

        private string formName = null;

        private string tableName = null;

        private List<RightInfo> listRights;

        private List<ParameterInfo> listParameter;

        public string FormName
        {
            get
            {
                return this.formName;
            }
            set
            {
                this.formName = value;
            }
        }

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

        public bool IsMerkezModul
        {
            get
            {
                return this.isMerkezModul;
            }
            set
            {
                this.isMerkezModul = value;
            }
        }

        public BarShortcut ItemShortcut
        {
            get
            {
                return this.itemShortcut;
            }
            set
            {
                this.itemShortcut = value;
            }
        }

        public List<ParameterInfo> ListParameter
        {
            get
            {
                if (this.listParameter == null)
                {
                    this.listParameter = new List<ParameterInfo>();
                }
                return this.listParameter;
            }
        }

        public List<RightInfo> ListRights
        {
            get
            {
                if (this.listRights == null)
                {
                    this.listRights = new List<RightInfo>();
                }
                return this.listRights;
            }
        }

        public int MaxInstance
        {
            get
            {
                return this.maxInstance;
            }
            set
            {
                this.maxInstance = value;
            }
        }

        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                this.tableName = value;
            }
        }

        public string Permission { get; set; }

        public ModuleInfo()
        {
            base.IsGroup = false;
        }

        internal ParameterInfo AddParameter(ParameterInfo item)
        {
            this.ListParameter.Add(item);
            return item;
        }

        internal RightInfo AddRight(RightInfo item)
        {
            this.ListRights.Add(item);
            return item;
        }
        public virtual void Show(string ModuleName, UtilParameters pFormParams)
        {
            this.Caption += " " + ModuleName;
            Show(pFormParams);
        }

        public virtual void Show(UtilParameters pFormParams)
        {
            FormBase activeInstance;
            bool flag;
            if (UtilConfig.GetActiveInstanceCount(this.FullKey) >= this.MaxInstance)
            {
                activeInstance = UtilConfig.GetActiveInstance(this.FullKey);
                if (pFormParams != null)
                {
                    activeInstance.FormParams = pFormParams;
                }
                activeInstance.Focus();
            }
            else
            {
                this.ShowModuleMesaj();
                activeInstance = null;
                if (activeInstance == null)
                {
                    string fullKey = this.FullKey;
                    if (!string.IsNullOrEmpty(this.FormName))
                    {
                        fullKey = string.Concat(base.NameSpace, ".", this.FormName);
                    }

                    if (!PermissionExtensions.IsGranted(this.Permission))
                    {
                        UtilMessage.Show(EnumUtilMessage.ModulBulunamadi, null, string.Concat("Bu bölüme erişmeye yetkiniz yoktur."), "Modül Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }

                    ObjectHandle objectHandle = null;
                    try
                    {
                        objectHandle = Activator.CreateInstance(base.AssemblyName, fullKey);
                    }
                    catch (Exception ex)
                    {
                        UtilMessage.Show(EnumUtilMessage.ModulBulunamadi, null, ex.Message, this.FullKey, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (objectHandle != null)
                    {
                        activeInstance = objectHandle.Unwrap() as FormBase;
                        objectHandle = null;
                    }
                }
                if (activeInstance == null)
                {
                    UtilMessage.Show(EnumUtilMessage.ModulBulunamadi, null, string.Concat(this.FullKey, " isimli modül bulunamadı."), "Modül Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {

                    if (!PermissionExtensions.IsGranted(this.Permission))
                    {
                        UtilMessage.Show(EnumUtilMessage.ModulBulunamadi, null, string.Concat("Bu bölüme erişmeye yetkiniz yoktur."), "Modül Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }

                    activeInstance.Module = this;
                    UtilConfig.AktifModuleInfo = this;
                    activeInstance.Text = base.Caption;
                    activeInstance.Name = this.FullKey;
                    if (pFormParams == null)
                    {
                        activeInstance.FormParams.Clear();
                        activeInstance.FormParamsChanged();
                    }
                    else
                    {
                        activeInstance.FormParams = pFormParams;
                    }
                    /*if ((!this.IsMerkezModul || this.TableName == null || BaseEntityAdapter.IsMerkez ? false : BaseEntityAdapter.GetIsMerkezTablo(this.TableName)))
					{
						foreach (Control control in activeInstance.Controls)
						{
							activeInstance.SetMerkezModulInfo(control);
						}
					}*/
                    if (this.IsDialog)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = (pFormParams == null || !pFormParams.Contains("IsDialogForm") ? true : !Convert.ToBoolean(pFormParams["IsDialogForm"]));
                    }
                    if (flag)
                    {
                        activeInstance.MdiParent = UtilConfig.MainForm;
                        activeInstance.Show();
                    }
                    else
                    {
                        activeInstance.ShowDialog();
                    }
                }
            }
        }
        /*
        public static void Show(string pFormName, UtilParameters pFormParams)
        {
            ModuleInfo moduleInfo = (ModuleInfo)UtilConfig.ListModuleInfo.Find((ModuleInfoGroup t) => t.FullKey == pFormName);
            if (moduleInfo != null)
            {
                moduleInfo.Show(pFormParams);
            }
            else
            {
                UtilMessage.Show(EnumUtilMessage.ModulBulunamadi, null, string.Concat(pFormName, " isimli modül bulunamadı."), "Modül Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }*/

        public static UtilParameters ShowFormAciklama(short pAciklamaTipi, string pAciklama)
        {
            UtilParameters utilParameter = new UtilParameters();
            utilParameter.Add("Baslik", pAciklama);
            utilParameter.Add("AciklamaTipi", 0);
            //ModuleInfo.Show("Editor.Ortak.Genel.FormAciklama", utilParameter);
            var result = XtraInputBox.Show(pAciklama, "İptal", "");
            if (result.IsDolu())
            {
                utilParameter.Add("Aciklama", result);
                utilParameter.DialogResult = DialogResult.OK;
            }
            else
                utilParameter.DialogResult = DialogResult.No;
            
            return utilParameter;
        }

        private void ShowModuleMesaj()
        {
            /*bool flag;
			ModulMesaj nullable = null;
			if (this.FullKey != null)
			{
				nullable = UtilConfig.ListModulMesaj.Find((ModulMesaj p) => p.Kodu == this.FullKey);
			}
			if ((!nullable.IsBos() ? false : base.ParentKey.IsDolu()))
			{
				nullable = UtilConfig.ListModulMesaj.Find((ModulMesaj p) => p.Kodu == base.ParentKey);
			}
			if (nullable.IsDolu())
			{
				if (nullable.UyariSuresi == 1)
				{
					UtilMessage.Show(nullable.Mesaj, "Uyarı");
				}
				else if (nullable.UyariSuresi != 2)
				{
					if (nullable.EkMesajGosterimTarihi.IsBos())
					{
						flag = false;
					}
					else if (!nullable.EkMesajGosterimTarihi.IsDolu())
					{
						flag = true;
					}
					else
					{
						DateTime value = nullable.EkMesajGosterimTarihi.Value;
						flag = !(value.AddHours((double)nullable.UyariSuresi) < UtilDateTime.Instance.Now);
					}
					if (!flag)
					{
						UtilMessage.Show(nullable.Mesaj, "Uyarı");
						nullable.EkMesajGosterimTarihi = new DateTime?(UtilDateTime.Instance.Now);
					}
				}
				else
				{
					UtilMessage.Show(nullable.Mesaj, "Uyarı");
					UtilConfig.ListModulMesaj.Remove(nullable);
				}
			}*/
        }
    }
}