//using Hashira.Entities.Interacts;
//using Hashira.Items;
//using Hashira.Players;
//using UnityEngine;
//using UnityEngine.Events;

//namespace Hashira.Stage
//{
//    public class ItemDropBox : MonoBehaviour, IInteractable
//    {
//        [SerializeField] private ItemSO[] items;
//        [SerializeField] private Vector2 _dropOffset;
//        [SerializeField] private UnityEvent OpenEvent;

//        public bool CanInteraction { get; set; } = true;

//        private bool _isInteractable = true;

//        private bool _isOpen = false;

//        public void Interaction(Player player)
//        {
//            if (_isInteractable == false) return;
//            if (_isOpen == false) return;

//            _isOpen = true;

//            for (int i = 0; i < items.Length; i++)
//            {
//                ItemDropUtility.DroppedItem(items[i].GetItemClass(), (Vector2)transform.position + _dropOffset);
//            }

//            OpenEvent?.Invoke();
//        }

//        public void OffInteractable()
//        {
//            _isInteractable = false;
//        }

//        public void OnInteractable()
//        {
//            _isInteractable = true;
//        }
//    }
//}
