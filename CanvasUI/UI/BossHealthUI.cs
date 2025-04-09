using DG.Tweening;
using Hashira.Bosses;
using Hashira.Entities;
using Hashira.StageSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _bossNameText;
        [SerializeField] private TextMeshProUGUI _healthText;
        private Slider _slider;
        private EntityHealth _entityHealth;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _slider = GetComponentInChildren<Slider>();
        }

        private void Start()
        {
            _rectTransform.anchoredPosition = new Vector2(0, 145);
            if (StageGenerator.Instance.IsCurrentBossStage(out Boss boss))
            {
                _entityHealth = boss.GetEntityComponent<EntityHealth>();
                _bossNameText.text = boss.BossDisplayName;
                _healthText.text = "100.00%";
                _rectTransform.DOAnchorPosY(0, 1f);
                _entityHealth.OnHealthChangedEvent += HandleHealthChanged;
            }
        }

        private void OnDestroy()
        {
            if (_entityHealth)
                _entityHealth.OnHealthChangedEvent -= HandleHealthChanged;
        }

        private void HandleHealthChanged(int previous, int current)
        {
            _slider.value = (float)current / _entityHealth.MaxHealth;
            _healthText.text = $"{((float)_entityHealth.Health/_entityHealth.MaxHealth*100f):F}%";
        }
    }
}
