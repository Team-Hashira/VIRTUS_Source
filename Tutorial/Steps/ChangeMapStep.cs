using DG.Tweening;
using Doryu.CustomAttributes;
using Hashira.Core;
using Hashira.MainScreen;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class ChangeMapStep : TutorialStep
    {
        [SerializeField]
        private GameObject _beforeMap, _toChangeMap;

        [SerializeField]
        private bool _usePlayerSpawnPosition;
        [ToggleField(nameof(_usePlayerSpawnPosition))]
        [SerializeField]
        private Transform _playerSpawnPosition;

        [SerializeField]
        private bool _useGlitch;


        public override void OnEnter()
        {
            base.OnEnter();
            if(_useGlitch)
            {
                MainScreenEffect.OnGlitch(2.3f, 0.1f).OnComplete(() => MainScreenEffect.OnGlitch(0, 0.5f));
                CameraManager.Instance.ShakeCamera(6f, 6f, 0.6f);
            }
            _beforeMap.SetActive(false);
            _toChangeMap.SetActive(true);
            if (_usePlayerSpawnPosition)
                PlayerManager.Instance.Player.transform.position = _playerSpawnPosition.position;
            _tutorialManager.NextStep();
        }
    }
}
