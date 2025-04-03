using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using Hashira.LightingControl;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Bullet : Projectile
    {
        public bool IsParryingable { get; set; }

        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, Transform owner, bool isEventSender = true, float gravity = 0)
        {
            base.Init(whatIsTarget, direction, speed, damage, owner, isEventSender, gravity);
            Owner = owner;
            IsParryingable = true;
        }

        protected override void OnHited(HitInfo hitInfo)
        {
            base.OnHited(hitInfo);

            var damageable = hitInfo.damageable;
            var hit = hitInfo.raycastHit;

            if (damageable != null)
            {
                CameraManager.Instance.ShakeCamera(8, 10, 0.15f, isAdd: false);
                LightingController.Aberration(0.3f, 0.15f);

                if (damageable is EntityHealth health && health.TryGetComponent(out Entity entity))
                {
                    ParticleSystem[] bulletHitEffects = gameObject.Pop(EffectPoolType.BulletHitEffect, hit.point + hit.normal * 0.1f, 
                        Quaternion.Euler(0, 0, -90) * transform.rotation).gameObject.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem bulletHitEffect in bulletHitEffects)
                    {
                        var mainModule = bulletHitEffect.main;
                        mainModule.startColor = _trailRenderer.startColor;
                    }
                }
            }
            else
            {
                //Effect
                ParticleSystem hitSparkleEffect = gameObject.Pop(EffectPoolType.HitSparkleEffect, hit.point + hit.normal * 0.1f, 
                    Quaternion.LookRotation(Vector3.back, hit.normal)).gameObject.GetComponent<ParticleSystem>();
                var mainModule = hitSparkleEffect.main;
                mainModule.startColor = _trailRenderer.startColor;
            }
        }
    }
}