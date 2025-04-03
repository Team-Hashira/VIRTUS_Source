using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.CanvasUI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private List<UIBase> _uiBaseList;

        private List<UIManagementDomain> _uiDomainList;
        private Dictionary<Type, UIManagementDomain> _uiDomainDict;

        [field: SerializeField]
        public Canvas MainCanvas { get; private set; }

        public static UIInteractor UIInteractor;

        public static Vector2 MousePosition;
        public static Vector2 WorldMousePosition;

        protected override void OnCreateInstance()
        {
            base.OnCreateInstance();
            _uiBaseList = new List<UIBase>();
            _uiDomainList = new List<UIManagementDomain>();
            _uiDomainDict = new Dictionary<Type, UIManagementDomain>();
        }

        private void Awake()
        {
            UIInteractor = FindFirstObjectByType<UIInteractor>();
        }

        private void Update()
        {
            MousePosition = Mouse.current.position.value;
            WorldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        }

        private void LateUpdate()
        {
            foreach (var doamin in _uiDomainList)
            {
                doamin.UpdateUI();
            }
        }

        public T GetDomain<T>(Type interfaceType) where T : UIManagementDomain
        {
            return _uiDomainDict[interfaceType] as T;
        }

        public T GetDomain<T>() where T : UIManagementDomain
        {
            return _uiDomainDict[typeof(T)] as T;
        }

        public T GetDomain<T, I>() where T : UIManagementDomain where I : IUserInterface
        {
            return _uiDomainDict[typeof(I)] as T;
        }

        public void AddUI(UIBase uiBase)
        {
            _uiBaseList.Add(uiBase);
            if (uiBase is IUserInterface ui)
            {
                foreach (Type interfaceType in uiBase.GetType().GetInterfaces())
                {
                    if (typeof(IUserInterface).IsAssignableFrom(interfaceType))
                    {
                        if (interfaceType == typeof(IUserInterface))
                            continue;
                        if (_uiDomainDict.TryGetValue(interfaceType, out var list))
                        {
                            _uiDomainDict[interfaceType].AddUI(ui);
                        }
                        else
                        {
                            string interfaceName = interfaceType.ToString();
                            int startIndex = interfaceName.IndexOf('I', 16);

                            string domainName =
                                interfaceName.Substring(startIndex + 1, interfaceName.IndexOf("UI", startIndex) - startIndex - 1);
                            Type domainType = Type.GetType($"{GetType().Namespace}.{domainName}Domain");
                            UIManagementDomain domain = Activator.CreateInstance(domainType) as UIManagementDomain;
                            _uiDomainList.Add(domain);
                            _uiDomainDict.Add(domainType, domain);
                            _uiDomainDict.Add(interfaceType, domain);
                            _uiDomainDict[interfaceType].AddUI(ui);
                        }
                    }
                }
            }
        }
        public void RemoveUI(UIBase uiBase)
        {
            _uiBaseList.Remove(uiBase);
            if (uiBase is IUserInterface ui)
            {
                foreach (Type interfaceType in uiBase.GetType().GetInterfaces())
                {
                    if (typeof(IUserInterface).IsAssignableFrom(interfaceType))
                    {
                        if (interfaceType == typeof(IUserInterface))
                            continue;
                        if (_uiDomainDict.TryGetValue(interfaceType, out var list))
                        {
                            _uiDomainDict[interfaceType].RemoveUI(ui);
                        }
                    }
                }
            }
        }
    }
}