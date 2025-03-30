using Hashira.CanvasUI;
using Hashira.CanvasUI.Option;
using Hashira.Core;
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
            TimeController.UndoTimeScale();
            PlayerDataManager.Instance.ResetData();
            SceneLoadingManager.LoadScene(SceneName.TitleScene);
        }

        public void Close()
        {
            IsOpened = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            TimeController.UndoTimeScale();
            _inputReader.PlayerActive(true);
        }

        public void Open()
        {
            IsOpened = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            TimeController.SetTimeScale(0);
            _inputReader.PlayerActive(false);
        }
    }
}
