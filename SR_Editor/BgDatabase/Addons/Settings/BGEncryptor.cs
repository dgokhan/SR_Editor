/*
<copyright file="BGEncryptor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// this interface let you to encrypt database data/file.
    /// Make sure your encryptor works correctly, otherwise you corrupt database.
    /// After encrypting and decrypting- the data should be exactly the same 
    /// </summary>
    public interface BGEncryptor
    {
        /// <summary>
        /// Encrypt byte array 
        /// </summary>
        ArraySegment<byte> Encrypt(ArraySegment<byte> data, string config);

        /// <summary>
        /// Decrypt byte array
        /// After encrypting and decrypting- the data should be exactly the same, otherwise database data will be corrupted.
        /// </summary>
        ArraySegment<byte> Decrypt(ArraySegment<byte> data, string config);
    }
}