using Crogen.CrogenPooling;
using Hashira.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.TargetPoint.UI
{
	public class TargetPointContent : MonoBehaviour, IPoolingObject
	{
		public Transform targetTrasform;
		private Color _imageColor;
		public Color ImageColor
		{
			set
			{
				_imageColor = value;
				_image.color = _imageColor;
			}
			get => _imageColor;
		}

		public string OriginPoolType { get; set; }
		GameObject IPoolingObject.gameObject { get; set; }

		private Transform _playerCameraTrm;
		private Transform _playerTrm;

		[SerializeField] private Image _image;
		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = transform as RectTransform;
			_playerCameraTrm = Camera.main.transform;
			_playerTrm = PlayerManager.Instance?.Player.transform;
		}

		private void FixedUpdate()
		{
			if (targetTrasform == null) return;
			float minX = _image.GetPixelAdjustedRect().width / 2;
			float maxX = Screen.width - minX;

			float minY = _image.GetPixelAdjustedRect().height / 2;
			float maxY = Screen.height - minY;
			Vector2 originPos = Camera.main.WorldToScreenPoint(targetTrasform.position);
			Vector2 pos = originPos;

			if (Vector3.Dot((targetTrasform.position - _playerCameraTrm.position), _playerCameraTrm.forward) < 0)
			{
				if (pos.x < Screen.width / 2)
					pos.x = maxX;
				else
					pos.x = minX;
			}


			if(pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
			{
				_image.enabled = true;
			}
			else
			{
				_image.enabled = false;
			}

			pos.x = Mathf.Clamp(pos.x, minX, maxX);
			pos.y = Mathf.Clamp(pos.y, minY + 37.4f, maxY);

			Vector2 dir = (originPos - pos).normalized;
			_rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

			_image.transform.position = pos;
		}

		public void OnPop()
		{
		}

		public void OnPush()
		{
		}
	}

}