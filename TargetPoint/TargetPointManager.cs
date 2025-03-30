using System.Collections.Generic;
using UnityEngine;
using Crogen.CrogenPooling;
using Hashira.TargetPoint.UI;

namespace Hashira.TargetPoint
{
	public struct TargetPointPair
	{
		public TargetPointPair(TargetPointContent targetPointContent, TargetPointSign targetPointSign)
		{
			this.targetPointContent = targetPointContent;
			this.targetPointSign = targetPointSign;
		}

		public TargetPointContent targetPointContent;
		public TargetPointSign targetPointSign;
	}

	public class TargetPointManager : MonoSingleton<TargetPointManager>
	{
		//[SerializeField] private Transform _parentCanvas;
		//[SerializeField] private WorldUIPoolType _targetPointContentPoolType = WorldUIPoolType.TargetPointContent;
		//[SerializeField] private WorldUIPoolType _targetPointSignPoolType = WorldUIPoolType.TargetPointMapSign;

		//private Dictionary<Transform, TargetPointPair> _targetPointContentDictionary = new Dictionary<Transform, TargetPointPair>();

		//public void ShowTargetPoint(Transform trm, Color color)
		//{
		//	if (_targetPointContentDictionary.ContainsKey(trm)) return;

		//	TargetPointContent targetPoint = gameObject.Pop(_targetPointContentPoolType, _parentCanvas) as TargetPointContent;
		//	TargetPointSign targetPointSign = gameObject.Pop(_targetPointSignPoolType, trm) as TargetPointSign;
		//	targetPointSign.SetColor(color);
		//	if (targetPoint == null) return;

		//	_targetPointContentDictionary.Add(trm, new TargetPointPair(targetPoint, targetPointSign));
		//	targetPoint.targetTrasform = trm;
		//	targetPoint.ImageColor = color;
		//}

		//public void CloseTargetPoint(Transform trm)
		//{
		//	if (!_targetPointContentDictionary.ContainsKey(trm)) return;

		//	TargetPointPair targetPointPiar = _targetPointContentDictionary[trm];
		//	if (targetPointPiar.targetPointContent == null) return;

		//	_targetPointContentDictionary.Remove(trm);
		//	targetPointPiar.targetPointContent.Push();
		//	targetPointPiar.targetPointSign.Push();
		//}

		//private void OnDestroy()
		//{
		//	_targetPointContentDictionary.Clear();
		//}
	}
}
