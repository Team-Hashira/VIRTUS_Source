using DG.Tweening;
using Hashira.MainScreen;
using UnityEngine;

namespace Hashira.GimmickSystem
{
    [CreateAssetMenu(fileName = "ScreenMoveGimmick", menuName = "SO/GimmickSystem/ScreenMoveGimmick")]
    public class ScreenMoveGimmickSO : GimmickSO
    {
        [SerializeField] private Vector2 _direction;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        
        public override void OnGimmick(IGimmickObject owner)
        {
            MainScreenEffect.OnLocalMoveScreenSide(_direction, _duration, _ease);
        }
    }
}