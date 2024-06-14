/*
<copyright file="BGFieldLocalizedText.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedText", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(TargetFieldType = typeof(BGFieldLocaleText))]
    public partial class BGFieldLocalizedText : BGFieldLocalizedA<string>
    {
        public const ushort CodeType = 94;
        public override ushort TypeCode => CodeType;

        //for new
        public BGFieldLocalizedText(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        //for existing
        internal BGFieldLocalizedText(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedText(meta, id, name);
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