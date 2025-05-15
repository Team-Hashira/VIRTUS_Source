using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira
{
    public class IceBlock : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private float _duration;
        private float _popTime;
        private Color _defaultColor;
        [SerializeField] private SpriteRenderer _visual;

        private void Awake()
        {
            _defaultColor = _visual.color;
        }

        public void Init(float duration)
        {
            _popTime = Time.time;
            _duration = duration;
        }

        private void Update()
        {
            Color color = _defaultColor;
            color.a *= (1 - Mathf.Pow((Time.time - _popTime) / _duration, 4));
            _visual.color = color;

            if (_popTime + _duration < Time.time)
            {
                this.Push();
            }
        }

        public void OnPop()
        {

        }

        public void OnPush()
        {

        }
    }
}
