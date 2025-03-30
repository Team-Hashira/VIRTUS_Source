using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Hashira.LightingControl
{
    public class LightingController : MonoBehaviour
    {
        public static Light2D GlobalLight { get; private set; }
        public static Volume Volume { get; private set; }

        private static Sequence _lightIntensitySeq;
        private static Sequence _aberrationSeq;

        public void Awake()
        {
            GlobalLight = GetComponentInChildren<Light2D>();
            Volume = GetComponentInChildren<Volume>();
        }

        public static void SetGlobalLightIntensity(float intensity, float time = 0, Ease ease = Ease.Linear)
        {
            float startIntensity = GlobalLight.intensity;
            if (_lightIntensitySeq != null && _lightIntensitySeq.IsActive()) _lightIntensitySeq.Kill();
            _lightIntensitySeq.Append(DOTween.To(() => startIntensity, 
                value => GlobalLight.intensity = value, intensity, time).SetEase(ease));
        }

        public static void Aberration(float intensity, float time = 0, Ease ease = Ease.Linear, bool isAdd = true)
        {
            if (LightingController.Volume.profile.TryGet(out ChromaticAberration chromaticAberration) == false) return;

            float startValue;

            if (isAdd)
            {
                startValue = Mathf.Clamp(chromaticAberration.intensity.value + intensity, 0, 1) * OptionData.GraphicSaveData.screenEffectValue;
            }
            else
            {
                startValue = Mathf.Max(chromaticAberration.intensity.value, intensity) * OptionData.GraphicSaveData.screenEffectValue;
            }

            if (_aberrationSeq != null && _aberrationSeq.IsActive()) _aberrationSeq.Kill();
            _aberrationSeq = DOTween.Sequence();
            _aberrationSeq
                .Append(
                    DOTween.To(() => startValue,
                    value => chromaticAberration.intensity.value = value,
                    0, time).SetEase(ease));
        }
    }
}
