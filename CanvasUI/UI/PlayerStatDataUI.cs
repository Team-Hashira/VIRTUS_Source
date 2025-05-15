using DG.Tweening;
using Hashira.Core;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class PlayerStatDataUI : MonoBehaviour
    {
        [SerializeField] private StatElementSO _targetStat;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _statValue;
        [SerializeField] private bool _isCeiling;
        private StatDictionary _playerStatDictionary;
        private Sequence _statTextSequence;

        private void Start()
        {
            _playerStatDictionary = PlayerManager.Instance.Player
                .GetEntityComponent<EntityStat>().StatDictionary;

            _playerStatDictionary[_targetStat].OnValueChanged += HandleValueChanged;
            _icon.sprite = _targetStat.statIcon;
            float value = _playerStatDictionary[_targetStat].Value;
            HandleValueChanged(value, value);
        }

        private void HandleValueChanged(float prev, float current)
        {
            _statTextSequence?.Kill();
            _statTextSequence = DOTween.Sequence();
            _statTextSequence
                .Append(_statValue.rectTransform.DOAnchorPosY(10f * Mathf.Sign(current - prev), 0.1f))
                .Append(_statValue.rectTransform.DOAnchorPosY(0, 0.1f));
            _statValue.SetText(_isCeiling ? Mathf.CeilToInt(current).ToString() : current.ToString("F2"));
        }

        private void OnDestroy()
        {
            if (_playerStatDictionary != null)
                _playerStatDictionary[_targetStat].OnValueChanged -= HandleValueChanged;
            _statTextSequence?.Kill();
        }
    }
}
