using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.Entities;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Bosses.BillboardClasses
{
    /// <summary>
    /// Core Class
    /// </summary>
    public class BillboardValue {  }

    //<Unity>/////////////////////////////////////////////////////////////////////////////////////////
    [Serializable]public class GameObjectValue : BillboardValue { public GameObject Value; }
    [Serializable]public class TransformValue : BillboardValue { public Transform Value; }
    [Serializable]public class LayerMaskValue : BillboardValue { public LayerMask Value; }
    [Serializable]public class ContactFilter2DValue : BillboardValue { public ContactFilter2D Value; }
    [Serializable]public class UnityEventValue : BillboardValue { public UnityEvent Value; }
    //<Unity>/////////////////////////////////////////////////////////////////////////////////////////

    
    //<Type>//////////////////////////////////////////////////////////////////////////////////////////
    [Serializable]public class FloatValue : BillboardValue { public float Value; }
    [Serializable]public class IntValue : BillboardValue { public int Value; }
    [Serializable]public class BoolValue : BillboardValue { public bool Value; }
    [Serializable]public class StringValue : BillboardValue { public string Value; }
    [Serializable]public class Vector3Value : BillboardValue { public Vector3 Value; }
    [Serializable]public class Vector2Value : BillboardValue { public Vector2 Value; }
    //<Type>//////////////////////////////////////////////////////////////////////////////////////////

    
    //<Entity>////////////////////////////////////////////////////////////////////////////////////////
    [Serializable]public class EntityValue : BillboardValue { public Entity Value; }
    [Serializable]public class BossValue : BillboardValue { public Boss Value; }
    //<Entity>////////////////////////////////////////////////////////////////////////////////////////
    
    
    //<Boss>//////////////////////////////////////////////////////////////////////////////////////////
    [Serializable]public class GiantGolemEyeValue : BillboardValue { public GiantGolemEye Value; }
    [Serializable]public class GiantGolemPlatformListValue : BillboardValue { public GiantGolemPlatformList Value; }
    //<Boss>//////////////////////////////////////////////////////////////////////////////////////////
}
