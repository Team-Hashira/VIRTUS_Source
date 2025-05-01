using Hashira.Bosses.BillboardClasses;
using Hashira.Core;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Players;
using Hashira.StageSystem;
using System;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class BossPattern
    {
        public Vector2Int phase = new Vector2Int(-1, -1);

        protected Boss Boss { get; private set; }
        protected Player Player { get; private set; }
        protected EntityStateMachine StateMachine { get; private set; }
        protected EntityStat EntityStat { get; private set; }
        protected BossMover Mover { get; private set; }
        protected EntityAnimator EntityAnimator { get; private set; }
        protected EntityRenderer EntityRenderer { get; private set; }
        protected Rigidbody2D Rigidbody { get; private set; }
        protected Transform Transform { get; private set; }
        protected GameObject GameObject { get; private set; }
        protected Stage CurrentStage => StageGenerator.Instance.GetCurrentStage();
        
        public T BillboardValue<T>(string valueName) where T : BillboardValue => Boss.BillboardValue<T>(valueName);
        
        public virtual void Initialize(Boss boss)
        {
            this.Boss = boss;
            Player = PlayerManager.Instance.Player;
            StateMachine = boss.GetEntityComponent<EntityStateMachine>();
            EntityStat = boss.GetEntityComponent<EntityStat>();
            Mover = boss.GetEntityComponent<BossMover>();
            EntityAnimator = boss.GetEntityComponent<EntityAnimator>();
            EntityRenderer = boss.GetEntityComponent<EntityRenderer>();
            Rigidbody = boss.GetComponent<Rigidbody2D>();
            Transform = boss.transform;
            GameObject = boss.gameObject;
        }

        public void EndPattern()    
        {
            StateMachine.ChangeState("Idle");
        }

        protected void EndPattern<T>() where T : BossPattern
        {
            BossPattern nextPattern = Boss.GetBossPattern<T>();
            Boss.SetCurrentBossPattern(nextPattern);
        }

        public void Groggy(float groggyDuration) => Boss.OnGroggy(groggyDuration);

        public virtual bool CanStart()
        {
            return true;
        }
        
        public virtual void OnStart()
        {
            Boss.OnPatternStartEvent?.Invoke();    
        }
        public virtual void OnEnd()
        {
            Boss.OnPatternEndEvent?.Invoke();    
        }
        public virtual void OnUpdate() { }
        public virtual void OnDrawGizmos(Transform transform) { }
        
        public virtual void OnDie() {}
    }
}
