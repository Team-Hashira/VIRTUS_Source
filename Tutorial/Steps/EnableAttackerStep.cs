using DG.Tweening;
using Hashira.Core;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class EnableAttackerStep : TutorialStep
    {
        private GameObject _attackerObject;

        private Material _attackMaterial;
        private readonly int _glowColorHash = Shader.PropertyToID("_GlowColor");

        [SerializeField]
        [ColorUsage(true, true)]
        private Color _glowColor;
        private Color _endColor;

        public override void Initialize(TutorialManager tutorialManager)
        {
            base.Initialize(tutorialManager);
            _attackerObject = PlayerManager.Instance.Player.Attacker.gameObject;
            _attackerObject.SetActive(false);

            _attackMaterial = _attackerObject.GetComponent<SpriteRenderer>().material;
            _endColor = _attackMaterial.GetColor(_glowColorHash);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _attackerObject.SetActive(true);
            _tutorialManager.NextStep();

            _attackMaterial.SetVector(_glowColorHash, _glowColor);
            _attackMaterial.DOVector(_endColor, _glowColorHash, 0.5f);
        }
    }
}
