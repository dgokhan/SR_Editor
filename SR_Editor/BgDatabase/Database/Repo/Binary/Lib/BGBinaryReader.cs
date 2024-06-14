/*
<copyright file="BGBinaryReader.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// binary reader helper
    /// </summary>
    public partial class BGBinaryReader
    {
        //source array
        private byte[] array;

        //cursor position
        private int cursor;

        /// <summary>
        /// current cursor position
        /// </summary>
        public int Cursor => cursor;

        /// <summary>
        /// underlying data length
        /// </summary>
        public int Length => array.Length;

        public BGBinaryReader(byte[] array)
        {
            this.array = array;
            cursor = 0;
        }

        public BGBinaryReader(ArraySegment<byte> array) => Reset(array);

        /// <summary>
        /// constructs new reader on top of internal array and set cursor to specified position
        /// </summary>
        public BGBinaryReader NewReader(int position = 0) => new BGBinaryReader(new ArraySegment<byte>(array, position, array.Length - position));

        /// <summary>
        /// set source array and set cursor to array segment offset 
        /// </summary>
        public void Reset(ArraySegment<byte> array)
        {
            this.array = array.Array;
            cursor = array.Offset;
        }

        /// <summary>
        /// move data cursor position 
        /// </summary>
        public void ShiftCursor(int delta) => cursor += delta;

        /// <summary>
        /// set cursor position 
        /// </summary>
        public void SetCursor(int position) => cursor = position;

        /// <summary>
        /// read int value
        /// </summary>
        public int ReadInt()
        {
            var result = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, cursor, BGFieldInt.SizeOfTheValue));
            cursor += BGFieldInt.SizeOfTheValue;
            return result;
        }

        /// <summary>
        /// read float value
        /// </summary>
        public float ReadFloat()
        {
            var result = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, cursor, BGFieldFloat.SizeOfTheValue));
            cursor += BGFieldFloat.SizeOfTheValue;
            return result;
        }

        /// <summary>
        /// read bool value
        /// </summary>
        public bool ReadBool()
        {
            var result = BGFieldBool.ValueFromBytes(new ArraySegment<byte>(array, cursor, BGFieldBool.SizeOfTheValue));
            cursor += BGFieldBool.SizeOfTheValue;
            return result;
        }

        /// <summary>
        /// read byte value
        /// </summary>
        public byte ReadByte()
        {
            var result = array[cursor];
            cursor += 1;
            return result;
        }

        /// <summary>
        /// read ID value
        /// </summary>
        public BGId ReadId()
        {
            var result = new BGId(array, cursor);
            cursor += 16;
            return result;
        }

        /// <summary>
        /// read string value
        /// </summary>
        public string ReadString()
        {
            var array = ReadByteArray();
            return array.Count == 0 ? null : Encoding.UTF8.GetString(array.Array, array.Offset, array.Count);
        }

        /// <summary>
        /// read array segment
        /// </summary>
        public ArraySegment<byte> ReadByteArray()
        {
            var length = ReadInt();
            var result = new ArraySegment<byte>(array, cursor, length);
            cursor += length;
            return result;
        }

        /// <summary>
        /// read array, action is called for each array element
        /// </summary>
        public void ReadArray(Action action)
        {
            var count = ReadInt();
            for (var i = 0; i < count; i++) action();
        }

        /// <summary>
        /// Clear internal state
        /// </summary>
        public void Dispose()
        {
            array = null;
            cursor = 0;
        }

        /// <summary>
        /// read short value
        /// </summary>
        public short ReadShort()
        {
            var result = BGFieldShort.ValueFromBytes(new ArraySegment<byte>(array, cursor, BGFieldShort.SizeOfTheValue));
            cursor += BGFieldShort.SizeOfTheValue;
            return result;
        }

        /// <summary>
        /// read long value
        /// </summary>
        public long ReadLong()
        {
            var result = BGFieldLong.ValueFromBytes(new ArraySegment<byte>(array, cursor, BGFieldLong.SizeOfTheValue));
            cursor += BGFieldLong.SizeOfTheValue;
            return result;
        }

        /// <summary>
        /// read sbyte
        /// </summary>
        public sbyte ReadSByte() => (sbyte)ReadByte();

        /// <summary>
        /// read ushort
        /// </summary>
        public ushort ReadUShort() => (ushort)ReadShort();

        /// <summary>
        /// read uint
        /// </summary>
        public uint ReadUInt() => (uint)ReadInt();

        /// <summary>
        /// read ulong
        /// </summary>
        public ulong ReadULong() => (ulong)ReadLong();

        /// <summary>
        /// read raw byte array (without count)
        /// </summary>
        public ArraySegment<byte> ReadByteArrayRaw(int length)
        {
            var result = new ArraySegment<byte>(array, cursor, length);
            cursor += length;
            return result;
        }
    }
}