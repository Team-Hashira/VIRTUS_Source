using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<EPopupUIName, List<IPopupUI>> _popupUIDictionary = new();

        [SerializeField] private Transform _stagePanelTrm;

        private void Awake()
        {
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPopupUI>().ToList().ForEach(popupUI =>
            {
                if (_popupUIDictionary.ContainsKey(popupUI.PopupUIName))
                    _popupUIDictionary[popupUI.PopupUIName].Add(popupUI);
                else
                    _popupUIDictionary.Add(popupUI.PopupUIName, new List<IPopupUI>() { popupUI });
            });
        }

        public void AddGameCanvas(Transform panel)
        {
            panel.SetParent(_stagePanelTrm);
        }

        public List<IPopupUI> PopupUIActive(EPopupUIName popupUI, bool isOn)
        {
            if (isOn) _popupUIDictionary[popupUI].ForEach(popupUI => popupUI.Show());
            else _popupUIDictionary[popupUI].ForEach(popupUI => popupUI.Hide());

            return _popupUIDictionary[popupUI];
        }
        public List<T> PopupUIActive<T>(EPopupUIName popupUI, bool isOn)
        {
            if (isOn) _popupUIDictionary[popupUI].ForEach(popupUI => popupUI.Show());
            else _popupUIDictionary[popupUI].ForEach(popupUI => popupUI.Hide());

            return _popupUIDictionary[popupUI].OfType<T>().ToList();
        }
    }
}
