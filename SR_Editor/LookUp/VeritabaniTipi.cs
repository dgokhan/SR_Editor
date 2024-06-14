using SR_Editor.LookUp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoyaleSupport;

namespace SR_Editor.LookUp
{
    public class LookUpVeritabaniTipi : LookUpEntityBase<EnumDatabase>
    {
        public LookUpVeritabaniTipi()
        {
        }

        public LookUpVeritabaniTipi(EnumDatabase pKey)
          : base(pKey)
        {
        }

        public LookUpVeritabaniTipi(EnumDatabase pKey, string pValue)
          : base(pKey, pValue)
        {
        }
    }
    public class VeritabaniTipi : LookUpEntityBaseList<EnumDatabase, LookUpVeritabaniTipi>
    {
        private static VeritabaniTipi _liste;

        public static VeritabaniTipi Liste
        {
            get
            {
                if (VeritabaniTipi._liste == null)
                {
                    VeritabaniTipi._liste = new VeritabaniTipi();
                    VeritabaniTipi._liste.LoadAll();
                }
                return VeritabaniTipi._liste;
            }
        }
        private static VeritabaniTipi _listeShared;

        public static VeritabaniTipi ListeShared
        {
            get
            {
                if (VeritabaniTipi._listeShared == null)
                {
                    VeritabaniTipi._listeShared = new VeritabaniTipi();
                    VeritabaniTipi._listeShared.LoadShared();
                }
                return VeritabaniTipi._listeShared;
            }
        }
        private static VeritabaniTipi _listeGame;

        public static VeritabaniTipi ListeGame
        {
            get
            {
                if (VeritabaniTipi._listeGame == null)
                {
                    VeritabaniTipi._listeGame = new VeritabaniTipi();
                    VeritabaniTipi._listeGame.LoadGame();
                }
                return VeritabaniTipi._listeGame;
            }
        }

        public VeritabaniTipi()
        {
            this.Load();
        }

        public void LoadAll()
        {
            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Shard, "Game"));
            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Log, "Game Log"));

            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Billing, "Billing"));
            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Account, "Account"));
            this.Load();
        }

        public void LoadShared()
        {
            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Billing, "Billing"));
            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Account, "Account"));
            this.Load();
        }

        public void LoadGame()
        {
            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Shard, "Game"));
            this.Add(new LookUpVeritabaniTipi(EnumDatabase.Log, "Game Log"));
            this.Load();
        }
    }
}
