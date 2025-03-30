using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hashira.Tutorials
{
    [Serializable]
    public class TutorialInfo
    {
        public TutorialInfoSO infoSO;
        public float delay;
    }

    public abstract class TutorialStep : MonoBehaviour
    {
        protected TutorialManager _tutorialManager;

        [SerializeField]
        private TutorialStep[] _withActiveStep;

        public virtual void Initialize(TutorialManager tutorialManager)
        {
            _tutorialManager = tutorialManager;
        }

        public virtual void OnEnter() 
        {
            foreach (var step in _withActiveStep)
                _tutorialManager.SetActiveStep(step, true);
        }
        public virtual void OnUpdate() { }
        public virtual void OnExit() 
        {
            foreach (var step in _withActiveStep)
                _tutorialManager.SetActiveStep(step, false);
        }
    }
}
