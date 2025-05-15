using Doryu.CustomAttributes;
using Hashira.Core;
using UnityEngine;

namespace Hashira.Combat
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        
        public bool followPosition = true;
        [ToggleField(nameof(followPosition))]
        public bool enableX = true;
        [ToggleField(nameof(followPosition))]
        public bool enableY = true;
        [ToggleField(nameof(followPosition))]
        public bool enableZ = true;
        [ToggleField(nameof(followPosition))]
        public Vector3 offset;
        
        public bool followRotation = true;

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void Awake()
        {
            target = PlayerManager.Instance.Player.transform;
        }

        private void LateUpdate()
        {
            if (followPosition)
                FollowToTargetPosition();

            if (followRotation)
                FollowToTargetRotation();
        }

        private void FollowToTargetPosition()
        {
            Vector3 targetPos = target.position + offset;

            targetPos = new Vector3(
                enableX ? targetPos.x : transform.position.x, 
                enableY ? targetPos.y : transform.position.y, 
                enableZ ? targetPos.z : transform.position.z);
            
            transform.position = targetPos;
        }
        
        private void FollowToTargetRotation()
        {
            transform.rotation = target.rotation;
        }
    }
}
