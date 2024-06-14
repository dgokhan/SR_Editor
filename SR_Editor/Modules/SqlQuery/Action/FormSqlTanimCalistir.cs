using Dapper.Contrib.Extensions;
using DevExpress.Data;
using DevExpress.Office.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraRichEdit.Services;
using MicroOrm.Dapper.Repositories;
using SR_Editor.Core;
using SR_Editor.Core.Exceptions;
using SR_Editor.EditorApplication;
using SR_Editor.Extension;
using SR_Editor.LookUp;
using SR_Editor.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoyaleSupport;
using System.Globalization;
using System.Net;
using SR_Editor.Modules.SqlQuery.Action.Controls;

namespace SR_Editor.Modules.SqlQuery.Action
{
    public partial class FormSqlTanimCalistir : FormBase, IFormBase
    {
        private const string PatternParametre = "@([a-zA-Z][a-zA-Z0-9_]*)";
        private Kosul kosul;
        private SqlDefinitionDto aktifSqlTanim;
        private readonly List<string> matchesParameters;
        private readonly Dictionary<SqlDefinitionParameterDto, string> paramDictionary;
        private readonly Dictionary<SqlDefinitionConditionDto, string> kosulDictionary;
        private List<SqlDefinitionParameterDto> kayitliParametreListesi;

        private long currentShard;

        public FormSqlTanimCalistir()
        {
            this.InitializeComponent();
            this.matchesParameters = new List<string>();
            this.paramDictionary = new Dictionary<SqlDefinitionParameterDto, string>();
            this.kosulDictionary = new Dictionary<SqlDefinitionConditionDto, string>();
            this.kayitliParametreListesi = new List<SqlDefinitionParameterDto>();
            this.kosul = new Kosul();

            richEditControlSorgu.Options.Search.RegExResultMaxGuaranteedLength = 2000;
            richEditControlSorgu.ReplaceService<ISyntaxHighlightService>(new CustomSyntaxHighlightService(richEditControlSorgu.Document));

            //Specify the richEdit's layout settings 
            richEditControlSorgu.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            richEditControlSorgu.Document.Sections[0].Page.Width = Units.InchesToDocumentsF(80f);
            richEditControlSorgu.Document.DefaultCharacterProperties.FontName = "Courier New";
            richEditControlSorgu.Document.DefaultCharacterProperties.FontSize = 9;
        }

        public void InitDesign()
        {
            UtilBarManager.InitBarManager(this.barManager1);
            UtilCommon.InitShortcut(this.Module, (BarItem)this.barButtonItemKaydet, EnumShortcut.Save);
            UtilCommon.InitShortcut(this.Module, (BarItem)this.barButtonItemIptal, EnumShortcut.Cancel);

            try
            {
                var api = new RoyaleSupportClient();
                var category = api.SqlDefinitionCategoryById(this.aktifSqlTanim.SqlDefinitionCategoryId);

                if (category.Type != EnumCategoryType.Game)
                    layoutControlItemServer.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }

            if (this.aktifSqlTanim.Id != 0)
                return;
            this.lookUpEditSqlTanimKategori.Enabled = false;

        }

        public void InitValidationRules()
        {
            this.ValidationProvider.SetValidationRule((Control)this.richEditControlSorgu, (ValidationRuleBase)UtilValidation.GetNotEmptyCondition());
            this.ValidationProvider.SetValidationRule((Control)this.textEditBaslik, (ValidationRuleBase)UtilValidation.GetNotEmptyCondition());
            this.ValidationProvider.SetValidationRule((Control)this.lookUpEditVeritabani, (ValidationRuleBase)UtilValidation.GetNotEmptyCondition());

            try
            {
                var api = new RoyaleSupportClient();
                var category = api.SqlDefinitionCategoryById(this.aktifSqlTanim.SqlDefinitionCategoryId);

                if (category.Type != EnumCategoryType.Game)
                    this.ValidationProvider.SetValidationRule((Control)this.lookUpEditServer, (ValidationRuleBase)UtilValidation.GetNotEmptyCondition());
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }
        }

        protected override bool Validate()
        {
            if (this.paramDictionary.Where(i => StringExtension.IsNull(i.Key.Title)).Count() > 0)
                throw new ExceptionIsKurali("Lütfen parametre başlıklarını boş geçmeyiniz.");
            return base.Validate();
        }

        public void InitMask()
        {
        }

        public void InitLookUp()
        {
            try
            {
                var api = new RoyaleSupportClient();
                var categories = api.SqlDefinitionCategories();

                this.lookUpEditSqlTanimKategori.Properties.DataSource = categories.ToList();
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }

            try
            {
                var api = new RoyaleSupportClient();
                var category = api.SqlDefinitionCategoryById(this.aktifSqlTanim.SqlDefinitionCategoryId);

                UtilLookUp.InitLookupEdit(this.lookUpEditVeritabani);
                if (category.Type == EnumCategoryType.Game)
                    this.lookUpEditVeritabani.Properties.DataSource = (object)VeritabaniTipi.ListeGame;
                else
                    this.lookUpEditVeritabani.Properties.DataSource = (object)VeritabaniTipi.ListeShared;
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }


            try
            {
                var api = new RoyaleSupportClient();
                var shards = api.Shards().ToList();

                UtilLookUp.InitLookupEdit(this.lookUpEditServer, "Id", "Name");
                this.lookUpEditServer.Properties.DataSource = shards;
                currentShard = shards.Count > 0 ? shards[0].Id : 0;


                this.bindingSourceShard.DataSource = shards.Count > 0 ? shards[0] : null;

            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }
        }

