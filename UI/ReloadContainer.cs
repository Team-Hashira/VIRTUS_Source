using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class ReloadContainer : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private CanvasGroup _canvasGroup;
        private float _maxTime = 0;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void HandleReload(float time)
        {
            if (Mathf.Approximately(time, 0))
            {
                _canvasGroup.alpha = 0;   
                _maxTime = 0;
            }
            else
            {
                _canvasGroup.alpha = 1;   
                if(_maxTime < time)
                    _maxTime = time;    
            }
            
            _image.fillAmount = 1-time/_maxTime;
        }
    }
}