using Hashira.CanvasUI;
using Hashira.CanvasUI.Option;
using Hashira.Core;
using Hashira.Players;
using UnityEngine;

namespace Hashira
{
    public class PauseMenuPanel : UIBase, IToggleUI
    {
        [SerializeField] private InputReaderSO _inputReader;
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private CustomButton _gamePlayBtn;
        [SerializeField] private CustomButton _optionBtn;
        [SerializeField] private CustomButton _titleBtn;

        private CanvasGroup _canvasGroup;

        public bool IsOpened { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _gamePlayBtn.OnClickEvent += HandleGamePlayEvent;
            _optionBtn.OnClickEvent += HandleOptionEvent;
            _titleBtn.OnClickEvent += HandleTitleEvent;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            Close();
        }

        private void HandleGamePlayEvent()
        {
            Close();
        }

        private void HandleOptionEvent()
        {
            Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>().OpenUI("Option");
        }

        private void HandleTitleEvent()
        {
            PlayerDataManager.Instance.ResetData();
            PlayerDataManager.Instance.ResetPlayerCardEffect(useDisable: PlayerManager.Instance != null);
            SceneLoadingManager.LoadScene(SceneName.TitleScene);
        }

        public void Close()
        {
            IsOpened = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            Hashira.CanvasUI.UIManager.Instance.RemovePauseMenu(this);
        }

        public void Open()
        {
            IsOpened = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            Hashira.CanvasUI.UIManager.Instance.AddPauseMenu(this);
        }
    }
}
