using DevExpress.XtraBars;
using DevExpress.XtraEditors.DXErrorProvider;
using MicroOrm.Dapper.Repositories;
using RoyaleSupport;
using SR_Editor.Core;
using SR_Editor.EditorApplication;
using SR_Editor.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR_Editor.Modules.SqlQuery.Action
{
    public partial class FormSqlTanimKategori : FormBase, IFormBase
    {
        private SqlDefinitionCategoryDto sqlTanimKategori;
        private int aktifSqlTanimKategoriId;
        public FormSqlTanimKategori()
        {
            InitializeComponent();
            this.IsDialog = true;
        }

        public void InitDesign()
        {
            UtilBarManager.InitBarManager(this.barManager1);
            UtilCommon.InitShortcut(this.Module, (BarItem)this.barButtonItemTamam, EnumShortcut.Save);
            UtilCommon.InitShortcut(this.Module, (BarItem)this.barButtonItemIptal, EnumShortcut.Cancel);
        }

        public void InitValidationRules()
        {
            this.ValidationProvider.SetValidationRule((Control)this.textEditAdi, (ValidationRuleBase)UtilValidation.GetNotEmptyCondition());
        }

        public void InitMask()
        {
        }

        public void InitLookUp()
        {
            UtilLookUp.InitLookupEdit(this.lookUpEditKategoriTipi);
            this.lookUpEditKategoriTipi.Properties.DataSource = (object)KategoriTipi.Liste;
        }

        public void InitData()
        {
            if (this.FormParams.Contains("Id"))
            {
                this.aktifSqlTanimKategoriId = Convert.ToInt32(this.FormParams["Id"]);

                try
                {
                    var api = new RoyaleSupportClient();
                    sqlTanimKategori = api.SqlDefinitionCategoryById(this.aktifSqlTanimKategoriId);
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }

            }
            else
            {

                this.sqlTanimKategori = new SqlDefinitionCategoryDto();
            }
            this.bindingSourceSqlTanimKategori.DataSource = (object)this.sqlTanimKategori;
        }

        public void InitRight()
        {
        }

        public override void FormParamsChanged()
        {
            this.InitData();
            this.InitDesign();
            this.InitMask();
            this.InitLookUp();
            this.InitValidationRules();
            this.InitRight();
        }

        protected override bool Kaydet()
        {
            this.bindingSourceSqlTanimKategori.EndEdit();
            if (this.sqlTanimKategori.Id == 0)
            {
                try
                {
                    var api = new RoyaleSupportClient();
                    sqlTanimKategori = api.SqlDefinitionCategoryPOST(new SqlDefinitionCategoryInput()
                    {
                        Name = sqlTanimKategori.Name,
                        Type = sqlTanimKategori.Type
                    });
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
            }
            else
            {
                try
                {
                    var api = new RoyaleSupportClient();
                    sqlTanimKategori = api.SqlDefinitionCategoryPUT(new SqlDefinitionCategoryInput()
                    {
                        Id = sqlTanimKategori.Id,
                        Name = sqlTanimKategori.Name,
                        Type = sqlTanimKategori.Type
                    });
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
            }
            return true;
        }

        private void barButtonItemTamam_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!this.Validate())
                return;
            this.Kaydet();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void barButtonItemIptal_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void bindingSourceSqlTanimKategori_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
