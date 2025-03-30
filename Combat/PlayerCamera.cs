using Unity.Cinemachine;
using UnityEngine;

namespace Hashira
{
    public class PlayerCamera : MonoBehaviour
    {
        [field:SerializeField] public CinemachineConfiner2D CinemachineConfiner2D { get; private set; }
        [field:SerializeField] public CinemachineVirtualCameraBase CinemachineVirtualCameraBase { get; private set; }
    }
}
