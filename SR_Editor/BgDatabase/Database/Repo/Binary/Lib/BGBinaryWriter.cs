/*
<copyright file="BGBinaryWriter.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// binary writer helper
    /// </summary>
    public partial class BGBinaryWriter
    {
        //builder data
        private readonly List<byte> list;
//        private readonly int[] arrayCount;
//        private int arrayCursor;

        /// <summary>
        /// current count of written bytes
        /// </summary>
        public int Count => list.Count;

        public BGBinaryWriter() : this(65536)
        {
        }

        public BGBinaryWriter(int size) => list = new List<byte>(size);

        /// <summary>
        /// clear internal state
        /// </summary>
        public void Clear() => list.Clear();

        /// <summary>
        /// Written bytes as byte array
        /// </summary>
        public byte[] ToArray() => list.ToArray();

        /// <summary>
        /// add string value
        /// </summary>
        public void AddString(string value) => AddByteArray(string.IsNullOrEmpty(value) ? null : Encoding.UTF8.GetBytes(value));

        /// <summary>
        /// add int value
        /// </summary>
        public void AddInt(int value)
        {
            var array = BGFieldInt.ValueToBytes(value);
            list.Add(array[0]);
            list.Add(array[1]);
            list.Add(array[2]);
            list.Add(array[3]);
        }

        /// <summary>
        /// add float value
        /// </summary>
        public void AddFloat(float value)
        {
            var array = BGFieldFloat.ValueToBytes(value);
            list.Add(array[0]);
            list.Add(array[1]);
            list.Add(array[2]);
            list.Add(array[3]);
        }

        /// <summary>
        /// add bool value
        /// </summary>
        public void AddBool(bool value) => list.AddRange(BGFieldBool.ValueToBytes(value));

        /// <summary>
        /// add byte value
        /// </summary>
        public void AddByte(byte value) => list.Add(value);


        /// <summary>
        /// add ID value
        /// </summary>
        public void AddId(BGId value) => list.AddRange(value.ToByteArray());

        /// <summary>
        /// add byte array value
        /// </summary>
        public void AddByteArray(byte[] value)
        {
            if (value == null || value.Length == 0) AddInt(0);
            else
            {
                AddInt(value.Length);
                list.AddRange(value);
            }
        }

        /// <summary>
        /// add array value, action is called for each array item
        /// </summary>
        public void AddArray(Action action, int count)
        {
            AddInt(count);
            if (count <= 0) return;
            action();
        }

        /// <summary>
        /// add raw byte array value (without array count)
        /// </summary>
        public void AddBytesRaw(byte[] value) => list.AddRange(value);

        /// <summary>
        /// get bytes count for provided string
        /// </summary>
        public static int GetBytesCount(string value)
        {
            if (string.IsNullOrEmpty(value)) return 4;
            return 4 + Encoding.UTF8.GetByteCount(value);
        }

        /// <summary>
        /// get bytes count for provided byte array (with counter)
        /// </summary>
        public static int GetBytesCount(byte[] value) => value == null ? 4 : 4 + value.Length;

        /// <summary>
        /// add short value
        /// </summary>
        public void AddShort(short value)
        {
            var array = BGFieldShort.ValueToBytes(value);
            list.Add(array[0]);
            list.Add(array[1]);
        }

        /// <summary>
        /// add long value
        /// </summary>
        public void AddLong(long value)
        {
            var array = BGFieldLong.ValueToBytes(value);
            list.Add(array[0]);
            list.Add(array[1]);
            list.Add(array[2]);
            list.Add(array[3]);
            list.Add(array[4]);
            list.Add(array[5]);
            list.Add(array[6]);
            list.Add(array[7]);
        }

        /// <summary>
        /// add sbyte value
        /// </summary>
        public void AddSByte(sbyte value) => AddByte((byte)value);

        /// <summary>
        /// add ushort value
        /// </summary>
        public void AddUShort(ushort value) => AddShort((short)value);

        /// <summary>
        /// add uint value
        /// </summary>
        public void AddUInt(uint value) => AddInt((int)value);

        /// <summary>
        /// add ulong value
        /// </summary>
        public void AddULong(ulong value) => AddLong((long)value);

        public override string ToString() => "[" + list.Count + "]";

        public void ReplaceInt(int value, int position)
        {
            var array = BGFieldInt.ValueToBytes(value);
            list[position] = array[0];
            list[position + 1] = array[1];
            list[position + 2] = array[2];
            list[position + 3] = array[3];
        }
    }
}