using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Hashira.Object
{
    public class DoorObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private EffectPoolType _breakParticle;
        [SerializeField] private List<Sprite> _spriteList;
        private SpriteRenderer _spriteRenderer;

        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; private set; }

        [field: SerializeField] public int Resistivity { get; set; } = 2;
        public bool IsEvasion { get; set; }

        private void Awake()
		{
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			CurrentHealth = MaxHealth;
		}

        public void ApplyDamage(AttackInfo attackInfo, RaycastHit2D raycastHit, bool popUpText = true)
        {
            CurrentHealth -= attackInfo.damage;

            var curPercent = CurrentHealth / (float)MaxHealth;
            if (_spriteList.Count > 0)
                _spriteRenderer.sprite = _spriteList[(int)(curPercent * _spriteList.Count)];

            gameObject.Pop(_breakParticle, transform.position, Quaternion.identity);

            if (CurrentHealth <= 0)
                Destroy(gameObject);
        }
    }
}
