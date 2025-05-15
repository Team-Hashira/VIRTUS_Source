using Hashira.Cards;
using Hashira.Cards.Effects;
using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI.Wells
{
    public class WellCardUpgradeView : MonoBehaviour
    {
        [SerializeField] private CustomButton _reSelectBtn, _upgradeBtn, _stopBtn;
        [SerializeField] private TextMeshProUGUI _cardUpgradeText, _percentText;

        private SelecrableCard _selecrableCard;
        private int _upgradeCost;

        private WellUI _wellUI;

        private int _percent = 80;

        private void Awake()
        {
            _reSelectBtn.OnClickEvent += HnadleReSelectEvent;
            _upgradeBtn.OnClickEvent += HnadleUpgradeEvent;
            _stopBtn.OnClickEvent += HandleStopEvent;
        }

        private void Start()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            UpdatePercentText();
        }

        private void HandleStopEvent()
        {
            _wellUI.EventEnd();
        }

        private void HnadleUpgradeEvent()
        {
            if (PlayerDataManager.Instance.IsMaxStackEffect(_selecrableCard.CardSO) == false)
            {
                if (Cost.TryRemoveCost(_upgradeCost))
                {
                    int random = Random.Range(0, 100);
                    if (random < _percent)
                    {
                        // 성공
                        PlayerDataManager.Instance.AddEffect(_selecrableCard.CardSO);
                        _selecrableCard.VisualSetup(_selecrableCard.CardSO);
                        _percent -= 10;
                        UpdatePercentText();
                    }
                    else
                    {
                        // 실패
                        PlayerDataManager.Instance.SetEffectStack(_selecrableCard.CardSO, 1);
                        _upgradeBtn.SetText("응답없음");
                        _upgradeBtn.ActiveHoverEvent(false);
                        _upgradeBtn.ActiveClickEvent(false);
                        _percentText.text = "강등";
                        _percentText.color = Color.red;
                    }
                    UpdateNeedCost();
                    _reSelectBtn.gameObject.SetActive(false);
                }
                if (PlayerDataManager.Instance.IsMaxStackEffect(_selecrableCard.CardSO))
                {
                    _upgradeBtn.SetText("최대중첩");
                    _upgradeBtn.ActiveHoverEvent(false);
                    _upgradeBtn.ActiveClickEvent(false);
                }
            }
        }

        public void UpdatePercentText()
        {
            _percentText.text = $"{_percent}%";
        }

        private void HnadleReSelectEvent()
        {
            Destroy(_selecrableCard.gameObject);
            transform.GetChild(0).gameObject.SetActive(false);
            _wellUI.Enable();
        }

        public void Init(SelecrableCard selecrableCard, WellUI wellUI)
        {
            _stopBtn.gameObject.SetActive(true);
            _wellUI = wellUI;
            _selecrableCard = selecrableCard;
            UpdateNeedCost();
        }

        public void UpdateNeedCost()
        {
            CardSO cardSO = _selecrableCard.CardSO;
            _upgradeCost = Mathf.CeilToInt(PlayerDataManager.Instance.GetCardNeedCost(cardSO) / 2);

            if (PlayerDataManager.Instance.IsMaxStackEffect(_selecrableCard.CardSO) || _percentText.color == Color.red)
            {
                _cardUpgradeText.text = $"";
            }
            else
            {
                _cardUpgradeText.text = $"{_upgradeCost}/{Cost.CurrentCost}";
            }
            _cardUpgradeText.color = _upgradeCost < Cost.CurrentCost ? Color.white : Color.red;
        }
    }
}
