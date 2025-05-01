using Hashira.Entities;
using Hashira.FSM;
using System.Text;
using UnityEngine;

namespace Hashira.Bosses.States
{
    public class BossPatternState : EntityState
    {
        private readonly Boss _boss;

        private readonly StringBuilder _stringBuilder;
        private int _currentPatternHash;

        public BossPatternState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _stringBuilder = new StringBuilder();
            _boss = entity as Boss;
        }

        public override void OnEnter()
        {
            if (_boss.CurrentBossPattern.CanStart() == false)
            {
                _entityStateMachine.ChangeState("Idle");
                return;
            }
            
            _stringBuilder.Clear();
            // ex) "GiantGoblin" + "DashPattern"<---필요한 건 뒤쪽 부분
            _stringBuilder.Append(_boss.CurrentBossPattern.GetType().Name);
            _stringBuilder.Remove(0, _boss.BossName.Length);
            _currentPatternHash = Animator.StringToHash(_stringBuilder.ToString());

            _entityAnimator?.ClearAnimationTriggerDictionary();
            _entityAnimator?.SetParam(_currentPatternHash, true);
            Debug.Log(_stringBuilder.ToString() + "Start");
            _boss.CurrentBossPattern?.OnStart();
        }

        public override void OnExit()
        {
            _boss.CurrentBossPattern?.OnEnd();
            Debug.Log(_stringBuilder.ToString() + "Exit");
            _entityAnimator?.SetParam(_currentPatternHash, false);    
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _boss.CurrentBossPattern?.OnUpdate();
        }
    }
}
