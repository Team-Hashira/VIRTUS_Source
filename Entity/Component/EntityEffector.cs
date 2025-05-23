using Hashira.EffectSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.Entities
{
    public class EntityEffector : MonoBehaviour, IEntityComponent
    {
        public Entity Entity { get; private set; }
        public event Action<Effect> EffectAddedEvent;
        public event Action<Effect> EffectRemovedEvent;
        protected Dictionary<Type, List<Effect>> _effectDictionary = new Dictionary<Type, List<Effect>>();

        public IEnumerable<List<Effect>> EffectLists => _effectDictionary.Values;

        public virtual void Initialize(Entity entity)
        {
            Entity = entity;
        }

        private void Update()
        {
            UpdateEffects();
        }

        protected virtual void OnDestroy()
        {
            InitEffectDict();
            ClearEffect();
        }

        public int GetEffectCount(Type effectType)
            => _effectDictionary[effectType].Count;

        public List<Effect> GetEffectList<T>(T effect) where T : Effect 
            => _effectDictionary.GetValueOrDefault(effect.GetType());

        public List<Effect> GetEffectList<T>() where T : Effect
            => _effectDictionary.GetValueOrDefault(typeof(T));

        public bool TryGetEffectList<T>(out List<Effect> effects) where T : Effect 
            => _effectDictionary.TryGetValue(typeof(T), out effects);
        
        
        public void AddEffect(Effect effect)
        {
            if (effect == null) return;

            Type type = effect.GetType();
            effect.name = type.Name;
            effect.entity = Entity;
            effect.entityEffector = this;
            effect.entityStat = Entity.GetEntityComponent<EntityStat>();

            if (_effectDictionary.ContainsKey(type))
            {
                if (_effectDictionary[type].Count > 0 && 
                    _effectDictionary[type] is ISingleEffect singleClassEffect)
                {
                    singleClassEffect.OnAddEffect(effect);
                }   
                else
                {
                    _effectDictionary[type].Add(effect);
                }
            }
            else
            {
                _effectDictionary.TryAdd(type, new List<Effect>());
                _effectDictionary[type].Add(effect);
            }

            effect.Enable();
            EffectAddedEvent?.Invoke(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            Type type = effect.GetType();

            if (effect != null)
            {
                effect.Disable();
                EffectRemovedEvent?.Invoke(effect);
                _effectDictionary[type].Remove(effect);
            }
            else
                Debug.Log($"Effect {type.Name} was not found");
        }

        private void UpdateEffects()
        {
            List<List<Effect>> effectLists = _effectDictionary.Values.ToList();
            for(int i = 0; i < effectLists.Count; i++)
            {
                for (int j = 0; j < effectLists[i].Count; j++)
                {
                    var effect = effectLists[i][j];
                    // 버프가 수명이 다해서 없어질 때 체크
                    if (effect == null)
                    {
                        InitEffectDict();
                        return;
                    }

                    EffectInterfaceLogic(effect);
                    effect.Update();
                }
            }
        }

        protected void InitEffectDict()
        {
            _effectDictionary = _effectDictionary
                .Where(x => x.Key != null)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private void EffectInterfaceLogic(Effect effect)
        {
            if (effect is ICoolTimeEffect coolTimeEffect)
            {
                coolTimeEffect.LifeTime += Time.deltaTime;
                if (coolTimeEffect.LifeTime > coolTimeEffect.Duration)
                {
                    coolTimeEffect.LifeTime = 0;
                    OnEndEffect(effect);
                }
            }

            if (effect is ICountingEffect countingEffect)
            {
                if (countingEffect.MaxCount < 0) return;
                if (countingEffect.MaxCount <= countingEffect.Count)
                {
                    countingEffect.Count = 0;
                    OnEndEffect(effect);
                }
            }
        }

        public void ClearEffect()
        {
            List<List<Effect>> effectListList = _effectDictionary.Values.ToList();
            foreach (List<Effect> effectList in effectListList)
            {
                List<Effect> removeEffectList = effectList.ToList();
                foreach (Effect effect in removeEffectList)
                {
                    RemoveEffect(effect);
                }
            }
        }

        // Effect들의 초기화(죽음)
        private void OnEndEffect(Effect effect)
        {
            if (effect is ICoolTimeEffect coolTimeEffect)
            {
                coolTimeEffect.OnTimeOut();
                coolTimeEffect.OnTimeOutEvent?.Invoke(effect);
            }
            
            // ILoopEffect는 Effect의 사이클 자체를 담당하는 특별한 인터페이스이기 때문에 얘만 이렇게 따로 빼둘 필요가 있음
            if (effect is ILoopEffect loopEffect)
            {
                loopEffect.Reset();
                return;
            }

            RemoveEffect(effect);
        }
    }
}
