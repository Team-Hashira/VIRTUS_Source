using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class SetActiveWithGlowStep : TutorialStep
    {
        [SerializeField]
        private GameObject[] _toActiveObjects;
        private SpriteRenderer[] _objectRenderers;
        [SerializeField]
        private float _activeDelay = 0.05f;

        [SerializeField]
        private bool _isActive = true;

        private Material _material;
        private readonly int _glowColorHash = Shader.PropertyToID("_GlowColor");

        [SerializeField]
        [ColorUsage(true, true)]
        private Color _glowColor;
        private Color _endColor;

        public override void Initialize(TutorialManager tutorialManager)
        {
            base.Initialize(tutorialManager);
            _objectRenderers = _toActiveObjects.Select(obj => obj.GetComponentInChildren<SpriteRenderer>()).ToArray();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            WaitForSeconds ws = new WaitForSeconds(_activeDelay);

            for (int i = 0; i < _toActiveObjects.Length; i++)
            {
                Material mat = _objectRenderers[i].material;
                _endColor = mat.GetColor(_glowColorHash);
                _toActiveObjects[i].SetActive(_isActive);
                mat.SetVector(_glowColorHash, _glowColor);
                mat.DOVector(_endColor, _glowColorHash, 0.5f);
                yield return ws;
            }

            _tutorialManager.NextStep();
        }
    }
}
