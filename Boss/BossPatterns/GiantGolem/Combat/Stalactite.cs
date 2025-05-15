using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class Stalactite : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private EffectPoolType _dieEffectPooltype;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private Transform _visaulizer;
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private DamageCaster2D _damageCaster;

        private void Awake()
        {
            _damageCaster = GetComponent<DamageCaster2D>();
        }

        private void Start()
        {
            _damageCaster.OnCasterSuccessEvent += OnDieHandle;
            _damageCaster.OnDamageCastSuccessEvent += OnDieHandle;
            _visaulizer.gameObject.SetActive(false);
        }

        private void OnDieHandle(RaycastHit2D hit)
        {
            PopCore.Pop(_dieEffectPooltype, hit.point, Quaternion.identity);
            this.Push();
        }

        private void OnDieHandle(HitInfo hitInfo)
        {
            // 맞으면 기절
            if (hitInfo.raycastHit.transform.TryGetComponent(out EntityEffector entityEffector))
            {
                Stun stunEffect = new Stun();
                stunEffect.Setup(0.65f);
                entityEffector.AddEffect(stunEffect);
            }
            OnDieHandle(hitInfo.raycastHit);
        }

        public void OnPop()
        {
            _visaulizer.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, _whatIsGround);
            if (hit.collider != null)
            {
                _visaulizer.gameObject.SetActive(true);
                _visaulizer.position = hit.point;
            }
            _damageCaster?.CastDamage(AttackInfo.defaultOneDamage, popupText: false);
        }

        public void OnPush()
        {
        }
    }
}
