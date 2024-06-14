/*
<copyright file="BGConfigurableBinaryI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Mark type as having some configuration which can be represented as byte array
    /// </summary>
    public interface BGConfigurableBinaryI
    {
        /// <summary>
        /// serialize config data as byte array
        /// </summary>
        /// <returns>config as string</returns>
        byte[] ConfigToBytes();

        /// <summary>
        /// restore config data from byte array
        /// </summary>
        void ConfigFromBytes(ArraySegment<byte> config);
    }
}