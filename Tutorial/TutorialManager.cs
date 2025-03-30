using Crogen.AttributeExtension;
using Crogen.CrogenPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _canvasTrm;

        private List<TutorialStep> _stepList;

        [SerializeField]
        private List<TutorialStep> _currentStepList;
        private int _currentStepIndex = 0;

        private int _lastIndex = 0;

        private Dictionary<string, TutorialPanel> _tutorialPanelDict;


        [Header("=====DEBUG=====")]
        [SerializeField]
        private TutorialStep _startStep;
        [SerializeField]
        private int _startIndex;

        private void Awake()
        {
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
            _startIndex = _stepList.IndexOf(_startStep);
            _currentStepIndex = _startIndex;
            _lastIndex = _currentStepIndex;
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
            _tutorialPanelDict.Add(key, panel);
            return panel;
        }

        public TutorialPanel GetTutorialPanel(string key)
        {
            if (_tutorialPanelDict.TryGetValue(key, out TutorialPanel panel))
                return panel;
            return null;
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
    }
}
