using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR_Editor.LookUp.Base;

namespace SR_Editor.LookUp
{
    public class LookUpAffectTipi : LookUpEntityBase<int>
    {
        public LookUpAffectTipi()
        {
        }

        public LookUpAffectTipi(int pKey)
            : base(pKey)
        {
        }

        public LookUpAffectTipi(int pKey, string pValue)
            : base(pKey, pValue)
        {
        }
    }
    public class AffectTipi : LookUpEntityBaseList<int, LookUpAffectTipi>
    {
        private static AffectTipi _liste;

        public static AffectTipi Liste
        {
            get
            {
                if (_liste == null)
                {
                    _liste = new AffectTipi();
                    _liste.LoadAll();
                }
                return _liste;
            }
        }
        public AffectTipi()
        {
            this.Load();
        }

        public void LoadAll()
        {
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_NONE, "NONE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_MOV_SPEED, "MOV SPEED"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_ATT_SPEED, "ATT SPEED"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_ATT_GRADE, "ATT GRADE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_INVISIBILITY, "INVISIBILITY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_STR, "STR"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_DEX, "DEX"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CON, "CON"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_INT, "INT"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_FISH_MIND_PILL, "FISH MIND PILL"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_POISON, "POISON"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_STUN, "STUN"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_SLOW, "SLOW"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_DUNGEON_READY, "DUNGEON READY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_DUNGEON_UNIQUE, "DUNGEON UNIQUE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_BUILDING, "BUILDING"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_REVIVE_INVISIBLE, "REVIVE INVISIBLE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_FIRE, "FIRE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CAST_SPEED, "CAST SPEED"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_HP_RECOVER_CONTINUE, "HP RECOVER CONTINUE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_SP_RECOVER_CONTINUE, "SP RECOVER CONTINUE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_POLYMORPH, "POLYMORPH"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_MOUNT, "MOUNT"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_WAR_FLAG, "WAR FLAG"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_BLOCK_CHAT, "BLOCK CHAT"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CHINA_FIREWORK, "CHINA FIREWORK"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_BOW_DISTANCE, "BOW DISTANCE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_DEF_GRADE, "DEF GRADE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_BLEEDING, "BLEEDING"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_RAMADAN_ABILITY, "RAMADAN ABILITY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_RAMADAN_RING, "RAMADAN RING"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_NOG_ABILITY, "NOG ABILITY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_HOLLY_STONE_POWER, "HOLLY STONE POWER"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_PREMIUM_START, "PREMIUM START"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_ITEM_BONUS, "ITEM BONUS"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_SAFEBOX, "SAFEBOX"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_AUTOLOOT, "AUTOLOOT"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_FISH_MIND, "FISH MIND"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_MARRIAGE_FAST, "MARRIAGE FAST"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_GOLD_BONUS, "GOLD BONUS"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_PREMIUM_END, "PREMIUM END"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_MALL, "MALL"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_NO_DEATH_PENALTY, "NO DEATH PENALTY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_SKILL_BOOK_BONUS, "SKILL BOOK BONUS"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_SKILL_NO_BOOK_DELAY, "SKILL NO BOOK DELAY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_HAIR, "HAIR"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_COLLECT, "COLLECT"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_EXP_BONUS_EURO_FREE, "EXP BONUS EURO FREE"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_EXP_BONUS_EURO_FREE_UNDER_15, "EXP BONUS EURO FREE UNDER 15"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_UNIQUE_ABILITY, "UNIQUE ABILITY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_1, "CUBE 1"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_2, "CUBE 2"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_3, "CUBE 3"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_4, "CUBE 4"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_5, "CUBE 5"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_6, "CUBE 6"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_7, "CUBE 7"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_8, "CUBE 8"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_9, "CUBE 9"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_10, "CUBE 10"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_11, "CUBE 11"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_CUBE_12, "CUBE 12"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_BLEND, "BLEND"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_HORSE_NAME, "HORSE NAME"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_MOUNT_BONUS, "MOUNT BONUS"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_AUTO_HP_RECOVERY, "AUTO HP RECOVERY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_AUTO_SP_RECOVERY, "AUTO SP RECOVERY"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_DRAGON_SOUL_QUALIFIED, "DRAGON SOUL QUALIFIED"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_DRAGON_SOUL_DECK_0, "DRAGON SOUL DECK 0"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_DRAGON_SOUL_DECK_1, "DRAGON SOUL DECK 1"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_PROTECT, "PROTECT"));
            this.Add(new LookUpAffectTipi((int)EnumAffect.AFFECT_QUEST_START_IDX, "QUEST START IDX"));

            this.Load();
        }
    }

}
