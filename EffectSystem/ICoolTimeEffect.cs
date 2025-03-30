using System;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public interface ICoolTimeEffect    
    {
        public float Duration { get; }
        public float LifeTime { get; set; }

        public void OnTimeOut();
    }
}
