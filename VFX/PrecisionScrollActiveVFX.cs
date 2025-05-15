using Crogen.CrogenPooling;
using Hashira.Players;
using UnityEngine;

namespace Hashira.VFX
{
    public class PrecisionScrollActiveVFX : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private Player _owner;

        [SerializeField]
        private Transform _aimingIcon;
        [SerializeField]
        private LineRenderer _lineRenderer;

        public void Initialize(Player owner)
        {
            _owner = owner;
        }

        public void UpdateTarget(Transform target)
        {
            _aimingIcon.position = target.position;
            _lineRenderer.SetPosition(0, _owner.Attacker.transform.position);
            _lineRenderer.SetPosition(1, target.position);
        }

        public void SetActive(bool isActive)
        {
            _aimingIcon.gameObject.SetActive(isActive);
            int posCount = isActive ? 2 : 0;
            _lineRenderer.positionCount = posCount;
        }

        public void OnPop()
        {
        }

        public void OnPush()
        {
        }
    }
}
