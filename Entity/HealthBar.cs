using Hashira.Entities;
using UnityEngine;

namespace Hashira.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private EntityHealth _entityHealth;
        [SerializeField] private Transform _health, _changedHealth;

        [Header("Health changing setting")]
        [SerializeField] private float _downSpeed = 8;
        [SerializeField] private float _changedBarWaitTime = 0.5f;

        private int _targetHealth;
        private float _targetHealthAmount;
        private float _lastDownTime;

        private void Awake()
        {
            _entityHealth.OnHealthChangedEvent += HandleHealthChangedEvent;
            _lastDownTime = Time.time;
        }

        private void Start()
        {
            HandleHealthChangedEvent(_entityHealth.Health, _entityHealth.Health);
        }

        private void HandleHealthChangedEvent(int prevHealth, int newHealth)
        {
            _targetHealth = newHealth;
            _targetHealthAmount = (float)newHealth / _entityHealth.MaxHealth;

            if (prevHealth > newHealth)
            {
                _lastDownTime = Time.time;
            }
        }

        private void Update()
        {
            if (Mathf.Abs(_health.localScale.x - _targetHealthAmount) > Mathf.Epsilon)
            {
                float healthChangeAmount = Mathf.Lerp(_health.localScale.x,
                    _targetHealthAmount, Time.deltaTime * _downSpeed);
                _health.localScale = new Vector3(healthChangeAmount, 1, 1);
            }
            if (Mathf.Abs(_changedHealth.localScale.x - _targetHealthAmount) > Mathf.Epsilon
                && _lastDownTime + _changedBarWaitTime < Time.time)
            {
                float healthChangeAmount = Mathf.Lerp(_changedHealth.localScale.x,
                    _targetHealthAmount, Time.deltaTime * _downSpeed);
                _changedHealth.localScale = new Vector3(healthChangeAmount, 1, 1);
            }
        }
    }
}