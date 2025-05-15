using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.StageSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public class ElectricBall : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private float _radius;
        [SerializeField] private float _speed;
        private List<SpriteRenderer> _visual;

        private float _direction;
        private float _duration;
        private float _popTime;
        private Color _defaultColor;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private AttackInfo _attackInfo;

        private void Awake()
        {
            _visual = new List<SpriteRenderer>();
            GetComponentsInChildren(_visual);
            _defaultColor = _visual[0].color;
        }

        public void Init(int damage, float duration)
        {
            _duration = duration;
            _attackInfo = new AttackInfo(damage, attackType: EAttackType.Electricity);

            _direction = (0.5f - Random.Range(0, 2)) * 2;
        }

        public void OnPop()
        {
            _popTime = Time.time;
            _visual.ForEach(renderer => renderer.color = _defaultColor);

            StageGenerator.Instance.OnNextStageEvent += HandleNextStageEvent;
        }

        private void HandleNextStageEvent()
        {
            this.Push();
        }

        public void OnPush()
        {
            StageGenerator.Instance.OnNextStageEvent -= HandleNextStageEvent;
        }

        private void FixedUpdate()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, _radius, Vector3.right * _direction, _speed * Time.fixedDeltaTime, _whatIsWall);
            if (hit)
            {
                transform.position += transform.right * (_direction * (hit.distance - 0.01f));
                _direction *= -1;
            }
            else
                transform.position += transform.right * (_speed * _direction * Time.fixedDeltaTime);

            Color color = _defaultColor;
            color.a = (1 - Mathf.Pow((Time.time - _popTime) / _duration, 4));
            _visual.ForEach(renderer => renderer.color = color);

            _visual[0].transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

            if (_popTime + _duration < Time.time)
            {
                this.Push();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out Enemy enemy) &&
                enemy.TryGetEntityComponent(out EntityHealth health))
            {
                health.ApplyDamage(_attackInfo);
                ElectricAttack electricAttack = PopCore.Pop(CardSubPoolType.ElectricAttack) as ElectricAttack;
                electricAttack.Init(transform.position, enemy.transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
