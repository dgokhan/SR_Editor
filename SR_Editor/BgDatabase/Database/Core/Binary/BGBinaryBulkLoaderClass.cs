namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with variable value size supporting bulk loading operation
    /// </summary>
    public interface BGBinaryBulkLoaderClass
    {
        /// <summary>
        /// More performant method of binary deserialization (this is by fact is optional micro-optimization)
        /// </summary>
        void FromBytes(BGBinaryBulkRequestClass request);
    }
}