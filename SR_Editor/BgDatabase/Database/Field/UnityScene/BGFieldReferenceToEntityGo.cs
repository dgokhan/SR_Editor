/*
<copyright file="BGFieldReferenceToEntityGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
#if !BG_SA
using Object = UnityEngine.Object;
#endif

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// reference to BGEntityGo (or any inherited from BGEntityGo) component
    /// </summary>
    [FieldDescriptor(Name = "entityReference", Folder = "Unity Scene", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerReferenceToEntityGo", 
        DeprecatedNote="Use objectReference field instead!")]
    public partial class BGFieldReferenceToEntityGo : BGFieldReferenceSingleA<BGEntityGo>
    {
        public const ushort CodeType = 76;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldReferenceToEntityGo(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        protected internal BGFieldReferenceToEntityGo(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        protected override BGId IdProvider(BGEntityGo component) => component.EntityId;

        /// <inheritdoc />
        protected override BGEntityGo GetById(BGId id)
        {
            var candidates = Object.FindObjectsOfType<BGEntityGo>();
            for (var i = 0; i < candidates.Length; i++)
            {
                var candidate = candidates[i];
                if (IdProvider(candidate) == id) return candidate;
            }

            return null;
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldReferenceToEntityGo(meta, id, name);
    }
}