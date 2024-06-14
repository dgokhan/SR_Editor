/*
<copyright file="BGCalcTypeCodeVector2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for Vector2 type
    /// </summary>
    public class BGCalcTypeCodeVector2 : BGCalcTypeCode<Vector2>
    {
        public const byte Code = 21;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => Vector2.zero;
        
        /// <inheritdoc />
        public override string Name => "Vector2";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value)
        {
            var vector = (Vector2)value;
            writer.AddFloat(vector.x);
            writer.AddFloat(vector.y);
        }

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => new Vector2(reader.ReadFloat(), reader.ReadFloat());

        /// <inheritdoc />
        public override string ValueToString(object value) => BGFieldVector2.ValueToString((Vector2)value);

        /// <inheritdoc />
        public override object ValueFromString(string value) => BGFieldVector2.ValueFromString(value);

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeVector3.Code:
                case BGCalcTypeCodeVector4.Code:
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public override object ConvertFrom(BGCalcTypeCode otherCode, object value)
        {
            if (otherCode == null) return value;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeVector3.Code:
                {
                    return (Vector2)(Vector3)value;
                }
                case BGCalcTypeCodeVector4.Code:
                {
                    return (Vector2)(Vector4)value;
                }
            }

            return value;
        }
    }
}