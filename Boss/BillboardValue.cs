using Hashira.Bosses.Patterns;
using Hashira.Bosses.Patterns.GiantGolem;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Bosses.BillboardClasses
{
    /// <summary>
    /// Core Class
    /// </summary>
    [Serializable]
    public class BillboardValue {  }

    public class GameObjectValue : BillboardValue { public GameObject Value; }

    public class TransformValue : BillboardValue { public Transform Value; }

    public class LayerMaskValue : BillboardValue { public LayerMask Value; }

    public class ContactFilter2DValue : BillboardValue { public ContactFilter2D Value; }

    public class UnityEventValue : BillboardValue { public UnityEvent Value; }

    public class GiantGolemHandValue : BillboardValue { public GiantGolemHand Value; }
    
    public class GiantGolemEyeValue : BillboardValue { public GiantGolemEye Value; }
    
    public class GiantGolemObstacleValue : BillboardValue { public GiantGolemObstacle Value; }

}
