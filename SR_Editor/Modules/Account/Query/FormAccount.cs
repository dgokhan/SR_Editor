using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using RoyaleSupport;
using SR_Editor.Core;
using SR_Editor.Core.Exceptions;
using SR_Editor.EditorApplication;
using SR_Editor.Forms;
using SR_Editor.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SR_Editor.Modules.Account.Query
{
    public partial class FormAccount : FormBase, IFormBase
    {
        private GameAccountDto aktifAccount;
        public FormAccount()
        {
            InitializeComponent();
        }

        public void InitData()
        {
            if (this.FormParams.Contains("UserId"))
            {
                var userId = this.FormParams["UserId"];
                var api = new RoyaleSupportClient();
                var account = api.FindAccountById(Convert.ToInt32(userId));

                this.FormParams["AccountName"] = account.Username;
            }

            if (this.FormParams.Contains("UserName"))
                this.FormParams["AccountName"] = this.FormParams["UserName"];


            if (!this.FormParams.Contains("AccountName"))
                return;

            //this.textEditAccountName.EditValue = this.FormParams["AccountName"].ToString();
            this.textEditAccountName.Text = this.FormParams["AccountName"].ToString();

            Listele();
        }

        public void InitDesign()
        {
        }

        public void InitLookUp()
        {

            //UtilLookUp.InitLookupEdit(this.repositoryItemLookUpEditJob,"Id","Aciklama");
            this.lookUpJobTipiBindingSource.DataSource = (object)JobTipi.Liste;
        }

        public void InitMask()
        {

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

        public void InitValidationRules()
        {
            this.ValidationProvider.SetValidationRule((Control)this.textEditAccountName, (ValidationRuleBase)UtilValidation.GetNotEmptyCondition());
        }

        protected override void Listele()
        {
            var username = this.textEditAccountName.Text;

            this.Text = $"{username} - Hesap Sorgulama";

            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                aktifAccount = api.FindAccountByUsername(username);
                this.bindingSourceAccount.DataSource = aktifAccount;

            }, () =>
            {
                layoutControlItemDetail.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemAccount.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                barButtonItemPunishment.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                aktifAccount = null;
            }, () =>
            {
                layoutControlItemDetail.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemAccount.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                barButtonItemPunishment.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                xtraTabControl.SelectedTabPageIndex = 0;
                xtraTabControl.SelectedTabPage = xtraTabPageKarakterler;
                ListeleKarakterler();
            });
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButtonSorgula_Click(object sender, EventArgs e)
        {
            if (!this.Validate())
                return;

            Listele();
        }

        private void Loading(System.Action action, System.Action fail, System.Action final = null)
        {
            SplashScreenManager.CloseForm(false);
            SplashScreenManager.ShowForm(typeof(EditorLoading));

            bool success = false;
            try
            {
                action?.Invoke();
                success = true;
            }
            catch (Exception ex)
            {
                fail?.Invoke();
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

                if (success)
                    final?.Invoke();
            }
        }

        private void gridViewKarakterler_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void xtraTabPageKarakterler_Enter(object sender, EventArgs e)
        {
        }

        private void gridViewKarakterler_DoubleClick(object sender, EventArgs e)
        {
            var selectedCharacter = this.gridViewKarakterler.FocusedRowHandle < 0 ? null : (GameAccountCharacterDto)this.gridViewKarakterler.GetRow(this.gridViewKarakterler.FocusedRowHandle);

            if (selectedCharacter == null)
                return;

            if (selectedCharacter.Status == "DELETED")
                return;

            UtilParameters pFormParams = new UtilParameters();
            pFormParams.Add("CharId", selectedCharacter.Char_id);
            pFormParams.Add("ShardId", selectedCharacter.Server_id);
            EditorApplication.EditorApplication.Module.CharacterModule.CharacterActionModule.Character.Show(pFormParams);
        }

        private void xtraTabControl_Selected(object sender, DevExpress.XtraTab.TabPageEventArgs e)
        {
            var index = e.PageIndex;
            if (index == 0)
            {
                ListeleKarakterler();
            }
            else if (index == 1)
            {
                ListeleYasaklamaGecmisi();
            }
            else if (index == 2)
            {
                ListeleMarketYuklemeleri();
            }
            else if (index == 3)
            {
                ListeleMarketHarcamalari();
            }
            else if (index == 6)
            {
                dateLoginStart.DateTime = DateTime.Now.AddMonths(-3);
                dateLoginFinish.DateTime = DateTime.Now;

                ListeleGirisKayitlari(dateLoginStart.DateTime, dateLoginFinish.DateTime);
            }
        }

        private void ListeleKarakterler()
        {
            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                var characters = api.FindCharactersByAccountId(aktifAccount.Id);
                this.bindingSourceKarakterler.DataSource = characters;

            }, () =>
            {
                this.bindingSourceKarakterler.DataSource = null;
            });
        }

        private void ListeleYasaklamaGecmisi()
        {
            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                var characters = api.FindPunishmentsByAccountId(aktifAccount.Id);
                this.bindingSourceYasaklamaGecmisi.DataSource = characters;

            }, () =>
            {
                this.bindingSourceYasaklamaGecmisi.DataSource = null;
            });
        }

        private void ListeleMarketYuklemeleri()
        {
            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                var characters = api.FindTransactionsByAccountId(aktifAccount.Id);
                this.bindingSourceYuklemeGecmisi.DataSource = characters;

            }, () =>
            {
                this.bindingSourceYuklemeGecmisi.DataSource = null;
            });
        }

        private void ListeleMarketHarcamalari()
        {
            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                var characters = api.FindShopHistoryByAccountId(aktifAccount.Id);
                this.bindingSourceMarketHarcamalari.DataSource = characters;

            }, () =>
            {
                this.bindingSourceMarketHarcamalari.DataSource = null;
            });
        }

        private void ListeleGirisKayitlari(DateTime startDate, DateTime finishDate)
        {
            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                var characters = api.FindLoginLogByAccountId(aktifAccount.Id, startDate, finishDate);
                this.bindingSourceGirisKayitlari.DataSource = characters.OrderByDescending(x => x.Login_time);

            }, () =>
            {
                this.bindingSourceGirisKayitlari.DataSource = null;
            });
        }

        private void FormAccount_Shown(object sender, EventArgs e)
        {
            if (this.FormParams.Contains("AccountName"))
                return;

            layoutControlItemDetail.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItemAccount.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            barButtonItemPunishment.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void barButtonItemPunishment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var formParams = new UtilParameters();
            formParams.Add("AccountId", aktifAccount.Username);

            EditorApplication.EditorApplication.Module.AccountModule.AccountActionModule.AccountPunishment.Show(formParams);

            if (formParams.DialogResult == DialogResult.OK)
            {
                if (xtraTabControl.SelectedTabPageIndex == 1)
                {
                    ListeleYasaklamaGecmisi();

                    var api = new RoyaleSupportClient();
                    aktifAccount = api.FindAccountByUsername(aktifAccount.Username);
                    this.bindingSourceAccount.DataSource = aktifAccount;
                }
            }
        }

        private void textEdit6_TextChanged(object sender, EventArgs e)
        {
            var status = this.textEdit6.Text;
            if (status == "OK")
            {
                this.textEdit6.ForeColor = Color.White;
                this.textEdit6.BackColor = Color.Green;
            }
            else if (status == "BLOCK")
            {
                this.textEdit6.ForeColor = Color.White;
                this.textEdit6.BackColor = Color.Red;
            }
            else
            {
                this.textEdit6.ForeColor = Color.Black;
                this.textEdit6.BackColor = Color.White;
            }
        }

        private void gridViewMarketHarcamalari_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
        }

        private void gridViewMarketHarcamalari_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Price_type" && e.IsGetData)
            {
                GridView view = sender as GridView;
                var type = Convert.ToInt32(view.GetRowCellValue(view.GetRowHandle(e.ListSourceRowIndex), "Price_type"));

                if (type == 0)
                    e.Value = "Dragon Point";
                else if (type == 1)
                    e.Value = "Dragon Mark";
                else
                    e.Value = "Unknown";

            }
        }

        private void gridViewMarketHarcamalari_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {

            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            if (info.Column.Caption == "Fiyat Tipi")
            {
                var text = "Unknown";

                int type = Convert.ToInt32(view.GetGroupRowValue(e.RowHandle, info.Column));
                if (type == 0)
                    text = "Dragon Point";
                else if (type == 1)
                    text = "Dragon Mark";

                string colorName = getColorName(type);
                info.GroupText = "Fiyat Tipi : <color=" + colorName + ">" + text +
                                 "</color> ";
                info.GroupText += "<color=Blue>" + view.GetGroupSummaryText(e.RowHandle) + "</color> ";
            }
        }
        string getColorName(int value)
        {
            if (value == 0) return "MediumOrchid";
            else if (value == 1) return "OrangeRed";
            else return "Blue";
        }

        private void textEditAccountName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!this.Validate())
                    return;

                Listele();
            }
        } 

        private void gridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Status")
            {
                if (e.CellValue != null && !string.IsNullOrWhiteSpace(e.CellValue.ToString()))
                    if (e.CellValue.ToString() == "DELETED")
                        e.Appearance.BackColor = Color.Red;
                    else
                        e.Appearance.BackColor = Color.Green;
            }
        }

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn) != null && view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString() != String.Empty)
                    Clipboard.SetText(view.GetRowCellValue(view.FocusedRowHandle, view.FocusedColumn).ToString());
                else
                    MessageBox.Show("The value in the selected cell is null or empty!");
                e.Handled = true;
            }
        }
    }
}