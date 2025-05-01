using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public enum ETimeLayer
    {
        PauseUI,
        InGame,
    }

    public static class TimeController
    {
        private static Dictionary<ETimeLayer, Stack<float>> _timeScaleChangedStack;

        private static float _defaultFixedDeltaTime;

        static TimeController()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime / Time.timeScale;

            _timeScaleChangedStack = new Dictionary<ETimeLayer, Stack<float>>()
            {
                { ETimeLayer.InGame, new Stack<float>() },
                { ETimeLayer.PauseUI, new Stack<float>() },
            };
        }

        public static void SetTimeScale(ETimeLayer timeLayer, float value)
        {
            _timeScaleChangedStack[timeLayer].Push(value);
            ApplyTimeScale();
        }

        public static void UndoTimeScale(ETimeLayer timeLayer)
        {
            if (_timeScaleChangedStack.Count == 0) return;
            _timeScaleChangedStack[timeLayer].Pop();
            ApplyTimeScale();
        }

        public static void ResetTimeScale(ETimeLayer timeLayer)
        {
            while (_timeScaleChangedStack[timeLayer].Count > 0)
            {
                _timeScaleChangedStack[timeLayer].Pop();
                ApplyTimeScale();
            }
        }

        private static void ApplyTimeScale()
        {
            foreach (ETimeLayer timeLayer in Enum.GetValues(typeof(ETimeLayer)))
            {
                if (_timeScaleChangedStack[timeLayer].Count > 0)
                {
                    Time.timeScale = _timeScaleChangedStack[timeLayer].Peek();
                    Time.fixedDeltaTime = _defaultFixedDeltaTime / Time.timeScale;
                    return;
                }
            }
            Time.timeScale = 1;
            Time.fixedDeltaTime = _defaultFixedDeltaTime / Time.timeScale;
        }
    }
}