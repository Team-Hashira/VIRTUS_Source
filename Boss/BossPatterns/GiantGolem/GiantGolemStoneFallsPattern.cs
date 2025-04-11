// using DG.Tweening;
// using Hashira.Bosses.BillboardClasses;
// using Hashira.Bosses.Patterns.GiantGolem;
// using Hashira.Combat;
// using System.Collections;
// using UnityEngine;
//
// namespace Hashira.Bosses.Patterns
// {
//     // 이거 레이저 패턴으로 바뀜(돌 떨어지는 거 아님)
//     public class GiantGolemStoneFallsPattern : GiantGolemPattern
//     {
//         [SerializeField] private float _duration = 8f;
//         [SerializeField] private float _stoneFallsHandPosY = 1;
//         [SerializeField] private float _stoneFallsHandSpeed = 1;
//         private float _currentLifeTime = 0f;
//         private GiantGolemHand _selectedHand;
//         private bool _positionMoveComplete = false;
//
//         private AttackVisualizer _selectedHandVisualizer;
//         
//         public override void OnStart()
//         {
//             base.OnStart();
//             _giantGolemEye.LookAtPlayerDirection = true;
//             _positionMoveComplete = false;
//             
//             // 어떤 손이 더 가깝냐?
//             float playerToHandRDis = Vector2.Distance(_handR.transform.position, Player.transform.position);
//             float playerToHandLDis = Vector2.Distance(_handL.transform.position, Player.transform.position);
//
//             // 손 선택
//             if (playerToHandLDis > playerToHandRDis) _selectedHand = _handR;
//             else _selectedHand = _handL;
//             
//             // 선택된 AttackVisualizer가져오기 및 초기화
//             _selectedHandVisualizer = _selectedHand.GetHandEffect<LaserHandEffect>().AttackVisualizer;
//             _selectedHandVisualizer.ResetDamageCastVisualSign();
//             
//             Sequence seq = DOTween.Sequence();
//                 // 미리 설정한 높이로 올라가기
//             seq.Append(_selectedHand.transform.DOMoveY(_stoneFallsHandPosY, 0.5f))
//                 // 손 아래쪽으로 펼치고 Visualizer 활성화
//                .AppendCallback(() => _selectedHand.SetActiveEffect(true, Vector2.down))
//                .Join(_selectedHandVisualizer.SetDamageCastSignValue(1, 1f))
//                 // 잠깐 흰색으로 바꾸고 
//                .AppendCallback(()=>_selectedHandVisualizer.SetDamageCastSignColor(Color.white))
//                .AppendInterval(0.1f)
//                 // 원래대으로 되돌리고 레이저를 발사. Visualizer는 끄기
//                .AppendCallback(() =>
//                {
//                    _selectedHandVisualizer.SetDamageCastSignOriginColor();
//                    _selectedHand.SetActiveEffect<LaserHandEffect>(true, Vector2.down);
//                    _selectedHandVisualizer.SetDamageCastSignValue(0);
//                    _currentLifeTime = Time.time;
//                    _positionMoveComplete = true;
//                });
//         }
//
//         public override void OnUpdate()
//         {
//             base.OnUpdate();
//
//             // 앞의 모든 선 동작이 지나야 움직일 수 있음
//             if (_positionMoveComplete == false) return;
//
//             // 플레이어 쪽으로 따라가기
//             StoneFallsHandFollowToPlayer();
//
//             // 시간지나면 패턴 비활성화
//             if (_duration + _currentLifeTime < Time.time)
//                 EndPattern();
//         }
//
//         private void StoneFallsHandFollowToPlayer()
//         {
//             _selectedHand.MoveToPlayer(Player, _stoneFallsHandSpeed, true, false);
//         }
//
//         public override void OnEnd()
//         {
//             // 눈이랑 손 위치 되돌리고 레이저도 꺼버리기
//             _giantGolemEye.LookAtPlayerDirection = false;
//             _selectedHand.ResetToOriginPosition();
//             _selectedHand.SetActiveEffect<LaserHandEffect>(false);
//             base.OnEnd();
//         }
//
//         public override void OnDrawGizmos(Transform transform)
//         {
//             base.OnDrawGizmos(transform);
//
//             Gizmos.DrawLine(new Vector3(-1000, _stoneFallsHandPosY), new Vector3(1000, _stoneFallsHandPosY));
//         }
//     }
// }
