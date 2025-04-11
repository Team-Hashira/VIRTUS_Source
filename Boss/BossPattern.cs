using Hashira.Core;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [Serializable]
    public class BossPattern
    {
        [Range(1, 10)] public int phase = 1;

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

        public virtual void Init(Boss boss)
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

        protected void EndPattern()
        {
            StateMachine.ChangeState("Idle");
        }

        protected void EndPattern<T>() where T : BossPattern
        {
            BossPattern nextPattern = Boss.GetBossPattern<T>();
            Boss.SetCurrentBossPattern(nextPattern);
        }

        public void OnGroggy(float groggyDuration)
        {
            Boss.CurrentMaxGroggyTime = groggyDuration;
            Mover.StopImmediately();
            StateMachine.ChangeState("Groggy");
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
    }
}
