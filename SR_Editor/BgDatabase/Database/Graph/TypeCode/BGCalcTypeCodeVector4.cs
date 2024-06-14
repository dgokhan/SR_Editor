/*
<copyright file="BGCalcTypeCodeVector4.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for Vector4 type
    /// </summary>
    public class BGCalcTypeCodeVector4 : BGCalcTypeCode<Vector4>
    {
        public const byte Code = 23;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => Vector4.zero;
        
        /// <inheritdoc />
        public override string Name => "Vector4";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value)
        {
            var vector = (Vector4)value;
            writer.AddFloat(vector.x);
            writer.AddFloat(vector.y);
            writer.AddFloat(vector.z);
            writer.AddFloat(vector.w);
        }

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => new Vector4(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());

        /// <inheritdoc />
        public override string ValueToString(object value) => BGFieldVector4.ValueToString((Vector4)value);

        /// <inheritdoc />
        public override object ValueFromString(string value) => BGFieldVector4.ValueFromString(value);

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeVector2.Code:
                case BGCalcTypeCodeVector3.Code:
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
                case BGCalcTypeCodeVector2.Code:
                {
                    return (Vector4)(Vector2)value;
                }
                case BGCalcTypeCodeVector3.Code:
                {
                    return (Vector4)(Vector3)value;
                }
            }

            return value;
        }
    }
}