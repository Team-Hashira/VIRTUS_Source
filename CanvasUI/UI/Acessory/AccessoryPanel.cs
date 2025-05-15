using DG.Tweening;
using Hashira.Accessories;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI.Accessories
{
    public class AccessoryPanel : UIBase, IToggleUI
    {
        public string Key { get; set; } = nameof(AccessoryPanel);
        private CanvasGroup _canvasGroup;

        private Sequence _activeSequence;

        [SerializeField] private AccessorySlot _newAccessorySlot;
        [SerializeField] private AccessorySlot[] _accessorySlots; 
        [SerializeField] private AccessoryDescriptionPanel _descriptionPanel;
        [SerializeField] private AccessoryPopupPanel _accessoryPopupPanel;
        
        [SerializeField] private Button _addPassiveAccessoryButton;
        [SerializeField] private Button _addActiveAccessoryButton;
        [SerializeField] private Button _accessoryDeleteButton;
        
        private bool _isFullSlot;
        
        private bool IsFullSlot()
        {
            bool isFull = true;

            foreach (var slot in _accessorySlots)
            {
                if (slot.Accessory != null)
                    continue;

                isFull = false;
                break;
            }
            
            _isFullSlot = isFull;
            
            return isFull;
        }

        private AccessorySlot GetEmptyAccessorySlot() => _accessorySlots.FirstOrDefault(x => x.Accessory == null);
        
        protected override void Awake()
        {
            base.Awake();
            //UIManager.Instance.AddUI(this);
            _canvasGroup = GetComponent<CanvasGroup>();

            // 안보이게
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;

            _addPassiveAccessoryButton.onClick.AddListener(HandleAddPassiveAccessory);
            _addActiveAccessoryButton.onClick.AddListener(HandleAddActiveAccessory);
            _accessoryDeleteButton.onClick.AddListener(HandleDeleteAccessory);
        }

        public void Init(AccessorySO newAccessory)
        {
            int index = 0;
            foreach (var accessory in Accessory.Accessories)
            {
                _accessorySlots[index].Init(accessory.AccessorySO);
                ++index;
            }
            
            _descriptionPanel.Init(newAccessory);
        }
        
        private void HandleAddPassiveAccessory() => SetAccessorySlot(EAccessoryType.Passive, _newAccessorySlot.Accessory);
        private void HandleAddActiveAccessory() => SetAccessorySlot(EAccessoryType.Active, _newAccessorySlot.Accessory);
        private void HandleDeleteAccessory()
        {
            Close();
        }
        
        private void SetAccessorySlot(EAccessoryType accessoryType, AccessorySO accessory)
        {
            if (_accessorySlots[(int)accessoryType - 1].Accessory == null)
            {
                _accessoryPopupPanel.Init(IsFullSlot() == true ? 1 : 2);    
                _accessoryPopupPanel.Open();
                _accessoryPopupPanel.OnAnswerEvent += HandleAnswerAccessoryPopup;
            }
            else
            {
                Accessory.EquipAccessory(accessoryType, _newAccessorySlot.Accessory);
                _accessorySlots[(int)accessoryType - 1].Init(accessory);
            }
        }

        private void ForceSetAccessorySlot(EAccessoryType accessoryType, AccessorySO accessory)
        {
            
        }
        
        private void HandleAnswerAccessoryPopup(int answerIndex)
        {
            // 슬롯이 꽉 차있을 때는 나올 수 있는 답이 하나 뿐임
            if (_isFullSlot == true)
            {
                //ForceSetAccessorySlot(); TODO
            }
            // 슬롯이 꽉 차있지 않을 때는 나올 수 있는 답이 두 개임
            else
            {
                if (answerIndex == 0)
                {
                    GetEmptyAccessorySlot().Init(_newAccessorySlot.Accessory);
                }
                else
                {
                    //ForceSetAccessorySlot(); TODO
                }    
            }
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
        }

        private void OnDestroy()
        {
            _activeSequence?.Kill(true);
        }
    }
}
