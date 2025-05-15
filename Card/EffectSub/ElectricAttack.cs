using Crogen.CrogenPooling;
using Hashira.Cards.Effects;
using System.Collections;
using UnityEngine;

namespace Hashira
{
    public class ElectricAttack : PushLifetime
    {
        [SerializeField] private ParticleSystem _electricAttack;

        public void Init(Vector3 startPos, Vector3 endPos)
        {
            Vector3 direction = endPos - startPos;
            
            var shapeModule = _electricAttack.shape;
            shapeModule.radius = Vector3.Magnitude(startPos - endPos) / 2;
            shapeModule.rotation = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            transform.position = (startPos + endPos) / 2;

        }
    }
}
