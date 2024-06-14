namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with constant value size supporting bulk loading operation
    /// </summary>
    public interface BGBinaryBulkLoaderStruct
    {
        /// <summary>
        /// More performant method of binary deserialization 
        /// </summary>
        void FromBytes(BGBinaryBulkRequestStruct request);
    }
}