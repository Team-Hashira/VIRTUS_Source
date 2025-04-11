using DG.Tweening;
using Hashira.CanvasUI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira
{
    public class CardRerollButton : UIBase, IClickableUI
    {
        [SerializeField] private int _rerollCost = 10;

        private UseableCardDrawer _useableCardDrower;
        [SerializeField] private TextMeshProUGUI _rerollCostText, _currentCostText;
        [SerializeField] private CustomButton _customButton;
        [SerializeField] private Image _icon;

        private Sequence _iconRollSeq;

        protected override void Awake()
        {
            _customButton.OnHoverEvent += HandleHoverEvent;
        }

        private void HandleHoverEvent(bool isEnter)
        {
            if (isEnter)
            {
                if (_iconRollSeq != null && _iconRollSeq.IsActive()) _iconRollSeq.Kill();
                _iconRollSeq = DOTween.Sequence();
                _iconRollSeq.Append(_icon.transform.DOLocalRotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo));
            }
            else
            {
                if (_iconRollSeq != null && _iconRollSeq.IsActive()) _iconRollSeq.Kill();
                _iconRollSeq = DOTween.Sequence();
                _iconRollSeq.Append(_icon.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo));
            }
        }

        public void SetCardDrower(UseableCardDrawer useableCardDrower)
        {
            _useableCardDrower = useableCardDrower;

            _rerollCostText.text = $"{_rerollCost}";
            Cost.OnCostChangedEvent += CostTextUpdate;
            CostTextUpdate(Cost.CurrentCost);
        }

        private void CostTextUpdate(int cost)
        {
            _currentCostText.text = $"{cost}";
        }

        public void OnClick(bool isLeft)
        {
            if (isLeft == false) return;

            if (Cost.TryRemoveCost(_rerollCost))
                _useableCardDrower.CardDraw();
            else
                PopupTextManager.Instance.PopupText("코스트가 부족합니다.", Color.red);
        }

        public void OnClickEnd(bool isLeft)
        {

        }

        private void OnDestroy()
        {
            if (_iconRollSeq != null && _iconRollSeq.IsActive()) _iconRollSeq.Kill();
        }
    }
}
