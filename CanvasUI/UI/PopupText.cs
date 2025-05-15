using TMPro;
using UnityEngine;

namespace Hashira
{
    public class PopupText : SimplePoolingObject
    {
        [SerializeField] private float _upSpeed;
        [SerializeField] private TextMeshProUGUI _text;

        protected override void Update()
        {
            base.Update();
            Color color = _text.color;
            color.a = 1 - CurLifetime / duration;
            _text.color = color;

            transform.position += Vector3.up * _upSpeed * Time.deltaTime;
        }

        public void Init(string text, Color color)
        {
            _text.color = color;
            _text.text = text;
        }
    }
}
