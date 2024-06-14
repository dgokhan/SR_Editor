/*
<copyright file="BGCalcTypeCodeStateful.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Stateful type code. Can have additional attributes, which can be read/written from binary array or string
    /// </summary>
    public interface BGCalcTypeCodeStateful
    {
        /// <summary>
        /// restore state from binary array
        /// </summary>
        void ReadState(BGBinaryReader reader);
        
        /// <summary>
        /// write state to binary array
        /// </summary>
        void WriteState(BGBinaryWriter writer);

        /// <summary>
        /// restore state from string
        /// </summary>
        void ReadState(string state);
        
        /// <summary>
        /// write state to string
        /// </summary>
        string WriteState();
    }
}