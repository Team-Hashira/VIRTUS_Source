using UnityEngine;

namespace Hashira.StageSystem.Area
{
    public class ObjectTriggerArea : TriggerArea
	{
		public void FixedUpdate()
		{
			if (Physics2D.OverlapBox(transform.position, size, 0, whatIsTarget))
				Invoke();
		}
	}
}
