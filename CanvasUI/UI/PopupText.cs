using Crogen.CrogenPooling;
using TMPro;
using UnityEngine;

namespace Hashira
{
    public class PopupText : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        [SerializeField] private float _upSpeed;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private SimplePoolingObject _simplePoolingObject;

        private void Update()
        {
            Color color = _text.color;
            color.a = 1 - _simplePoolingObject.CurLifetime / _simplePoolingObject.duration;
            _text.color = color;

            transform.position += Vector3.up * _upSpeed;
        }

        public void Init(string text, Color color)
        {
            _text.color = color;
            _text.text = text;
        }

        public void OnPop()
        {
            
        }

        public void OnPush()
        {

        }
    }
}
