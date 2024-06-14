using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.LookUp.Base
{
    public class LookUpEntityBase<TId>
    {
        private TId id = default(TId);

        private string aciklama;

        public TId Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string Aciklama
        {
            get
            {
                return this.aciklama;
            }
            set
            {
                this.aciklama = value;
            }
        }

        public LookUpEntityBase()
        {
        }

        public LookUpEntityBase(TId pId)
        {
            this.id = pId;
        }

        public LookUpEntityBase(TId pKey, string pAciklama)
        {
            this.id = pKey;
            this.aciklama = pAciklama;
        }
    }
}
