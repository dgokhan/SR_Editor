using SR_Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication.SqlQuery.Action
{
    public class SqlQueryActionModule : ModuleInfoGroup
    {
        public ModuleInfo SqlTanimListesi
        {
            get
            {
                return base["FormSqlTanimListesi"];
            }
        }
        public ModuleInfo SqlTanim
        {
            get
            {
                return base["FormSqlTanim"];
            }
        }
        public ModuleInfo SqlTanimCalistir
        {
            get
            {
                return base["FormSqlTanimCalistir"];
            }
        }
        public ModuleInfo SqlTanimKategori
        {
            get
            {
                return base["FormSqlTanimKategori"];
            }
        }
        public SqlQueryActionModule()
        {
            base.NameSpace = "SR_Editor.Modules.SqlQuery.Action";
            base.Caption = "İşlemler";

            base.Add(new ModuleInfo()
            {
                Key = "FormSqlTanimListesi",
                Caption = "Sorgu Listesi",
                IsMenuVisible = true,
                IsDialog = false,
                Permission = RoyaleSupportPermissions.SqlQuery.Default
            });

            base.Add(new ModuleInfo()
            {
                Key = "FormSqlTanim",
                Caption = "Tanım",
                IsMenuVisible = false,
                IsDialog = true,
                Permission = RoyaleSupportPermissions.SqlQuery.Admin
            });
            
            base.Add(new ModuleInfo()
            {
                Key = "FormSqlTanimCalistir",
                Caption = "Sorgu Çalıştır",
                IsMenuVisible = false,
                IsDialog = false,
                Permission = RoyaleSupportPermissions.SqlQuery.Default
            });

            base.Add(new ModuleInfo()
            {
                Key = "FormSqlTanimKategori",
                Caption = "Kategori Tanım",
                IsMenuVisible = false,
                IsDialog = true,
                Permission = RoyaleSupportPermissions.SqlQuery.Admin
            });

        }
    }
}
