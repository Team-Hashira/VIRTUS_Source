using Hashira.GimmickSystem;
using UnityEngine;

namespace Hashira.Tutorials
{
    public class ApplyGimmickStep : TutorialStep, IGimmickObject
    {
        [field: SerializeField]
        public GimmickSO GimmickSO { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();
            GimmickSO.OnGimmick(this);
            _tutorialManager.NextStep();
        }
    }
}
