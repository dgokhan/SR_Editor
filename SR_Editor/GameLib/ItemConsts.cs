using System;

namespace GameLib
{
    [Serializable]
    public enum EnumSealBind
    {
        SEAL_BIND_FLAG_DROP,
        SEAL_BIND_FLAG_UPGRADE,
        SEAL_BIND_FLAG_SELL,
        SEAL_BIND_FLAG_ENCHANT,
        SEAL_BIND_FLAG_TRADE,
        SEAL_BIND_FLAG_UNSEAL,
        SEAL_BIND_FLAG_MAX,
    };

    [Serializable]
    public enum EnumSealDate
    {
        SEAL_DATE_DEFAULT_TIMESTAMP = 0, // 2015/11/12 it's -1
        SEAL_DATE_UNLIMITED_TIMESTAMP = -1, // 2015/11/12 it doesn't exist
    };

    [Serializable]
    public enum EnumSealItem
    {
        SEAL_ITEM_BINDING_VNUM = 50263,
        SEAL_ITEM_UNBINDING_VNUM = 50264,
    };

    [Serializable]
    public enum EnumPetData
    {
        EGG_USE_SUCCESS = 0,
        EGG_USE_FAILED_TIMEOVER = 2,
        EGG_USE_FAILED_BECAUSE_NAME = 1,
        GROWTH_PET_ITEM_USE_COOL_TIME = 1,
        PET_EGG_USE_TRUE = 0,
        PET_EGG_USE_FAILED_BECAUSE_TRADING = 1,
        PET_EGG_USE_FAILED_BECAUSE_SHOP_OPEN = 2,
        PET_EGG_USE_FAILED_BECAUSE_MALL_OPEN = 3,
        PET_EGG_USE_FAILED_BECAUSE_SAFEBOX_OPEN = 4,
        PET_HATCHING_MONEY = 100000,
        PET_NAME_MAX_SIZE = 20,
        PET_NAME_MIN_SIZE = 4,
    };

    [Serializable]
    public enum EnumItemType : int
    {
        ITEM_TYPE_NONE, //0
        ITEM_TYPE_WEAPON,
        ITEM_TYPE_ARMOR,
        ITEM_TYPE_USE,
        ITEM_TYPE_AUTOUSE, //4
        ITEM_TYPE_MATERIAL, //5
        ITEM_TYPE_SPECIAL,
        ITEM_TYPE_TOOL, //7
        ITEM_TYPE_LOTTERY,
        ITEM_TYPE_ELK,
        ITEM_TYPE_METIN, //10
        ITEM_TYPE_CONTAINER, //11
        ITEM_TYPE_FISH,
        ITEM_TYPE_ROD, //13
        ITEM_TYPE_RESOURCE, //14
        ITEM_TYPE_CAMPFIRE, //15
        ITEM_TYPE_UNIQUE, //16
        ITEM_TYPE_SKILLBOOK, //17
        ITEM_TYPE_QUEST, //18
        ITEM_TYPE_POLYMORPH, //19
        ITEM_TYPE_TREASURE_BOX,
        ITEM_TYPE_TREASURE_KEY,
        ITEM_TYPE_SKILLFORGET, //22
        ITEM_TYPE_GIFTBOX, //23
        ITEM_TYPE_PICK, //24
        ITEM_TYPE_HAIR,
        ITEM_TYPE_TOTEM,
        ITEM_TYPE_BLEND,
        ITEM_TYPE_COSTUME,
        ITEM_TYPE_DS,
        ITEM_TYPE_SPECIAL_DS,
        ITEM_TYPE_EXTRACT,
        ITEM_TYPE_SECONDARY_COIN,
        ITEM_TYPE_RING,
        ITEM_TYPE_BELT,

        ITEM_TYPE_PET, //35
        ITEM_TYPE_MEDIUM, //36

        ITEM_TYPE_MAX_NUM,
    };

