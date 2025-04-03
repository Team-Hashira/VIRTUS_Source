using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Hashira.Core.StatSystem
{
    public enum EModifyMode
    {
        Add,
        Percent,
    }
    
    public enum EModifyLayer
    {
        Default,
        Last,
    }

    public struct StatModifier
    {
        private float _originValue;
        private bool _canValueOverlap;

        private int _overlapCount;
        public EModifyMode Mode { get; private set; }
        public float Value { get; private set; }

        public StatModifier(float originValue, EModifyMode mode, bool canValueOverlap)
        {
            _originValue = originValue;
            Mode = mode;
            _overlapCount = 1;
            _canValueOverlap = canValueOverlap;

            Value = originValue;
        }

        public static StatModifier operator ++(StatModifier modifier)
        {
            modifier._overlapCount++;
            if (modifier._canValueOverlap)
                modifier.Value += modifier._originValue;
            return modifier;
        }
        public static StatModifier operator --(StatModifier modifier)
        {
            modifier._overlapCount--;
            if (modifier._canValueOverlap)
                modifier.Value -= modifier._originValue;
            return modifier;
        }
        public static implicit operator int(StatModifier modifier)
        {
            return modifier._overlapCount;
        }
    }

    [Serializable]
    public class StatElement : ICloneable
    {
        [HideInInspector] public string Name;
        public StatElementSO elementSO;
        [SerializeField] private float _baseValue;
        private Dictionary<EModifyLayer, Dictionary<string, StatModifier>> _modifiers;

        public event Action<float, float> OnValueChanged;
        public event Action<int, int> OnIntValueChanged;

        public float Value { get; private set; }
        public int IntValue { get; private set; }

        private bool _isUseClamp;
        private bool _isUseModifier;

        public void Initialize(bool isUseClamp = true, bool isUseModifier = true)
        {
            _isUseClamp = isUseClamp;
            _isUseModifier = isUseModifier;
            
            SetDictionary();
            SetValue();
        }

        private void SetDictionary()
        {
            _modifiers ??= new Dictionary<EModifyLayer, Dictionary<string, StatModifier>>()
            {
                { EModifyLayer.Default, new Dictionary<string, StatModifier>() },
                { EModifyLayer.Last, new Dictionary<string, StatModifier>() },
            };
        }

        private void SetValue()
        {
            float value = _baseValue;
            if (_isUseModifier)
            {

                foreach (var modifier in _modifiers.Values)
                {
                    float totalAddModifier = 0;
                    float totalPercentModifier = 0;
                    foreach (var statModifier in modifier.Values)
                    {
                        switch (statModifier.Mode)
                        {
                            case EModifyMode.Add:
                                totalAddModifier += statModifier.Value;
                                break;
                            case EModifyMode.Percent:
                                totalPercentModifier += statModifier.Value;
                                break;
                        }
                    }
                    value = (value + totalAddModifier) * (1 + totalPercentModifier / 100);
                }
            }

            if (elementSO != null && _isUseClamp)
                value = Mathf.Clamp(value, elementSO.minMaxValue.x, elementSO.minMaxValue.y);

            int intValue = Mathf.CeilToInt(value);

            if (Value != value) OnValueChanged?.Invoke(Value, value);
            if (IntValue != intValue) OnIntValueChanged?.Invoke(IntValue, intValue);

            Value = value;
            IntValue = intValue;
        }

        public void AddModify(string key, float value, EModifyMode eModifyMode, EModifyLayer eModifyLayer, bool canValueOverlap = true)
        {
            if (_modifiers[eModifyLayer].ContainsKey(key))
            {
                _modifiers[eModifyLayer][key]++;
            }
            else
            {
                StatModifier modifier = new StatModifier(value, eModifyMode, canValueOverlap);
                _modifiers[eModifyLayer][key] = modifier;
            }

            SetValue();
        }
        public void AddModify(string key, StatModifier statModifier, EModifyLayer eModifyLayer)
        {
            if (_modifiers[eModifyLayer].ContainsKey(key))
            {
                _modifiers[eModifyLayer][key]++;
            }
            else
            {
                _modifiers[eModifyLayer][key] = statModifier;
            }

            SetValue();
        }
        public void RemoveModify(string key, EModifyLayer eModifyLayer)
        {
            if (_modifiers[eModifyLayer].ContainsKey(key))
            {
                _modifiers[eModifyLayer][key]--;
                if (_modifiers[eModifyLayer][key] == 0)
                    _modifiers[eModifyLayer].Remove(key);
                SetValue();
            }
            else
                Debug.LogWarning($"[{key}]Key not found for statModifier");
        }

        public object Clone()
        {
            StatElement clonedStatElement = (StatElement)MemberwiseClone();
            clonedStatElement._modifiers = new Dictionary<EModifyLayer, Dictionary<string, StatModifier>>();
            return clonedStatElement;
        }
    }

    [Serializable]
    public class StatElementAdjustment
    {
        [HideInInspector] public string Name;
        public StatElementSO elementSO;
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public bool IsPercentAdjustment { get; private set; }
    }
}