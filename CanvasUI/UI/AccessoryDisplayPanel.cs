using AYellowpaper.SerializedCollections;
using Hashira.Accessories;
using System;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class AccessoryDisplayPanel : MonoBehaviour
    {
        [SerializeField]
        [SerializedDictionary("Type", "UI")]
        private SerializedDictionary<EAccessoryType, AccessoryDisplayUI> _accessoryDisplayUIDict;

        private void Awake()
        {
            Accessory.OnAccessoryChangeEvent += HandleOnAccessoryChangeEvent;
            HandleOnAccessoryChangeEvent(EAccessoryType.None, EAccessoryType.Passive);
            HandleOnAccessoryChangeEvent(EAccessoryType.None, EAccessoryType.Active);
        }

        private void HandleOnAccessoryChangeEvent(EAccessoryType previous, EAccessoryType current)
        {
            if (current == EAccessoryType.None)
                _accessoryDisplayUIDict[previous]?.Display(null);
            else
                _accessoryDisplayUIDict[current]?.Display(Accessory.GetAccessoryEffect(current)?.AccessorySO);
        }

        private void OnDestroy()
        {
            Accessory.OnAccessoryChangeEvent -= HandleOnAccessoryChangeEvent;
        }
    }
}
