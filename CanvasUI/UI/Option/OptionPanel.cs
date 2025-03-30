using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI.Option
{
    public class OptionPanel : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private TabController _tapController;
        [SerializeField] private Button _closeBtn;

        private CanvasGroup _canvasGroup;

        public bool IsOpened { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _closeBtn?.onClick.AddListener(() =>
            {
                _tapController.SaveData();
                UIManager.Instance.GetDomain<ToggleDomain>().CloseUI("Option");
            });
            _canvasGroup = GetComponent<CanvasGroup>();

            OptionData.Init(); // 이전 저장값 불러오기
            _tapController.Init(); // 현재 불러와진 데이터로 설정창 세팅하기
        }

        private void Start()
        {
            Close();
        }

        public void SaveData() => _tapController.SaveData();

        public void Open()
        {
            IsOpened = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Close()
        {
            IsOpened = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
