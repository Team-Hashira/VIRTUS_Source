using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using System.Collections;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class StoneFallsHandEffect : MonoBehaviour, IGiantGolemHandEffect
    {
        [SerializeField] private ProjectilePoolType _stonePoolType;
        [SerializeField] private float _firstDelay = 1;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireRange = 1;
        [SerializeField] private float _fireDelay = 0.1f;
        private float _currentFireTime = 0f;
        private bool _currentActive;

        public void Init(GiantGolemHand giantGolemHand)
        {
        }

        private void Update()
        {
            if (_fireDelay + _currentFireTime < Time.time && _currentActive)
            {
                StoneGenerate();
                _currentFireTime = Time.time;
            }
        }

        private void StoneGenerate()
        {
            GiantBounceRock stone = gameObject.Pop(_stonePoolType, (Vector2)_firePoint.position + Random.insideUnitCircle * _fireRange - new Vector2(0, 1), Quaternion.Euler(0, 0, Random.Range(0, 360))) as GiantBounceRock;
            stone.Init(1, -Vector3.up, 50f, null);
            stone.transform.localScale = Vector3.one * Random.Range(1f, 2);
        }

        public void SetActive(bool active)
        {
            StartCoroutine(CoroutineOnStart(active));
        }

        public void OnAnimatorTrigger(EAnimationTriggerType triggerType, int count)
        {
        }

        private IEnumerator CoroutineOnStart(bool active)
        {
            if(active)
                yield return new WaitForSeconds(_firstDelay);
            _currentActive = active;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_firePoint == null) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_firePoint.position, _fireRange / 2f);
            Gizmos.color = Color.white;
        }
#endif
    }
}
