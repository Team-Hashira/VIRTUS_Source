using Hashira.MainScreen;
using System;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class ScreenRotationCheckStep : TutorialStep
    {
        public override void OnEnter()
        {
            base.OnEnter();
            MainScreenEffect.OnScreenRotateEvent += HandleOnScreenRotateEvent;
        }

        private void HandleOnScreenRotateEvent()
        {
            _tutorialManager.NextStep();
        }

        public override void OnExit()
        {
            MainScreenEffect.OnScreenRotateEvent -= HandleOnScreenRotateEvent;
            base.OnExit();
        }
    }
}
