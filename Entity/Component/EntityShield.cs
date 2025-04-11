using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.StatSystem;
using Hashira.EffectSystem.Effects;
using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Hashira.Entities.Components
{
    public class EntityShield : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IDamageable
    {
        private Entity _entity;

        private EntityEffector _entityEffector;
        private EntityRenderer _entityRenderer;

        [SerializeField]
        private Vector2 _offset;

        private int _currentHP;

        public bool IsEvasion { get; set; }

        private void OnValidate()
        {
            transform.localPosition = _offset;
        }

        public void Initialize(Entity entity)
        {
            _entity = entity;

            _entityEffector = entity.GetEntityComponent<EntityEffector>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
        }

        public void AfterInit()
        {
            var stat = _entity.GetEntityComponent<EntityStat>();
            _currentHP = stat.StatDictionary[StatName.Health].IntValue * 2;
        }

        private void Update()
        {
            transform.position = _entity.transform.position + new Vector3(_offset.x * _entityRenderer.FacingDirection, _offset.y);
            transform.rotation = Quaternion.Euler(0, 90 - 90 * _entityRenderer.FacingDirection, 0);
        }

        public void ApplyDamage(AttackInfo attackInfo, RaycastHit2D raycastHit, bool popUpText = true)
        {
            _currentHP -= attackInfo.damage;
            if(_currentHP <= 0)
            {
                Stun stunEffect = new Stun();
                stunEffect.Setup(0.2f);
                _entityEffector.AddEffect(stunEffect);
                PopCore.Pop(EffectPoolType.ShieldBrokenEffect, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }
}
