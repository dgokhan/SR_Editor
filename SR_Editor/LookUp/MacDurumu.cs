using SR_Editor.LookUp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.LookUp
{
    public class LookUpMacDurumu : LookUpEntityBase<byte>
    {
        public LookUpMacDurumu()
        {
        }

        public LookUpMacDurumu(byte pKey)
          : base(pKey)
        {
        }

        public LookUpMacDurumu(byte pKey, string pValue)
          : base(pKey, pValue)
        {
        }
    }
    public class MacDurumu : LookUpEntityBaseList<byte, LookUpMacDurumu>
    {
        private static MacDurumu _liste;

        public static MacDurumu Liste
        {
            get
            {
                return MacDurumu._liste ?? (MacDurumu._liste = new MacDurumu());
            }
        }

        public MacDurumu()
        {
            this.Load();
        }

        public override sealed void Load()
        {
            this.Add(new LookUpMacDurumu((byte)0, "Baslamadi"));
            this.Add(new LookUpMacDurumu((byte)1, "Basladi"));
            this.Add(new LookUpMacDurumu((byte)2, "Bitti"));
        }
    }
    public enum EnumMacDurumu
    {
        Baslamadi = 0,
        Basladi = 1,
        Bitti = 2,
    }
}
