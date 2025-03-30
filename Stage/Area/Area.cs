using UnityEngine;
using UnityEngine.Events;

namespace Hashira.StageSystem.Area
{
    public abstract class Area : MonoBehaviour
    {
		public UnityEvent ClearEvent;
		public UnityEvent PlayerEnterEvent;
		public UnityEvent PlayerExitEvent;
	}
}
