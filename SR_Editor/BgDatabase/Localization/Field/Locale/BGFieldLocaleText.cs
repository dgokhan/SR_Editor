/*
<copyright file="BGFieldLocaleText.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleText")]
    [BGLocaleField(Name = "text", DelegateFieldType = typeof(BGFieldText))]
    public partial class BGFieldLocaleText : BGFieldLocaleStringA
    {
        public const ushort CodeType = 86;
        public override ushort TypeCode => CodeType;

        public BGFieldLocaleText(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocaleText(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocaleText(meta, id, name);
        }
    }
}