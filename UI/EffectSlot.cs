using TMPro;
using UnityEngine;
using Hashira.EffectSystem;
using UnityEngine.UI;
using System;

namespace Hashira.UI.Effect
{
    [Obsolete]
    public class EffectSlot : MonoBehaviour
    {
        [SerializeField] private Image _coolTimeGauge;
        [SerializeField] private TextMeshProUGUI _coolTimeText;

        [SerializeField] private Image _iconImage;
        
        public EffectSystem.Effect effectBase;

        public void Init(EffectSystem.Effect effectBase)
        {
            //여기서 다른 UI 정보들까지 싹 다 초기화
            this.effectBase = effectBase;
            _coolTimeGauge.fillAmount = 0;
            _coolTimeText.text = string.Empty;
            //_iconImage.sprite = effectBase.CardSO.cardSprite;
        }

        private void Update()
        {
            {
                if (effectBase is ICoolTimeEffect coolTimeEffect)
                {
                    _coolTimeGauge.fillAmount = coolTimeEffect.LifeTime / coolTimeEffect.Duration;
                }
            }

            if (effectBase is ICountingEffect countingEffect)
            {
                // 0은 그리지 않음
                if (countingEffect.Count == 0) _coolTimeText.text = string.Empty;
                else _coolTimeText.text = (countingEffect.Count).ToString();
                return;
            }

            {
                if(effectBase is ICoolTimeEffect coolTimeEffect)
                    _coolTimeText.text = (coolTimeEffect.Duration - coolTimeEffect.LifeTime).ToString("0.0");
            }
        }

        public bool Equals(EffectSystem.Effect target)
        {
            return effectBase.name.Equals(target.name);
        }
    }
}