        public void InitData()
        {
            if (this.FormParams.Contains("Id"))
            {
                try
                {
                    var api = new RoyaleSupportClient();
                    var sqlDefinition = api.SqlDefinitionById(Convert.ToInt32(this.FormParams["Id"]));

                    this.aktifSqlTanim = sqlDefinition;
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }

            }
            else
            {
                this.aktifSqlTanim = new SqlDefinitionDto()
                {
                    SqlDefinitionCategoryId = Convert.ToInt32(this.FormParams["SqlTanimKategoriId"])
                };
            }

            this.bindingSourceSqlEkran.DataSource = (object)this.aktifSqlTanim;

            this.bindingSourceShard.DataSource = this.currentShard;

            this.Text = aktifSqlTanim.Name;

            if (this.aktifSqlTanim.Id.Equals(0))
                return;


            try
            {
                var api = new RoyaleSupportClient();
                var patameters = api.SqlDefinitionParameters(this.aktifSqlTanim.Id);

                this.kayitliParametreListesi = patameters.ToList();
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }



            this.ParametreIslemleri();
            this.KosulIslemleri();
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
            this.bindingSourceSqlTanimListe.EndEdit();
            this.bindingSourceSqlEkran.EndEdit();
            //if (!(this.pusulaEntities.Sistem.SqlTanimParametreAdapter.Iptal(this.pusulaEntities.Sistem.SqlTanimParametreQuery.GetParametrelerBySqlTanimId(this.aktifSqlTanim.Id, true)).SonucKodu == EnumSonucTipi.IslemBasarili))
            //    return false;


            foreach (string matchesParameter in this.matchesParameters)
            {
                UserControlParamaterAndValue paramaterAndValue = (UserControlParamaterAndValue)((IEnumerable<Control>)this.xtraScrollableControlParametreler.Controls.Find("Parameter_" + matchesParameter.Replace("@", ""), true)).FirstOrDefault<Control>();
                if (paramaterAndValue.IsNull() || paramaterAndValue.Value.IsNull())
                    throw new ExceptionBeklenmeyen("Parametre boş olamaz.");
            }


            foreach (string matchesParameter in this.matchesParameters)
            {
                UserControlParamaterAndValue paramaterAndValue = (UserControlParamaterAndValue)((IEnumerable<Control>)this.xtraScrollableControlParametreler.Controls.Find("Parameter_" + matchesParameter.Replace("@", ""), true)).FirstOrDefault<Control>();
                if (paramaterAndValue.IsNull() || paramaterAndValue.Value.IsNull())
                    throw new ExceptionBeklenmeyen("Parametre boş olamaz.");
            }

            if (aktifSqlTanim.Id == 0)
            {

                try
                {
                    var api = new RoyaleSupportClient();
                    var definition = api.SqlDefinitionPOST(new SqlDefinitionInput()
                    {
                        Name = aktifSqlTanim.Name,
                        Sql = aktifSqlTanim.Sql,
                        Description = aktifSqlTanim.Description,
                        DatabaseType = aktifSqlTanim.DatabaseType,
                        SqlDefinitionCategoryId = aktifSqlTanim.SqlDefinitionCategoryId
                    });

                    this.aktifSqlTanim = definition;
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
                    aktifSqlTanim = api.SqlDefinitionPUT(new SqlDefinitionInput()
                    {
                        Id = aktifSqlTanim.Id,
                        Name = aktifSqlTanim.Name,
                        Sql = aktifSqlTanim.Sql,
                        Description = aktifSqlTanim.Description,
                        DatabaseType = aktifSqlTanim.DatabaseType,
                        SqlDefinitionCategoryId = aktifSqlTanim.SqlDefinitionCategoryId
                    });
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
            }

            try
            {
                var api = new RoyaleSupportClient();
                api.AllSqlDefinitionConditions(this.aktifSqlTanim.Id);
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }

            try
            {
                var api = new RoyaleSupportClient();
                api.AllSqlDefinitionParameters(this.aktifSqlTanim.Id);
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }

            this.ParametreDegerleriniAl();
            foreach (var keyValuePair in this.paramDictionary)
            {
                SqlDefinitionParameterInput sqlTanimParametre = new SqlDefinitionParameterInput();
                sqlTanimParametre.Name = keyValuePair.Key.Name;
                sqlTanimParametre.Title = keyValuePair.Key.Title;
                sqlTanimParametre.ParameterDbValue = keyValuePair.Value;
                sqlTanimParametre.ParameterDbType = keyValuePair.Key.ParameterDbType;
                sqlTanimParametre.SqlDefinitionId = this.aktifSqlTanim.Id;

                try
                {
                    var api = new RoyaleSupportClient();
                    api.SqlDefinitionParameterPUT(sqlTanimParametre);
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
            }

            this.KosullariAl();
            foreach (var keyValuePair in this.kosulDictionary)
            {

                SqlDefinitionConditionInput sqlTanimParametre = new SqlDefinitionConditionInput();
                sqlTanimParametre.Name = keyValuePair.Key.Name;
                sqlTanimParametre.Priority = keyValuePair.Key.Priority;
                sqlTanimParametre.Sql = keyValuePair.Value;
                sqlTanimParametre.WarningMessage = keyValuePair.Key.WarningMessage;
                sqlTanimParametre.SqlDefinitionId = this.aktifSqlTanim.Id;
                sqlTanimParametre.IsResultExists = keyValuePair.Key.IsResultExists;
                sqlTanimParametre.IsContinueAction = keyValuePair.Key.IsContinueAction;

                try
                {
                    var api = new RoyaleSupportClient();
                    api.SqlDefinitionConditionPUT(sqlTanimParametre);
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
            }
            return true;
        }

        private void ParametreDegerleriniAl()
        {
            this.paramDictionary.Clear();
            foreach (string matchesParameter in this.matchesParameters)
            {
                UserControlParamaterAndValue paramaterAndValue = (UserControlParamaterAndValue)((IEnumerable<Control>)this.xtraScrollableControlParametreler.Controls.Find("Parameter_" + matchesParameter.Replace("@", ""), true)).FirstOrDefault<Control>();
                if (paramaterAndValue.IsNotNull() && paramaterAndValue.Value.IsNotNull())
                    this.paramDictionary.Add(new SqlDefinitionParameterDto()
                    {
                        Name = matchesParameter,
                        ParameterDbType = (int)paramaterAndValue.DbType,
                        Title = paramaterAndValue.Baslik,
                    }, paramaterAndValue.Value.ToString());
            }
        }

        private void KosullariAl()
        {
            this.kosulDictionary.Clear();


            foreach (Control item in this.xtraScrollableControlKosullar.Controls)
            {
                var kosul = item as UserControlCondition;
                this.kosulDictionary.Add(new SqlDefinitionConditionDto()
                {
                    Name = kosul.textEditKosulAdi.EditValue.ToString(),
                    Priority = Convert.ToInt32(kosul.numericUpDownKosulOncelik.Value),
                    Sql = kosul.richEditControlSorgu.Text,
                    WarningMessage = kosul.textEditKosulUyari.EditValue.IsNull() ? "" : kosul.textEditKosulUyari.EditValue.ToString(),
                    IsContinueAction = Convert.ToBoolean(kosul.radioGroupKosulIslemDevam.EditValue),
                    IsResultExists = Convert.ToBoolean(kosul.radioGroupKosulSonuc.EditValue),
                }, kosul.textEditKosulAdi.EditValue.ToString());
            }
        }

        private void GridDoldur(SqlResult result)
        {
            DataSet dataSet = new DataSet();

            var dataTable = new DataTable();

            foreach (var item in result.Columns)
            {
                dataTable.Columns.Add(item);
            }

            foreach (var item in result.Rows)
            {
                dataTable.Rows.Add(item.ToArray());
            }

            dataSet.Tables.Add(dataTable);
            this.gridViewParametreler.Columns.Clear();
            this.gridControlParametreler.DataSource = (object)dataSet.Tables[0];
            foreach (GridColumn column in (CollectionBase)this.gridViewParametreler.Columns)
            {
                column.OptionsColumn.ReadOnly = true;
                if (column.FieldName.Length * 8 + 10 > column.Width)
                    column.Width = column.FieldName.Length * 8 + 10;
                if (column.VisibleIndex == 1)
                {
                    column.SummaryItem.SummaryType = SummaryItemType.Count;
                    column.SummaryItem.DisplayFormat = "{0:n0} Adet";
                }
            }
        }

        public DataSet ExecuteSql(string sql, Dictionary<SqlDefinitionParameterDto, string> parameters)
        {
            //bool flag = false;
            //DbConnection connection = null;
            //var kategori = _sqlTanimKategoriRepository.FindById(this.aktifSqlTanim.SqlTanimKategoriId);
            //
            //switch (kategori.KategoriTipi)
            //{
            //    case (int)EnumKategoriTipi.Shared:
            //        {
            //            switch (this.aktifSqlTanim.VeritabaniId)
            //            {
            //                case (int)EnumVeritabaniTipi.Account:
            //                    connection = GetConnectionAccount();
            //                    break;
            //                case (int)EnumVeritabaniTipi.Panel:
            //                    connection = GetConnectionPanel();
            //                    break;
            //            }
            //        }
            //        break;
            //    case (int)EnumKategoriTipi.Game:
            //        {
            //            connection = GetConnection(this.currentShard, (DatabaseType)this.aktifSqlTanim.VeritabaniId);
            //        }
            //        break;
            //}
            //
            //if (connection == null)
            //    throw new ExceptionBeklenmeyen("Veritabanı seçilemedi.");
            //
            //DbCommand storeCommand = connection.CreateCommand();
            //storeCommand.CommandText = sql;
            //DataSet dataSet;
            //try
            //{
            //    if (parameters.IsNotNull())
            //    {
            //        foreach (KeyValuePair<SqlTanimParametre, string> parameter in parameters)
            //        {
            //            SqlTanimParametre key = parameter.Key;
            //            EnumSqlParametreTipi parametreDbTipi = (EnumSqlParametreTipi)key.ParametreDbTipi;
            //            SqlParameter sqlParameter1 = new SqlParameter(key.Ad, (object)parameter.Value);
            //            sqlParameter1.DbType = EnumSqlParametreTipiConverter.ToDbType(parametreDbTipi);
            //            SqlParameter sqlParameter2 = sqlParameter1;
            //            storeCommand.Parameters.Add((object)sqlParameter2);
            //            storeCommand.CommandTimeout = 12000;
            //        }
            //    }
            //    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            //    sqlDataAdapter.SelectCommand = (SqlCommand)storeCommand;
            //    if (storeCommand.Connection.State == ConnectionState.Closed)
            //    {
            //        storeCommand.Connection.Open();
            //        flag = true;
            //    }
            //    dataSet = new DataSet();
            //    sqlDataAdapter.Fill(dataSet, "SQL");
            //}
            //finally
            //{
            //    if (flag)
            //        storeCommand.Connection.Close();
            //    storeCommand.Cancel();
            //    storeCommand.Dispose();
            //}
            //return dataSet;

            return null;
        }

        public DataSet SqlCalistir(string sql, List<SqlDefinitionParameterDto> parameters)
        {
            //bool flag = false;
            //DbCommand storeCommand = new SqlConnection(EditorApplication.EditorApplication.ToolConnection).CreateCommand();
            //storeCommand.CommandText = sql;
            //DataSet dataSet;
            //try
            //{
            //    foreach (SqlTanimParametre parameter in parameters)
            //    {
            //        EnumSqlParametreTipi parametreDbTipi = (EnumSqlParametreTipi)parameter.ParametreDbTipi;
            //        SqlParameter sqlParameter1 = new SqlParameter(parameter.Ad, (object)parameter.ParametreDeger);
            //        sqlParameter1.DbType = EnumSqlParametreTipiConverter.ToDbType(parametreDbTipi);
            //        SqlParameter sqlParameter2 = sqlParameter1;
            //        storeCommand.Parameters.Add((object)sqlParameter2);
            //        storeCommand.CommandTimeout = 120;
            //    }
            //    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            //    sqlDataAdapter.SelectCommand = (SqlCommand)storeCommand;
            //    if (storeCommand.Connection.State == ConnectionState.Closed)
            //    {
            //        storeCommand.Connection.Open();
            //        flag = true;
            //    }
            //    dataSet = new DataSet();
            //    sqlDataAdapter.Fill(dataSet, "SQL");
            //}
            //finally
            //{
            //    if (flag)
            //        storeCommand.Connection.Close();
            //    storeCommand.Cancel();
            //    storeCommand.Dispose();
            //}
            //return dataSet;

            return null;
        }


        public DbConnection CreateDbConnection(int shardId, DatabaseType databaseType)
        {
            if (EditorApplication.EditorApplication.Connections.TryGetValue(shardId, out var shardConnections))
            {
                if (!shardConnections.ContainsKey(databaseType))
                    throw new ExceptionBeklenmeyen($"Selected database (id: {databaseType}) not found.");

                return new SqlConnection(shardConnections[databaseType]);
            }

            throw new ExceptionBeklenmeyen($"Selected shard (id: {shardId}) not found.");
        }

        public DbConnection GetConnection(int shardId, DatabaseType databaseType)
        {
            var _connection = EditorApplication.EditorApplication.Connections;

            return CreateDbConnection(shardId, databaseType);
        }
        public DbConnection GetConnectionAccount()
        {
            var _connection = EditorApplication.EditorApplication.Connections;

            return new SqlConnection(EditorApplication.EditorApplication.AccountConnection);
        }

        public DbConnection GetConnectionPanel()
        {
            var _connection = EditorApplication.EditorApplication.Connections;

            return new SqlConnection(EditorApplication.EditorApplication.PanelConnection);
        }


        public bool CheckSql(string sql, Dictionary<SqlDefinitionParameterDto, string> parameters)
        {
            //DbConnection connection = null;
            //var kategori = _sqlTanimKategoriRepository.FindById(this.aktifSqlTanim.SqlTanimKategoriId);
            //
            //switch (kategori.KategoriTipi)
            //{
            //    case (int)EnumKategoriTipi.Shared:
            //        {
            //            switch (this.aktifSqlTanim.VeritabaniId)
            //            {
            //                case (int)EnumVeritabaniTipi.Account:
            //                    connection = GetConnectionAccount();
            //                    break;
            //                case (int)EnumVeritabaniTipi.Panel:
            //                    connection = GetConnectionPanel();
            //                    break;
            //            }
            //        }
            //        break;
            //    case (int)EnumKategoriTipi.Game:
            //        {
            //            connection = GetConnection(this.currentShard, (DatabaseType)this.aktifSqlTanim.VeritabaniId);
            //        }
            //        break;
            //}
            //
            //if (connection == null)
            //    throw new ExceptionBeklenmeyen("Veritabanı seçilemedi.");
            //
            //DbCommand storeCommand = connection.CreateCommand();
            //storeCommand.CommandText = sql;
            //bool flag1 = false;
            //bool flag2;
            //try
            //{
            //    foreach (KeyValuePair<SqlTanimParametre, string> parameter in parameters)
            //    {
            //        SqlTanimParametre key = parameter.Key;
            //        EnumSqlParametreTipi parametreDbTipi = (EnumSqlParametreTipi)key.ParametreDbTipi;
            //        SqlParameter sqlParameter1 = new SqlParameter(key.Ad, (object)parameter.Value);
            //        sqlParameter1.DbType = EnumSqlParametreTipiConverter.ToDbType(parametreDbTipi);
            //        SqlParameter sqlParameter2 = sqlParameter1;
            //        storeCommand.Parameters.Add((object)sqlParameter2);
            //        storeCommand.CommandTimeout = 12000;
            //    }
            //    if (storeCommand.Connection.State == ConnectionState.Closed)
            //    {
            //        storeCommand.Connection.Open();
            //        flag1 = true;
            //    }
            //    storeCommand.ExecuteNonQuery();
            //    flag2 = true;
            //}
            //catch (Exception ex)
            //{
            //    flag2 = false;
            //}
            //finally
            //{
            //    if (flag1)
            //        storeCommand.Connection.Close();
            //    storeCommand.Parameters.Clear();
            //    storeCommand.Dispose();
            //}
            //return flag2;
            return false;
        }

        public void ExportToExcel(DataTable pDataTable, string pExcelFilePath)
        {
            /*GridControl gridControl = new GridControl();
            GridView gridView = new GridView();
            gridControl.MainView = (BaseView)gridView;
            gridControl.ViewCollection.AddRange(new BaseView[1]
            {
        (BaseView) gridView
            });
            gridView.GridControl = gridControl;
            gridControl.BindingContext = new BindingContext();
            gridControl.DataSource = (object)pDataTable;
            gridView.ExportToXls(pExcelFilePath);*/
        }

        private void MailGonder()
        {
            //this.ExportToExcel(this.pusulaEntities.Sistem.SqlTanimQuery.ExecuteSql(this.memoEditSql.EditValue.ToString(), this.paramDictionary).Tables[0], "D:\\Test.xls");
        }

        public void SqlDogrula()
        {
            //this.kosul.SqlDogrulandiMi = this.CheckSql(this.richEditControlSorgu..ToString(), this.paramDictionary);
        }

        public bool SelectCumlesiDogrula()
        {
            return true;
            if (StringExtension.IsNull(this.aktifSqlTanim.Sql))
                return false;
            bool flag = false;
            if (this.aktifSqlTanim.Sql.Trim().ToLower().StartsWith("select"))
                flag = true;
            this.kosul.SelectCumlesiMi = flag;
            if (this.kosul.SelectCumlesiMi)
                return flag;
            int num = (int)UtilMessage.Show(EnumUtilMessage.SqlTanimSelect, null, "Select cümlesi değil", "Eksik Bilgi", MessageBoxButtons.OK);
            return false;
        }

        private void ParameteTipleriniBelirle()
        {
            this.xtraScrollableControlParametreler.Controls.Clear();
            this.matchesParameters.Clear();
            MatchCollection source = new Regex("@([a-zA-Z][a-zA-Z0-9_]*)").Matches(this.aktifSqlTanim.Sql);
            this.matchesParameters.Clear();
            foreach (Capture capture in source.Cast<Match>().Where<Match>((Func<Match, bool>)(match => !this.matchesParameters.Contains(match.Value))))
                this.matchesParameters.Add(capture.Value);
            int num = 0;
            foreach (string matchesParameter in this.matchesParameters)
            {
                string mach = matchesParameter;
                var sqlTanimParametre = this.kayitliParametreListesi.Where(d => d.Name == mach).FirstOrDefault() ?? new SqlDefinitionParameterDto();
                string parametreAdi = mach.Replace("@", "");
                UserControlParamaterAndValue paramaterAndValue = new UserControlParamaterAndValue(parametreAdi, sqlTanimParametre.Title, true, (byte)sqlTanimParametre.ParameterDbType);
                paramaterAndValue.Location = new Point(0, num * 30);
                paramaterAndValue.Name = "Parameter_" + parametreAdi;
                this.xtraScrollableControlParametreler.Controls.Add((Control)paramaterAndValue);
                ++num;
            }
            this.kosul.ParametreTipleriBelirlendiMi = true;
        }
        private void KosullariBelirle()
        {
            this.xtraScrollableControlKosullar.Controls.Clear();

            if (this.aktifSqlTanim.Id == 0)
                return;


            var kosullar = new List<SqlDefinitionConditionDto>();
            try
            {
                var api = new RoyaleSupportClient();
                kosullar = api.SqlDefinitionConditions(this.aktifSqlTanim.Id).ToList();
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }

            foreach (var item in kosullar)
            {
                UserControlCondition kosul = new UserControlCondition();
                kosul.Location = new Point(0, this.xtraScrollableControlKosullar.Controls.Count * 129);
                kosul.Name = "Kosul_" + this.xtraScrollableControlKosullar.Controls.Count;

                kosul.textEditKosulAdi.EditValue = item.Name;
                kosul.textEditKosulUyari.EditValue = item.WarningMessage;

                kosul.numericUpDownKosulOncelik.Value = item.Priority;
                kosul.radioGroupKosulIslemDevam.EditValue = item.IsContinueAction;
                kosul.radioGroupKosulSonuc.EditValue = item.IsResultExists;

                kosul.richEditControlSorgu.Text = item.Sql;


                kosul.simpleButtonSil.Click += (ss, ee) =>
                {
                    this.xtraScrollableControlKosullar.Controls.Remove(kosul);

                    int i = 0;
                    foreach (Control itemx in this.xtraScrollableControlKosullar.Controls)
                    {
                        itemx.Location = new Point(0, i * 129);
                        i++;
                    }

                };
                this.xtraScrollableControlKosullar.Controls.Add((Control)kosul);
            }


        }

        private void barButtonItemKaydet_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.richEditControlSorgu.Focus();
            this.textEditBaslik.Focus();
            if (!this.Validate() || !this.Kaydet())
                return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void barButtonItemIptal_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnParametreTipleri_Click(object sender, EventArgs e)
        {
            this.ParametreIslemleri();
        }

        private void KosulIslemleri()
        {
            this.KosullariBelirle();
        }

        private void ParametreIslemleri()
        {
            this.kosul = new Kosul();
            if (!this.SelectCumlesiDogrula())
                return;
            this.ParameteTipleriniBelirle();
        }
        long ToInt(string addr)
        {
            // careful of sign extension: convert to uint first;
            // unsigned NetworkToHostOrder ought to be provided.
            return (long)(uint)IPAddress.NetworkToHostOrder(
                (int)IPAddress.Parse(addr).Address);
        }
        private void barButtonItemCalistir_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.kosul == null)
            {
                int num1 = (int)UtilMessage.Show(EnumUtilMessage.SqlTanimParametreTipi, null, "Lütfen önce parametre tiplerini belirleyiniz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (!this.kosul.ParametreTipleriBelirlendiMi)
            {
                int num2 = (int)UtilMessage.Show(EnumUtilMessage.SqlTanimSQLParametreTipi, null, "Lütfen SQL cümlenizdeki parametre tiplerini belirleyiniz....", "Eksik Bilgi", MessageBoxButtons.OK);
            }
            else
            {
                if (this.kosul.ParametreTipleriBelirlendiMi)
                {
                    this.ParametreDegerleriniAl();
                    if (!base.Validate())
                        return;

                    //this.SqlDogrula();
                }
                /*if (!this.kosul.SqlDogrulandiMi)
                {
                    int num3 = (int)UtilMessage.Show(EnumUtilMessage.SqlTanimParametreTipiveDegeri, null, "Lütfen SQL Cümleciğinizi,parametre tiplerinizi ve değerlerini doğrulayın...", "Eksik Bilgi", MessageBoxButtons.OK);
                }
                else
                {*/
                //if (!this.kosul.SqlDogrulandiMi)
                //    return;

                if (!base.Validate())
                    return;


                this.KosullariAl();
                this.ParametreDegerleriniAl();

                foreach (string matchesParameter in this.matchesParameters)
                {
                    UserControlParamaterAndValue paramaterAndValue = (UserControlParamaterAndValue)((IEnumerable<Control>)this.xtraScrollableControlParametreler.Controls.Find("Parameter_" + matchesParameter.Replace("@", ""), true)).FirstOrDefault<Control>();
                    if (paramaterAndValue.IsNull() || paramaterAndValue.Value.IsNull())
                        throw new ExceptionBeklenmeyen("Parametre boş olamaz.");
                }

                var api = new RoyaleSupportClient();

                var paramDict = this.paramDictionary;

                var inputs = new List<SqlInput>();
                foreach (var item in paramDict)
                {

                    var val = item.Value;
                    if (item.Key.ParameterDbType == 2)
                    {
                        val = DateTime.Parse(item.Value).ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
                    }
                    else if (item.Key.ParameterDbType == 4)
                    {
                        if (!IPAddress.TryParse(item.Value, out var ipAddress))
                        {
                            throw new ExceptionBeklenmeyen("Geçersiz ip adresi.");
                        }
                        val = ToInt(item.Value).ToString();
                    }
                    inputs.Add(new SqlInput() { Name = item.Key.Name, Value = val });
                }

                var result = api.Execute(this.aktifSqlTanim.Id, (int)this.currentShard, inputs);



                //foreach (var kosul in kosulDictionary.OrderBy(p => p.Key.Priority))
                //{
                //
                //    DataSet dataSet = this.ExecuteSql(kosul.Key.Sql.ToString(), this.paramDictionary);
                //    var rows = dataSet.Tables[0].Rows;
                //
                //    var mesaj = kosul.Key.WarningMessage;
                //
                //    if (kosul.Key.IsResultExists && rows.Count > 0)
                //    {
                //        if (kosul.Key.IsContinueAction)
                //        {
                //            //UtilMessage.ShowIslemBasariliMesaji("Koşul sağlandı", kosul.Key.Adi + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam ediliyor.");
                //        }
                //        else
                //        {
                //            throw new ExceptionBeklenmeyen("Koşul: " + kosul.Key.Name + Environment.NewLine + Environment.NewLine + "İşleme devam edilemiyor." + Environment.NewLine + Environment.NewLine + "Mesaj: " + mesaj);
                //        }
                //    }
                //    else if (!kosul.Key.IsResultExists && rows.Count == 0)
                //    {
                //        if (kosul.Key.IsContinueAction)
                //        {
                //            //UtilMessage.ShowIslemBasariliMesaji("Koşul sağlandı", kosul.Key.Adi + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam ediliyor.");
                //        }
                //        else
                //        {
                //            throw new ExceptionBeklenmeyen("Koşul: " + kosul.Key.Name + Environment.NewLine + Environment.NewLine + "İşleme devam edilemiyor." + Environment.NewLine + Environment.NewLine + "Mesaj: " + mesaj);
                //        }
                //    }
                //    else if (!kosul.Key.IsResultExists && rows.Count > 0)
                //    {
                //        throw new ExceptionBeklenmeyen("Koşul: " + kosul.Key.Name + Environment.NewLine + Environment.NewLine + "İşleme devam edilemiyor." + Environment.NewLine + Environment.NewLine + "Mesaj: " + mesaj);
                //    }
                //    else if (kosul.Key.IsResultExists && rows.Count == 0)
                //    {
                //        throw new ExceptionBeklenmeyen("Koşul: " + kosul.Key.Name + Environment.NewLine + Environment.NewLine + "İşleme devam edilemiyor." + Environment.NewLine + Environment.NewLine + "Mesaj: " + mesaj);
                //    }
                //}

                this.GridDoldur(result);
                //}
            }
        }

        private void barButtonItemParametreOlustur_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.richEditControlSorgu.Focus();
            this.textEditBaslik.Focus();
            this.ParametreIslemleri();
        }

        private void barButtonItemMailAt_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.MailGonder();
        }

        private void lookUpEditServer_EditValueChanged(object sender, EventArgs e)
        {
            object editValue = ((BaseEdit)sender).EditValue;
            currentShard = Convert.ToInt32(editValue);
        }

        private void richEditControl1_RtfTextChanged(object sender, EventArgs e)
        {
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

            richEditControlSorgu.GetService<ISyntaxHighlightService>().Execute();
        }

        private void simpleButtonKosulEkle_Click(object sender, EventArgs e)
        {
            UserControlCondition kosul = new UserControlCondition();
            kosul.Location = new Point(0, this.xtraScrollableControlKosullar.Controls.Count * 129);
            kosul.Name = "Kosul_" + this.xtraScrollableControlKosullar.Controls.Count;
            kosul.simpleButtonSil.Click += (ss, ee) =>
            {
                this.xtraScrollableControlKosullar.Controls.Remove(kosul);

                int i = 0;
                foreach (Control item in this.xtraScrollableControlKosullar.Controls)
                {
                    item.Location = new Point(0, i * 129);
                    i++;
                }

            };
            this.xtraScrollableControlKosullar.Controls.Add((Control)kosul);
        }

        private void barButtonItemKosullariTestEt_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.KosullariAl();
            this.ParametreDegerleriniAl();

            foreach (string matchesParameter in this.matchesParameters)
            {
                UserControlParamaterAndValue paramaterAndValue = (UserControlParamaterAndValue)((IEnumerable<Control>)this.xtraScrollableControlParametreler.Controls.Find("Parameter_" + matchesParameter.Replace("@", ""), true)).FirstOrDefault<Control>();
                if (paramaterAndValue.IsNull() || paramaterAndValue.Value.IsNull())
                    throw new ExceptionBeklenmeyen("Parametre boş olamaz.");
            }

            foreach (var kosul in kosulDictionary.OrderBy(p => p.Key.Priority))
            {

                DataSet dataSet = this.ExecuteSql(kosul.Key.Sql.ToString(), this.paramDictionary);
                var rows = dataSet.Tables[0].Rows;

                var mesaj = kosul.Key.WarningMessage;

                if (kosul.Key.IsResultExists && rows.Count > 0)
                {
                    if (kosul.Key.IsContinueAction)
                    {
                        UtilMessage.ShowIslemBasariliMesaji("Koşul sağlandı", kosul.Key.Name + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam ediliyor.");
                    }
                    else
                    {
                        UtilMessage.ShowIslemBasariliMesaji("Koşul sağlandı", kosul.Key.Name + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam edilmiyor.");
                    }
                }
                else if (!kosul.Key.IsResultExists && rows.Count == 0)
                {
                    if (kosul.Key.IsContinueAction)
                    {
                        UtilMessage.ShowIslemBasariliMesaji("Koşul sağlandı", kosul.Key.Name + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam ediliyor.");
                    }
                    else
                    {
                        UtilMessage.ShowIslemBasariliMesaji("Koşul sağlandı", kosul.Key.Name + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam edilmiyor.");
                    }
                }
                else if (!kosul.Key.IsResultExists && rows.Count > 0)
                {
                    UtilMessage.ShowIslemBasarisizMesaji("Koşul sağlanamadı", kosul.Key.Name + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam edilmiyor." + Environment.NewLine + mesaj);
                }
                else if (kosul.Key.IsResultExists && rows.Count == 0)
                {
                    UtilMessage.ShowIslemBasarisizMesaji("Koşul sağlanamadı", kosul.Key.Name + ": " + $"(Sonuç sayısı: {rows.Count})" + " İşleme devam edilmiyor." + Environment.NewLine + mesaj);
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItemKarakteriSorgula.Enabled = false;
            toolStripMenuItemHesabiSorgula.Enabled = false;

            var rowHandle = gridViewParametreler.FocusedRowHandle;
            if (rowHandle == -1)
                return;

            //check for player id
            if (this.lookUpEditServer.Enabled)
            {
                var column = gridViewParametreler.Columns.ColumnByFieldName("player_id");
                if(column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("playerid");
                if(column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("charid");
                if(column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("char_id");
                if(column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("pid");

                if (column == null)
                {
                    toolStripMenuItemKarakteriSorgula.Enabled = false;
                }
                else
                {
                    toolStripMenuItemKarakteriSorgula.Enabled = true;
                }
            }


            {
                var column = gridViewParametreler.Columns.ColumnByFieldName("account_id");
                if (column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("accountid");
                if (column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("user_id");
                if (column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("userid");
                if (column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("account_name");
                if (column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("accountname");
                if (column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("user_name");
                if (column == null)
                    column = gridViewParametreler.Columns.ColumnByFieldName("username");


                if (column == null)
                {
                    toolStripMenuItemHesabiSorgula.Enabled = false;
                }
                else
                {
                    toolStripMenuItemHesabiSorgula.Enabled = true;
                }
            }

            //check for player id
                    //if (this.lookUpEditServer.Enabled)
                }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var clickedItem = e.ClickedItem;
            

            if (clickedItem.Tag == "Hesap")
            {
                var rowHandle = gridViewParametreler.FocusedRowHandle;
                if (rowHandle == -1)
                    return;

                {
                    var column = gridViewParametreler.Columns.ColumnByFieldName("account_id");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("accountid");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("user_id");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("userid");

                    if (column == null)
                    {
                        toolStripMenuItemHesabiSorgula.Enabled = false;
                    }
                    else
                    {
                        if (int.TryParse(gridViewParametreler.GetRowCellValue(rowHandle, column).ToString(),
                                out var charId))
                        {

                            var formParams = new UtilParameters();
                            formParams.Add("UserId", charId);

                            EditorApplication.EditorApplication.Module.AccountModule.AccountQueryModule.Account.Show(formParams);

                        }

                        return;
                    }
                }
                {
                    var column = gridViewParametreler.Columns.ColumnByFieldName("account_name");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("accountname");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("user_name");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("username");

                    if (column == null)
                    {
                        toolStripMenuItemHesabiSorgula.Enabled = false;
                    }
                    else
                    {

                        var username = gridViewParametreler.GetRowCellValue(rowHandle, column).ToString();
                        {

                            var formParams = new UtilParameters();
                            formParams.Add("UserName", username);

                            EditorApplication.EditorApplication.Module.AccountModule.AccountQueryModule.Account.Show(formParams);

                        }
                    }
                }

            }
            else if (clickedItem.Tag == "Karakter")
            {
                var rowHandle = gridViewParametreler.FocusedRowHandle;
                if (rowHandle == -1)
                    return;

                //check for player id
                if (this.lookUpEditServer.Enabled)
                {
                    var column = gridViewParametreler.Columns.ColumnByFieldName("player_id");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("playerid");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("charid");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("char_id");
                    if (column == null)
                        column = gridViewParametreler.Columns.ColumnByFieldName("pid");

                    if (column == null)
                    {
                        toolStripMenuItemKarakteriSorgula.Enabled = false;
                    }
                    else
                    {

                        if (int.TryParse(gridViewParametreler.GetRowCellValue(rowHandle, column).ToString(),
                                out var charId))
                        {

                            var formParams = new UtilParameters();
                            formParams.Add("ShardId", Convert.ToInt32(this.lookUpEditServer.EditValue));
                            formParams.Add("CharId", charId);

                            EditorApplication.EditorApplication.Module.CharacterModule.CharacterActionModule.Character.Show(formParams);

                        }
                    }
                }
            }
        }
    }
}
