/*
<copyright file="BGMTFieldFactory.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Factory for multi-threaded fields
    /// </summary>
    public static class BGMTFieldFactory
    {
        private static readonly Dictionary<Type, Func<BGField, BGMTField>> Type2Provider = new Dictionary<Type, Func<BGField, BGMTField>>();

        static BGMTFieldFactory()
        {
            // primitives
            Type2Provider.Add(typeof(BGFieldBool), field => new BGMTFieldCached<bool>(field));
            Type2Provider.Add(typeof(BGFieldDouble), field => new BGMTFieldCached<double>(field));
            Type2Provider.Add(typeof(BGFieldDecimal), field => new BGMTFieldCached<decimal>(field));
            Type2Provider.Add(typeof(BGFieldFloat), field => new BGMTFieldCached<float>(field));
            Type2Provider.Add(typeof(BGFieldGuid), field => new BGMTFieldCached<Guid>(field));
            Type2Provider.Add(typeof(BGFieldInt), field => new BGMTFieldCached<int>(field));
            Type2Provider.Add(typeof(BGFieldLong), field => new BGMTFieldCached<long>(field));
            Type2Provider.Add(typeof(BGFieldString), field => new BGMTFieldCached<string>(field));
            Type2Provider.Add(typeof(BGFieldText), field => new BGMTFieldCached<string>(field));

            // primitives nullable
            Type2Provider.Add(typeof(BGFieldBoolNullable), field => new BGMTFieldCached<bool?>(field));
            Type2Provider.Add(typeof(BGFieldDoubleNullable), field => new BGMTFieldCached<double?>(field));
            Type2Provider.Add(typeof(BGFieldFloatNullable), field => new BGMTFieldCached<float?>(field));
            Type2Provider.Add(typeof(BGFieldGuidNullable), field => new BGMTFieldCached<Guid?>(field));
            Type2Provider.Add(typeof(BGFieldIntNullable), field => new BGMTFieldCached<int?>(field));
            Type2Provider.Add(typeof(BGFieldLongNullable), field => new BGMTFieldCached<long?>(field));

            //special
            Type2Provider.Add(typeof(BGFieldEntityName), field => new BGMTFieldCached<string>(field));
            Type2Provider.Add(typeof(BGFieldId), field => new BGMTFieldCached<BGId>(field));

            //Unity primitives
            Type2Provider.Add(typeof(BGFieldBounds), field => new BGMTFieldCached<Bounds>(field));
            Type2Provider.Add(typeof(BGFieldColor), field => new BGMTFieldCached<Color>(field));
            Type2Provider.Add(typeof(BGFieldKeyCode), field => new BGMTFieldCached<KeyCode>(field));
            Type2Provider.Add(typeof(BGFieldQuaternion), field => new BGMTFieldCached<Quaternion>(field));
            Type2Provider.Add(typeof(BGFieldRay), field => new BGMTFieldCached<Ray>(field));
            Type2Provider.Add(typeof(BGFieldRay2d), field => new BGMTFieldCached<Ray2D>(field));
            Type2Provider.Add(typeof(BGFieldRect), field => new BGMTFieldCached<Rect>(field));
            Type2Provider.Add(typeof(BGFieldVector2), field => new BGMTFieldCached<Vector2>(field));
            Type2Provider.Add(typeof(BGFieldVector3), field => new BGMTFieldCached<Vector3>(field));
            Type2Provider.Add(typeof(BGFieldVector4), field => new BGMTFieldCached<Vector4>(field));

            //Unity primitives nullables
            Type2Provider.Add(typeof(BGFieldColorNullable), field => new BGMTFieldCached<Color?>(field));
            Type2Provider.Add(typeof(BGFieldQuaternionNullable), field => new BGMTFieldCached<Quaternion?>(field));
            Type2Provider.Add(typeof(BGFieldVector2Nullable), field => new BGMTFieldCached<Vector2?>(field));
            Type2Provider.Add(typeof(BGFieldVector3Nullable), field => new BGMTFieldCached<Vector3?>(field));
            Type2Provider.Add(typeof(BGFieldVector4Nullable), field => new BGMTFieldCached<Vector4?>(field));

            //Relations
            Type2Provider.Add(typeof(BGFieldRelationSingle), field => new BGMTFieldRelationSingle(field));
            Type2Provider.Add(typeof(BGFieldRelationMultiple), field => new BGMTFieldRelationMultiple(field));
            Type2Provider.Add(typeof(BGFieldManyRelationsSingle), field => new BGMTFieldManyTablesRelationSingle(field));
            Type2Provider.Add(typeof(BGFieldManyRelationsMultiple), field => new BGMTFieldManyTablesRelationMultiple(field));
            Type2Provider.Add(typeof(BGFieldNested), field => new BGMTFieldNested(field));

            //enums
            Type2Provider.Add(typeof(BGFieldEnum), field => new BGMTFieldEnum(field));
            Type2Provider.Add(typeof(BGFieldEnumShort), field => new BGMTFieldEnumShort(field));
            Type2Provider.Add(typeof(BGFieldEnumByte), field => new BGMTFieldEnumByte(field));
        }

        public static bool IsSupported(Type fieldType)
        {
            return Type2Provider.ContainsKey(fieldType);
        }

        public static List<Type> GetAllFieldTypes()
        {
            var result = new List<Type>();
            foreach (var pair in Type2Provider) result.Add(pair.Key);
            return result;
        }

        public static BGMTField Create(BGMTMeta meta, BGField field)
        {
            if (!Type2Provider.TryGetValue(field.GetType(), out var provider)) return null;
            var result = provider(field);
            result.Meta = meta;
            return result;
        }
    }
}