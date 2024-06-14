/*
<copyright file="BGId.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Id is 16 bytes, based on Guid generation, used to uniquely identify some object 
    /// </summary>
    public partial struct BGId : IEquatable<BGId>
    {
        public static readonly BGId Empty = new BGId();

        private readonly ulong key1;
        private readonly ulong key2;

        /// <summary>
        /// Generate new id , based on Guid
        /// </summary>
        public static BGId NewId => new BGId(Guid.NewGuid().ToByteArray());

        /// <summary>
        /// Parse string value. If value is not valid returns BGId.Empty
        /// </summary>
        public static BGId Parse(string value)
        {
            if (value == null || value.Length != 22) return Empty;
            try
            {
                return new BGId(value);
            }
            catch
            {
                return Empty;
            }
        }

        /// <summary>
        /// Try to parse string value. return success
        /// </summary>
        public static bool TryParse(string value, out BGId id)
        {
            id = Empty;
            if (value == null || value.Length != 22) return false;
            try
            {
                id = new BGId(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// is id is empty
        /// </summary>
        public bool IsEmpty => (key1 | key2) == 0;

        /// <summary>
        /// construct id from 22 symbols Base64-encoded string   
        /// </summary>
        public BGId(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length != 22) throw new FormatException("Invalid BGID: value should be 22 symbols long, invalid value is inside brackets [" + value + "]");

            try
            {
                var bytes = Convert.FromBase64String(value + "==");
                // key1 = (ulong)ToInt64(bytes, 0);
                // key2 = (ulong)ToInt64(bytes, 8);
                //this is micro-optimization
                key1 =  (ulong)((uint)(bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24))
                                | ((long)(bytes[4] | (bytes[5] << 8) | (bytes[6] << 16) | (bytes[7] << 24)) << 32));
                key2 =  (ulong)((uint)(bytes[8] | (bytes[9] << 8) | (bytes[10] << 16) | (bytes[11] << 24))
                                | ((long)(bytes[12] | (bytes[13] << 8) | (bytes[14] << 16) | (bytes[15] << 24)) << 32));
            }
            catch
            {
                throw new FormatException("Invalid BGID: invalid value is inside brackets [" + value + "]");
            }
        }

        /// <summary>
        /// construct id from 16 bytes   
        /// </summary>
        public BGId(byte[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length != 16) throw new FormatException("Invalid BGID: value should be 16 bytes long");

            // key1 = (ulong)ToInt64(value, 0);
            // key2 = (ulong)ToInt64(value, 8);
            //this is micro-optimization
            key1 =  (ulong)((uint)(value[0] | (value[1] << 8) | (value[2] << 16) | (value[3] << 24))
                            | ((long)(value[4] | (value[5] << 8) | (value[6] << 16) | (value[7] << 24)) << 32));
            key2 =  (ulong)((uint)(value[8] | (value[9] << 8) | (value[10] << 16) | (value[11] << 24))
                            | ((long)(value[12] | (value[13] << 8) | (value[14] << 16) | (value[15] << 24)) << 32));
        }

        /// <summary>
        /// construct id using byte array data at specified index  
        /// </summary>
        public BGId(byte[] value, int startIndex)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startIndex + 16 > value.Length) throw new FormatException($"Invalid BGID: startIndex + 16 < value.Length, startIndex={startIndex}, value.Length={value.Length}");
            
            // key1 = (ulong)ToInt64(value, startIndex);
            // key2 = (ulong)ToInt64(value, startIndex + 8);
            //this is micro-optimization
            key1 =  (ulong)((uint)(value[startIndex] | (value[startIndex + 1] << 8) | (value[startIndex + 2] << 16) | (value[startIndex + 3] << 24))
                    | ((long)(value[startIndex + 4] | (value[startIndex + 5] << 8) | (value[startIndex + 6] << 16) | (value[startIndex + 7] << 24)) << 32));
            key2 =  (ulong)((uint)(value[startIndex + 8] | (value[startIndex + 9] << 8) | (value[startIndex + 10] << 16) | (value[startIndex + 11] << 24))
                            | ((long)(value[startIndex + 12] | (value[startIndex + 13] << 8) | (value[startIndex + 14] << 16) | (value[startIndex + 15] << 24)) << 32));
        }

        /// <summary>
        /// construct id using 2 ulong values  
        /// </summary>
        public BGId(ulong key1, ulong key2)
        {
            this.key1 = key1;
            this.key2 = key2;
        }

        /// <summary>
        /// construct id using 2 long values  
        /// </summary>
        public BGId(long key1, long key2)
        {
            this.key1 = (ulong)key1;
            this.key2 = (ulong)key2;
        }

        /// <summary>
        /// construct id using Guid value  
        /// </summary>
        public BGId(Guid value) : this(value.ToByteArray())
        {
        }
        
        //from byte array to long 
        private static long ToInt64(byte[] value, int offset)
        {
            if (offset > value.Length - 8) throw new FormatException("start index more than value.Length - 8" + offset + ">=" + value.Length);

            return (uint)(value[offset] | (value[offset + 1] << 8) | (value[offset + 2] << 16) | (value[offset + 3] << 24))
                   | ((long)(value[offset + 4] | (value[offset + 5] << 8) | (value[offset + 6] << 16) | (value[offset + 7] << 24)) << 32);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is BGId id && Equals(id);

        /// <inheritdoc />
        public bool Equals(BGId other) => key1 == other.key1 && key2 == other.key2;

        /// <inheritdoc />
        public override int GetHashCode() => (int)key1 ^ (int)(key1 >> 32) ^ (int)key2 ^ (int)(key2 >> 32);

        public static bool operator ==(BGId a, BGId b) => a.key1 == b.key1 && a.key2 == b.key2;

        public static bool operator !=(BGId a, BGId b) => !(a == b);

        /// <summary>
        /// Convert id to 22 symbols Base64-encoded string  
        /// </summary>
        public override string ToString() => Convert.ToBase64String(ToByteArray()).Substring(0, 22);

        /// <summary>
        /// Serilize id value to byte array 
        /// </summary>
        public byte[] ToByteArray()
        {
            var result = new byte[16];
            ToBytes(result, 0, key1);
            ToBytes(result, 8, key2);
            return result;
        }

        /// <summary>
        /// Serialize id value to byte array at specified coordinates
        /// </summary>
        public void ToByteArray(byte[] result, int start)
        {
            ToBytes(result, start, key1);
            ToBytes(result, start + 8, key2);
        }

        /// <summary>
        /// Serialize id value to 2 ulong values 
        /// </summary>
        public void ToULongKeys(out ulong key1, out ulong key2)
        {
            key1 = this.key1;
            key2 = this.key2;
        }

        /// <summary>
        /// Serialize id value to 2 long values 
        /// </summary>
        public void ToLongKeys(out long key1, out long key2)
        {
            key1 = (long)this.key1;
            key2 = (long)this.key2;
        }

        // Serialize ulong value to byte array at specified coordinates
        private static void ToBytes(byte[] result, int offset, ulong data)
        {
            result[offset] = (byte)data;
            result[offset + 1] = (byte)(data >> 8);
            result[offset + 2] = (byte)(data >> 16);
            result[offset + 3] = (byte)(data >> 24);
            result[offset + 4] = (byte)(data >> 32);
            result[offset + 5] = (byte)(data >> 40);
            result[offset + 6] = (byte)(data >> 48);
            result[offset + 7] = (byte)(data >> 56);
        }
        
        /// <summary>
        /// Serialize id value to 2 long values 
        /// </summary>
        public Guid ToGuid() => new Guid(ToByteArray());
    }
}