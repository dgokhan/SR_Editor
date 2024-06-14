/*
<copyright file="BGFieldReferenceToUnityObjectList.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// reference to BGWithId components. Single ID value is used to reference multiple components
    /// </summary>
    [FieldDescriptor(Name = "objectListReference", Folder = "Unity Scene", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerReferenceToUnityObjectList")]
    public partial class BGFieldReferenceToUnityObjectList : BGFieldReferenceA<List<BGWithId>>
    {
        public const ushort CodeType = 79;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldReferenceToUnityObjectList(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldReferenceToUnityObjectList(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        public override bool ReadOnly => true;

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override List<BGWithId> this[int entityIndex]
        {
            get
            {
                var value = GetStoredValue(entityIndex);
                if (value == BGId.Empty) return null;

                var result = BGWithId.GetAll(value);
                /*
                var candidates = UnityEngine.Object.FindObjectsOfType<BGWithId>();
                for (var i = 0; i < candidates.Length; i++)
                {
                    var candidate = candidates[i];
                    if (candidate.Id != value) continue;

                    result = result ?? new List<BGWithId>();
                    result.Add(candidate);
                }
                */

                return result;
            }
            set { }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldReferenceToUnityObjectList(meta, id, name);
    }
}