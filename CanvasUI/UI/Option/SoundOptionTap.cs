using UnityEngine;
using UnityEngine.Audio;

namespace Hashira.CanvasUI.Option
{
    public class SoundOptionTap : OptionTab
    {
        [SerializeField] private AudioMixer _audioMixer;

        public override void Init()
        {
            base.Init();
            foreach (var setter in _optionSetterList)
            {
                switch (setter.setterTarget)
                {
                    case nameof(OptionData.SoundSaveData.masterSound):
                        if (setter is OptionSetter<float> masterSoundSetter)
                        {
                            masterSoundSetter.OnValueEvent += value => SetAudioMixer("MasterVolume", value);
                            masterSoundSetter.Value = OptionData.SoundSaveData.masterSound;
                        }
                        break;
                    case nameof(OptionData.SoundSaveData.effectSound):
                        if (setter is OptionSetter<float> effectSoundSetter)
                        {
                            effectSoundSetter.OnValueEvent += value => SetAudioMixer("SFXVolume", value);
                            effectSoundSetter.Value = OptionData.SoundSaveData.effectSound;
                        }
                        break;
                    case nameof(OptionData.SoundSaveData.backgroundSound):
                        if (setter is OptionSetter<float> backgroundSoundSetter)
                        {
                            backgroundSoundSetter.OnValueEvent += value => SetAudioMixer("BGMVolume", value);
                            backgroundSoundSetter.Value = OptionData.SoundSaveData.backgroundSound;
                        }
                        break;
                    default:
                        return;
                }
            }
        }

        private void SetAudioMixer(string audioGrop, float value)
        {
            if (value < Mathf.Epsilon) value = Mathf.Epsilon;
            _audioMixer.SetFloat(audioGrop, Mathf.Log10(value) * 20);
        }

        public override void SaveData()
        {
            foreach (var setter in _optionSetterList)
            {
                switch (setter.setterTarget)
                {
                    case nameof(OptionData.SoundSaveData.masterSound):
                        if (setter is OptionSetter<float> masterSoundSetter)
                            OptionData.SoundSaveData.masterSound = masterSoundSetter.Value;
                        break;
                    case nameof(OptionData.SoundSaveData.effectSound):
                        if (setter is OptionSetter<float> effectSoundSetter)
                            OptionData.SoundSaveData.effectSound = effectSoundSetter.Value;
                        break;
                    case nameof(OptionData.SoundSaveData.backgroundSound):
                        if (setter is OptionSetter<float> backgroundSoundSetter)
                            OptionData.SoundSaveData.backgroundSound = backgroundSoundSetter.Value;
                        break;
                    default:
                        return;
                }
            }
            OptionData.SaveData();
        }
    }
}
