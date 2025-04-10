using TMPro;
using UnityEngine;

namespace Hashira.UI
{
    public class CostUI : MonoBehaviour
    {
        private RectTransform _rectTransform;
        
        [SerializeField] private TextMeshProUGUI _costText;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        private void Start()
        {
            _costText.text = Cost.CurrentCost.ToString();
            Cost.OnCostChangedEvent += OnCostChangedHandle;
        }

        private void OnDestroy()
        {
            Cost.OnCostChangedEvent -= OnCostChangedHandle;
        }

        private void OnCostChangedHandle(int currentCost)
        {
            _costText.text = currentCost.ToString();
        }
    }
}
