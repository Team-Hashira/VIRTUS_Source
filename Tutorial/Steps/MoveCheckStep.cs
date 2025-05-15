using Hashira.Core;
using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class MoveCheckStep : TutorialStep
    {
        private Player _player;

        private PlayerMover _playerMover;

        private bool _isMoved;
        //Jump는 InGameEvents에 PlayerJumpEvent로 처리할 수 있는데 일단 이게 더 보기 좋아서 이거로 함
        private bool _isJumped;

        public override void OnEnter()
        {
            base.OnEnter();
            _player = PlayerManager.Instance.Player;
            _playerMover = _player.GetEntityComponent<PlayerMover>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(_isMoved && _isJumped)
                _tutorialManager.NextStep();
            if(!_isMoved)
                _isMoved = Mathf.Abs(_playerMover.Velocity.x) > 1; 
            if(!_isJumped)
                _isJumped = _playerMover.Velocity.y > 1;
        }
    }
}
