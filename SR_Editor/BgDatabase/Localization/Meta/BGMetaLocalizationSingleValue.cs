/*
<copyright file="BGMetaLocalizationSingleValue.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    [MetaDescriptor(Name = "localizationSingleValue", ManagerType = "BansheeGz.BGDatabase.Editor.BGMetaManagerLocalizationSingleValue")]
    public partial class BGMetaLocalizationSingleValue : BGMetaLocalizationA
    {
        public const ushort CodeType = 5;

        public BGMetaLocalizationSingleValue(BGRepo repo, string name, Type fieldType) : base(repo, name, fieldType)
        {
        }

        internal BGMetaLocalizationSingleValue(BGRepo repo, BGId id, string name) : base(repo, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        protected override Func<BGRepo, BGId, string, BGMetaEntity> CreateMetaFactory()
        {
            return (repo, id, name) => new BGMetaLocalizationSingleValue(repo, id, name);
        }

        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Methods
        //================================================================================================
        //This is used by reflection (BGLocalizationEditorUglyHacks.IsAssignableFrom) !!
        //this method does not work if no locales present
        private Type ValueType
        {
            get
            {
                var field = FindField(f => f is BGFieldLocaleI);
                return field?.ValueType;
            }
        }
    }
}