using Crogen.CrogenPooling;
using System.Collections;
using UnityEngine;

namespace Hashira.VFX
{
	public class SoundEffect : MonoBehaviour, IPoolingObject
	{
		public string OriginPoolType { get; set; }
		GameObject IPoolingObject.gameObject { get; set; }

		private Transform _visualTrm;
		private SpriteRenderer _spriteRenderer;
		private readonly int _sizeValueID = Shader.PropertyToID("_Size");
		private readonly int _alphaValueID = Shader.PropertyToID("_Alpha");

		private void Awake()
		{
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			_visualTrm = _spriteRenderer.transform;
		}

		public void Init(float scale)
		{
			_visualTrm.localScale = Vector3.one * scale;
			StartCoroutine(SetSizeCoroutine());
		}

		public void OnPop()
		{
		}

		public void OnPush()
		{
		}

		private IEnumerator SetSizeCoroutine()
		{
			float percent = 0;
			float curTime = 0;
			float duration = 0.2f;
			while (curTime < duration)
			{
				percent = curTime / duration;
				_spriteRenderer.material.SetFloat(_sizeValueID, EaseOutExpo(percent));
				_spriteRenderer.material.SetFloat(_alphaValueID, EaseOutExpo(1-percent));
				curTime += Time.deltaTime;

				yield return null;
			}

			yield return new WaitForSeconds(duration);
			_spriteRenderer.material.SetFloat(_sizeValueID, 1);
			this.Push();
		}

		float EaseOutExpo(float x)
		{
			return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
		}

		float EaseInExpo(float x)
		{
			return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
		}
	}
}
