using System;

namespace GameLib
{
    [Serializable]
    [Flags]
    public enum EnumMobResist : byte
    {
        MOB_RESIST_SWORD,
        MOB_RESIST_TWOHAND,
        MOB_RESIST_DAGGER,
        MOB_RESIST_BELL,
        MOB_RESIST_FAN,
        MOB_RESIST_BOW,
        MOB_RESIST_FIRE,
        MOB_RESIST_ELECT,
        MOB_RESIST_MAGIC,
        MOB_RESIST_WIND,
        MOB_RESIST_POISON,
        MOB_RESISTS_MAX_NUM
    }

    [Serializable]
    [Flags]
    public enum EnumMobEnchant : byte
    {
        MOB_ENCHANT_CURSE,
        MOB_ENCHANT_SLOW,
        MOB_ENCHANT_POISON,
        MOB_ENCHANT_STUN,
        MOB_ENCHANT_CRITICAL,
        MOB_ENCHANT_PENETRATE,
        MOB_ENCHANTS_MAX_NUM
    }

    [Serializable]
    [Flags]
    public enum EnumMobAiFlag : short
    {
        AIFLAG_AGGRESSIVE = (1 << 0),
        AIFLAG_NOMOVE = (1 << 1),
        AIFLAG_COWARD = (1 << 2),
        AIFLAG_NOATTACKSHINSU = (1 << 3),
        AIFLAG_NOATTACKJINNO = (1 << 4),
        AIFLAG_NOATTACKCHUNJO = (1 << 5),
        AIFLAG_ATTACKMOB = (1 << 6),
        AIFLAG_BERSERK = (1 << 7),
        AIFLAG_STONESKIN = (1 << 8),
        AIFLAG_GODSPEED = (1 << 9),
        AIFLAG_DEATHBLOW = (1 << 10),
        AIFLAG_REVIVE = (1 << 11),
        AIFLAG_UNK13 = (1 << 12),
        AIFLAG_UNK14 = (1 << 13),
    }


    [Serializable]
    [Flags]
    public enum EnumMobImmuneFlag : short
    {
        IMMUNE_STUN = (1 << 0),
        IMMUNE_SLOW = (1 << 1),
        IMMUNE_FALL = (1 << 2),
        IMMUNE_CURSE = (1 << 3),
        IMMUNE_POISON = (1 << 4),
        IMMUNE_TERROR = (1 << 5),
        IMMUNE_REFLECT = (1 << 6),
    }

    [Serializable]
    [Flags]
    public enum EnumMobRaceFlag : int
    {
        RACEFLAG_ANIMAL = (1 << 0),
        RACEFLAG_UNDEAD = (1 << 1),
        RACEFLAG_DEVIL = (1 << 2),
        RACEFLAG_HUMAN = (1 << 3),
        RACEFLAG_ORC = (1 << 4),
        RACEFLAG_MILGYO = (1 << 5),
        RACEFLAG_INSECT = (1 << 6),
        RACEFLAG_FIRE = (1 << 7),
        RACEFLAG_ICE = (1 << 8),
        RACEFLAG_DESERT = (1 << 9),
        RACEFLAG_TREE = (1 << 10),
        RACEFLAG_ATT_ELEC = (1 << 11),
        RACEFLAG_ATT_FIRE = (1 << 12),
        RACEFLAG_ATT_ICE = (1 << 13),
        RACEFLAG_ATT_WIND = (1 << 14),
        RACEFLAG_ATT_EARTH = (1 << 15),
        RACEFLAG_ATT_DARK = (1 << 16),
    }

    [Serializable]
    [Flags]
    public enum EnumMobClickEvent : byte
    {
        CLICK_EVENT_NONE = 0,
        CLICK_EVENT_BATTLE = 1,
        CLICK_EVENT_SHOP = 2,
        CLICK_EVENT_TALK = 3,
        CLICK_EVENT_VEHICLE = 4,

        CLICK_EVENT_MAX_NUM,
    }
}