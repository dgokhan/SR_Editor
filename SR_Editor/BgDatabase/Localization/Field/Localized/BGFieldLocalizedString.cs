/*
<copyright file="BGFieldLocalizedString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedString", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(Name = "string", TargetFieldType = typeof(BGFieldLocaleString))]
    public partial class BGFieldLocalizedString : BGFieldLocalizedA<string>
    {
        public const ushort CodeType = 93;
        public override ushort TypeCode => CodeType;

        //for new
        public BGFieldLocalizedString(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        //for existing
        internal BGFieldLocalizedString(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedString(meta, id, name);
        }

        //================================================================================================
        //                                              Custom search value
        //================================================================================================
        protected override string ValueToSearchString(string val, int entityIndex)
        {
            return val;
        }
    }
}