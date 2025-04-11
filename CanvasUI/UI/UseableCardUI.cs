using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Cards;
using Hashira.Cards.Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class UseableCardUI : SetupCardVisual, IClickableUI, IHoverableUI
    {
        private readonly static int _GlitchValueHash = Shader.PropertyToID("_Value");

        private UseableCardDrawer _useableCardDrawer;
        private Image _cardImage;
        private CanvasGroup _canvasGroup;

        [Header("=====UseableCardUI setting=====")]
        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private CustomButton _useButton, _cancelButton;
        [field: SerializeField] public ChildrenMaterialController ChildrenMaterialController { get; private set; }
        [SerializeField] private float _useDistance = 14;
        private float _useDistanceSqr;

        private bool _isSelected;
        private bool _isFixationCard;
        private Vector2 _targetScale;

        private Sequence _useSeq;

        private int _needCost;

        private void Awake()
        {
            _cardImage = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _useDistanceSqr = _useDistance * _useDistance;

            _useButton.OnClickEvent += CardUse;
            _cancelButton.OnClickEvent += () => ActiveSelectMode(false);
            ActiveSelectMode(false, false);

            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
            PlayerDataManager.Instance.EffectAddedEvent += HandleEffectAddedEvent;

            Vector2 anchor = new Vector2(0.5f, 0.5f);
            RectTransform.anchorMin = anchor;
            RectTransform.anchorMax = anchor;
        }

        private void ActiveSelectMode(bool active, bool setCardDrawer = true)
        {
            _isSelected = active;
            _useButton.gameObject.SetActive(active);
            _cancelButton.gameObject.SetActive(active);
            if (active)
            {
                _targetScale = Vector3.one * 1.6f;
                if (setCardDrawer) _useableCardDrawer.CardSelect(this);
            }
            else
            {
                _targetScale = Vector3.one;
                if (setCardDrawer) _useableCardDrawer.CardSelectCancel(this);
            }
        }

        private void LockToggle()
        {
            if (_isFixationCard)
            {
                CardManager.Instance.UnFixationCard(this);
                OnCursorEnter();
            }
            else
            {
                CardManager.Instance.FixationCard(this);
            }
            _useableCardDrawer.CardLockMode(true);
        }
        public void OnLockMode(bool active)
        {
            if (active)
            {
                int fixedCardCount = CardManager.Instance.FixedCardList.Count;
                if (_isFixationCard)
                {
                    _costText.text = CardManager.Instance.FixedCardNeedCost[fixedCardCount - 1].ToString();
                    _costText.color = Color.blue;
                }
                else
                {
                    _costText.text = CardManager.Instance.FixedCardNeedCost[fixedCardCount].ToString();
                    _costText.color = Color.red;
                }
            }
            else
            {
                VisualSetup(CardSO);
                _costText.color = Color.black;
            }
        }

        private void Update()
        {
            SelectZoonIn();

            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * 10);
        }

        private void SelectZoonIn()
        {
            if (_isSelected == false) return;

            RectTransform.anchoredPosition = Vector3.Lerp(RectTransform.anchoredPosition, new Vector2(0, 100), Time.deltaTime * 10);
        }

        public void SetCard(UseableCardDrawer useableCardDrawer)
        {
            _useableCardDrawer = useableCardDrawer;
            _needCost = CardSO.needCost + PlayerDataManager.Instance.GetAdditionalNeedCost(CardSO);
            SetFixationCard(false);
        }

        private void CardUse()
        {
            ActiveSelectMode(false, false);
            if (Cost.TryRemoveCost(_needCost))
            {
                PlayerDataManager.Instance.AddEffect(CardSO.GetEffectInstance<CardEffect>());

                _useableCardDrawer.CardSelectCancel(null);
                _useableCardDrawer.CardDraw();
                if (_useSeq != null && _useSeq.IsActive()) _useSeq.Kill();
                _useSeq = DOTween.Sequence();
                _useSeq.Append(DOTween.To(() => 0f, value => ChildrenMaterialController.SetValue(_GlitchValueHash, value), 1f, 0.05f).SetEase(Ease.OutQuad));
                _useSeq.Append(DOTween.To(() => 1f, value => _canvasGroup.alpha = value, 0f, 0.05f).SetEase(Ease.InSine));
                _useSeq.AppendCallback(() => Destroy(gameObject));
            }
            else
            {
                PopupTextManager.Instance.PopupText("코스트가 부족합니다.", Color.red);
                _useableCardDrawer.CardSelectCancel(this);
                transform.SetParent(_useableCardDrawer.transform);
            }
        }

        public void OnClick(bool isLeft)
        {
            if (_useableCardDrawer.IsLockMode)
            {
                LockToggle();
                return;
            }

            if (_isFixationCard) return;
            if (_isSelected) return;

            if (isLeft)
            {
                ActiveSelectMode(true);
            }
        }

        public void SetFixationCard(bool isOn)
        {
            if (isOn)
            {
                _targetScale = Vector3.one;
            }
            _isFixationCard = isOn;
            _cardImage.color = isOn ? Color.gray : Color.white;
        }

        public void OnClickEnd(bool isLeft)
        {
        }

        public void OnCursorEnter()
        {
            if (_isFixationCard) return;
            if (_isSelected) return;

            _targetScale = Vector3.one * 1.1f;
        }

        public void OnCursorExit()
        {
            if (_isFixationCard) return;
            if (_isSelected) return;

            _targetScale = Vector3.one;
        }

        private void HandleEffectAddedEvent(CardEffect effect)
        {
            VisualSetup(CardSO);
            _needCost = CardSO.needCost + PlayerDataManager.Instance.GetAdditionalNeedCost(CardSO);
        }

        private void OnDestroy()
        {
            PlayerDataManager.Instance.EffectAddedEvent -= HandleEffectAddedEvent;
        }
    }
}
