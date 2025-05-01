using Hashira.EffectSystem;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerEffector : EntityEffector
    {
        private static Dictionary<Type, List<Effect>> _permanentEffectDictionary = new Dictionary<Type, List<Effect>>();

        private void Start()
        {
            foreach (var effectPair in _permanentEffectDictionary)
            {
                _effectDictionary.Add(effectPair.Key, effectPair.Value);
                foreach (Effect effect in effectPair.Value)
                {
                    effect.entity = Entity;
                    effect.entityEffector = this;
                    effect.entityStat = Entity.GetEntityComponent<EntityStat>();
                    effect.Enable();
                }
            }
        }
        
        protected override void OnDestroy()
        {
            InitEffectDict();
            _permanentEffectDictionary.Clear();
            foreach (var effectPair in _effectDictionary)
            {
                _permanentEffectDictionary.Add(effectPair.Key, effectPair.Value);                
            }
        }
    }
}