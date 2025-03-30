using Hashira.Core.EventSystem;
using System;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class ShotCheckStep : TutorialStep
    {
        public override void OnEnter()
        {
            GameEventChannel.AddListener<ProjectileShootEvent>(HandleOnProjectileShoot);
            base.OnEnter();
        }

        private void HandleOnProjectileShoot(ProjectileShootEvent evt)
        {
            _tutorialManager.NextStep();
        }

        public override void OnExit()
        {
            base.OnExit();
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleOnProjectileShoot);
        }
    }
}
