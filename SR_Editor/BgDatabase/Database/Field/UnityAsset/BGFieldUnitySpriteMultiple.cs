/*
<copyright file="BGFieldUnitySpriteMultiple.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// sprite array field
    /// </summary>
    [FieldDescriptor(Name = "unitySpriteMultiple", Folder = "Unity Asset", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerUnitySpriteMultiple")]
    public class BGFieldUnitySpriteMultiple : BGFieldUnityAssetArrayA<Sprite>
    {
        public const ushort CodeType = 56;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public BGFieldUnitySpriteMultiple(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldUnitySpriteMultiple(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldUnitySpriteMultiple(meta, id, name);
    }
}