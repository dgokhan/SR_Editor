/*
<copyright file="BGFieldLocaleString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Text;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocaleString")]
    [BGLocaleField(Name = "string", DelegateFieldType = typeof(BGFieldString))]
    public partial class BGFieldLocaleString : BGFieldLocaleStringA
    {
        public const ushort CodeType = 85;
        public override ushort TypeCode => CodeType;

        public BGFieldLocaleString(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldLocaleString(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocaleString(meta, id, name);
        }
    }
}