using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Cards;
using Hashira.Cards.Effects;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class UseableCardUI : UIBase, IClickableUI, IHoverableUI, IPoolingObject
    {
        private readonly static int _GlitchValueHash = Shader.PropertyToID("_Value");

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        public CardSO CardSO { get; private set; }

        private UseableCardDrawer _useableCardDrawer;
        private Image _cardImage;
        private CanvasGroup _canvasGroup;

        [SerializeField] private InputReaderSO _inputReader;
        [SerializeField] private CustomButton _lockButton;
        [SerializeField] private SetupCardVisual _setupCardVisual;
        [field: SerializeField] public ChildrenMaterialController ChildrenMaterialController { get; private set; }
        [SerializeField] private float _useDistance = 14;
        private float _useDistanceSqr;

        private bool _isDrag;
        private bool _isUseable;
        private bool _isFixationCard;
        private Vector2 _mousePivot;

        private Sequence _useSeq;

        private int _needCost;

        protected override void Awake()
        {
            base.Awake();
            _cardImage = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _useDistanceSqr = _useDistance * _useDistance;

            _lockButton.OnClickEvent += HandleClickEvent;
        }

        private void HandleClickEvent()
        {
            if (_isFixationCard)
                CardManager.Instance.UnFixationCard(this);
            else
                CardManager.Instance.FixationCard(this);
        }

        private void Update()
        {
            MouseTracking();

            Vector3 targetPos = Camera.main.ScreenToWorldPoint(_useableCardDrawer.CardUsePos.position);
            Vector3 myPos = Camera.main.ScreenToWorldPoint(transform.position);
            float distance = (targetPos - myPos).sqrMagnitude;
            bool isUseableDistance = distance < _useDistanceSqr;
            if (_isUseable == false && isUseableDistance)
            {
                _isUseable = true;
                if (_needCost <= Cost.CurrentCost)
                    _cardImage.color = Color.yellow;
                else
                    _cardImage.color = Color.red;
            }
            else if (_isUseable == true && isUseableDistance == false)
            {
                _isUseable = false;
                _cardImage.color = Color.white;
            }
        }

        private void MouseTracking()
        {
            if (_isDrag == false) return;

            RectTransform.position = _inputReader.MousePosition + _mousePivot;
        }

        public void SetCard(CardSO cardSO, UseableCardDrawer useableCardDrawer)
        {
            CardSO = cardSO;
            _useableCardDrawer = useableCardDrawer;
            _setupCardVisual.Setup(cardSO);
            _needCost = CardSO.needCost + PlayerDataManager.Instance.GetAdditionalNeedCost(CardSO);
            SetFixationCard(false);
        }

        private void CardUse()
        {
            _useableCardDrawer.OnCardUsed(CardSO);
            if (_useSeq != null && _useSeq.IsActive()) _useSeq.Kill();
            _useSeq = DOTween.Sequence();
            _useSeq.Append(DOTween.To(() => 0f, value => ChildrenMaterialController.SetValue(_GlitchValueHash, value), 1f, 0.05f).SetEase(Ease.OutQuad));
            _useSeq.Append(DOTween.To(() => 1f, value => _canvasGroup.alpha = value, 0f, 0.05f).SetEase(Ease.InSine));
            _useSeq.AppendCallback(() => this.Push());
        }

        public void OnClick(bool isLeft)
        {
            if (_isDrag) return;

            if (_isFixationCard == false && isLeft)
            {
                _useableCardDrawer.ExitSpread(this);
                //_useableCardDrawer.CardUseHint.enabled = true;
                _isDrag = true;
                _lockButton.gameObject.SetActive(false);
                _mousePivot = (Vector2)RectTransform.position - _inputReader.MousePosition;
                transform.SetParent(_useableCardDrawer.DragCardTrm);
            }
        }

        public void SetFixationCard(bool isOn)
        {
            _isFixationCard = isOn;
            _cardImage.color = isOn ? Color.gray : Color.white;
            if (isOn) transform.localScale = Vector3.one;
        }

        public void OnClickEnd(bool isLeft)
        {
            if (_isDrag == false) return;
            if (isLeft)
            {
                //_useableCardDrawer.CardUseHint.enabled = false;
                _isDrag = false;
                _lockButton.gameObject.SetActive(true);

                if (_isUseable)
                {
                    if (Cost.TryRemoveCost(_needCost))
                    {
                        PlayerDataManager.Instance.AddEffect(CardSO.GetEffectInstance());
                        CardUse();
                    }
                    else
                    {
                        PopupTextManager.Instance.PopupText("코스트가 부족합니다.", Color.red);
                        _useableCardDrawer.EnterSpread(this);
                        transform.SetParent(_useableCardDrawer.transform);
                    }
                }
                else
                {
                    _useableCardDrawer.EnterSpread(this);
                    transform.SetParent(_useableCardDrawer.transform);
                }
            }

        }

        public void OnCursorEnter()
        {
            if (_isFixationCard) return;

            transform.localScale = Vector3.one * 1.1f;
        }

        public void OnCursorExit()
        {
            if (_isFixationCard) return;

            transform.localScale = Vector3.one;
        }

        public void OnPop()
        {
            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
            PlayerDataManager.Instance.EffectAddedEvent += HandleEffectAddedEvent;
        }

        private void HandleEffectAddedEvent(CardEffect effect)
        {
            _setupCardVisual.Setup(CardSO);
            _needCost = CardSO.needCost + PlayerDataManager.Instance.GetAdditionalNeedCost(CardSO);
        }

        public void OnPush()
        {
            PlayerDataManager.Instance.EffectAddedEvent -= HandleEffectAddedEvent;
        }
    }
}
