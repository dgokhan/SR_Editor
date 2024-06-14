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

namespace SR_Editor.Modules.Character.Query
{
    public partial class FormCharacterList : FormBase, IFormBase
    {
        private GameAccountDto aktifAccount;
        public FormCharacterList()
        {
            InitializeComponent();
        }

        public void InitData()
        {

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

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButtonSorgula_Click(object sender, EventArgs e)
        {
            CharacterSearch(this.textEditAccountName.Text);
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

        private void FormAccount_Shown(object sender, EventArgs e)
        {
        }

        private void gridViewCharacters_DoubleClick(object sender, EventArgs e)
        {
        }

        private void gridViewKarakterler_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void gridViewKarakterler_DoubleClick(object sender, EventArgs e)
        {

        }

        private void gridViewKarakterler_DoubleClick_1(object sender, EventArgs e)
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

        private void CharacterSearch(string characterName)
        {
            if (!this.Validate())
                return;

            var username = characterName;

            this.Text = $"{username} - Karakter Arama";

            Loading(() =>
            {
                var api = new RoyaleSupportClient();
                var characters = api.FindCharactersByName(username);
                this.bindingSourceKarakterler.DataSource = characters.ToList();

            }, () =>
            {
                this.bindingSourceKarakterler.DataSource = null;
            }, () =>
            {
            });
        }

        private void textEditAccountName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CharacterSearch(this.textEditAccountName.Text);
            }
        }
    }
}