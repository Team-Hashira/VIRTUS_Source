using Hashira.EffectSystem;
using System;
using UnityEngine;

namespace Hashira
{
    public interface ISingleEffect
    {
        public Action<Effect> OnAddEffectEvent { get; set; }
        public void OnAddEffect(Effect effect);
    }
}
