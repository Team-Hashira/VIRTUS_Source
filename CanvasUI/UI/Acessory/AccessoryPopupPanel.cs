using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI.Accessories
{
    public class AccessoryPopupPanel : UIBase, IToggleUI
    {
        [SerializeField] private Button[] _answerButtons;
        public string Key { get; set; }
        public int AnswerCount { get; private set; }
        private CanvasGroup _canvasGroup;
        private Sequence _activeSequence;
        public event Action<int> OnAnswerEvent; 
        
        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;

            for (int i = 0; i < AnswerCount; i++)
            {
                int index = i;
                _answerButtons[i].onClick.AddListener(() =>
                {
                    OnAnswerEvent?.Invoke(index);
                    this.Close();
                });
            }
        }

        public void Init(int answerCount)
        {
            AnswerCount = answerCount;
        }
        
        public void Open()
        {
            _activeSequence?.Kill(true);
            _activeSequence = DOTween.Sequence();
            _activeSequence
                .Append(_canvasGroup.DOFade(1, 0.5f))
                .AppendCallback(() =>
                {
                    _canvasGroup.blocksRaycasts = true;
                    _canvasGroup.interactable = true;
                });
        }

        public void Close()
        {
            _activeSequence?.Kill(true);
            _activeSequence = DOTween.Sequence();
            _activeSequence
                .AppendCallback(() =>
                {
                    _canvasGroup.blocksRaycasts = false;
                    _canvasGroup.interactable = false;
                })
                .Append(_canvasGroup.DOFade(0, 0.25f));

            foreach (var answerButton in _answerButtons)
                answerButton.onClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            _activeSequence?.Kill();
        }
    }
}
