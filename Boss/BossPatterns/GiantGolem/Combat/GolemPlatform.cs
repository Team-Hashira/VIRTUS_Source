using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class GolemPlatform : MonoBehaviour
    {
        [SerializeField] private Transform[] _platformPoints;

        public Transform[] GetPlatformPoints(int count)
        {
            if (count > _platformPoints.Length) return null;
            
            List<Transform> points = _platformPoints.ToList();

            for (int i = 0; i < _platformPoints.Length - count; i++)
            {
                points.RemoveAt(Random.Range(0, points.Count));
            }
            
            return points.ToArray();
        }
    }
}
