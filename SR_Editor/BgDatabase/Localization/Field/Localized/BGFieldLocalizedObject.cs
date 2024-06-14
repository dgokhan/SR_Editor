/*
<copyright file="BGFieldLocalizedObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    [FieldDescriptor(Name = "localizedObject", Folder = "Localization", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerLocalized")]
    [BGLocalizedField(Name = "unityObject", TargetFieldType = typeof(BGFieldLocaleObject))]
    public partial class BGFieldLocalizedObject : BGFieldLocalizedAssetA<Object>
    {
        public const ushort CodeType = 90;
        public override ushort TypeCode => CodeType;

        public BGFieldLocalizedObject(BGMetaEntity meta, string name, BGMetaLocalization to) : base(meta, name, to)
        {
        }

        internal BGFieldLocalizedObject(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory()
        {
            return (meta, id, name) => new BGFieldLocalizedObject(meta, id, name);
        }
    }
}