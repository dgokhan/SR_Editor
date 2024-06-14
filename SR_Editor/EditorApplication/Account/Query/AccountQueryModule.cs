using SR_Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication.Account.Query
{
    public class AccountQueryModule : ModuleInfoGroup
    {
        public ModuleInfo Account
        {
            get
            {
                return base["FormAccount"];
            }
        }
        public AccountQueryModule()
        {
            base.NameSpace = "SR_Editor.Modules.Account.Query";
            base.Caption = "İşlemler";

            base.Add(new ModuleInfo()
            {
                Key = "FormAccount",
                Caption = "Hesap Sorgulama",
                IsMenuVisible = true,
                IsDialog = false,
                Permission = RoyaleSupportPermissions.Account.Default
            });
        }
    }
}
