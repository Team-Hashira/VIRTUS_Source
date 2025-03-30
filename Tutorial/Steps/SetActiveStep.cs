using System.Collections;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class SetActiveStep : TutorialStep
    {
        [SerializeField]
        private GameObject[] _toActiveObjects;
        [SerializeField]
        private float _activeDelay = 0.05f;

        [SerializeField]
        private bool _isActive = true;

        public override void OnEnter()
        {
            base.OnEnter();
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            WaitForSeconds ws = new WaitForSeconds(_activeDelay);

            foreach (var obj in _toActiveObjects)
            {
                obj.SetActive(_isActive);
                yield return ws;
            }

            _tutorialManager.NextStep();
        }
    }
}
