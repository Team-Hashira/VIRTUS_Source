using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Hashira.Object
{
    public class LightObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private Light2D _light;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private EffectPoolType _breakEffect;

        [field: SerializeField] public int Resistivity { get; set; } = 1;
        public bool IsEvasion { get; set; }

        public void ApplyDamage(AttackInfo attackInfo, RaycastHit2D raycastHit, bool popUpText = true)
        {
            _light.enabled = false;
            _collider.enabled = false;
            gameObject.Pop(_breakEffect, transform.position, Quaternion.identity);
        }
    }
}
