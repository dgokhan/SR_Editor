/*
<copyright file="BGRepoConstants.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// PROBABLY NOT USED??
    /// </summary>
    public partial class BGRepoConstants
    {
        public partial class MetaConstants
        {
            public const string SheetName = "_meta";
            public const string FieldId = "_id";
            public const string FieldName = "_name";
            public const string FieldType = "_type";
            public const string FieldSystem = "_system";
            public const string FieldAddon = "_addon";
            public const string FieldUniqueName = "_uniqueName";
            public const string FieldEmptyName = "_emptyName";
            public const string FieldSingleton = "_singleton";
            public const string FieldComment = "_comment";
            public const string FieldConfig = "_config";

            public static string[] Fields
            {
                get
                {
                    return new[]
                    {
                        FieldId,
                        FieldName,
                        FieldType,
                        FieldSystem,
                        FieldAddon,
                        FieldUniqueName,
                        FieldEmptyName,
                        FieldSingleton,
                        FieldConfig
                    };
                }
            }
        }

        public partial class FieldConstants
        {
            public const string SheetName = "_field";
            public const string FieldId = "_id";
            public const string FieldName = "_name";
            public const string FieldMetaId = "_metaId";
            public const string FieldType = "_type";
            public const string FieldSystem = "_system";
            public const string FieldAddon = "_addon";
            public const string FieldDefaultValue = "_defaultValue";
            public const string FieldRequired = "_required";
            public const string FieldConfig = "_config";

            public static string[] Fields
            {
                get
                {
                    return new[]
                    {
                        FieldId,
                        FieldName,
                        FieldType,
                        FieldSystem,
                        FieldAddon,
                        FieldMetaId,
                        FieldConfig,
                        FieldDefaultValue,
                        FieldRequired
                    };
                }
            }
        }

        public partial class AddonConstants
        {
            public const string SheetName = "_addon";
            public const string FieldType = "_type";
            public const string FieldConfig = "_config";

            public static string[] Fields
            {
                get
                {
                    return new[]
                    {
                        //0
                        FieldType,
                        //1
                        FieldConfig
                    };
                }
            }
        }
    }
}