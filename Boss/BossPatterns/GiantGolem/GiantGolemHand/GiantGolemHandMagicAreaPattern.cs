using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Combat;
using Hashira.VFX;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemHandMagicAreaPattern : GiantGolemHandPattern
    {
        [SerializeField] private EffectPoolType _magicAreaVFXPoolType;
        [SerializeField] private EffectPoolType _magicCircleVFXPoolType;
        [SerializeField] private AttackVisualizer _attackVisualizer;
        [SerializeField] private float _visualizerPosX;
        
        private bool _canAttack;
        private MagicAreaVFX _magicAreaVFXObject;

        private Sequence _attackSequence;
        
        public override void Initialize(Boss boss)
        {
            base.Initialize(boss);
            _attackVisualizer.InitDamageCastVisualSign();
        }

        public override void OnStart()
        {
            base.OnStart();
            _canAttack = false;
            _attackVisualizer.SetAlpha(1);

            float playerPosXSign = Mathf.Sign(Player.transform.position.x);
            
            _attackVisualizer.transform.position = new Vector3(_visualizerPosX * playerPosXSign, 0, _attackVisualizer.transform.position.z);

            _attackSequence = DOTween.Sequence();
            _attackSequence.Append(Transform.DOMoveY(Transform.position.y + 1, 0.25f))
               .AppendCallback(() => _attackVisualizer.gameObject.SetActive(true))
               .Append(_attackVisualizer.SetDamageCastValue(1, 1))
               .AppendInterval(1)
               .AppendCallback(()=> PopCore.Pop(_magicCircleVFXPoolType, Transform))
               .AppendInterval(1)
               .AppendCallback(() =>
               {
                   _magicAreaVFXObject = PopCore.Pop(_magicAreaVFXPoolType, _attackVisualizer.transform.position, Quaternion.identity) as MagicAreaVFX;
                   _attackVisualizer.gameObject.SetActive(false);
                   _canAttack = true;
               });
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (_canAttack) _attackVisualizer.DamageCaster.CastDamage(AttackInfo.defaultOneDamage, popupText: false);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            _attackSequence?.Kill();
            _magicAreaVFXObject?.DelayPush();
            _attackVisualizer.SetAlpha(0);
            ReturnToOriginPosition();
        }
    }
}
