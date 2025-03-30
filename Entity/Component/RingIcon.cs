using UnityEngine;

namespace Hashira.Entities.Components
{
    public class RingIcon : MonoBehaviour
    {
        private readonly static int _FillAmountShaderHash = Shader.PropertyToID("_FillAmount");

        [SerializeField] private Transform _visualTrm;
        [SerializeField] private SpriteRenderer _charging;

        private Material _chargingMat;

        private void Start()
        {
            _chargingMat = _charging.material;
            _chargingMat.SetFloat(_FillAmountShaderHash, 0);
            SetActive(false);
        }

        public void SetAmount(float amouont)
        {
            amouont = Mathf.Clamp(amouont, 0f, 1f);
            _chargingMat.SetFloat(_FillAmountShaderHash, amouont);
        }

        public void SetActive(bool active)
            => _visualTrm.gameObject.SetActive(active);
    }
}
