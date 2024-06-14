using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraTreeList;
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
    public partial class FormSqlTanimListesi : FormBase, IFormBase
    {
        private SqlDefinitionCategoryDto aktifSqlTanimKategori;
        private List<SqlDefinitionDto> sqlTanimListesi;
        private SqlDefinitionDto aktifSqlTanim;

        public FormSqlTanimListesi()
        {
            InitializeComponent();
        }


        public void InitDesign()
        {
            UtilBarManager.InitBarManager(this.barManager1);
            UtilCommon.InitShortcut(this.Module, (BarItem)this.barButtonItemKaydet, EnumShortcut.Save);
            UtilCommon.InitShortcut(this.Module, (BarItem)this.barButtonItemIptal, EnumShortcut.Cancel);
            UtilCommon.InitPopupMenu(this.treeListSqlTanimKategori, this.popupMenuSqlTanimKategori);
            UtilCommon.InitPopupMenu(this.gridViewSqlTanimListe, this.popupMenuSqlTanim);
        }

        public void InitValidationRules()
        {
        }

        protected override bool Validate()
        {
            return base.Validate();
        }

        public void InitMask()
        {
        }

        public void InitLookUp()
        {
        }

        public void InitData()
        {
            this.Listele();
        }

        public void InitRight()
        {
        }

        public override void FormParamsChanged()
        {
            this.InitDesign();
            this.InitData();
            this.InitMask();
            this.InitLookUp();
            this.InitValidationRules();
            this.InitRight();
        }

        protected override bool Kaydet()
        {
            this.bindingSourceSqlTanimListe.EndEdit();
            //this.pusulaEntities.Sistem.SaveChanges();
            return true;
        }

        protected override void Listele()
        {
            var api = new RoyaleSupportClient();

            try
            {
                var categories = api.SqlDefinitionCategories();
                this.bindingSourceSqlTanimKategori.DataSource = categories;
            }
            catch (ApiException exception)
            {
                UtilMessage.Show(exception.Message);
            }

            this.treeListSqlTanimKategori.RefreshDataSource();
            this.ListeleDetay();
        }

        private void ListeleDetay()
        {
            if (this.treeListSqlTanimKategori.FocusedNode != null)
            {
                this.aktifSqlTanimKategori = (SqlDefinitionCategoryDto)this.treeListSqlTanimKategori.GetDataRecordByNode(this.treeListSqlTanimKategori.FocusedNode);
                this.layoutControlGroupSorguListesi.Text = $"Sorgu Listesi - {aktifSqlTanimKategori.Name}";

                var api = new RoyaleSupportClient();

                try
                {
                    var definitions = api.SqlDefinitions(aktifSqlTanimKategori.Id);

                    this.sqlTanimListesi = definitions.ToList();
                    this.bindingSourceSqlTanimListe.DataSource = (object)this.sqlTanimListesi;
                    this.gridViewSqlTanimListe.RefreshData();
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
            }
            else
                this.bindingSourceSqlTanimListe.DataSource = (object)null;
        }

        private bool ShowFormSqlTanim(int? pId, int pSqlTanimKategoriId)
        {
            UtilParameters pFormParams = new UtilParameters();
            if (pId.HasValue)
                pFormParams.Add("Id", (object)pId.Value);
            pFormParams.Add("SqlTanimKategoriId", (object)pSqlTanimKategoriId);
            EditorApplication.EditorApplication.Module.SqlQueryModule.SqlQueryActionModule.SqlTanim.Show(pFormParams);
            return pFormParams.DialogResult == DialogResult.OK;
        }

        private bool ShowFormSqlTanimCalistir(int? pId, int pSqlTanimKategoriId)
        {
            UtilParameters pFormParams = new UtilParameters();
            if (pId.HasValue)
                pFormParams.Add("Id", (object)pId.Value);
            pFormParams.Add("SqlTanimKategoriId", (object)pSqlTanimKategoriId);
            EditorApplication.EditorApplication.Module.SqlQueryModule.SqlQueryActionModule.SqlTanimCalistir.Show(pFormParams);
            return pFormParams.DialogResult == DialogResult.OK;
        }

        private bool ShowFormSqlTanimKategori(int? pId)
        {
            UtilParameters pFormParams = new UtilParameters();
            if (pId.HasValue)
                pFormParams.Add("Id", (object)pId.Value);
            EditorApplication.EditorApplication.Module.SqlQueryModule.SqlQueryActionModule.SqlTanimKategori.Show(pFormParams);
            return pFormParams.DialogResult == DialogResult.OK;
        }

        private EditorIslemSonuc IptalEtSqlTanim()
        {
            EditorIslemSonuc EditorIslemSonuc = new EditorIslemSonuc(EnumSonucTipi.IslemBasarili);
            if (this.gridViewSqlTanimListe.FocusedRowHandle >= 0)
            {
                var result = UtilMessage.ShowSoruMesaji("Tanımı silmek istediğinize emin misiniz?", "Onay");
                if (result == DialogResult.No)
                    return EditorIslemSonuc;

                var row = (SqlDefinitionDto)this.gridViewSqlTanimListe.GetRow(this.gridViewSqlTanimListe.FocusedRowHandle);
                // var tanim = this._sqlTanimRepository.Find(p => p.Id == row.Id && p.Service == 1);
                //tanim.Service = 0;
                //_sqlTanimRepository.Update(tanim);
                var api = new RoyaleSupportClient();

                try
                {
                    api.SqlDefinitionDELETE(row.Id);
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
            }
            return EditorIslemSonuc;
        }

        private EditorIslemSonuc IptalEtSqlTanimKategori()
        {
            EditorIslemSonuc EditorIslemSonuc = new EditorIslemSonuc(EnumSonucTipi.IslemBasarili);
            if (this.treeListSqlTanimKategori.FocusedNode != null)
            {
                var result = UtilMessage.ShowSoruMesaji("Kategoriyi silmek istediğinize emin misiniz?", "Onay");
                if (result == DialogResult.No)
                    return EditorIslemSonuc;

                var kategori = (SqlDefinitionCategoryDto)this.treeListSqlTanimKategori.GetDataRecordByNode(this.treeListSqlTanimKategori.FocusedNode);
                //var kategoriTanim = _sqlTanimKategoriRepository.Find(p => p.Id == kategori.Id);
                //kategoriTanim.Service = 0;
                //_sqlTanimKategoriRepository.Update(kategoriTanim);

                var api = new RoyaleSupportClient();

                try
                {
                    api.SqlDefinitionCategoryDELETE(kategori.Id);
                }
                catch (ApiException exception)
                {
                    UtilMessage.Show(exception.Message);
                }
                this.Listele();
            }
            return EditorIslemSonuc;
        }

        private void barButtonItemIptal_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void treeListSqlTanimKategori_FocusedNodeChanged(
          object sender,
          FocusedNodeChangedEventArgs e)
        {
            if (this.treeListSqlTanimKategori.FocusedNode == null)
                return;
            this.ListeleDetay();
        }

        private void treeListSqlTanimKategori_DoubleClick(object sender, EventArgs e)
        {
            if (this.aktifSqlTanimKategori == null)
                return;
            if (!this.ShowFormSqlTanimKategori(new int?((int)this.aktifSqlTanimKategori.Id)))
                return;
            this.Listele();
        }

        private void barButtonItemSil_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(this.IptalEtSqlTanimKategori().SonucKodu == EnumSonucTipi.IslemBasarili))
                return;
            this.ListeleDetay();
        }

        private void barButtonItemKategoriEkle_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!this.ShowFormSqlTanimKategori(new int?()))
                return;
            this.Listele();
        }

        private void barButtonItemSqlTanimEkle_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (aktifSqlTanimKategori == null)
                return;

            if (!this.ShowFormSqlTanim(new int?(), (int)this.aktifSqlTanimKategori.Id))
                return;
            this.ListeleDetay();
        }

        private void barButtonItemSqlTanimSil_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(this.IptalEtSqlTanim().SonucKodu == EnumSonucTipi.IslemBasarili))
                return;
            this.ListeleDetay();
        }

        private void gridViewSqlTanimListe_DoubleClick(object sender, EventArgs e)
        {
            if (aktifSqlTanimKategori == null)
                return;
            this.aktifSqlTanim = this.gridViewSqlTanimListe.FocusedRowHandle < 0 ? (SqlDefinitionDto)null : (SqlDefinitionDto)this.gridViewSqlTanimListe.GetRow(this.gridViewSqlTanimListe.FocusedRowHandle);
            if (!this.ShowFormSqlTanimCalistir(new int?((int)this.aktifSqlTanim.Id), (int)this.aktifSqlTanimKategori.Id))
                return;
            this.ListeleDetay();
        }

        private void gridViewSqlTanimListe_FocusedRowChanged(
          object sender,
          FocusedRowChangedEventArgs e)
        {
            if (this.gridViewSqlTanimListe.FocusedRowHandle >= 0)
                this.aktifSqlTanim = (SqlDefinitionDto)this.gridViewSqlTanimListe.GetRow(this.gridViewSqlTanimListe.FocusedRowHandle);
            else
                this.aktifSqlTanim = (SqlDefinitionDto)null;
        }

        private void barButtonItemDuzenle_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (aktifSqlTanimKategori == null)
                return;

            this.aktifSqlTanim = this.gridViewSqlTanimListe.FocusedRowHandle < 0 ? (SqlDefinitionDto)null : (SqlDefinitionDto)this.gridViewSqlTanimListe.GetRow(this.gridViewSqlTanimListe.FocusedRowHandle);
            if (!this.ShowFormSqlTanim(new int?((int)this.aktifSqlTanim.Id), (int)this.aktifSqlTanimKategori.Id))
                return;
            this.ListeleDetay();
        }
    }
}
