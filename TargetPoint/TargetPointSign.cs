using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.TargetPoint.UI
{
	public class TargetPointSign : MonoBehaviour, IPoolingObject
	{
		public string OriginPoolType { get; set; }
		GameObject IPoolingObject.gameObject { get; set; }

		private SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void SetColor(Color color)
		{
			_spriteRenderer.color = color;
		}

		public void OnPop()
		{
		}

		public void OnPush()
		{
		}
	}
}
