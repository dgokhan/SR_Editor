/*
<copyright file="BGCalcTypeCodeVector3.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// type code for Vector3 type
    /// </summary>
    public class BGCalcTypeCodeVector3 : BGCalcTypeCode<Vector3>
    {
        public const byte Code = 22;

        /// <inheritdoc />
        public override bool SupportDefaultValue => true;
        
        /// <inheritdoc />
        public override byte TypeCode => Code;
        
        /// <inheritdoc />
        public override object DefaultValue => Vector3.zero;
        
        /// <inheritdoc />
        public override string Name => "Vector3";

        /// <inheritdoc />
        public override void ValueToBytes(BGBinaryWriter writer, object value)
        {
            var vector = (Vector3)value;
            writer.AddFloat(vector.x);
            writer.AddFloat(vector.y);
            writer.AddFloat(vector.z);
        }

        /// <inheritdoc />
        public override object ValueFromBytes(BGBinaryReader reader) => new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());

        /// <inheritdoc />
        public override string ValueToString(object value) => BGFieldVector3.ValueToString((Vector3)value);

        /// <inheritdoc />
        public override object ValueFromString(string value) => BGFieldVector3.ValueFromString(value);

        /// <inheritdoc />
        public override bool CanBeConvertedFrom(BGCalcTypeCode otherCode)
        {
            if (otherCode == null) return false;
            switch (otherCode.TypeCode)
            {
                case BGCalcTypeCodeVector2.Code:
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
                case BGCalcTypeCodeVector2.Code:
                {
                    return (Vector3)(Vector2)value;
                }
                case BGCalcTypeCodeVector4.Code:
                {
                    return (Vector3)(Vector4)value;
                }
            }

            return value;
        }
    }
}