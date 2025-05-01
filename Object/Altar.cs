using Crogen.CrogenPooling;
using Hashira.CanvasUI;
using Hashira.EffectSystem;
using Hashira.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Entities.Interacts
{

    [Serializable]
    public struct AltarInfo
    {
        public string effectName;
        public Sprite visualSprite;
    }
    
    public class Altar : KeyInteractObject
    {
        [SerializeField] private List<AltarInfo> _altarInfoList;
        [SerializeField] private EffectPoolType _giftVFXPoolType;
        [SerializeField] private Transform _visualTransform;
        private int _giftIndex;
        
        protected override void Awake()
        {
            base.Awake();
            _giftIndex = Random.Range(0, _altarInfoList.Count);
            _visualTransform.GetComponent<SpriteRenderer>().sprite = _altarInfoList[_giftIndex].visualSprite;
        }
        
        public override void Interaction(Player player)
        {
            base.Interaction(player);
            StartCoroutine(GiveEffectToPlayer(player));
    
            CanInteraction = false;
        }

        private IEnumerator GiveEffectToPlayer(Player player)
        {
            // 플레이어 구속
            _inputReader.PlayerActive(false);
            CanvasUI.UIManager.Instance.GetDomain<ToggleDomain, IToggleUI>().CloseUI("GameDataUIPanel");
            
            // Effect 주기
            yield return new WaitForSeconds(1f);
            CameraManager.Instance.ShakeCamera(10, 100, 0.75f);
            LightingControl.LightingController.Aberration(1, 0.75f);
            PopCore.Pop(_giftVFXPoolType, player.transform.position, Quaternion.identity);
            CreateAndAddEffect(player, _altarInfoList[_giftIndex].effectName);
            yield return new WaitForSeconds(1f);
            
            CanvasUI.UIManager.Instance.GetDomain<ToggleDomain, IToggleUI>().OpenUI("GameDataUIPanel");
            yield return new WaitForSeconds(1f);
            _inputReader.PlayerActive(true);
        }

        private void CreateAndAddEffect(Player player, string effectName)
        {
            Type type = Type.GetType($"Hashira.EffectSystem.Effects.{effectName}");
            EntityEffector effector = player.GetEntityComponent<EntityEffector>();
            
            Effect effect = Activator.CreateInstance(type) as Effect;
            effector.AddEffect(effect);
        }
    }
}
