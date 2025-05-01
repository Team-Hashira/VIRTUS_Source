using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class Stun : Effect, ICoolTimeEffect, ISingleEffect
    {
        private EntityStateMachine _entityStateMachine;

        public float Duration { get; private set; }
        public float LifeTime { get; set; }
        public Action<Effect> OnTimeOutEvent { get; set; }
        public Action<Effect> OnAddEffectEvent { get; set; }

        private string _newState;

        public void Setup(float duration)
        {
            Duration = duration;
        }

        public override void Enable()
        {
            base.Enable();
            _entityStateMachine = entity.GetEntityComponent<EntityStateMachine>();
            _newState = _entityStateMachine.GetShareVariable<string>("TargetState");
            if (string.IsNullOrEmpty(_newState))
                _newState = _entityStateMachine.StartState.stateName;
            _entityStateMachine.ChangeState("Stun");
        }

        public override void Disable()
        {
            base.Disable();
            _entityStateMachine.ChangeState(_newState);
        }

        public void OnAddEffect(Effect effect)
        {
            Stun stun = effect as Stun;
            if (stun.Duration > Duration - LifeTime)
            {
                Duration = stun.Duration;
                LifeTime = 0;
            }
        }

        public void OnTimeOut()
        {
        }
    }
}
