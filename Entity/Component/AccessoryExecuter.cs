using Hashira.Accessories;
using Hashira.Core;
using Hashira.StageSystem;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Hashira.Accessories
{
    public class AccessoryExecuter : MonoBehaviour
    {
        [SerializeField]
        private InputReaderSO _inputReader;

        [Header("====DEBUG====")]
        [SerializeField]
        private AccessorySO _accessory;

        private void Awake()
        {
            Accessory.EquipAccessory(EAccessoryType.Passive, _accessory, true);
        }

        private void Update()
        {
            Accessory.GetAccessoryEffect(EAccessoryType.Passive)?.PassiveSkill();
        }

        private void ActiveSkill()
        {
            Accessory.GetAccessoryEffect(EAccessoryType.Active)?.ActiveSkill();
        }
    }
}