    [Serializable]
    public enum EnumResourceSubTypes : byte
    {
        RESOURCE_FISHBONE = 0,
        RESOURCE_WATERSTONEPIECE = 1,
        RESOURCE_WATERSTONE = 2,
        RESOURCE_BLOOD_PEARL = 3,
        RESOURCE_BLUE_PEARL = 4,
        RESOURCE_WHITE_PEARL = 5,
        RESOURCE_BUCKET = 6,
        RESOURCE_CRYSTAL = 7,
        RESOURCE_GEM = 8,
        RESOURCE_STONE = 9,
        RESOURCE_METIN = 10,
        RESOURCE_ORE = 11,
    };

    [Serializable]
    public enum EnumPetSubTypes : byte
    {
        PET_EGG = 0,
        PET_UPBRINGING = 1,
        PET_BAG = 2,
        PET_FEEDSTUFF = 3,
        PET_SKILL = 4,
        PET_SKILL_DEL_BOOK = 5,
    };

    [Serializable]
    public enum EnumMediumSubTypes : byte
    {
        MEDIUM_MOVE_COSTUME_ATTR = 0,
    };

    [Serializable]
    public enum EnumWeaponSubTypes : byte
    {
        WEAPON_SWORD, //0
        WEAPON_DAGGER,
        WEAPON_BOW, //2
        WEAPON_TWO_HANDED, //3
        WEAPON_BELL, //4
        WEAPON_FAN, //5
        WEAPON_ARROW, //6
        WEAPON_MOUNT_SPEAR, //7
        WEAPON_CLAW = 8, //8
        WEAPON_QUIVER = 9, //9
        WEAPON_BOUQUET = 10, //10
        WEAPON_NUM_TYPES, //11 2015/11/12

        WEAPON_NONE = WEAPON_NUM_TYPES + 1,
    };

    [Serializable]
    public enum EnumMaterialSubTypes : byte
    {
        MATERIAL_LEATHER,
        MATERIAL_BLOOD,
        MATERIAL_ROOT,
        MATERIAL_NEEDLE,
        MATERIAL_JEWEL,
        MATERIAL_DS_REFINE_NORMAL,
        MATERIAL_DS_REFINE_BLESSED,
        MATERIAL_DS_REFINE_HOLLY,
    };

    [Serializable]
    public enum EnumArmorSubTypes : byte
    {
        ARMOR_BODY,
        ARMOR_HEAD,
        ARMOR_SHIELD,
        ARMOR_WRIST,
        ARMOR_FOOTS,
        ARMOR_NECK,
        ARMOR_EAR,
        ARMOR_NUM_TYPES
    };

    [Serializable]
    public enum EnumCostumeSubTypes : byte
    {
        COSTUME_BODY,
        COSTUME_HAIR,
        COSTUME_MOUNT = 2, //2
        COSTUME_ACCE = 3, //3
        COSTUME_WEAPON = 4, //4
        COSTUME_NUM_TYPES,
    };


    [Serializable]
    public enum EnumUniqueSubTypes : byte
    {
        UNIQUE_NONE,
        UNIQUE_BOOK,
        UNIQUE_SPECIAL_RIDE,
        UNIQUE_SPECIAL_MOUNT_RIDE,
    };

