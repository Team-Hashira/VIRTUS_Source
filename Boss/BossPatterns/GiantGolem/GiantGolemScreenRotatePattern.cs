// using DG.Tweening;
// using Hashira.Bosses.BillboardClasses;
// using UnityEngine;
//
// namespace Hashira.Bosses.Patterns
// {
//     public class GiantGolemScreenRotatePattern : GiantGolemPattern
//     {
//         private Transform _face;
//
//         public override void Init(Boss boss)
//         {
//             base.Init(boss);
//             _face = boss.BillboardValue<TransformValue>("Face").Value;
//         }
//          
//         public override void OnStart()
//         {
//             base.OnStart();
//             _giantGolemEye.LookAtPlayerDirection = false;
//             CatchWall();
//         }
//
//         private void CatchWall()
//         {
//             Vector2 wallPosR = new(MainScreen.MainScreenEffect.worldScreenPositionMax.x-1, 0);
//             Vector2 wallPosL = new(MainScreen.MainScreenEffect.worldScreenPositionMin.x+1, 0);
//             
//             int rotValue = Mathf.Abs(MainScreen.MainScreenEffect.GetTransform().eulerAngles.z) < 0.1f ? (Random.Range(0, 2) * 2 - 1) * 35 : 0;
//             Sequence seq = DOTween.Sequence();
//             seq.AppendCallback(()=>
//                {
//                    _handR.SetActiveEffect(true, Vector2.right);
//                    _handL.SetActiveEffect(true, Vector2.right);
//                })
//                .Append(_handL.transform.DOMove(wallPosR, 1f))
//                .Join(_handR.transform.DOMove(wallPosL, 1f))
//                .AppendInterval(0.1f)
//                .JoinCallback(() =>
//                {
//                    _handR.SetActiveEffect(false);
//                    _handL.SetActiveEffect(false);
//                })
//                .AppendInterval(1f)
//                .AppendCallback(()=>CameraManager.Instance.ShakeCamera(20, 10, 0.25f))
//                .Append(MainScreen.MainScreenEffect.OnRotate(rotValue, 0.25f))
//                .Join(MainScreen.MainScreenEffect.OnScaling(rotValue == 0 ? 1f : 0.75f, 0.25f))
//                .Join(MainScreen.MainScreenEffect.OnGlitch(1, 0.25f))
//                .Join(_face.DORotate(new Vector3(0, 0, -rotValue), 0.25f))
//                .AppendCallback(() =>
//                {
//                    SoundManager.Instance.PlaySFX("GiantGolemScreenRotate", Transform.position, 1);
//                })
//                .AppendInterval(1f)
//                .Join(MainScreen.MainScreenEffect.OnGlitch(0, 0.25f))
//                .JoinCallback(() =>
//                {
//                    _handR.SetActiveEffect(true);
//                    _handL.SetActiveEffect(true);
//                })
//                .AppendInterval(0.1f)
//                .JoinCallback(() =>
//                {
//                    _handR.SetActiveEffect(false);
//                    _handL.SetActiveEffect(false);
//                })
//                .Append(_handL.ResetToOriginPosition(1f))
//                .Join(_handR.ResetToOriginPosition(1f))
//                .OnComplete(EndPattern<GiantGolemStampPattern>);
//         }
//
//         public override void OnEnd()
//         {
//             base.OnEnd();
//             _giantGolemEye.LookAtPlayerDirection = false;
//         }
//     }
// }
