using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Helpers;
using DevExpress.XtraSplashScreen;
using RoyaleSupport;
using SR_Editor.Core;
using SR_Editor.Core.Exceptions;
using SR_Editor.EditorApplication;
using SR_Editor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR_Editor.Modules.Account.Action
{
    public partial class FormAccountPunishment : FormBase, IFormBase
    {
        public FormAccountPunishment()
        {
            InitializeComponent();

        }

        private AccountBanHistoryInput accountPunishment;

        public override void FormParamsChanged()
        {
            this.InitData();
            this.InitDesign();
            this.InitMask();
            this.InitLookUp();
            this.InitValidationRules();
            this.InitRight();
        }

        public void InitData()
        {
            if (this.FormParams.Contains("AccountId"))
            {
                Loading(() =>
                {
                    var api = new RoyaleSupportClient();
                    var accountDetail = api.FindAccountByUsername(this.FormParams["AccountId"].ToString());

                    accountPunishment = new AccountBanHistoryInput()
                    {
                        AccountId = accountDetail.Id,
                        CharId = 0,
                        CharName = "",
                        IsBlocked = accountDetail.Status != "BLOCK",
                        UserName = accountDetail.Username,
                        Note = "",
                        Reason = ""
                    };

                    if (!accountPunishment.IsBlocked)
                    {
                        labelControlActionText.ForeColor = Color.LimeGreen;
                        labelControlActionText.Text = "Bu işlem hesabın yasaklamasını kaldırır.";
                    }


                    this.textEditAccountStatus.Text = accountDetail.Status;

                    this.bindingSourceAccountPunishment.DataSource = accountPunishment;
                }, () =>
                {
                    throw new ExceptionBeklenmeyen("Hesap bulunamadı.");
                });

            }
            else
            {
                throw new ExceptionBeklenmeyen("Hesap bulunamadı.");
            }
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
        public void InitDesign()
        {
        }

        public void InitLookUp()
        {
        }

        public void InitMask()
        {
        }

        public void InitRight()
        {
        }

        public void InitValidationRules()
        {
            this.ValidationProvider.SetValidationRule(this.memoEditReason, UtilValidation.GetNotEmptyCondition());
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!this.Validate())
                return;

            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                api.AccountPunishment(accountPunishment);

                this.FormParams.DialogResult = DialogResult.OK;
                DialogResult = DialogResult.OK;
                this.Close();
            }, () =>
            {

            });


        }

        private void listBoxControlTaslak_SelectedValueChanged(object sender, EventArgs e)
        {
            this.accountPunishment.Reason = listBoxControlTaslak.SelectedValue.ToString();
            this.memoEditReason.EditValue = this.accountPunishment.Reason;
        }
    }
}