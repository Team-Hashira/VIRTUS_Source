// using DG.Tweening;
// using Hashira.MainScreen;
// using System.Collections;
// using UnityEngine;
//
// namespace Hashira.Bosses.Patterns
// {
//     public class GiantGolemLaserPattern : GiantGolemPattern
//     {
//         private bool _isFindModePlayer { get; set; } = false;
//         [SerializeField] private float _followSpeed = 15f;
//         [SerializeField] private float _findModeDuration = 3;
//         private float _findModeTime = 0;
//         [SerializeField] private int _findMaxCount = 2;
//         private int _currentFindIndex = 0;
//         [SerializeField] private float _laserAttackDuration = 2f;
//         
//         public override void OnStart()
//         {
//             base.OnStart();
//             _giantGolemEye.LookAtPlayerDirection = true;
//             _currentFindIndex = 0;
//             _isFindModePlayer = false;
//
//             _laserHandEffectL = _handL.GetHandEffect<LaserHandEffect>();
//             _laserHandEffectR = _handR.GetHandEffect<LaserHandEffect>();
//             
//             MoveHands().OnComplete(()=>
//             {
//                 _handR.SetActiveEffect(true, Vector2.right);
//                 _handL.SetActiveEffect(true, Vector2.down);
//                 _laserHandEffectL.AttackVisualizer.ResetDamageCastVisualSign();
//                 _laserHandEffectR.AttackVisualizer.ResetDamageCastVisualSign();
//                 _laserHandEffectL.AttackVisualizer.SetDamageCastSignValue(1, _findModeDuration);
//                 _laserHandEffectR.AttackVisualizer.SetDamageCastSignValue(1, _findModeDuration);
//                 _isFindModePlayer = true;
//                 _findModeTime = Time.time;
//             });
//         }
//
//         //Obstacle
//         //private void CreateObstacle()
//         //{
//         //    GiantGolemObstacle obstacle = GameObject.Instantiate(
//         //        Boss.BillboardValue<GiantGolemObstacleValue>("GiantGolemObstacle").Value,
//         //        Transform.position, Quaternion.identity);
//         //    
//         //    obstacle.Throw(Player.transform.position);
//         //}
//         
//         private Tween MoveHands()
//         {
//             Vector2 playerPos = Player.transform.position;
//             Vector2 worldScreenPosMax = MainScreenEffect.worldScreenPositionMax;
//             Sequence seq = DOTween.Sequence();
//             seq.Append(_handR.transform.DOMove(new Vector3(worldScreenPosMax.x-2, playerPos.y), 1.45f).SetEase(Ease.InQuad));
//             seq.Join(_handL.transform.DOMove(new Vector3(playerPos.x, worldScreenPosMax.y-2), 1.45f).SetEase(Ease.InQuad));
//             return seq;
//         }
//
//         public override void OnUpdate()
//         {
//             base.OnUpdate();
//             if (_isFindModePlayer)
//             {
//                 _handR.MoveToPlayer(Player, _followSpeed, false, true);
//                 _handL.MoveToPlayer(Player, _followSpeed, true, false);
//
//                 if (_findModeTime + _findModeDuration < Time.time)
//                 {
//                     _laserHandEffectL.AttackVisualizer.SetDamageCastSignColor(Color.white);
//                     _laserHandEffectR.AttackVisualizer.SetDamageCastSignColor(Color.white);
//                     _laserHandEffectL.ShowVisualizer();
//                     _laserHandEffectR.ShowVisualizer();
//                     
//                     _isFindModePlayer = false;
//                     _findModeTime = Time.time;
//                     
//                     Boss.StartCoroutine(CoroutineLaserAttack());
//                 }
//             }
//         }
//
//         public override void OnEnd()
//         {
//             base.OnEnd();
//             _giantGolemEye.LookAtPlayerDirection = false;
//             _handR.SetActiveEffect(false);
//             _handL.SetActiveEffect(false);
//             Sequence seq = DOTween.Sequence();
//             seq.AppendInterval(0.1f)
//                .Append(_handR.ResetToOriginPosition())
//                .Join(_handL.ResetToOriginPosition());
//         }
//
//         private IEnumerator CoroutineLaserAttack()
//         {
//             yield return new WaitForSeconds(0.1f);
//             _laserHandEffectL.AttackVisualizer.SetDamageCastSignOriginColor();
//             _laserHandEffectR.AttackVisualizer.SetDamageCastSignOriginColor();
//             
//             _handR.SetActiveEffect(true, Vector2.right);
//             _handL.SetActiveEffect(true, Vector2.down);
//             
//             // 레이저 키기
//             _laserHandEffectL.AttackVisualizer.SetDamageCastSignValue(0);
//             _laserHandEffectR.AttackVisualizer.SetDamageCastSignValue(0);
//             _handR.SetActiveEffect<LaserHandEffect>(true);
//             _handL.SetActiveEffect<LaserHandEffect>(true);
//             
//             // 셰이킹
//             CameraManager.Instance.ShakeCamera(10, 4, 0.23f);
//             CameraManager.Instance.ShakeCamera(15,30, _laserAttackDuration);
//             
//             // 기다렸다 레이저 끄기
//             yield return new WaitForSeconds(_laserAttackDuration);
//             _handR.SetActiveEffect<LaserHandEffect>(false);
//             _handL.SetActiveEffect<LaserHandEffect>(false);
//             
//             ++_currentFindIndex;
//             if (_currentFindIndex >= _findMaxCount)
//             {
//                 yield return new WaitForSeconds(1f);
//                 EndPattern();
//             }
//             else
//             {
//                 _isFindModePlayer = true;
//                 _laserHandEffectL.AttackVisualizer.SetDamageCastSignValue(1, _findModeDuration);
//                 _laserHandEffectR.AttackVisualizer.SetDamageCastSignValue(1, _findModeDuration);
//                 _findModeTime = Time.time;    
//             }
//         }
//     }
// }
