using DG.Tweening;
using Hashira.CanvasUI;
using TMPro;
using UnityEngine;

namespace Hashira.UI
{
    public class CostUI : UIBase, IToggleUI
    {
        public string Key { get; set; } = nameof(CostUI);

        [SerializeField] private TextMeshProUGUI _costText;

        private Sequence _costTextSequence;
        
        private void Start()
        {
            _costText.text = Cost.CurrentCost.ToString();
            Cost.OnCostChangedEvent += OnCostChangedHandle;
        }

        private void OnDestroy()
        {
            _costTextSequence?.Kill();
            Cost.OnCostChangedEvent -= OnCostChangedHandle;
        }

        private void OnCostChangedHandle(int currentCost)
        {
            _costTextSequence?.Kill();
            _costTextSequence = DOTween.Sequence();
            _costTextSequence
                .Append(_costText.rectTransform.DOAnchorPosY(10f, 0.1f))
                .Append(_costText.rectTransform.DOAnchorPosY(0, 0.1f));
            _costText.text = currentCost.ToString();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
