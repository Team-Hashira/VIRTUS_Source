using DG.Tweening;
using Hashira.Players;
using System;
using TMPro;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public abstract class KeyInteractObject : MonoBehaviour, IInteractable
    {
        protected readonly static int _FillAmountShaderHash = Shader.PropertyToID("_FillAmount");

        [Header("==========KeyInteractObject setting==========")]
        [SerializeField] protected GameObject _keyGuideObject;
        [SerializeField] protected TMP_Text _keyText;
        [SerializeField] protected InputReaderSO _inputReader;

        public event Action OnInteractionEvent;

        public bool CanInteraction { get; set; } = true;

        private Sequence _seq;

        protected virtual void Awake()
        {
            _keyGuideObject.SetActive(false);
        }

        public virtual void Interaction(Player player)
        {
            if (CanInteraction == false) return;
            OnInteractionEvent?.Invoke();
			OnInteractionEvent = null;
		}

        public virtual void OffInteractable()
        {
            _seq.Clear();
            _seq = DOTween.Sequence();
            _seq.Append(_keyGuideObject.transform.DOScale(new Vector3(1.2f, 0, 1), 0.1f).SetEase(Ease.InQuad))
                .AppendCallback(() => _keyGuideObject.SetActive(false));
        }

        public virtual void OnInteractable()
        {
			if (CanInteraction == false) return;
			_keyText.text = _inputReader.InteractKey;
            _keyGuideObject.SetActive(true);
            _seq.Clear();
            _seq = DOTween.Sequence();
            _seq.OnStart(() => _keyGuideObject.transform.localScale = new Vector3(1.2f, 0, 1))
                .Append(_keyGuideObject.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.OutBack));
        }

        protected virtual void OnDestroy()
        {
            _seq.Clear();
        }
    }
}
