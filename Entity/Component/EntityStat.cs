using Hashira.Core.StatSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityStat : MonoBehaviour, IEntityComponent, IStatable
    {
        [SerializeField] private List<StatElement> _overrideStatElementList = new List<StatElement>();
        [SerializeField] private StatBaseSO _baseStat;

        public StatDictionary StatDictionary { get; private set; }

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _baseStat = Instantiate(_baseStat);
            StatDictionary = new StatDictionary(_overrideStatElementList, _baseStat);
        }

#if UNITY_EDITOR
        private void OnValidate()
		{
            _overrideStatElementList.ForEach(e => e.Name = e.elementSO.statName);
		}
#endif 
    }
}