using SR_Editor.Core;
using SR_Editor.EditorApplication.Account;
using SR_Editor.EditorApplication.Account.Query;
using SR_Editor.EditorApplication.Character;
using SR_Editor.EditorApplication.SqlQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication
{
    public class Module : ModuleInfoGroup
    {
        private SqlQueryModule sqlQueryModule;

        public SqlQueryModule SqlQueryModule
        {
            get
            {
                if (this.sqlQueryModule == null)
                {
                    this.sqlQueryModule = new SqlQueryModule();
                }
                return this.sqlQueryModule;
            }
        }

        private AccountModule accountModule;

        public AccountModule AccountModule
        {
            get
            {
                if (this.accountModule == null)
                {
                    this.accountModule = new AccountModule();
                }
                return this.accountModule;
            }
        }

        private CharacterModule characterModule;

        public CharacterModule CharacterModule
        {
            get
            {
                if (this.characterModule == null)
                {
                    this.characterModule = new CharacterModule();
                }
                return this.characterModule;
            }
        }
        public Module()
        {
            base.Key = "SR_Editor";
            base.NameSpace = "SR_Editor";
            base.Caption = "Module List";
            base.Add(this.SqlQueryModule);
            base.Add(this.AccountModule);
            base.Add(this.CharacterModule);
        }
    }
}
