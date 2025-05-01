using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Animator _animatorCompo;
        [SerializeField] private Sprite[] _healthSprites;

        private readonly int _healthAnimationHash = Animator.StringToHash("Health");
        private readonly int _setHealthAnimationHash = Animator.StringToHash("SetHealth");


        public void SetLevel(int level, bool isAnimation)
        {
            if (isAnimation == false)
            {
                _animatorCompo?.SetTrigger(_setHealthAnimationHash);
                _image.sprite = _healthSprites[level];
            }
            _animatorCompo?.SetInteger(_healthAnimationHash, level);
        }
    }
}
