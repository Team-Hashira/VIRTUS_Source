using Crogen.CrogenPooling;
using Hashira.CanvasUI;
using Hashira.Core;
using Hashira.Core.DamageHandler;
using Hashira.StageSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private Stage _tutorialStage;
        [SerializeField]
        private Transform _canvasTrm;

        private List<TutorialStep> _stepList;

        private List<TutorialStep> _currentStepList;
        private int _currentStepIndex = 0;
        private int _lastIndex = 0;

        private Dictionary<string, TutorialPanel> _tutorialPanelDict;

        [Header("=====DEBUG=====")]
        [SerializeField]
        private TutorialStep _startStep;

        private Dictionary<string, object> _shareVariableDict;

        private void Awake()
        {
            _shareVariableDict = new Dictionary<string, object>();

            _canvasTrm = UIManager.Instance.MainCanvas.transform;

            StageGenerator.Instance.SetCurrentStage(_tutorialStage);
            _stepList = new List<TutorialStep>();
            _currentStepList = new List<TutorialStep>();

            _tutorialPanelDict = new Dictionary<string, TutorialPanel>();
            for (int i = 0; i < transform.childCount; i++)
            {
                _stepList.Add(transform.GetChild(i).GetComponent<TutorialStep>());
            }
            _stepList.ForEach(step =>
            {
                step.Initialize(this);
                step.gameObject.SetActive(false);
            });
            _lastIndex = _stepList.IndexOf(_startStep);

            ShieldHandler handler = new ShieldHandler(int.MaxValue, true);
            handler.SetOrderInLayer(int.MinValue);
            PlayerManager.Instance.Player.EntityHealth.AddDamageHandler(EDamageHandlerLayer.First, handler);
        }

        private void Start()
        {
            _stepList[_currentStepIndex].gameObject.SetActive(true);
            _currentStepList.Add(_stepList[_currentStepIndex]);
            _stepList[_currentStepIndex].OnEnter();
        }

        public void NextStep()
        {
            _stepList[_currentStepIndex].OnExit();
            _stepList[_currentStepIndex].gameObject.SetActive(false);
            _currentStepList.Clear();
            _currentStepIndex = ++_lastIndex;
            _stepList[_currentStepIndex].gameObject.SetActive(true);
            _currentStepList.Add(_stepList[_currentStepIndex]);
            _stepList[_currentStepIndex].OnEnter();
        }

        private void Update()
        {
            List<TutorialStep> tutorialStepList = _currentStepList.ToList();
            foreach (var step in tutorialStepList)
                step.OnUpdate();
        }

        public TutorialPanel GenerateTutorialPanel(string key)
        {
            TutorialPanel panel = PopCore.Pop(UIPoolType.TutorialPanel, _canvasTrm) as TutorialPanel;
            panel.transform.localScale = Vector3.one;
            _tutorialPanelDict.Add(key, panel);
            return panel;
        }

        public TutorialPanel GetTutorialPanel(string key)
        {
            if (_tutorialPanelDict.TryGetValue(key, out TutorialPanel panel))
                return panel;
            return null;
        }

        public void CloseTutorialPanel(string key)
        {
            if (_tutorialPanelDict.TryGetValue(key, out TutorialPanel panel))
            {
                panel.Close();
                _tutorialPanelDict.Remove(key);
            }
        }

        public void SetActiveStep(TutorialStep step, bool isActive)
        {
            if (!isActive)
                step.OnExit();
            step.gameObject.SetActive(isActive);
            if (isActive)
            {
                _lastIndex = Mathf.Max(_stepList.IndexOf(step), _lastIndex);
                step.OnEnter();
                _currentStepList.Add(step);
            }
        }

        public void SetShareVariable(string key, object value)
        {
            if (_shareVariableDict.ContainsKey(key))
                _shareVariableDict[key] = value;
            else
                _shareVariableDict.Add(key, value);
        }

        public T GetShareVariable<T>(string key)
        {
            if(_shareVariableDict.TryGetValue(key, out object value))
                return (T)value;
            return default;
        }
    }
}
