using System;

namespace Hashira.EffectSystem
{
    public interface ICountingEffect
    {
        public int MaxCount { get; set; }
        public int Count { get; set; }
    }
}
