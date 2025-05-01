using DG.Tweening;
using Hashira.Combat;
using Hashira.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities
{
    public class Entity : MonoBehaviour, IAttackable
    {
        private Dictionary<Type, IEntityComponent> _componentDict;

        private Tween _rotateTween;

        protected virtual void Awake()
        {
            _componentDict = new Dictionary<Type, IEntityComponent>();

            AddComponentsToDictionary();
            InitializeComponent();
            AfterIntiialize();
        }

        protected virtual void AddComponentsToDictionary()
        {
            GetComponentsInChildren<IEntityComponent>().ToList()
                .ForEach(component => _componentDict.Add(component.GetType(), component));
        }

        protected virtual void InitializeComponent()
        {
            _componentDict.Values.ToList()
                .ForEach(component => component.Initialize(this));
        }

        protected virtual void AfterIntiialize()
        {
            _componentDict.Values.OfType<IAfterInitialzeComponent>().ToList()
                        .ForEach(component => component.AfterInit());
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnDestroy()
        {
            GetComponentsInChildren<IEntityDisposeComponent>().ToList()
                .ForEach(component => component.Dispose());
        }

        public T GetEntityComponent<T>() where T : class, IEntityComponent
        {
            if (_componentDict.TryGetValue(typeof(T), out IEntityComponent compo))
            {
                return compo as T;
            }

            Type findType = _componentDict.Keys.FirstOrDefault(x => x.IsSubclassOf(typeof(T)));
            if (findType != null)
                return _componentDict[findType] as T;

            return default;
        }

        public bool TryGetEntityComponent<T>(out T component, bool isDerived = false) where T : class, IEntityComponent
        {
            component = null;
            if (_componentDict.TryGetValue(typeof(T), out IEntityComponent compo))
            {
                component = compo as T;
                return true;
            }

            if (!isDerived) return false;


            Type findType = _componentDict.Keys.FirstOrDefault(x => x.IsSubclassOf(typeof(T)));
            if (findType != null)
            {
                component = _componentDict[findType] as T;
                return true;
            }

            return false;
        }

        public void Rotate(float value, float duration = 0.25f, RotateMode rotateMode = RotateMode.Fast, Ease ease = Ease.OutBounce)
        {
            _rotateTween?.Kill();
            _rotateTween = transform.DORotate(new Vector3(0, 0, value), duration, rotateMode).SetEase(ease);
        }
    }
}