using System.Collections.Generic;
using System;
using UnityEngine;

namespace Hashira.Core.EventSystem
{
    public class GameEvent
    {
    }

    public static class GameEventChannel
    {
        private static Dictionary<Type, Action<GameEvent>> _eventDictionary = new();
        private static Dictionary<Delegate, Action<GameEvent>> _lookUpTable = new();

        public static void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookUpTable.ContainsKey(handler) == false)
            {
                Action<GameEvent> castHandler = (evt) => handler(evt as T);
                _lookUpTable[handler] = castHandler;

                Type evtType = typeof(T);
                if (_eventDictionary.ContainsKey(evtType))
                {
                    _eventDictionary[evtType] += castHandler;
                }
                else
                {
                    _eventDictionary[evtType] = castHandler;
                }
            }
            else
            {
                Debug.LogError("이미 구독된 이벤트 함수.");
            }
        }

        public static void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            Type evtType = typeof(T);
            if (_lookUpTable.TryGetValue(handler, out Action<GameEvent> action))
            {
                if (_eventDictionary.TryGetValue(evtType, out Action<GameEvent> internalAction))
                {
                    internalAction -= action;
                    if (internalAction == null)
                        _eventDictionary.Remove(evtType);
                    else
                        _eventDictionary[evtType] = internalAction;
                }
                _lookUpTable.Remove(handler);
            }
        }

        public static void RaiseEvent(GameEvent evt)
        {
            if (_eventDictionary.TryGetValue(evt.GetType(), out Action<GameEvent> handlers))
            {
                handlers?.Invoke(evt);
            }
        }

        public static void Clear()
        {
            _eventDictionary.Clear();
            _lookUpTable.Clear();
        }
    }
}
