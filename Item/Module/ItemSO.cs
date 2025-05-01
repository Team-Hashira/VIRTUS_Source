using Hashira.Items;
using System;
using UnityEngine;

namespace Hashira.Items
{
    public abstract class ItemSO : ScriptableObject
    {
        public Sprite sprite;
        public string displayName;
        public new string name;
        public virtual string Description { get; set; }

        public string className;
        private string _prevClassName;

        private Type _classType;
        [Space]
        [Header("ItemEffect의 객체. 아무것도 안뜨면 직렬화 가능한 변수가 없다는 뜻")]
        [SerializeReference]
        private ItemEffect _effectInstance;

        private void OnValidate()
        {
            CachingTypeAndInstance();
        }

        private void CachingTypeAndInstance()
        {
            if (_prevClassName != className || _classType == null)
            {
                try
                {
                    string typeName = $"{GetType().Namespace}.Effects.{className}";
                    Type t = Type.GetType(typeName);
                    _classType = t;
                    _prevClassName = className;
                    CreateInstance();
                }
                catch (Exception ex)
                {
                    _effectInstance = null;
                    Debug.LogError($"ItemSO의 className이 유효하지 않지롱 : {ex.Message}");
                }
            }
            if (_effectInstance == null && _classType != null)
                CreateInstance();

        }

        public Type GetEffectType()
        {
            if (_classType == null)
                CachingTypeAndInstance();
            return _classType;
        }

        private void CreateInstance()
        {
            _effectInstance = Activator.CreateInstance(_classType) as ItemEffect;
            _effectInstance.Initialize(this);
        }

        public T GetEffectInstance<T>() where T : ItemEffect
        {
            if (_effectInstance == null)
            {
                CreateInstance();
            }
            else if (_effectInstance.ItemSO == null)
            {
                _effectInstance.Initialize(this);
            }
            return _effectInstance as T;
        }
    }
}
