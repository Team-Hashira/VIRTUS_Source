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
        [SerializeField] protected TMP_Text _keyText, _nameText;
        [SerializeField] protected InputReaderSO _inputReader;
        [SerializeField] protected SpriteRenderer _itemSprite;
        [SerializeField] protected SpriteRenderer _holdOutlineSprite;

        protected Material _holdOutlineMat;

        public event Action OnInteractionEvent;

        public bool CanInteraction { get; set; } = true;

        protected virtual void Awake()
        {
            _keyGuideObject.SetActive(false);
            _holdOutlineMat = _holdOutlineSprite.material;
            _holdOutlineMat.SetFloat(_FillAmountShaderHash, 0);
        }

        public virtual void Interaction(Player entity)
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
