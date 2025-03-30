using UnityEngine;

namespace Hashira.Utils
{
    public class TextMeshProSortingLayer : MonoBehaviour
    {
        [field: SerializeField]
        public string SortingLayer { get; set; }
        [field: SerializeField]
        public int LayerOrder { get; set; }

        private void OnValidate()
        {
            var renderer = GetComponent<Renderer>();
            renderer.sortingLayerName = SortingLayer;
            renderer.sortingOrder = LayerOrder;
        }
    }
}
