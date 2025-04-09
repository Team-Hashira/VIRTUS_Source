using Hashira.Cards.Effects;
using Hashira.Items;
using System;
using System.Runtime.InteropServices;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira
{
    public abstract class ItemSO<T> : ScriptableObject where T : ItemEffect<T>
    {
        public Sprite sprite;
        public string displayName;
        public new string name;
        public virtual string Description { get; set; }

        public string className;
        private string _prevClassName;

        private Type _classType;
        [SerializeReference]
        private T _effectInstance;

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
                    string typeName = $"{typeof(T).Namespace}.{className}";
                    Type t = Type.GetType(typeName);
                    _classType = t;
                    _prevClassName = className;
                    CreateInstance();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"ItemSO의 className이 유효하지 않지롱 : {ex.Message}");
                }
            }
            if (_effectInstance == null)
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
            _effectInstance = Activator.CreateInstance(_classType) as T;
            ItemEffect<T> effect = _effectInstance as ItemEffect<T>;
            effect.Initialize(this);
        }

        public T GetEffectInstance()
        {
            if (_effectInstance == null)
            {
                CreateInstance();
                return default;
            }
            return _effectInstance;
        }
    }
}
