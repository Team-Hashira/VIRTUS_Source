using Hashira.Bosses.BillboardClasses;
using Hashira.Bosses.Patterns;
using Hashira.Enemies;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Bosses
{
    [Serializable]
    public class BossPatternPair
    {
        [Delayed] public string patternName;
        public bool enable = true;
        [SerializeReference] public BossPattern pattern;
    }

    [Serializable]
    public class BillboardPair
    {
        [Delayed] public string valueName = string.Empty;
        [Delayed] public string typeName = string.Empty;
        [SerializeReference] public BillboardValue billboardValue = null;
    }

    public class Boss : Enemy
    {
        [field:SerializeField] public LayerMask WhatIsGround { get; private set; }
        [field:SerializeField] public int Priority { get; private set; }
        [field:SerializeField] public string BossName { get; private set; }
        [field:SerializeField] public string BossDisplayName { get; private set; }
        [field:SerializeField, Space] public bool IsPassive { get; private set; }
        [Range(1, 10), Space] public int maxPhase = 1;
        [Range(1, 10), Space] public int currentPhase = 1;
        [field:SerializeField] public float PatternPickDelay { get; private set; } = 1.85f;
            
        public BillboardPair[] billboard;
        public BossPatternPair[] bossPatterns;
        public BossPattern CurrentBossPattern { get; private set; }

        public float CurrentMaxGroggyTime { get; set; }
        public static Dictionary<string, Boss> CurrentBosses { get; private set; } = new Dictionary<string, Boss>();

        public Action OnPatternStartEvent;
        public Action OnPatternEndEvent;
        
        #region Initialize Boss

        protected override void Awake()
        {
            base.Awake();
            CurrentBosses.TryAdd(BossDisplayName, this);
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            for (int i = 0; i < bossPatterns.Length; i++)
                bossPatterns[i].pattern.Init(this);
            
            _entityHealth = GetEntityComponent<EntityHealth>();
            _entityStat = GetEntityComponent<EntityStat>();
        }
        #endregion

        protected override void HandleDieEvent(Entity _)
        {
            Debug.Log("보스 죽음");
            PlayerDataManager.Instance.AddKillCount(true);
            _entityStateMachine.ChangeState("Dead");
        }

        #region Pattern Utils
        public BossPattern GetRandomBossPattern()
        {
            if (bossPatterns.Length == 0) return null;
            
            BossPatternPair[] selectedBossPatterns = bossPatterns.Where(x => x.enable && x.pattern.phase == currentPhase).ToArray(); 
            int index = UnityEngine.Random.Range(0, selectedBossPatterns.Length);
            
            return selectedBossPatterns[index].pattern;
        }
        public T GetBossPattern<T>() where T : BossPattern
        {
            return bossPatterns.FirstOrDefault(x => x.pattern.GetType() == typeof(T))?.pattern as T;
        }
        public void SetCurrentBossPattern(BossPattern bossPattern)
        {
            CurrentBossPattern = bossPattern;
            _entityStateMachine.ChangeState("Pattern");
        }
        public T BillboardValue<T>(string valueName) where T : BillboardValue
        {
            for (int i = 0; i < billboard.Length; i++)
            {
                if (billboard[i].valueName == valueName)
                    return billboard[i].billboardValue as T;
            }
            return null;
        }

        public float GetGroundFloorPosY()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1000, WhatIsGround);
            return hit.point.y;
        }
        #endregion

        public void AddPhase()
        {
            ++currentPhase;
            currentPhase = Mathf.Clamp(currentPhase, 1, maxPhase);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (bossPatterns == null) return;
            for (int i = 0; i < bossPatterns.Length; i++)
                bossPatterns[i].pattern?.OnDrawGizmos(transform);
        }
        private void OnValidate()
        {
            for (int i = 0; i < billboard.Length; i++)
            {
                Type type = Type.GetType($"Hashira.Bosses.BillboardClasses.{billboard[i].typeName}Value");
                if (type == null) continue;
                
                for (int j = 0; j < i; j++)
                {
                    if (billboard[i].billboardValue == billboard[j].billboardValue)
                    {
                        billboard[i].billboardValue = Activator.CreateInstance(type) as BillboardValue;
                    }
                }
                if (billboard[i].billboardValue == null || billboard[i].billboardValue.GetType() != type)
                {

                    try
                    {
                        billboard[i].billboardValue = Activator.CreateInstance(type) as BillboardValue;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }

            for (int i = 0; i < bossPatterns.Length; i++)
            {
                Type type = Type.GetType($"Hashira.Bosses.Patterns.{bossPatterns[i].patternName}");
                for (int j = 0; j < i; j++)
                {
                    if (bossPatterns[i].pattern == bossPatterns[j].pattern)
                    {
                        bossPatterns[i].pattern = Activator.CreateInstance(type) as BossPattern;
                    }
                }
                if (bossPatterns[i].pattern == null || bossPatterns[i].pattern.GetType() != type)
                {
                    try
                    {
                        bossPatterns[i].pattern = Activator.CreateInstance(type) as BossPattern;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
#endif
    }
}