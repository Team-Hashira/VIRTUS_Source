using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class PlayerImage : MonoBehaviour
    {
        private Animator _animator;
        private Image _image;
        public Material material => _image.material;
        private int actionCount = 3;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _image = GetComponent<Image>();
        }

        public void EndTrigger()
        {
            int randomActionNum = GetRandomActionNum();

            _animator.SetFloat("Sleep", randomActionNum);
        }

        private int GetRandomActionNum()
        {
            return Random.Range(-5, actionCount);
        }
    }
}
