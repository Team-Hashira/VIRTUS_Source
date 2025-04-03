using Hashira.Core.MoveSystem;
using Hashira.Enemies.PublicStates;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Enemies.Bee.CommonBee
{
    public class CommonBee : AirEnemy
    {
        [SerializeField]
        private List<Transform> _bezierPositionList;
        [SerializeField]
        private bool _showDebug = true;
        [field: SerializeField]
        public DamageCaster2D DamageCaster { get; private set; }

        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
            _entityStateMachine.GetState<AirEnemyBezierPatrolState>().BezierPositionList = _bezierPositionList.ToVector2List();
            _entityStateMachine.SetShareVariable("TargetState", "Refresh");
            _enemyMover.GetMoveProcessor<XYSmoothProcessor>().Speed = 10f;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_bezierPositionList == null || _bezierPositionList.Count <= 0) return;
            if (!_showDebug) return;
            List<Vector2> vectorList = _bezierPositionList.ToVector2List();
            int density = 100;
            float percent = 0;
            float additionalValue = 1f / density;
            Vector2 prevPos = new Vector2(float.NaN, float.NaN);
            Gizmos.color = Color.yellow;
            while (percent < 1)
            {
                Vector2 curPos = MathEx.Bezier(percent, vectorList);
                if (prevPos.x == float.NaN)
                    Gizmos.DrawLine(curPos, curPos);
                else
                    Gizmos.DrawLine(prevPos, curPos);
                prevPos = curPos;
                percent += additionalValue;
            }
            Gizmos.color = Color.red;
            prevPos = new Vector2(float.NaN, float.NaN);
            vectorList.ForEach(pos =>
            {
                Gizmos.DrawSphere(pos, 0.3f);
                if (prevPos.x == float.NaN)
                    Gizmos.DrawLine(pos, pos);
                else
                    Gizmos.DrawLine(prevPos, pos);
                prevPos = pos;
            });
        }
#endif
    }
}