    [Serializable]
    public enum EnumUseSubTypes : byte
    {
        USE_POTION, // 0
        USE_TALISMAN,
        USE_TUNING,
        USE_MOVE,
        USE_TREASURE_BOX,
        USE_MONEYBAG,
        USE_BAIT,
        USE_ABILITY_UP,
        USE_AFFECT,
        USE_CREATE_STONE,
        USE_SPECIAL, // 10
        USE_POTION_NODELAY,
        USE_CLEAR,
        USE_INVISIBILITY,
        USE_DETACHMENT,
        USE_BUCKET,
        USE_POTION_CONTINUE,
        USE_CLEAN_SOCKET,
        USE_CHANGE_ATTRIBUTE,
        USE_ADD_ATTRIBUTE,
        USE_ADD_ACCESSORY_SOCKET, // 20
        USE_PUT_INTO_ACCESSORY_SOCKET,
        USE_ADD_ATTRIBUTE2,
        USE_RECIPE,
        USE_CHANGE_ATTRIBUTE2,
        USE_BIND,
        USE_UNBIND,
        USE_TIME_CHARGE_PER,
        USE_TIME_CHARGE_FIX, // 28
        USE_PUT_INTO_BELT_SOCKET,
        USE_PUT_INTO_RING_SOCKET,
        USE_CHANGE_COSTUME_ATTR, // 31
        USE_RESET_COSTUME_ATTR, // 32
    };

    [Serializable]
    public enum EnumDragonSoulSubType : byte
    {
        DS_SLOT1,
        DS_SLOT2,
        DS_SLOT3,
        DS_SLOT4,
        DS_SLOT5,
        DS_SLOT6,
        DS_SLOT_NUM_TYPES = 6,
    };

    [Serializable]
    public enum EnumMetinSubTypes
    {
        METIN_NORMAL,
        METIN_GOLD,
    };

    [Serializable]
    [Flags]
    public enum EnumItemAntiFlag
    {
        ANTI_FEMALE = (1 << 0),
        ANTI_MALE = (1 << 1),
        ANTI_WARRIOR = (1 << 2),
        ANTI_ASSASSIN = (1 << 3),
        ANTI_SURA = (1 << 4),
        ANTI_SHAMAN = (1 << 5),
        ANTI_GET = (1 << 6),
        ANTI_DROP = (1 << 7),
        ANTI_SELL = (1 << 8),
        ANTI_EMPIRE_A = (1 << 9),
        ANTI_EMPIRE_B = (1 << 10),
        ANTI_EMPIRE_R = (1 << 11),
        ANTI_SAVE = (1 << 12),
        ANTI_GIVE = (1 << 13),
        ANTI_PKDROP = (1 << 14),
        ANTI_STACK = (1 << 15),
        ANTI_MYSHOP = (1 << 16),
        ANTI_SAFEBOX = (1 << 17),
        ANTI_WOLFMAN = (1 << 18),
    };

    [Serializable]
    [Flags]
    public enum EnumItemFlag
    {
        ITEM_REFINEABLE = (1 << 0),
        ITEM_SAVE = (1 << 1),
        ITEM_STACKABLE = (1 << 2),
        ITEM_COUNT_PER_1GOLD = (1 << 3),
        ITEM_SLOW_QUERY = (1 << 4),
        ITEM_RARE = (1 << 5),
        ITEM_UNIQUE = (1 << 6),
        ITEM_MAKECOUNT = (1 << 7),
        ITEM_IRREMOVABLE = (1 << 8),
        ITEM_CONFIRM_WHEN_USE = (1 << 9),
        ITEM_QUEST_USE = (1 << 10),
        ITEM_QUEST_USE_MULTIPLE = (1 << 11),
        ITEM_UNUSED03 = (1 << 12), // UNUSED03
        ITEM_LOG = (1 << 13),
        ITEM_APPLICABLE = (1 << 14),
    };

    [Serializable]
    [Flags]
    public enum EnumWearPositions
    {
        WEAR_BODY, // 0
        WEAR_HEAD, // 1
        WEAR_FOOTS, // 2
        WEAR_WRIST, // 3
        WEAR_WEAPON, // 4
        WEAR_NECK, // 5
        WEAR_EAR, // 6
        WEAR_UNIQUE1, // 7
        WEAR_UNIQUE2, // 8
        WEAR_ARROW, // 9
        WEAR_SHIELD, // 10

