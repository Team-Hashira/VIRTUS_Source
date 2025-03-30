using UnityEngine;

namespace Hashira.Core.AnimationSystem
{
    [CreateAssetMenu(fileName = "AnimatorParamSO", menuName ="SO/Animation/ParamSO")]
    public class AnimatorParamSO : ScriptableObject
    {
        public string paramName;
        public int hash;

        private void OnValidate()
        {
            hash = Animator.StringToHash(paramName);
        }
    }
}
