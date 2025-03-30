using UnityEngine;
using System.Linq;

namespace Hashira.Core.StatSystem
{
    //Stat의 변하지 않는 값을 가지고있음
    [CreateAssetMenu(fileName = "StatElement", menuName = "SO/Stat/StatElement")]
    public class StatElementSO : ScriptableObject
    {
        public string statName;
        public string displayName;
        public Vector2 minMaxValue;
        public Sprite statIcon;
        [Tooltip("낮을 수록 좋은 값인가")]
        public bool isPreferLow;

        public void GenerateStatNameClass()
        {

        }
    }
}