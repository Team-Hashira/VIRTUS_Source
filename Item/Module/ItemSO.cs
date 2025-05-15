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
        public abstract string Description { get; }

        public string className;

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
            if (_effectInstance == null || _effectInstance.GetType().Name != className)
            {
                try
                {
                    string typeName = $"{GetType().Namespace}.Effects.{className}";
                    Type t = Type.GetType(typeName);
                    _classType = t;
                    CreateInstance();
                }
                catch (Exception ex)
                {
                    _effectInstance = null;
                    Debug.LogError($"ItemSO의 className이 유효하지 않지롱 : {ex.Message}");
                }
            }
            if (_classType == null || _classType != _effectInstance?.GetType())
                _classType = _effectInstance?.GetType();
            _effectInstance?.Initialize(this);

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
