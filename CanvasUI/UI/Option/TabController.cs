using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI.Option
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private OptionTab[] _tabs;
        [SerializeField] private CustomButton[] _tabBtns;
        [SerializeField] private TextMeshProUGUI[] _tabBtnTexts;

        private int _currentOptionTabIndex;

        private Color _defaultButtonTextColor;

        public void Init()
        {
            // 현재 데이터를 설정창에 반영
            foreach (OptionTab optionTab in _tabs)
            {
                optionTab.Init();
            }

            // 버튼-탭 연결
            for (int i = 0; i < _tabBtns.Length; i++)
            {
                int index = i;
                _tabBtns[i].OnClickEvent += () => SetOptionTap(index);
            }
        }

        public void SaveData()
        {
            for (int i = 0; i < _tabs.Length; i++)
            {
                _tabs[i].SaveData();
            }
        }

        private void Start()
        {
            _currentOptionTabIndex = 0;
            _tabs[0].Open();
            _tabBtnTexts = new TextMeshProUGUI[_tabBtns.Length];
            for (int i = 0; i < _tabBtns.Length; i++)
            {
                _tabBtnTexts[i] = _tabBtns[i].GetComponentInChildren<TextMeshProUGUI>();
            }
            _defaultButtonTextColor = _tabBtnTexts[0].color;
            _tabBtnTexts[0].color = Color.white;
        }

        private void SetOptionTap(int optionTabIndex)
        {
            _tabs[_currentOptionTabIndex].Close();
            _tabBtnTexts[_currentOptionTabIndex].color = _defaultButtonTextColor;
            _currentOptionTabIndex = optionTabIndex;
            _tabBtnTexts[_currentOptionTabIndex].color = Color.white;
            _tabs[_currentOptionTabIndex].Open();
            _title.text = _tabs[_currentOptionTabIndex].GetName();
        }
    }
}