        WEAR_ABILITY1, // 11
        WEAR_ABILITY2, // 12
        WEAR_ABILITY3, // 13
        WEAR_ABILITY4, // 14
        WEAR_ABILITY5, // 15
        WEAR_ABILITY6, // 16
        WEAR_ABILITY7, // 17
        WEAR_ABILITY8, // 18
        WEAR_COSTUME_BODY, // 19
        WEAR_COSTUME_HAIR, // 20

        WEAR_RING1,
        WEAR_COSTUME_MOUNT = WEAR_RING1, // costume_mount == ring1

        WEAR_RING2,
        WEAR_COSTUME_ACCE = WEAR_RING2, // costume_acce == ring2

        WEAR_BELT,

        WEAR_COSTUME_WEAPON, // 24

        WEAR_MAX_NUM = 32,
    };

    [Serializable]
    [Flags]
    public enum EnumItemWearableFlag
    {
        WEARABLE_BODY = (1 << 0),
        WEARABLE_HEAD = (1 << 1),
        WEARABLE_FOOTS = (1 << 2),
        WEARABLE_WRIST = (1 << 3),
        WEARABLE_WEAPON = (1 << 4),
        WEARABLE_NECK = (1 << 5),
        WEARABLE_EAR = (1 << 6),
        WEARABLE_UNIQUE = (1 << 7),
        WEARABLE_SHIELD = (1 << 8),
        WEARABLE_ARROW = (1 << 9),
    }

    [Serializable]
    public enum EnumApplyTypes : byte
    {
        APPLY_NONE, // 0
        APPLY_MAX_HP, // 1
        APPLY_MAX_SP, // 2
        APPLY_CON, // 3
        APPLY_INT, // 4
        APPLY_STR, // 5
        APPLY_DEX, // 6
        APPLY_ATT_SPEED, // 7
        APPLY_MOV_SPEED, // 8
        APPLY_CAST_SPEED, // 9
        APPLY_HP_REGEN, // 10
        APPLY_SP_REGEN, // 11
        APPLY_POISON_PCT, // 12
        APPLY_STUN_PCT, // 13
        APPLY_SLOW_PCT, // 14
        APPLY_CRITICAL_PCT, // 15
        APPLY_PENETRATE_PCT, // 16
        APPLY_ATTBONUS_HUMAN, // 17
        APPLY_ATTBONUS_ANIMAL, // 18
        APPLY_ATTBONUS_ORC, // 19
        APPLY_ATTBONUS_MILGYO, // 20
        APPLY_ATTBONUS_UNDEAD, // 21
        APPLY_ATTBONUS_DEVIL, // 22
        APPLY_STEAL_HP, // 23
        APPLY_STEAL_SP, // 24
        APPLY_MANA_BURN_PCT, // 25
        APPLY_DAMAGE_SP_RECOVER, // 26
        APPLY_BLOCK, // 27
        APPLY_DODGE, // 28
        APPLY_RESIST_SWORD, // 29
        APPLY_RESIST_TWOHAND, // 30
        APPLY_RESIST_DAGGER, // 31
        APPLY_RESIST_BELL, // 32
        APPLY_RESIST_FAN, // 33
        APPLY_RESIST_BOW, // 34
        APPLY_RESIST_FIRE, // 35
        APPLY_RESIST_ELEC, // 36
        APPLY_RESIST_MAGIC, // 37
        APPLY_RESIST_WIND, // 38
        APPLY_REFLECT_MELEE, // 39
        APPLY_REFLECT_CURSE, // 40
        APPLY_POISON_REDUCE, // 41
        APPLY_KILL_SP_RECOVER, // 42
        APPLY_EXP_DOUBLE_BONUS, // 43
        APPLY_GOLD_DOUBLE_BONUS, // 44
        APPLY_ITEM_DROP_BONUS, // 45
        APPLY_POTION_BONUS, // 46
        APPLY_KILL_HP_RECOVER, // 47
        APPLY_IMMUNE_STUN, // 48
        APPLY_IMMUNE_SLOW, // 49
        APPLY_IMMUNE_FALL, // 50
        APPLY_SKILL, // 51
        APPLY_BOW_DISTANCE, // 52
        APPLY_ATT_GRADE_BONUS, // 53
        APPLY_DEF_GRADE_BONUS, // 54
        APPLY_MAGIC_ATT_GRADE, // 55
        APPLY_MAGIC_DEF_GRADE, // 56
        APPLY_CURSE_PCT, // 57
        APPLY_MAX_STAMINA, // 58
        APPLY_ATT_BONUS_TO_WARRIOR, // 59
        APPLY_ATT_BONUS_TO_ASSASSIN, // 60
        APPLY_ATT_BONUS_TO_SURA, // 61
        APPLY_ATT_BONUS_TO_SHAMAN, // 62
        APPLY_ATT_BONUS_TO_MONSTER, // 63
        APPLY_MALL_ATTBONUS,
        APPLY_MALL_DEFBONUS,
        APPLY_MALL_EXPBONUS,
        APPLY_MALL_ITEMBONUS,
        APPLY_MALL_GOLDBONUS,
        APPLY_MAX_HP_PCT,
        APPLY_MAX_SP_PCT,
        APPLY_SKILL_DAMAGE_BONUS,
        APPLY_NORMAL_HIT_DAMAGE_BONUS,
        APPLY_SKILL_DEFEND_BONUS,
        APPLY_NORMAL_HIT_DEFEND_BONUS,
        APPLY_EXTRACT_HP_PCT, //75
        APPLY_PC_BANG_EXP_BONUS, //76
        APPLY_PC_BANG_DROP_BONUS, //77
        APPLY_RESIST_WARRIOR, //78
        APPLY_RESIST_ASSASSIN, //79
        APPLY_RESIST_SURA, //80
        APPLY_RESIST_SHAMAN, //81
        APPLY_ENERGY, //82
        APPLY_DEF_GRADE,
        APPLY_COSTUME_ATTR_BONUS,
        APPLY_MAGIC_ATTBONUS_PER,
        APPLY_MELEE_MAGIC_ATTBONUS_PER,

