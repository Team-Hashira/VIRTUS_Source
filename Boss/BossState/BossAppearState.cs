using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Bosses.States
{
    public class BossAppearState : EntityState
    {
        private readonly Boss _boss;
        private BossMover _bossMover;
        private Collider2D[] _colliders;
        private bool[] _collidersOriginEnable;
        
        public BossAppearState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _boss = entity as Boss;
            _bossMover = entity.GetEntityComponent<BossMover>();
            _colliders = entity.GetComponentsInChildren<Collider2D>();
            _collidersOriginEnable = new bool[_colliders.Length];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _bossMover.SetGravity(false);
            _entityAnimator.OnAnimationTriggeredEvent += HandleAppearStateEnd;

            for (int i = 0; i < _colliders.Length; i++)
            {
                _collidersOriginEnable[i] = _colliders[i].enabled;
                _colliders[i].enabled = false;
            }
        }

        private void HandleAppearStateEnd(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.End)
                _entityStateMachine.ChangeState("Idle");
        }

        public override void OnExit()
        {
            _bossMover.SetGravity(true);
            for (int i = 0; i < _colliders.Length; i++)
                _colliders[i].enabled = _collidersOriginEnable[i];
            
            _entityAnimator.OnAnimationTriggeredEvent -= HandleAppearStateEnd;
            base.OnExit();
        }
    }
}
