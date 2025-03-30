using UnityEngine;

namespace Hashira.Tutorials
{
    public class DelayStep : TutorialStep
    {
        [SerializeField]
        private float _delayTime;
        private float _delayTimer = 0;

        public override void OnEnter()
        {
            base.OnEnter();
            _delayTimer = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _delayTimer += Time.deltaTime;
            if (_delayTimer > _delayTime)
            {
                _tutorialManager.NextStep();
            }
        }
    }
}
