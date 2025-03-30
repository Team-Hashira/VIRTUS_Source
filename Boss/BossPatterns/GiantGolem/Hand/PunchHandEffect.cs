using Hashira.Combat;
using Hashira.Core;
using Hashira.Entities;
using Hashira.Entities.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class PunchHandEffect : MonoBehaviour, IGiantGolemHandEffect
    {
        [SerializeField] private AttackVisualizer _attackVisualizer;
        [SerializeField] private ParticleSystem _punchParticle;
        private GiantGolemHand _giantGolemHand;
        public void Init(GiantGolemHand giantGolemHand)
        {
            _giantGolemHand = giantGolemHand;
            _attackVisualizer.ResetDamageCastVisualSign();
        }

        public void SetActive(bool active)
        {
            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;
            _attackVisualizer.DamageCaster.transform.position = new Vector3(_giantGolemHand.transform.position.x, _attackVisualizer.transform.position.y);
            _attackVisualizer.DamageCaster.CastDamage(1, Vector2.zero, new Vector2(Mathf.Sign(playerPos.x - transform.position.x) * 10f, 0), EAttackType.Default, false);
            _attackVisualizer.SetDamageCastSignValue(0f);
            SoundManager.Instance.PlaySFX("GiantGolemPunch", transform.position, 1, Random.Range(0.9f, 1f));
            _punchParticle?.Play(true);
        }

        public void OnAnimatorTrigger(EAnimationTriggerType triggerType, int count)
        {
            
        }
    }
}
