using DG.Tweening;
using Hashira.Core.Attribute;
using Hashira.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class HealthUI : UIBase, IToggleUI
    {
        [SerializeField]
        private Image[] _icons;
        [SerializeField]
        private Vector2[] _iconOffsets;
        [Dependent]
        [SerializeField]
        private bool _syncWithIcons;


        [SerializeField]
        private EntityHealth _targetHealth;

        private Sequence _openSequence;

        public string Key { get; set; }

        public void Open()
        {
            _openSequence = DOTween.Sequence();
            //float baseTime = 0.08f;
            for (int i = 0; i < _icons.Length; i++)
            {
                //_openSequence.Insert(baseTime * i, _icons[i].)
            }
        }

        public void Close()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            _targetHealth.OnHealthChangedEvent += HandleOnHealthChangeEvent;
        }

        private void HandleOnHealthChangeEvent(int prev, int current)
        {
            for (int i = 0; i < _icons.Length; i++)
            {
                if (i < current)
                    _icons[i].gameObject.SetActive(true);
                else
                    _icons[i].gameObject.SetActive(false);
            }
        }
    }
}
