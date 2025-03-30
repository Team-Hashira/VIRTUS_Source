using Hashira.Entities.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemHandAnimator : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        private Dictionary<EAnimationTriggerType, int> _triggerDictionary = new();
        public event OnAnimationTriggeredEvent OnAnimationTriggeredEvent;

        private void AnimationTrigger(EAnimationTriggerType eAnimationTriggerType)
        {
            if (_triggerDictionary.ContainsKey(eAnimationTriggerType))
                _triggerDictionary[eAnimationTriggerType]++;
            else
                _triggerDictionary[eAnimationTriggerType] = 1;
            OnAnimationTriggeredEvent?.Invoke(eAnimationTriggerType, _triggerDictionary[eAnimationTriggerType]);
        }

        public void ClearAnimationTriggerDictionary()
            => _triggerDictionary.Clear();

        public void SetFloat(int hash, float value)=>Animator.SetFloat(hash, value);
        public void SetBool(int hash, bool value)=>Animator.SetBool(hash, value);
    }
}
