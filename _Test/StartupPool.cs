using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira
{
    public abstract class StartupPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private PoolCategorySO _poolCategorySO;
        [SerializeField] private GameObject _poolablePrefab;
         
        private void Start()
        {
            if (_poolCategorySO == null || _poolablePrefab == null) return;

            foreach (PoolPair poolPair in _poolCategorySO.pairs)
            {
                if (poolPair.prefab == _poolablePrefab)
                {
                    string poolType = $"{_poolCategorySO.name}.{poolPair.poolType}";
                    IPoolingObject poolingObject = gameObject.Pop(poolType, transform);
                    T popedObject = poolingObject.gameObject.GetComponent<T>();
                    PopObjectSetting(popedObject);
                    break;
                }
            }
        }

        protected abstract void PopObjectSetting(T popedObject);

        private void OnValidate()
        {
            if (_poolablePrefab == null) return;

            if (_poolablePrefab.GetComponent<IPoolingObject>() == null)
            {
                _poolablePrefab = null;
                Debug.LogError($"{_poolablePrefab.name} is not IPoolingObject");
            }
            else if (_poolablePrefab.GetComponent<T>() == null)
            {
                _poolablePrefab = null;
                Debug.LogError($"{_poolablePrefab.name} is not {typeof(T).ToString()}");
            }
        }
    }
}
