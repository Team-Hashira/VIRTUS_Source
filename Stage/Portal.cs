using Hashira.Players;
using Hashira.StageSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hashira.Entities.Interacts
{
    public class Portal : KeyInteractObject
    {
        [Header("==========Portal==========")]
        [SerializeField] private SpriteRenderer _visual;
        [SerializeField] private Light2D _light;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float _rotateSpeed;

        private StageTypeSO _stageData;

        private void Update()
        {
            _visual.transform.Rotate(0, 0, _rotateSpeed);
        }

        public void Init(StageTypeSO stageTypeSO)
        {
            _visual.color = stageTypeSO.stageColor;
            _light.color = stageTypeSO.stageColor;
            _stageData = stageTypeSO;

            var mainModule = _particleSystem.main;
            mainModule.startColor = stageTypeSO.stageColor;
            _particleSystem.Play();
        }

        public override void Interaction(Player player)
        {
            base.Interaction(player);
            StageGenerator.Instance.NextStage(_stageData);
        }
    }
}
