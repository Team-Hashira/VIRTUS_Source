using Crogen.AttributeExtension;
using Doryu.CustomAttributes;
using Hashira.Core.AnimationSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

namespace Hashira.Entities.Components
{
    [Serializable]
    public class StateInfo
    {
        public StateSO state;
        public AnimatorParamSO toOverrideParam;
    }

    public class EntityStateMachine : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
    {
        private Entity _entity;

        [SerializeField]
        private List<StateInfo> _stateInfoList;
        [field: SerializeField]
        public StateSO StartState { get; private set; }

        private Dictionary<Type, EntityState> _stateTypeDictionary;
        private Dictionary<string, EntityState> _stateDictionary;
        private Dictionary<string, object> _shareVariableDict;

        public EntityState CurrentState { get; private set; }
        public string CurrentStateName { get; private set; }

        /// <summary>
        /// Super Armor인 상황이다 == 그로기 State로 가지 않는다.
        /// </summary>
        public bool IsSuperArmored { get; set; } = false;

        public void Initialize(Entity entity)
        {
            _entity = entity;

            _stateTypeDictionary = new Dictionary<Type, EntityState>();
            _stateDictionary = new Dictionary<string, EntityState>();
            _shareVariableDict = new Dictionary<string, object>();

            for(int i = 0; i <  _stateInfoList.Count; i++)
            {
                _stateInfoList[i].state = _stateInfoList[i].state.Clone() as StateSO;
                if (_stateInfoList[i].toOverrideParam != null)
                {
                    _stateInfoList[i].state.animatorParam = _stateInfoList[i].toOverrideParam;
                }
            }
        }

        private void Start()
        {
            ChangeState(StartState.stateName);
        }

        private void Update()
        {
            CurrentState.OnUpdate();
        }

        public EntityState GetState(string stateName)
        {
            if (_stateDictionary.TryGetValue(stateName, out EntityState entityState))
            {
                return entityState;
            }
            return null;
        }

        public T GetState<T>() where T : EntityState
        {
            if (_stateTypeDictionary.TryGetValue(typeof(T), out EntityState entityState))
            {
                return entityState as T;
            }
            return null;
        }

        public void ChangeState(string newState, bool isForced = false)
        {
            if (IsSuperArmored && !isForced)
                return;
            if (_stateDictionary.TryGetValue(newState, out EntityState entityState))
            {
                CurrentState?.OnExit();
                CurrentState = entityState;
                CurrentStateName = newState;
                CurrentState.OnEnter();
            }
            else
            {
                Debug.LogError($"Fail to find {newState}.");
                return;
            }
        }

        public void ChangeState(Type stateType, bool isForced = false)
        {
            if (IsSuperArmored && !isForced)
                return;
            if (_stateTypeDictionary.TryGetValue(stateType, out EntityState entityState))
            {
                CurrentState?.OnExit();
                CurrentState = entityState;
                CurrentStateName = entityState.StateSO.stateName;
                CurrentState.OnEnter();
            }
            else
            {
                Debug.LogError($"Fail to find {stateType.ToString()}.");
                return;
            }
        }

        public void ChangeState(StateSO newState, bool isForced = false)
        {
            ChangeState(newState.stateName, isForced);
        }
        /// <summary>
        /// 한프레임 쉬고 ChangeState를 호출해주는 함수입니다.
        /// </summary>
        /// <param name="newState"></param>
        public void DelayedChangeState(string newState)
        {
            IEnumerator DelayCoroutine()
            {
                yield return null;
                ChangeState(newState);
            }
            StartCoroutine(DelayCoroutine());
        }
        public void DelayedChangeState(StateSO newState)
        {
            IEnumerator DelayCoroutine()
            {
                yield return null;
                ChangeState(newState);
            }
            StartCoroutine(DelayCoroutine());
        }
        public void DelayedChangeState(Type stateType)
        {
            IEnumerator DelayCoroutine()
            {
                yield return null;
                ChangeState(stateType);
            }
            StartCoroutine(DelayCoroutine());
        }


        public T GetShareVariable<T>(string key)
        {
            if (_shareVariableDict.ContainsKey(key))
            {
                return (T)_shareVariableDict[key];
            }
            return default(T);
        }

        public bool TryGetShareVariable<T>(string key, out T variable)
        {
            variable = default(T);
            if (_shareVariableDict.ContainsKey(key))
            {
                variable = (T)_shareVariableDict[key];
                return true;
            }
            return false;
        }

        public void SetShareVariable(string key, object value)
        {
            if (_shareVariableDict.ContainsKey(key))
            {
                _shareVariableDict[key] = value;
            }
            else
            {
                _shareVariableDict.Add(key, value);
            }
        }

        public void AfterInit()
        {
            //StringBuilder보다 $""이 가벼움.
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach (var state in _stateList)
            //{
            //    stringBuilder.Append(_namespace);
            //    stringBuilder.Append(state.ToString());
            //    Type t = Type.GetType(stringBuilder.ToString());
            //    stringBuilder.Clear();
            //}

            foreach (var stateInfo in _stateInfoList)
            {
                var state = stateInfo.state;
                string stateName = state.ifClassNameIsDifferent ? state.className : state.stateName;
                string className = state.isFullName ? stateName : $"{_entity.GetType().FullName}{stateName}State";
                try
                {
                    Type t = Type.GetType(className);
                    EntityState entityState = Activator.CreateInstance(t, _entity, state) as EntityState;
                    _stateTypeDictionary.Add(entityState.GetType(), entityState);
                    _stateDictionary.Add(state.stateName, entityState);
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException)
                        Debug.LogError($"Fail to Create State class({state.stateName}, {className}). : {ex.InnerException.Message}\nStackTrace : {ex.InnerException.StackTrace}");
                    else
                        Debug.LogError($"Fail to Create State class({state.stateName}, {className}). : {ex.Message}");
                }
            }
        }
    }
}
