namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Supported HTTP methods. Default means that the method is not explicitly set
    /// </summary>
    public enum BGLiveUpdateHttpMethodEnum : byte
    {
        /// <summary>
        /// default method is used to not break existing code 
        /// </summary>
        Default,
        /// <summary>
        /// HTTP GET
        /// </summary>
        Get,
        /// <summary>
        /// HTTP POST
        /// </summary>
        Post,
    }
}