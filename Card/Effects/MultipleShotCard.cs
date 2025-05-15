using Hashira.Core.EventSystem;
using System.Collections;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class MultipleShotCard : CardEffect
    {
        private int _shootCount;
        private int _needShootCount = 4;

        private Attacker _attacker;

        public override void Enable()
        {
            base.Enable();
            _attacker = player.Attacker;
            GameEventChannel.AddListener<ProjectileShootEvent>(HandleAttackerShootEvnet);
            _shootCount = 0;

            _needShootCount = 5 - stack;
        }

        private void HandleAttackerShootEvnet(ProjectileShootEvent projectileShootEvent)
        {
            if (projectileShootEvent.isPlayerInput)
            {
                _shootCount++;
                if (_shootCount >= _needShootCount)
                {
                    _shootCount = 0;
                    _attacker.StartCoroutine(MultipleShootDelayCoroutine(0.15f));
                }
            }
        }

        private IEnumerator MultipleShootDelayCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            _attacker?.Shoot();
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleAttackerShootEvnet);
        }
    }
}
