using Crogen.CrogenPooling;
using Hashira.Bosses.BillboardClasses;
using Hashira.Bosses.Patterns;
using Hashira.Core.EventSystem;
using Hashira.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entity = Hashira.Entities.Entity;
using LayerMask = UnityEngine.LayerMask;

namespace Hashira.Bosses
{
    [Serializable]
    public class BossPatternPair
    { 
        public bool enable = true;
        [SerializeReference, SubclassSelector]
        public BossPattern pattern;
    }

    [Serializable]
    public class BillboardPair
    {
        [Delayed] public string valueName = string.Empty;
        [SerializeReference, SubclassSelector]
        public BillboardValue billboardValue = null;
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
        
        private BossMover _bossMover;
        
        #region Initialize Boss

        protected override void Awake()
        {
            base.Awake();
            CurrentBosses.TryAdd(BossDisplayName, this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CurrentBosses.Remove(BossDisplayName);
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            for (int i = 0; i < bossPatterns.Length; i++)
                bossPatterns[i].pattern.Initialize(this);

            _bossMover = GetEntityComponent<BossMover>();
        }
        #endregion

        protected override void HandleDieEvent(Entity _)
        {
            Debug.Log("보스 죽음");
            CurrentBossPattern?.OnDie();
            
            var killEnemyEvent = InGameEvents.KillEnemyEvent;
            GameEventChannel.RaiseEvent(killEnemyEvent);
            
            gameObject.Pop(_dieEffect, transform.position, Quaternion.identity);
            _entityStateMachine.ChangeState("Dead");
            
            PlayerDataManager.Instance.AddKillCount(true);
            Cost.AddCost(_killCost);
            Destroy(gameObject);
        }

        #region Pattern Utils
        
        public BossPattern GetRandomBossPattern()
        {
            if (bossPatterns.Length == 0) return null;
            
            BossPatternPair[] selectedBossPatterns = 
                bossPatterns.Where(x => 
                    x.enable // 켜져있으면서
                    //  현재 페이즈가 패턴의 페이즈 범위 내에 있으거나 패턴의 페이즈의 최소값이 -1이거나 최대값이 -1이면
                    && ((x.pattern.phase.x <= currentPhase && currentPhase <= x.pattern.phase.y) || x.pattern.phase.x == -1 || x.pattern.phase.y == -1)) 
                    .ToArray(); // 실행할 패턴에 포함 
                
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
        public void SetCurrentBossPattern<T>() where T : BossPattern
        {
            CurrentBossPattern = GetBossPattern<T>();
            _entityStateMachine.ChangeState("Pattern");
        }
        
        #endregion

        #region Billboard

        public T BillboardValue<T>(string valueName) where T : BillboardValue
        {
            for (int i = 0; i < billboard.Length; i++)
            {
                if (billboard[i].valueName == valueName)
                    return billboard[i].billboardValue as T;
            }
            return null;
        }

        #endregion
        
        public void AddPhase()
        {
            currentPhase = Mathf.Clamp(currentPhase + 1, 1, maxPhase);
        }
        
        public void OnGroggy(float groggyDuration)
        {
            CurrentMaxGroggyTime = groggyDuration;
            _bossMover.StopImmediately();
            _entityStateMachine.ChangeState("Groggy");
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (bossPatterns == null) return;
            foreach (var bossPatternPair in bossPatterns)
                bossPatternPair.pattern?.OnDrawGizmos(transform);
        }
#endif
    }
}