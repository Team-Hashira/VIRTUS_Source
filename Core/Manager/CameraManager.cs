using DG.Tweening;
using Hashira;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private AYellowpaper.SerializedCollections.SerializedDictionary<string, CinemachineCamera> _cameraDictionary = new AYellowpaper.SerializedCollections.SerializedDictionary<string, CinemachineCamera>();

    private DG.Tweening.Sequence _shakeSequence;
    
    private CinemachineVirtualCameraBase _currentCamera;
    public CinemachineVirtualCameraBase CurrentCamera
    {
        get
        {
            if (_currentCamera == null)
            {
                CinemachineVirtualCameraBase currentCam = CinemachineCore.GetVirtualCamera(0);
                _currentCamera = currentCam;
            }

            return _currentCamera;
        }
        private set
        {
            _currentCamera = value;
            CurrentMultiChannel = CurrentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private CinemachineBasicMultiChannelPerlin _currentMultiChannel;
    private CinemachineBasicMultiChannelPerlin CurrentMultiChannel
    {
        get
        {
            Debug.Assert(CurrentCamera != null, "뭔데");
            if (_currentMultiChannel == null)
                _currentMultiChannel = CurrentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            return _currentMultiChannel;
        }
        set => _currentMultiChannel = value;
    }

    public void MoveToPlayerPositionImmediately()
    {
        CurrentCamera.ForceCameraPosition(CurrentCamera.Follow.position, Quaternion.identity);
    }

    public void ChangeCamera(CinemachineCamera camera)
    {
        CurrentMultiChannel.AmplitudeGain = 0;
        CurrentMultiChannel.FrequencyGain = 0;

        CurrentCamera.Priority = 10;
        CurrentCamera = camera;
        CurrentCamera.Priority = 11;
        CurrentMultiChannel = CurrentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ChangeCamera(string cameraName)
    {
        CurrentMultiChannel.AmplitudeGain = 0;
        CurrentMultiChannel.FrequencyGain = 0;

        CurrentCamera.Priority = 10;
        CurrentCamera = _cameraDictionary[cameraName];
        CurrentCamera.Priority = 11;
        CurrentMultiChannel = CurrentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    
    public void ShakeCamera(float amplitude, float frequency, float time, AnimationCurve curve, bool isAdd = true)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        float startAmp;
        float startFre;
        if (isAdd)
        {
            startAmp = CurrentMultiChannel.AmplitudeGain + amplitude * OptionData.GraphicSaveData.screenEffectValue;
            startFre = CurrentMultiChannel.FrequencyGain + frequency * OptionData.GraphicSaveData.screenEffectValue;
        }
        else
        {
            startAmp = Mathf.Max(CurrentMultiChannel.AmplitudeGain, amplitude) * OptionData.GraphicSaveData.screenEffectValue;
            startFre = Mathf.Max(CurrentMultiChannel.FrequencyGain, frequency) * OptionData.GraphicSaveData.screenEffectValue;
        }

        _shakeSequence
            .Append(
                DOTween.To(() => startAmp,
                value => CurrentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(curve))
            .Join(
                DOTween.To(() => startFre,
                value => CurrentMultiChannel.FrequencyGain = value,
                0, time).SetEase(curve));
    }
    public void ShakeCamera(float amplitude, float frequency, float time, Ease ease = Ease.Linear, bool isAdd = true)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        float startAmp;
        float startFre;
        if (isAdd)
        {
            startAmp = CurrentMultiChannel.AmplitudeGain + amplitude * OptionData.GraphicSaveData.screenEffectValue;
            startFre = CurrentMultiChannel.FrequencyGain + frequency * OptionData.GraphicSaveData.screenEffectValue;
        }
        else
        {
            startAmp = Mathf.Max(CurrentMultiChannel.AmplitudeGain, amplitude) * OptionData.GraphicSaveData.screenEffectValue;
            startFre = Mathf.Max(CurrentMultiChannel.FrequencyGain, frequency) * OptionData.GraphicSaveData.screenEffectValue;
        }

        _shakeSequence
            .Append(
                DOTween.To(() => startAmp,
                value => CurrentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(ease))
            .Join(
                DOTween.To(() => startFre,
                value => CurrentMultiChannel.FrequencyGain = value,
                0, time).SetEase(ease));
    }
}
