using Hashira.Accessories.Effects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Accessories
{
    [CreateAssetMenu(fileName = "AccessorySetSO", menuName = "SO/Accessory/AccessorySetSO")]
    public class AccessorySetSO : ScriptableObject
    {
        public List<AccessorySO> accessoryList;

        public void ResetAll()
        {
            if(accessoryList.Count > 0)
            {
                accessoryList.ForEach(accessory => accessory.GetEffectInstance<AccessoryEffector>().Reset());
            }
        }
    }
}
