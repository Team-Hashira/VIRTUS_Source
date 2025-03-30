using Hashira.Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class MultipleShotCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        public float Duration { get; set; } = 5;
        public float Time { get; set; }

        private int _shootCount;
        private int _needShootCount = 4;

        private Attacker _attacker;

        public override void Enable()
        {
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

        public override void Update()
        {
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleAttackerShootEvnet);
        }
    }
}
