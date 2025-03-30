using UnityEngine;

namespace Hashira.CanvasUI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Animator _animatorCompo;

        private readonly int _healthAnimationHash = Animator.StringToHash("Health");
        private readonly int _setHealthAnimationHash = Animator.StringToHash("SetHealth");

        public void SetLevel(int level, bool isAnimation)
        {
            _animatorCompo.SetInteger(_healthAnimationHash, level);
            if (isAnimation == false)
                _animatorCompo.SetTrigger(_setHealthAnimationHash);
        }
    }
}
