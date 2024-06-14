using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.Utility
{
    internal class Kosul
    {
        public bool ParametreTipleriBelirlendiMi { get; set; }

        public bool SqlDogrulandiMi { get; set; }

        public bool SelectCumlesiMi { get; set; }

        public Kosul()
        {
            this.ParametreTipleriBelirlendiMi = false;
            this.SqlDogrulandiMi = false;
            this.SelectCumlesiMi = false;
        }

        public bool KaydedilsinMi
        {
            get
            {
                return this.ParametreTipleriBelirlendiMi && this.SqlDogrulandiMi && this.SelectCumlesiMi;
            }
        }
    }
}
