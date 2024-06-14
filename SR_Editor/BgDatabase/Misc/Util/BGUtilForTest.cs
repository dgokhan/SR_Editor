/*
<copyright file="BGUtilForTest.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public static class BGUtilForTest
    {
        public enum TestEnumInt
        {
            first = 0,
            second = -1000000000,
            third = -2000000000,
            forth = -2147483648,
            fifth = 1000000000,
            sixth = 2000000000,
            seventh = 2147483647
        }

        public enum TestEnumShort : short
        {
            first = 0,
            second = -16000,
            third = -32000,
            forth = -32768,
            fifth = 16000,
            sixth = 32000,
            seventh = 32767
        }

        public enum TestEnumByte : byte
        {
            first = 0,
            second = 1,
            third = 127,
            forth = 200,
            fifth = 255
        }
    }
}