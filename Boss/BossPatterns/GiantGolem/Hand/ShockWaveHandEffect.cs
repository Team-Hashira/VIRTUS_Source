using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class ShockWaveHandEffect : MonoBehaviour, IGiantGolemHandEffect
    {
        [SerializeField] private EffectPoolType _shockWaveVFXPoolType;
        [SerializeField] private float _shockWaveDelay = 1f;
        private float _currentShockWaveTime = 0;
        private bool _active;
        
        public void Init(GiantGolemHand giantGolemHand)
        {
        }

        public void SetActive(bool active)
        {
            _active = active;
        }

        public void OnAnimatorTrigger(EAnimationTriggerType triggerType, int count)
        {
            
        }

        private void Update()
        {
            if (_active == false) return;
            if (_currentShockWaveTime + _shockWaveDelay < Time.time)
            {
                gameObject.Pop(_shockWaveVFXPoolType, transform.position, Quaternion.identity);
                gameObject.Pop(_shockWaveVFXPoolType, transform.position, Quaternion.Euler(0, 0, 180));
                _currentShockWaveTime = Time.time;
            }
        }
    }
}