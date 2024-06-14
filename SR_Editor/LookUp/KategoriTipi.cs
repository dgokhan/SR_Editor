using SR_Editor.LookUp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoyaleSupport;

namespace SR_Editor.LookUp
{
    public class LookUpKategoriTipi : LookUpEntityBase<EnumCategoryType>
    {
        public LookUpKategoriTipi()
        {
        }

        public LookUpKategoriTipi(EnumCategoryType pKey)
          : base(pKey)
        {
        }

        public LookUpKategoriTipi(EnumCategoryType pKey, string pValue)
          : base(pKey, pValue)
        {
        }
    }
    public class KategoriTipi : LookUpEntityBaseList<EnumCategoryType, LookUpKategoriTipi>
    {
        private static KategoriTipi _liste;

        public static KategoriTipi Liste
        {
            get
            {
                return KategoriTipi._liste ?? (KategoriTipi._liste = new KategoriTipi());
            }
        }

        public KategoriTipi()
        {
            this.Load();
        }

        public override sealed void Load()
        {
            this.Add(new LookUpKategoriTipi(EnumCategoryType.Account, "Account"));
            this.Add(new LookUpKategoriTipi(EnumCategoryType.Billing, "Billing"));
            this.Add(new LookUpKategoriTipi(EnumCategoryType.Game, "Game"));
        }
    }
}
