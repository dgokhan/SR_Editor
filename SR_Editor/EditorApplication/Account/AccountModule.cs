using SR_Editor.Core;
using SR_Editor.EditorApplication.Account.Action;
using SR_Editor.EditorApplication.Account.Query;
using SR_Editor.EditorApplication.Character.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication.Account
{
    public class AccountModule : ModuleInfoGroup
    {
        private AccountQueryModule accountQueryModule;
        public AccountQueryModule AccountQueryModule
        {
            get
            {
                if (this.accountQueryModule == null)
                {
                    this.accountQueryModule = new AccountQueryModule();
                }
                return this.accountQueryModule;
            }
        }
        private AccountActionModule accountActionModule;
        public AccountActionModule AccountActionModule
        {
            get
            {
                if (this.accountActionModule == null)
                {
                    this.accountActionModule = new AccountActionModule();
                }
                return this.accountActionModule;
            }
        }
        public AccountModule()
        {
            base.AssemblyName = "SR_Editor";
            base.NameSpace = "SR_Editor.Modules.Account";
            base.Caption = "Hesap İşlemleri";
            //base.ImageName = "Defination.png";
            base.Add(this.AccountQueryModule);
            base.Add(this.AccountActionModule);
        }
    }
}
