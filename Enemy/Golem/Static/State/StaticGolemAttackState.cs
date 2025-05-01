using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.Core;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.Golem.StaticGolem
{
    public class StaticGolemAttackState : EntityState
    {
        private readonly StaticGolem _staticGolem;
        private Laser _currentLaserObject;

        public StaticGolemAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _staticGolem = entity as StaticGolem;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _staticGolem.IsEyeFollowToPlayer = true;
            _entityAnimator.OnAnimationTriggeredEvent += HandleAnimationTriggered;
        }
        
        private void HandleAnimationTriggered(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.Trigger)
            {
                _currentLaserObject = PopCore.Pop(_staticGolem.LaserPoolType, _staticGolem.EyeAttackPoint.position, Quaternion.identity) as Laser;
                _currentLaserObject.SetAlpha(0);
                _currentLaserObject.isPenetrate = true;
                _currentLaserObject.ShowVisualizer(0.85f)
                    .Join(DOTween.To(x=>_currentLaserObject.SetAlpha(x), 0.3f, 1f, 0.85f).SetEase(Ease.InExpo))
                    .AppendCallback(() => _staticGolem.IsEyeFollowToPlayer = false)
                    .Append(_currentLaserObject.Blink(0.6f))
                    .Append(_currentLaserObject.StartAttack(duration: 0.35f));
            }
            
            if (triggertype == EAnimationTriggerType.End)
            {
                _entityStateMachine.ChangeState("Refresh");
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_currentLaserObject && _staticGolem.IsEyeFollowToPlayer)
            {
                Vector2 attackDir = (PlayerManager.Instance.Player.transform.position - _staticGolem.transform.position).normalized;
                _currentLaserObject.SetAttackDirection(attackDir);
            }
        }

        public override void OnExit()
        {
            _entityAnimator.OnAnimationTriggeredEvent -= HandleAnimationTriggered;
            _currentLaserObject?.Push();
            base.OnExit();
        }
    }
}
