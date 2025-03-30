using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGoblinSpinPattern : BossPattern
    {
        [SerializeField] private float _duration = 7.5f;
        [SerializeField] private float _moveSpeed = 15f;
        [SerializeField] private float _allowPlayerDistance = 0.1f;
        private float _spinCurrentTime;

        [SerializeField] private DamageCaster2D _damageCaster;
        [SerializeField] private float _damageCastDelay = 0.25f;
        private float _currentDamageCastTime;

        public override void OnStart()
        {
            base.OnStart();
            _spinCurrentTime = Time.time;
            _currentDamageCastTime = Time.time;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var dir = Player.transform.position.x - Transform.position.x;
            var normalizedDir = Mathf.Sign(dir);
            EntityRenderer.LookTarget(normalizedDir);

            if (Mathf.Abs(dir) > _allowPlayerDistance)
                Mover.SetMovement(Vector2.right * Mathf.Sign(dir) * _moveSpeed);

            if (_currentDamageCastTime + _damageCastDelay < Time.time)
            {
                _currentDamageCastTime = Time.time;
                _damageCaster.CastDamage(1, false);
            }

            if (_spinCurrentTime + _duration < Time.time)
                OnGroggy(3f);
        }
    }
}
