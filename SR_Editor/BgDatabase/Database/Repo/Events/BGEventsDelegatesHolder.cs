using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Helper class for internal usage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BGEventsDelegatesHolder<T> where T : EventArgs
    {
        public EventHandler<T> Handler;
    }
}