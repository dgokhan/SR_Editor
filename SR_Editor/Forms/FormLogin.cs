using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SR_Editor.Core;
using SR_Editor.Framework;
using SR_Editor.Framework.Opcode;
using System.Net.Http;
using Newtonsoft.Json;
using SR_Editor.Api;

namespace SR_Editor.Forms
{
    public partial class FormLogin : FormBase
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        public void InitValidationRules()
        {
            base.ValidationProvider.SetValidationRule(this.textEditUsername, UtilValidation.GetNotEmptyCondition());
            base.ValidationProvider.SetValidationRule(this.textEditPassword, UtilValidation.GetNotEmptyCondition());
        }

        private void Giris()
        {
            base.DialogResult = DialogResult.None;



            if (this.Validate())
            {
                //var endpoint = "https://localhost:44380/connect/token";
                var endpoint = "http://51.68.178.17:4545/connect/token";

                Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "client_id", "RoyaleSupport_Public" },
                { "scope", "offline_access RoyaleSupport" },
                { "grant_type", "password" },
                { "username", this.textEditUsername.Text },
                { "password", this.textEditPassword.Text},
            };

                FormUrlEncodedContent data = new FormUrlEncodedContent(values);

                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.PostAsync(endpoint, data).Result;

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<LoginResultOk>(response.Content.ReadAsStringAsync().Result);
                    EditorApplication.EditorApplication.AccessToken = result.access_token;

                    var api = new RoyaleSupport.RoyaleSupportClient();
                    EditorApplication.EditorApplication.Configuration = api.ApplicationConfiguration(false);

                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<LoginResultFail>(response.Content.ReadAsStringAsync().Result);
                    MessageBox.Show(result.error_description, "Login", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }

                this.simpleButtonLogin.Enabled = true;
            }
            else
            {
                //base.DialogResult = DialogResult.OK;
            }
        }

        public static DialogResult GirisFormuAc()
        {
            FormLogin formLogin = new FormLogin();
            return formLogin.ShowDialog();
        }
        private void simpleButtonLogin_Click(object sender, EventArgs e)
        {
            Giris();
        }

        public override void FormParamsChanged()
        {
            this.InitValidationRules();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.FormParamsChanged();
        }

        public override void Client_onReceive(Packet pkt)
        {

        }

        private void textEditUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Giris();
            }
        }

        private void textEditPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Giris();
            }
        }
    }
}