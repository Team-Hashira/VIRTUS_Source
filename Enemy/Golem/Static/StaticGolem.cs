using Crogen.CrogenPooling;
using Hashira.Core;
using Hashira.Enemies.PublicStates;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.Golem.StaticGolem
{
    public class StaticGolem : Enemy
    {
        [field:SerializeField] public Transform EyeAttackPoint { get; private set; }
        [field:SerializeField] public ProjectilePoolType LaserPoolType {get; private set;}

        public bool IsEyeFollowToPlayer { get; set; } = false;
        
        Player _player;

        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
            _entityStateMachine.GetState<EnemyIdlePublicState>().TargetState = "Attack";
        }

        private void Start()
        {
            _player = PlayerManager.Instance.Player;
        }

        protected override void Update()
        {
            base.Update();
            EyeFollowToPlayer();
        }

        private void EyeFollowToPlayer()
        {
            if (IsEyeFollowToPlayer == false) return;

            Vector2 lookDir = (_player.transform.position - EyeAttackPoint.parent.position).normalized * 0.25f;
                
            EyeAttackPoint.localPosition = Vector2.Lerp(EyeAttackPoint.localPosition, lookDir, Time.deltaTime * 10);
        }
    }
}
