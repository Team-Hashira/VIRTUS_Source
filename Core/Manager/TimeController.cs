using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public static class TimeController
    {
        private static Stack<float> _timeScaleChangedStack = new();

        private static float _defaultFixedDeltaTime;

        static TimeController()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime / Time.timeScale;
        }

        public static void SetTimeScale(float value)
        {
            _timeScaleChangedStack.Push(Time.timeScale);
			Time.timeScale = value;
            Time.fixedDeltaTime = _defaultFixedDeltaTime / Time.timeScale;
        }

        public static void UndoTimeScale()
        {
            if (_timeScaleChangedStack.Count == 0) return;
			Time.timeScale = _timeScaleChangedStack.Pop();
            Time.fixedDeltaTime = _defaultFixedDeltaTime / Time.timeScale;
        }

        public static void ResetTimeScale()
        {
            while (_timeScaleChangedStack.Count > 0)
            {
                Time.timeScale = _timeScaleChangedStack.Pop();
                Time.fixedDeltaTime = _defaultFixedDeltaTime / Time.timeScale;
            }
        }
    }
}