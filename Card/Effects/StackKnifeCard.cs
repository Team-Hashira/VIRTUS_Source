using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class StackKnifeCard : CardEffect
    {
        private List<Knife> _knifeList;
        [SerializeField] private int[] _damageByStack = { 10, 15, 15 };
        [SerializeField] private int _maxKnifeStack = 8;

        public override void Enable()
        {
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleHitEvent);
            _knifeList = new List<Knife>();
        }

        private void HandleHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            if (projectileHitEvent.hitInfo.damageable != null)
            {
                Knife knife = PopCore.Pop(CardSubPoolType.Knife, player.transform.position, Quaternion.identity) as Knife;
                knife.Init(_damageByStack[stack - 1], _knifeList.Count, _maxKnifeStack, player, stack >= 3);
                _knifeList.Add(knife);

                if (_knifeList.Count >= _maxKnifeStack)
                {
                    Transform target = projectileHitEvent.hitInfo.raycastHit.collider.transform;
                    player.StartCoroutine(KnifeShootCoroutine(0.08f, target));
                }
            }
        }

        private IEnumerator KnifeShootCoroutine(float delay, Transform target)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
            List<Knife> knifeList = _knifeList.ToList();
            _knifeList.Clear();
            foreach (Knife knife in knifeList)
            {
                knife.Shoot(target);
                yield return waitForSeconds;
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleHitEvent);
        }

        public override void Update()
        {

        }
    }
}
