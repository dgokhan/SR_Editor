using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Generic default list pool with static instance  
    /// </summary>
    public class BGListPoolDefault<T> : BGObjectPool<List<T>>
    {
        public static readonly BGListPoolDefault<T> I = new BGListPoolDefault<T>();

        private BGListPoolDefault() : base(() => new List<T>(), list => list.Clear())
        {
        }
    }
}