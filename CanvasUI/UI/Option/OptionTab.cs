using System.Collections.Generic;
using UnityEngine;

namespace Hashira.CanvasUI.Option
{
    public abstract class OptionTab : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }
        [SerializeField] public string _tapName;

        protected List<OptionSetterBase> _optionSetterList;


        public virtual void Init()
        {
            _optionSetterList = new List<OptionSetterBase>();
            GetComponentsInChildren(_optionSetterList);
            foreach (var setter in _optionSetterList)
                setter.Init();
            Close();
        }

        public abstract void SaveData();

        public string GetName()
        {
            return _tapName;
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            foreach (var setter in _optionSetterList)
                setter.Active(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            foreach (var setter in _optionSetterList)
                setter.Active(false);
        }
    }
}
