using AYellowpaper.SerializedCollections;
using Hashira.Cards.Effects;
using Hashira.EffectSystem;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards
{
    public enum ECardType
    {
        Bullet,
        Stat,
        Magic
    }

    [CreateAssetMenu(fileName = "Card", menuName = "SO/Card/Card")]
    public class CardSO : ScriptableObject
    {
        [Header("==========CardSO==========")]
        public ECardType cardType;
        public Sprite cardborderSprite;
        public Sprite cardSprite;
        public string cardDisplayName;
        public string cardName;
        [TextArea]
        public string[] cardDescriptions;
        public int needCost;
        public int maxOverlapCount;

        public string effectClassName;
        private Type _effectType;

        [Doryu.CustomAttributes.Uncorrectable]
        [SerializeField] private string _effectTypeIs;


        protected virtual void OnValidate()
        {
            if (_effectType != null && _effectType.Name == effectClassName)
            {
                _effectTypeIs = _effectType?.Name;
                return;
            }

            FindType();

            _effectTypeIs = _effectType?.Name;
        }

        private void FindType()
        {
            string className = $"{typeof(CardEffect).Namespace}.{effectClassName}";
            try
            {
                _effectType = Type.GetType(className);
            }
            catch (Exception e)
            {
                Debug.LogError($"{className} is not found.\n" +
                    $"Error : {e.ToString()}");
            }
        }

        public virtual CardEffect GetEffectClass()
        {
            if (_effectType == null)
                FindType();

            _effectTypeIs = _effectType?.Name;

            CardEffect cardEffect = Activator.CreateInstance(_effectType) as CardEffect;
            cardEffect.Init(this);
            return cardEffect;
        }
    }
}
