using SR_Editor.LookUp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.LookUp
{
    public class LookUpJobTipi : LookUpEntityBase<byte>
    {
        public LookUpJobTipi()
        {
        }

        public LookUpJobTipi(byte pKey)
          : base(pKey)
        {
        }

        public LookUpJobTipi(byte pKey, string pValue)
          : base(pKey, pValue)
        {
        }
    }
    public class JobTipi : LookUpEntityBaseList<byte, LookUpJobTipi>
    {
        private static JobTipi _liste;

        public static JobTipi Liste
        {
            get
            {
                if (_liste == null)
                {
                    _liste = new JobTipi();
                    _liste.LoadAll();
                }
                return _liste;
            }
        }
        public JobTipi()
        {
            this.Load();
        }

        public void LoadAll()
        {
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_WARRIOR_M, "Warrior Male"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_ASSASSIN_W, "Assassin Female"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_SURA_M, "Sura Male"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_SHAMAN_W, "Shaman Female"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_WARRIOR_W, "Warrior Female"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_ASSASSIN_M, "Assassin Male"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_SURA_W, "Sura Female"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_SHAMAN_M, "Shaman Male"));
            this.Add(new LookUpJobTipi((byte)EnumRace.MAIN_RACE_WOLFMAN_M, "Wolfman"));

            this.Load();
        }
    }

    public enum EnumRace : byte
    {
        MAIN_RACE_WARRIOR_M,
        MAIN_RACE_ASSASSIN_W,
        MAIN_RACE_SURA_M,
        MAIN_RACE_SHAMAN_W,
        MAIN_RACE_WARRIOR_W,
        MAIN_RACE_ASSASSIN_M,
        MAIN_RACE_SURA_W,
        MAIN_RACE_SHAMAN_M,
        MAIN_RACE_WOLFMAN_M,
    }

    public enum EnumJob : int
    {
        JOB_WARRIOR,
        JOB_ASSASSIN,
        JOB_SURA,
        JOB_SHAMAN,
        JOB_WOLFMAN,
        JOB_MAX_NUM
    };

    public static class JobExtensions
    {

        public static EnumJob RaceToJob(this EnumRace race)
        {
            if (race == (EnumRace)8)
                return EnumJob.JOB_WOLFMAN;
            const int JOB_NUM = 4;
            return (EnumJob)((int)race % JOB_NUM);
        }
    }

}
