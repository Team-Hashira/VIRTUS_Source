using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Enemies;
using Hashira.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira
{
    public class Hurricane : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _duration;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private SpriteRenderer _visual;

        private float _popTime;
        private float _damageDelay = 1;
        private float _lastDamageTime;
        private float _defaultAlpha = 0.6f;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private AttackInfo _attackInfo;

        public void Init(int damage)
        {
            _attackInfo = new AttackInfo(damage);
        }

        public void OnPop()
        {
            _popTime = Time.time;
            _lastDamageTime = 0;
            _visual.color = new Color(1, 1, 1, _defaultAlpha);
        }

        public void OnPush()
        {

        }

        private void FixedUpdate()
        {
            _visual.transform.Rotate(new Vector3(0, 0, -10f));
            _visual.color = new Color(1, 1, 1, (1 - Mathf.Pow((Time.time - _popTime) / _duration, 4)) * _defaultAlpha);
            if (_popTime + _duration < Time.time)
            {
                this.Push();
            }
            else
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _radius, Vector2.zero, 0, _whatIsTarget);
                bool onDamage = _lastDamageTime + _damageDelay < Time.time;
                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out Enemy enemy) &&
                        enemy.TryGetEntityComponent(out EntityMover entityMover, true))
                    {
                        Vector2 dir = (transform.position - hit.transform.position).normalized * 3.3f;
                        entityMover.AddForce(dir);

                        if (onDamage && enemy.TryGetEntityComponent(out EntityHealth health))
                            health.ApplyDamage(_attackInfo);
                    }
                }
                if (onDamage)
                    _lastDamageTime = Time.time;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
