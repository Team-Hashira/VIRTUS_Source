using DG.Tweening;
using Hashira.MainScreen;
using UnityEngine;

namespace Hashira.GimmickSystem
{
    [CreateAssetMenu(fileName = "ScreenRotateGimmick", menuName = "SO/GimmickSystem/ScreenRotateGimmick")]
    public class ScreenRotateGimmickSO : GimmickSO
    {
        [SerializeField] private float _angle;
        [SerializeField] private float _duration;
        [SerializeField] private RotateMode _rotateMode = RotateMode.Fast;
        [SerializeField] private Ease _ease;
        
        public override void OnGimmick(IGimmickObject owner)
        {
            MainScreenEffect.OnRotate(_angle, _duration, _rotateMode, _ease);
        }
    }
}