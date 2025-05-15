using Crogen.CrogenPooling;
using Crogen.AttributeExtension;
using Hashira.Combat;
using Hashira.Core.StatSystem;
using System;
using UnityEngine;
using Hashira.Entities.Components;
using Hashira.Core;
using System.Collections.Generic;
using Hashira.Core.DamageHandler;
using System.Linq;

namespace Hashira.Entities
{
    public enum EAttackType
    {
        Default,        // 기본 공격
        Fixed,          // 고정피해
        Fire,           // 화염피해
        Electricity,    // 전기피해
        Bleeding,       // 출혈피해
    }

    public struct Shield
    {
        public Shield(int value, Guid guid)
        {
            this.value = value;
            this.guid = guid;
        }

        public int value;
        public Guid guid;
    }
    public delegate void OnHealthChangedEvent(int previous, int current);
    public delegate void OnHitEvent(int hp);
    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IDamageable
    {
        public int Health { get; private set; }

        public Entity Owner { get; private set; }
        private StatElement _maxHealth;
        private bool _isInvincible;
        public bool IsDie { get; private set; }

        public int MaxHealth => _maxHealth.IntValue;
        public event OnHealthChangedEvent OnHealthChangedEvent;
        public event OnHitEvent OnHitEvent;
        public event Action<Entity> OnDieEvent;

        private EntityMover _entityMover;
        private EntityStateMachine _entityStateMachine;
        private EntityRenderer _entityRenderer;

        public bool canKnockback = true;
        public bool IsKnockback { get; private set; }
        [HideInInspectorByCondition(nameof(canKnockback))]
        public float knockbackTime = 0.2f;
        private float _currentknockbackTime = 0;
        private Vector2 _knockbackDirection;

        private Dictionary<EDamageHandlerLayer, List<DamageHandler>> _damageHandlerDict;

        private int _evasionCount = 0;

        public void Initialize(Entity entity)
        {
            Owner = entity;
            _damageHandlerDict = new Dictionary<EDamageHandlerLayer, List<DamageHandler>>();
            foreach (EDamageHandlerLayer layerEnum in Enum.GetValues(typeof(EDamageHandlerLayer)))
            {
                _damageHandlerDict.Add(layerEnum, new List<DamageHandler>());
            }
        }

        public virtual void AfterInit()
        {
            _maxHealth = Owner.GetEntityComponent<EntityStat>().StatDictionary[StatName.Health];
            _entityMover = Owner.GetEntityComponent<EntityMover>();
            _entityStateMachine = Owner.GetEntityComponent<EntityStateMachine>();
            Owner.TryGetEntityComponent<EntityRenderer>(out _entityRenderer, true);

            _isInvincible = _maxHealth == null;
            Health = MaxHealth;
        }

        public void ApplyDamage(AttackInfo attackInfo, RaycastHit2D raycastHit = default, bool popUpText = true)
        {
            if (IsDie || _evasionCount > 0) return;

            int prev = Health;
            CalculateDamage(ref attackInfo);

            if (popUpText)
            {
                Vector3 textPos = raycastHit != default ? raycastHit.point : Owner.transform.position;
                CreateDamageText(attackInfo.damage, textPos, attackInfo.attackType);
            }

            Health -= attackInfo.damage;
            if (attackInfo.damage > 0)
                _entityRenderer?.Blink(0.1f);

            if (Health < 0)
                Health = 0;
            OnHitEvent?.Invoke(Health);
            OnHealthChangedEvent?.Invoke(prev, Health);

            if (attackInfo.knockback != Vector2.zero)
                OnKnockback(attackInfo.knockback.normalized, attackInfo.knockback.magnitude);

            if (Health == 0) Die();

            return;
        }

        public void SetHealth(int health)
        {
            Health = health;
        }

        private void CreateDamageText(int damage, Vector3 textPos, EAttackType attackType)
        {
            Color color = EnumUtility.AttackTypeColorDict[attackType];

            DamageText damageText = PopCore.Pop(WorldUIPoolType.DamageText, textPos, Quaternion.identity).gameObject.GetComponent<DamageText>();
            damageText.Init(damage, color);
        }

        protected virtual void CalculateDamage(ref AttackInfo attackInfo)
        {
            //int finalDamage = attackType == EAttackType.HeadShot ? damage * 2 : damage;
            for (int i = 0; i < DamageHandlerOrder.OrderList.Count; i++)
            {
                List<DamageHandler> handlerList = _damageHandlerDict[DamageHandlerOrder.OrderList[i]];
                EDamageHandlerStatus status = CalculateDamageHandlerList(ref attackInfo, handlerList);
                if (status == EDamageHandlerStatus.Stop)
                    break;
            }
        }

        public EDamageHandlerStatus CalculateDamageHandlerList(ref AttackInfo attackInfo, List<DamageHandler> handlerList)
        {
            foreach (DamageHandler handler in handlerList)
            {
                EDamageHandlerStatus status = handler.Calculate(ref attackInfo);
                if (status == EDamageHandlerStatus.Stop)
                    return EDamageHandlerStatus.Stop;
            }
            return EDamageHandlerStatus.Continue;
        }

        public void ApplyRecovery(int recovery)
        {
            if (IsDie) return;

            int prev = Health;
            Health += recovery;
            if (Health > MaxHealth)
                Health = MaxHealth;
            OnHealthChangedEvent?.Invoke(prev, Health);
        }

        public void ModifyEvasion(bool isEvasion)
        {
            if (isEvasion)
                _evasionCount++;
            else
                _evasionCount--;
        }

        public int GetEvasion() => _evasionCount;

        public void Resurrection()
        {
            IsDie = false;
            ApplyRecovery(MaxHealth);
        }

        public void Die()
        {
            IsDie = true;
            OnDieEvent?.Invoke(Owner);
            OnDieEvent = null;
        }

        private void Update()
        {
            if (IsKnockback)
            {
                _entityMover.SetMovement(_knockbackDirection, true);

                _currentknockbackTime += Time.deltaTime;
                if (_currentknockbackTime > knockbackTime)
                {
                    _currentknockbackTime = 0;
                    IsKnockback = false;
                    _entityMover.isManualMove = true;
                    _entityMover.StopImmediately();
                }
            }
        }

        public void OnKnockback(Vector2 hitDir, float knockbackPower)
        {
            _entityMover.StopImmediately();
            _entityMover.isManualMove = false;
            _knockbackDirection = hitDir.normalized * (knockbackPower / _entityMover.Rigidbody2D.mass);
            //_entityStateMachine.ChangeState("Hit");
            IsKnockback = true;
        }

        public void AddDamageHandler(EDamageHandlerLayer layer, DamageHandler handler)
        {
            if (_damageHandlerDict[layer] == null)
                _damageHandlerDict[layer] = new List<DamageHandler>();
            handler.Initialize(Owner);
            _damageHandlerDict[layer].Add(handler);
            _damageHandlerDict[layer].OrderBy(h => h.OrderInLayer);
        }

        public bool RemoveDamageHandler(DamageHandler handler)
        {
            foreach (var list in _damageHandlerDict.Values)
            {
                bool isRemoved = list.Remove(handler);
                if (isRemoved)
                    return isRemoved;
            }
            return false;
        }

        public void RemoveDamageHandler(EDamageHandlerLayer layer, DamageHandler handler)
        {
            _damageHandlerDict[layer].Remove(handler);
        }
    }
}