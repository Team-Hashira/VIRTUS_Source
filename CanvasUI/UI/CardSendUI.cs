using Hashira.Cards;
using Hashira.StageSystem;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class CardSendUI : MonoBehaviour
    {
        [SerializeField] private CardScrollView _cardSpreader;
        [SerializeField] private CustomButton _cardSendBtn, _nextFloorBtn;
        [SerializeField] private TextMeshProUGUI _floorText, _needCost;

        private List<CardSO> _sendCardList;
        private List<SetupCardVisual> _setupCardVisualList;

        [SerializeField] private int _defaultNeedCost;
        [SerializeField] private int _addNeedCost;
        private int _currentNeedCost;
        private int _lastRandomIndex;

        private void Start()
        {
            _currentNeedCost = _defaultNeedCost;
            UpdateCardSendCostText(_currentNeedCost);
            _sendCardList = new List<CardSO>();

            List<CardSO> cardList
                = PlayerDataManager.Instance.CardEffectList.Select(cardEffect => cardEffect.CardSO).ToList();
            _cardSpreader.CreateCard(cardList, true);
            _setupCardVisualList = _cardSpreader.GetCardList<SetupCardVisual>();
            _lastRandomIndex = _setupCardVisualList.Count;

            _cardSendBtn.OnClickEvent += HandleCardSendEvent;
            _nextFloorBtn.OnClickEvent += HandleNextFloorEvent;

            int floor = StageGenerator.currentFloorIdx;
            _floorText.text = $"{floor - 1}  >>  {floor}";
        }

        public void UpdateCardSendCostText(int cost)
        {
            _needCost.text = $"{Cost.CurrentCost}/{cost}";
            _needCost.color = Cost.CurrentCost < cost ? Color.red : Color.white;
        }

        private void HandleCardSendEvent()
        {
            if (_lastRandomIndex >= 0 && Cost.TryRemoveCost(_currentNeedCost))
            {
                _currentNeedCost += _addNeedCost;
                UpdateCardSendCostText(_currentNeedCost);
                int randomIndex = UnityEngine.Random.Range(0, _lastRandomIndex);
                SetupCardVisual selectedSetupCardVisual = _setupCardVisualList[randomIndex];
                selectedSetupCardVisual.CardBorderImage.color = Color.yellow;
                _sendCardList.Add(selectedSetupCardVisual.CardSO);

                SetupCardVisual temp = _setupCardVisualList[randomIndex];
                _setupCardVisualList[randomIndex] = _setupCardVisualList[_lastRandomIndex - 1];
                _setupCardVisualList[_lastRandomIndex - 1] = temp;
                _lastRandomIndex--;
            }
        }

        private void HandleNextFloorEvent()
        {
            PlayerDataManager.Instance.ResetPlayerCardEffect(_sendCardList);
            SceneLoadingManager.LoadScene(SceneName.GameScene);
        }

        private void OnDestroy()
        {
            _cardSendBtn.OnClickEvent -= HandleCardSendEvent;
            _nextFloorBtn.OnClickEvent -= HandleNextFloorEvent;
        }
    }
}
