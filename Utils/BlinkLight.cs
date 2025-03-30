using Doryu.CustomAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hashira
{
    [RequireComponent(typeof(Light2D))]
    public class BlinkLight : MonoBehaviour
    {
        [SerializeField] private float _blinkDuration;
        [SerializeField] private AnimationCurve _intensityCurve;
        private float _currentTime;
        [SerializeField, Uncorrectable] private float _startInensity;
        private Light2D _light;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _startInensity = _light.intensity;
        }

        private void OnEnable()
        {
            _currentTime = 0;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime < _blinkDuration)
                _light.intensity = _startInensity * _intensityCurve.Evaluate(_currentTime / _blinkDuration);
            else if ( _currentTime > _blinkDuration)
                _light.intensity = _startInensity * _intensityCurve.Evaluate(1);
        }
    }
}
