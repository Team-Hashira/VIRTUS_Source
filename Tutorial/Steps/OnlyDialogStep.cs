using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class OnlyDialogStep : UsePanelStep
    {
        [SerializeField]
        private List<TutorialInfo> _infoList;

        [SerializeField]
        private bool _onEnterTextToEmpty = true, _onExitTextToEmpty = false;
        [SerializeField]
        private bool _callNextStep = true;

        public override void OnEnter()
        {
            base.OnEnter();
            if (_onEnterTextToEmpty)
                _panel.SetText("", false, false);
            StartCoroutine(DialogCoroutine());
        }

        private IEnumerator DialogCoroutine()
        {
            yield return new WaitForSeconds(_duration);
            foreach (var info in _infoList)
            {
                float duration = info.infoSO.tutorialText.GetAnimationDuration(info.infoSO.textSpeed);
                _panel.SetText(info.infoSO.tutorialText, speed: info.infoSO.textSpeed);
                yield return new WaitForSeconds(duration + info.delay);
            }
            if (_callNextStep)
                _tutorialManager.NextStep();
        }

        public override void OnExit()
        {
            base.OnExit();
            if (_onExitTextToEmpty)
                _panel.SetText("", false, false);
        }
    }
}
