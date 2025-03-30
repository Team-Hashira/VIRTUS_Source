using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public interface IGiantGolemHandEffect
    {
        public void Init(GiantGolemHand giantGolemHand);
        public void SetActive(bool active);
        public void OnAnimatorTrigger(EAnimationTriggerType triggerType , int count);
    }
}
