﻿using System;

namespace Data.API
{
    [Serializable]
    public class CachePolicy
    {
        public static readonly int Unused = -1;
        public static readonly int Infinite = -2;

        public int AbsoluteSeconds { get; set; }
        public int SlidingSeconds { get; set; }
        public int RefillCount { get; set; }

        public CachePolicy()
        {
            AbsoluteSeconds = CachePolicy.Unused;
            SlidingSeconds = CachePolicy.Unused;
            RefillCount = 3;
        }

        public virtual CachePolicy Clone()
        {
            return new CachePolicy
                {
                    AbsoluteSeconds = this.AbsoluteSeconds,
                    SlidingSeconds = this.SlidingSeconds,
                    RefillCount = this.RefillCount
                };
        }
    }
}
