using SR_Editor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication.Account.Action
{
    public class AccountActionModule : ModuleInfoGroup
    {
        public ModuleInfo AccountPunishment
        {
            get
            {
                return base["FormAccountPunishment"];
            }
        }
        public AccountActionModule()
        {
            base.NameSpace = "SR_Editor.Modules.Account.Action";
            base.Caption = "İşlemler";

            base.Add(new ModuleInfo()
            {
                Key = "FormAccountPunishment",
                Caption = "Hesap Yasaklama",
                IsMenuVisible = false,
                IsDialog = true,
                Permission = RoyaleSupportPermissions.Account.AccountPunishment
            });
        }
    }
}
