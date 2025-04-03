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
    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IDamageable
    {
        public int Health { get; private set; }
        public Stack<Shield> Shields { get; set; } = new Stack<Shield>();

        public Entity Owner { get; private set; }
        private StatElement _maxHealth;
        private bool _isInvincible;
        public bool IsDie { get; private set; }

        public int MaxHealth => _maxHealth.IntValue;
        public event OnHealthChangedEvent OnHealthChangedEvent;
        public event Action<Entity> OnDieEvent;

        private EntityMover _entityMover;
        private EntityStateMachine _entityStateMachine;
        private EntityRenderer _entityRenderer;
        
        public bool canKnockback = true;
        [Obsolete]
        public bool IsEvasion { get; set; }
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
            _entityMover = Owner.GetEntityComponent<EntityMover>(true);
            _entityStateMachine = Owner.GetEntityComponent<EntityStateMachine>();
            Owner.TryGetEntityComponent<EntityRenderer>(out _entityRenderer, true);
            
            _isInvincible = _maxHealth == null;
            Health = MaxHealth;
        }

        public void ApplyDamage(AttackInfo attackInfo, RaycastHit2D raycastHit = default, bool popUpText = true)
        {
            if (IsDie || _evasionCount > 0) return;

            int shieldValue = 0;

            while (shieldValue < attackInfo.damage && Shields.Count() > 0)
            {
                // 감쇠시킬 쉴드 값 더해주기
                Shield shield = Shields.Pop();
                shieldValue += shield.value;

                // 받은 데미지보다 쉴드값이 더 클 때
                if (shieldValue > attackInfo.damage)
                {
                    Shields.Push(new Shield(shieldValue - attackInfo.damage, shield.guid));

                    // 아예 상쇄시키기
                    return;
                }
            }

            // 데미지 감쇠
            attackInfo.damage -= shieldValue;

            int prev = Health;
            int finalDamage = CalculateDamage(attackInfo.damage, attackInfo.attackType);

            if (popUpText)
            {
                Vector3 textPos = raycastHit != default ? raycastHit.point : Owner.transform.position;
                CreateDamageText(finalDamage, textPos, attackInfo.attackType);
            }

            Health -= finalDamage;
            if (finalDamage > 0)
                _entityRenderer?.Blink(0.1f);
            
            if (Health < 0)
                Health = 0;
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

            DamageText damageText = gameObject.Pop(WorldUIPoolType.DamageText, textPos, Quaternion.identity).gameObject.GetComponent<DamageText>();
            damageText.Init(damage, color);
        }

        protected virtual int CalculateDamage(int damage, EAttackType attackType)
        {
            //int finalDamage = attackType == EAttackType.HeadShot ? damage * 2 : damage;
            int finalDamage = damage;
            for (int i = 0; i < DamageHandlerOrder.OrderList.Count; i++)
            {
                List<DamageHandler> handler = _damageHandlerDict[DamageHandlerOrder.OrderList[i]];
                EDamageHandlerStatus status = CalculateDamageHandlerList(finalDamage, attackType, handler, out finalDamage);
                if (status == EDamageHandlerStatus.Stop)
                    break;
            }
            return finalDamage;
        }

        public EDamageHandlerStatus CalculateDamageHandlerList(int damage, EAttackType attackType, List<DamageHandler> handlerList, out int finalDamage)
        {
            finalDamage = damage;
            foreach (DamageHandler handler in handlerList)
            {
                EDamageHandlerStatus status = handler.Calculate(damage, attackType, out finalDamage);
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

        #region Shield

        public Shield GetShield(Guid guid)
            => Shields.FirstOrDefault(x => x.guid.Equals(guid));

        public void AddShield(int value, Guid guid)
        {
            Shields.Push(new Shield(value, guid));
        }

        public Guid AddShield(int value)
        {
            Guid guid = Guid.NewGuid();
            Shields.Push(new Shield(value, guid));
            return guid;
        }

        #endregion

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
            _damageHandlerDict[layer].Add(handler);
        }

        public void RemoveDamageHandler(DamageHandler handler)
        {
            foreach (var list in _damageHandlerDict.Values)
            {
                bool isRemoved = list.Remove(handler);
                if (isRemoved)
                    break;
            }
        }
    }
}