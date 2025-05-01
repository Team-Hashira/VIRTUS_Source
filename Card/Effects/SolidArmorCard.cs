using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.DamageHandler;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class SolidArmorCard : CardEffect
    {
        private int _hitStack = 0;

        private EntityHealth _entityHealth;
        private ShieldHandler _shieldHandler;
        private SpriteRenderer _shieldSpriteRenderer;
        private IPoolingObject _shieldPool;

        public override void Enable()
        {
            _hitStack = 0;
            _entityHealth = player.GetEntityComponent<EntityHealth>();
            _entityHealth.OnHealthChangedEvent += HandlePlayerHitEvent;

            _shieldHandler = new ShieldHandler(1, true);
            _shieldHandler.SetOrderInLayer(-100);
            _shieldHandler.OnBreakEvent += HandleShieldBreakEvent;
            _shieldPool = PopCore.Pop(CardSubPoolType.SolidArmor, player.transform);
            _shieldSpriteRenderer = _shieldPool.gameObject.GetComponent<SpriteRenderer>();
            _shieldSpriteRenderer.color = new Color(1, 1, 1, 0);
        }

        private void HandleShieldBreakEvent(AttackInfo info)
        {
            _hitStack = 0;
            for (int i = 0; i < 5; i++)
            {
                SolidArmorProjectile solidArmorProjectile = PopCore.Pop(ProjectilePoolType.SolidArmorProjectile, player.transform.position, Quaternion.identity) as SolidArmorProjectile;
                solidArmorProjectile.Init(player, Random.insideUnitCircle);
            }
            _shieldSpriteRenderer.color = new Color(1, 1, 1, 0);
        }

        private void HandlePlayerHitEvent(int prev, int current)
        {
            if (prev <= current) return;

            _hitStack++;
            if (_hitStack == 2)
            {
                _shieldHandler.Reset();
                _entityHealth.AddDamageHandler(EDamageHandlerLayer.First, _shieldHandler);
            }
            _shieldSpriteRenderer.color = new Color(1, 1, 1, (float)_hitStack / 2);
        }

        public override void Update()
        {

        }

        public override void Disable()
        {
            _entityHealth.OnHealthChangedEvent -= HandlePlayerHitEvent;
            if (_hitStack == 2)
            {
                _shieldHandler.OnBreakEvent -= HandleShieldBreakEvent;
                _entityHealth.RemoveDamageHandler(EDamageHandlerLayer.First, _shieldHandler);
            }
            _shieldPool.Push();
        }
    }
}
