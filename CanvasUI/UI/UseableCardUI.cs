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
        private CanvasGroup _canvasGroup;

        [Header("=====UseableCardUI setting=====")]
        [SerializeField] private Image _lockImage;
        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private float _useDistance = 14;
        [SerializeField] private Image _lockIcon;
        private float _useDistanceSqr;

        public int Index { get; private set; }

        private bool _isSelected;
        private bool _isFixationCard;
        private Vector2 _targetScale;

        private Sequence _useSeq;

        private int _needCost;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _useDistanceSqr = _useDistance * _useDistance;


            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
            PlayerDataManager.Instance.EffectAddedEvent += HandleEffectAddedEvent;

            Vector2 anchor = new Vector2(0.5f, 0.5f);
            RectTransform.anchorMin = anchor;
            RectTransform.anchorMax = anchor;

            ActiveLockIcon(false);
            _targetScale = Vector3.one;
        }

        public void ActiveLockIcon(bool active)
        {
            _lockIcon.gameObject.SetActive(active);
        }

        public void ShakeLockIcon()
        {
            _lockIcon.transform.DOKill();
            _lockIcon.transform.DOShakePosition(0.25f, 12f, 20).SetEase(Ease.OutCubic);
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

        public void SetCard(UseableCardDrawer useableCardDrawer, int index)
        {
            Index = index;
            _useableCardDrawer = useableCardDrawer;
            _needCost = PlayerDataManager.Instance.GetCardNeedCost(CardSO);
            SetFixationCard(false);
        }

        public void CardUse()
        {
            if (Cost.TryRemoveCost(_needCost))
            {
                PlayerDataManager.Instance.AddEffect(CardSO);

                _useableCardDrawer.CardSelectCancel(null);
                _useableCardDrawer.CardDraw();
                if (_useSeq != null && _useSeq.IsActive()) _useSeq.Kill();
                _useSeq = DOTween.Sequence();
                _useSeq.Append(DOTween.To(() => 0f, value => MaterialController.SetValue(_GlitchValueHash, value), 1f, 0.05f).SetEase(Ease.OutQuad));
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

        public void ActiveSelectMode(bool active)
        {
            _targetScale = active ? Vector3.one * 1.6f : Vector3.one;
            _isSelected = active;
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
                _useableCardDrawer.CardSelect(this);
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
            _lockImage.color = new Color(0, 0, 0, isOn ? 0.5f : 0f);
        }

        public void OnClickEnd(bool isLeft)
        {
        }

        public void OnCursorEnter()
        {
            if (_isFixationCard) return;
            if (_isSelected) return;

            if (_useableCardDrawer.IsLockMode)
            {
                ShakeLockIcon();
                ActiveLockIcon(true);
            }

            _targetScale = Vector3.one * 1.1f;
        }

        public void OnCursorExit()
        {
            if (_isFixationCard) return;
            if (_isSelected) return;

            if (_useableCardDrawer.IsLockMode)
                ActiveLockIcon(false);

            _targetScale = Vector3.one;
        }

        private void HandleEffectAddedEvent(CardEffect effect)
        {
            VisualSetup(CardSO);
            _needCost = PlayerDataManager.Instance.GetCardNeedCost(CardSO);
        }

        private void OnDestroy()
        {
            _useSeq?.Kill(true);
            if (PlayerDataManager.Instance != null) PlayerDataManager.Instance.EffectAddedEvent -= HandleEffectAddedEvent;
        }
    }
}
