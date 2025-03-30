using Hashira.Entities.Components;

namespace Hashira.EffectSystem.Effects
{
    public class Stun : Effect, ICoolTimeEffect, ISingleEffect
    {
        private EntityStateMachine _entityStateMachine;

        public float Duration { get; private set; }
        public float LifeTime { get; set; }

        public void Setup(float duration)
        {
            Duration = duration;
        }

        public override void Enable()
        {
            base.Enable();

            _entityStateMachine = entity.GetEntityComponent<EntityStateMachine>();
            _entityStateMachine.ChangeState("Stun");
        }

        public override void Disable()
        {
            base.Disable();
            _entityStateMachine.ChangeState(_entityStateMachine.StartState);
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