        APPLY_RESIST_ICE,
        APPLY_RESIST_EARTH,
        APPLY_RESIST_DARK,

        APPLY_ANTI_CRITICAL_PCT,
        APPLY_ANTI_PENETRATE_PCT,

        APPLY_BLEEDING_REDUCE = 92, //92
        APPLY_BLEEDING_PCT = 93, //93
        APPLY_ATT_BONUS_TO_WOLFMAN = 94, //94
        APPLY_RESIST_WOLFMAN = 95, //95
        APPLY_RESIST_CLAW = 96, //96

        APPLY_ACCEDRAIN_RATE = 97, //97

        APPLY_RESIST_MAGIC_REDUCTION = 98, //98

        APPLY_MOUNT = 99,

        MAX_APPLY_NUM = 100,
    }

    [Serializable]
    [Flags]
    public enum EnumImmuneFlags
    {
        IMMUNE_PARA = (1 << 0),
        IMMUNE_CURSE = (1 << 1),
        IMMUNE_STUN = (1 << 2),
        IMMUNE_SLEEP = (1 << 3),
        IMMUNE_SLOW = (1 << 4),
        IMMUNE_POISON = (1 << 5),
        IMMUNE_TERROR = (1 << 6),
    }

    [Serializable]
    public enum EnumLimitTypes : byte
    {
        LIMIT_NONE,

        LIMIT_LEVEL,
        LIMIT_STR,
        LIMIT_DEX,
        LIMIT_INT,
        LIMIT_CON,
        LIMIT_PCBANG,

        LIMIT_REAL_TIME,

        LIMIT_REAL_TIME_START_FIRST_USE,

        LIMIT_TIMER_BASED_ON_WEAR,

        LIMIT_MAX_NUM
    };

}