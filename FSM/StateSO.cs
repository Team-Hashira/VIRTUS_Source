using Doryu.CustomAttributes;
using Hashira.Core.AnimationSystem;
using UnityEngine;

namespace Hashira.FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/StateSO")]
    public class StateSO : ScriptableObject
    {
        public string stateName;

        [Tooltip("내가 넣은 stateName이 실제 클래스 명과 다를때 켜야 합니다.\nstateName을 이용해 클래스 이름을 뽑는 방법은\n{EntityName}stateName{State}\n입니다.")]
        public bool ifClassNameIsDifferent = false;
        [Tooltip("실제 클래스 명을 넣어주세요. 이 클래스는 State가 적용될 Entity와 같은 Namespace에 위치해야합니다.")]
        [ToggleField("ifClassNameIsDifferent")]
        public string className;
        [Tooltip("만약 State가 적용될 Entity와 다른 Namespace에 존재하는 State를 가져와야 하고, Namespace를 포함한 전체 클래스 명을 적었다면 켜야합니다.")]
        [ToggleField("ifClassNameIsDifferent")]
        public bool isFullName;

        public AnimatorParamSO animatorParam;
    }
}
