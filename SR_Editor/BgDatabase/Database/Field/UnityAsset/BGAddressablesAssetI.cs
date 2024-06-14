/*
<copyright file="BGAddressablesAssetI.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// field, supporting loading assets from addressables
    /// </summary>
    public interface BGAddressablesAssetI : BGFieldUnityAssetI
    {
        /// <summary>
        /// convert internal stored value to Addressables supported format 
        /// </summary>
        string GetAddressablesAddress(int entityIndex);
    }

    /// <summary>
    /// custom interface for supporting custom loader model
    /// </summary>
    public interface BGAddressablesAssetCustomLoaderI
    {
        BGAddressablesLoaderModel GetAddressablesLoaderModel(int entityIndex);
    }

    /// <summary>
    /// custom  loader model, supporting additional parent asset type
    /// </summary>
    public class BGAddressablesLoaderModel
    {
        public string Address;
        public Type Type;

        public BGAddressablesLoaderModel(string address, Type type)
        {
            Address = address;
            Type = type;
        }
    }
}