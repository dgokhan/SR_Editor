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
using SR_Editor.LookUp;

namespace SR_Editor.Modules.Character.Action
{
    public partial class FormCharacterPunishment : FormBase, IFormBase
    {
        public FormCharacterPunishment()
        {
            InitializeComponent();

        }

        private ChatBanHistoryInput accountPunishment;

        public override void FormParamsChanged()
        {
            this.InitData();
            this.InitDesign();
            this.InitMask();
            this.InitLookUp();
            this.InitValidationRules();
            this.InitRight();
        }

        private string ToHumanReadable(int minute)
        {
            TimeSpan t = TimeSpan.FromMinutes(minute);
            return string.Format("{0:D1} Gün {1:D1} Saat {2:D1} Dakika",
                t.Days,
                t.Hours,
                t.Minutes);
        }

        public void InitData()
        {
            if (this.FormParams.Contains("CharId") && this.FormParams.Contains("ShardId"))
            {
                Loading(() =>
                {
                    var api = new RoyaleSupportClient();
                    var characterDetail = api.GameCharacter(Convert.ToInt32(this.FormParams["ShardId"]),Convert.ToInt32(this.FormParams["CharId"]));

                    accountPunishment = new ChatBanHistoryInput()
                    {
                        ServerId = Convert.ToInt32(this.FormParams["ShardId"]),
                        CharId = characterDetail.Id,
                        CharName = characterDetail.Name,
                        Duration = 1,
                        UserName = "",
                        Note = "",
                        Reason = ""
                    };

                    var affects = api.GameCharacterAffect(Convert.ToInt32(this.FormParams["ShardId"]),
                        Convert.ToInt32(this.FormParams["CharId"])).ToList();

                    var chatBan = affects.FirstOrDefault(p => p.Type == (int)EnumAffect.AFFECT_BLOCK_CHAT);

                    if (chatBan != null)
                    {
                        var duration = ToHumanReadable(chatBan.Duration / 60);
                        this.textEditAccountStatus.Text = duration;
                        this.textEditAccountStatus.BackColor = Color.Red;
                        this.textEditAccountStatus.ForeColor = Color.White;

                        radioGroup1.Properties.Items.FirstOrDefault(x => Convert.ToString(x.Tag) == "NoPunishment").Enabled = true;

                        radioGroup1.SelectedIndex = 0;
                    }
                    else
                    {
                        this.textEditAccountStatus.Text = "Yasağı yok";
                        this.textEditAccountStatus.BackColor = Color.Green;
                        this.textEditAccountStatus.ForeColor = Color.White;

                        radioGroup1.Properties.Items.FirstOrDefault(x => Convert.ToString(x.Tag) == "NoPunishment").Enabled = false;

                        radioGroup1.SelectedIndex = 1;
                    }



                    this.bindingSourceAccountPunishment.DataSource = accountPunishment;
                }, () =>
                {
                    throw new ExceptionBeklenmeyen("Karakter bulunamadı.");
                });

            }
            else
            {
                throw new ExceptionBeklenmeyen("Karakter bulunamadı.");
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
            this.ValidationProvider.SetValidationRule(this.radioGroup1, UtilValidation.GetNotEmptyCondition());
        }

        private void bbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!this.Validate())
                return;

            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                api.ChatPunishment(accountPunishment);

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