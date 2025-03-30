using DG.Tweening;
using Hashira.Core.Attribute;
using Hashira.Pathfind;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class StageEndingPanel : UIBase, IToggleUI
    {
        [field: SerializeField]
        public string Key { get; set; }

        private Sequence _openSequence;

        [Header("제발!!! 오른쪽 위쪽 왼쪽 아래쪽 순으로 넣어주세요!!!!!!!!")]
        [SerializeField]
        private RectTransform[] _boxRects;
        [SerializeField]
        private Vector2[] _boxOriginPositions;
        [SerializeField]
        [Dependent]
        private bool _syncWithRects;

        private void OnValidate()
        {
            if(_syncWithRects)
            {
                for (int i = 0; i < 4; i++)
                {
                    _boxOriginPositions[i] = _boxRects[i].anchoredPosition;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < 4; i++)
            {
                _boxRects[i].anchoredPosition = _boxOriginPositions[i];
            }
        }

        private void Update()
        {
            if(Keyboard.current.oKey.wasPressedThisFrame)
            {
                Open();
            }
        }

        public void Open()
        {
            float offset = 0.05f;
            Vector2[] directions = Direction2D.GetDirections(DirectionType.Left, DirectionType.Down, DirectionType.Right, DirectionType.Up);
            for (int i = 0; i < 4; i++)
            {
                Vector2 destination = directions[i] * 1500f;
                _openSequence.Insert(offset * i, _boxRects[i].DOAnchorPosX(destination.x, 0.7f));
                _openSequence.Insert(offset * i + 0.2f, _boxRects[i].DOAnchorPosY(destination.y, 0.5f));
            }
        }

        public void Close()
        {
        }
    }
}
