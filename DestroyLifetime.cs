using UnityEngine;

public class DestroyLifetime : Lifetime
{
    public override void DelayDie()
    {
        Destroy(gameObject);
    }
}
