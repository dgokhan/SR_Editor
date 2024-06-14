using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.LookUp.Base
{

    public class LookUpEntityBaseList<TId, TEntity> : KeyedCollection<TId, TEntity> where TEntity : LookUpEntityBase<TId>
    {
        protected override TId GetKeyForItem(TEntity item)
        {
            return item.Id;
        }

        public virtual void Load()
        {
            //if (DilCeviri.LanguageId != 1)
            //{
            //    for (int i = 0; i < base.Items.Count; i++)
            //    {
            //        TEntity tEntity = base[i];
            //        TEntity tEntity2 = base[i];
            //        tEntity.Aciklama = DilCeviri.GetCeriviString(tEntity2.Aciklama);
            //    }
            //}
        }
    }
}
