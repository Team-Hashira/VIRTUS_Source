using Hashira.Combat;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public class ShieldHandler : DamageHandler
    {
        public bool IsCountShield { get; private set; }
        public int Shield { get; private set; }

        public event Action<AttackInfo> OnBreakEvent;

        private int _currentShield;

        public ShieldHandler(int shieldValue, bool isCountShield)
        {
            Shield = shieldValue;
            IsCountShield = isCountShield;
            _currentShield = Shield;
        }

        public void Reset()
            => _currentShield = Shield;

        public override EDamageHandlerStatus Calculate(ref AttackInfo attackInfo)
        {
            if (IsCountShield)
            {
                _currentShield--;
                attackInfo.damage = 0;
                if (_currentShield <= 0)
                {
                    _owner.GetEntityComponent<EntityHealth>().RemoveDamageHandler(this);
                    OnBreakEvent?.Invoke(attackInfo);
                    Reset();
                }
                return EDamageHandlerStatus.Stop;
            }
            else
            {
                _currentShield -= attackInfo.damage;
                if (_currentShield <= 0)
                {
                    attackInfo.damage = -_currentShield;
                    _owner.GetEntityComponent<EntityHealth>().RemoveDamageHandler(this);
                    OnBreakEvent?.Invoke(attackInfo);
                    Reset();
                    return EDamageHandlerStatus.Continue;
                }
                else
                {
                    attackInfo.damage = 0;
                    return EDamageHandlerStatus.Stop;
                }
            }
        }
    }
}
