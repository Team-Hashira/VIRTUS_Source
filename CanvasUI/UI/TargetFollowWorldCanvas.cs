using System;
using UnityEngine;

namespace Hashira.UI
{
    public class TargetFollowWorldCanvas : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _scaleMultiplier = Vector3.one * 10f;
        private void LateUpdate()
        {
            if (_target == null) return;
            
            transform.SetPositionAndRotation(_target.position, _target.rotation);
            
            Vector3 finalScale = _target.localScale * 0.01f;
            for (int i = 0; i < 3; i++)
                finalScale[i] *= _scaleMultiplier[i];

            transform.localScale = finalScale;
        }
    }
}
