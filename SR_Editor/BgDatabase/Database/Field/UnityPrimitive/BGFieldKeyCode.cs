/*
<copyright file="BGFieldKeyCode.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// KeyCode Field 
    /// </summary>
    [FieldDescriptor(Name = "keyCode", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerKeyCode")]
    public partial class BGFieldKeyCode : BGFieldCachedEnumA<KeyCode>
    {
        public const ushort CodeType = 63;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public BGFieldKeyCode(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldKeyCode(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldKeyCode(meta, id, name);
    }
}