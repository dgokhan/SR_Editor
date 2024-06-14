using SR_Editor.Core;
using SR_Editor.EditorApplication.SqlQuery.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.EditorApplication.SqlQuery
{
    public class SqlQueryModule : ModuleInfoGroup
    {
        private SqlQueryActionModule sqlQueryActionModule;
        public SqlQueryActionModule SqlQueryActionModule
        {
            get
            {
                if (this.sqlQueryActionModule == null)
                {
                    this.sqlQueryActionModule = new SqlQueryActionModule();
                }
                return this.sqlQueryActionModule;
            }
        }
        public SqlQueryModule()
        {
            base.AssemblyName = "SR_Editor";
            base.NameSpace = "SR_Editor.Modules.SqlQuery";
            base.Caption = "Sorgu İşlemleri";
            //base.ImageName = "Defination.png";
            base.Add(this.SqlQueryActionModule);
        }
    }
}
