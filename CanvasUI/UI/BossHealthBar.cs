using Hashira.Bosses;
using Hashira.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class BossHealthBar : MonoBehaviour
    {
        private bool _isInitialized = false;
        
        private Slider _slider;
        public Boss Boss { get; private set; }
        private EntityHealth _entityHealth;
        
        public void Init(Boss boss)
        {
            Boss = boss;
            _entityHealth = boss.GetEntityComponent<EntityHealth>();
            
            _slider = GetComponentInChildren<Slider>();
            
            _isInitialized = true;
            
            _entityHealth.OnHealthChangedEvent += HandleHealthChanged;
        }

        
        private void Update()
        {
            if (_isInitialized == false) return;
        }

        private void OnDestroy()
        {
            if (_entityHealth)
                _entityHealth.OnHealthChangedEvent -= HandleHealthChanged;
        }

        private void HandleHealthChanged(int previous, int current)
        {
            _slider.value = (float)current / _entityHealth.MaxHealth;
        }
    }
}
