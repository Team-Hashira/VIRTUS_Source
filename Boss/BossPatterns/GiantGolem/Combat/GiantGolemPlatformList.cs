using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class GiantGolemPlatformList : MonoBehaviour
    {
        private bool _isInvoked = false;
        [SerializeField] private UnityEvent OnAllDestroyEvent;
        
        [SerializeField] private GiantGolemPlatform[] _platforms;
        
        public GiantGolemPlatform[] GetAllPlatforms() => _platforms;
        public GiantGolemPlatform GetPlatform(int i) => _platforms[i];

        public GiantGolemPlatform[] GetRandomPlatforms(int count)
        {
            if (count >= _platforms.Length) return GetAllPlatforms();
            
            GiantGolemPlatform[] selectedPlatforms = GetAllPlatforms();
            selectedPlatforms.Shuffle();
            GiantGolemPlatform[] platforms = new GiantGolemPlatform[count];

            for (int i = 0; i < count; i++)
                platforms[i] = selectedPlatforms[i];
            
            return platforms;
        }

        public Tween AllMoveToOrigin(float duration = 2f)
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < _platforms.Length; i++)
            {
                seq.Join(_platforms[i].MoveToOrigin(duration));
            }
            
            return seq;
        }

        public Tween AllShake(float duration = 2f, float strength = 0.2f, int vibrato = 10)
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < _platforms.Length; i++)
            {
                seq.Join(_platforms[i].Shake(duration, strength, vibrato));
            }
            
            return seq;
        }
        
        public void DestroyAllPlatforms()
        {
            foreach (GiantGolemPlatform giantGolemPlatform in GetAllPlatforms())
            {
                giantGolemPlatform.Shake(1f, 18f, 30).OnComplete(() =>
                {
                    if (_isInvoked == false)
                    {
                        OnAllDestroyEvent?.Invoke();
                        _isInvoked = true;
                    }
                    giantGolemPlatform.gameObject.SetActive(false);
                });
            }
        }
    }
}
