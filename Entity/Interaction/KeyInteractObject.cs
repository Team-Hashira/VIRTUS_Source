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
			_keyGuideObject.SetActive(false);
        }

        public virtual void OnInteractable()
        {
			if (CanInteraction == false) return;
			_keyText.text = _inputReader.InteractKey;
            _keyGuideObject.SetActive(true);
        }
    }
}
