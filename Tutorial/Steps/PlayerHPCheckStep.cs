using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class PlayerHPCheckStep : TutorialStep
    {
        [SerializeField]
        private EntityHealth _playerHealth;

        [SerializeField]
        private int _threshold;

        public override void OnEnter()
        {
            base.OnEnter();
            _playerHealth.OnHealthChangedEvent += HandleOnHealthChangeEvent;
        }

        private void HandleOnHealthChangeEvent(int previous, int current)
        {
            if (current <= _threshold)
            {
                _tutorialManager.NextStep();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _playerHealth.OnHealthChangedEvent -= HandleOnHealthChangeEvent;
        }
    }
}
