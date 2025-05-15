using Hashira.Accessories.Effects;
using Hashira.Core;
using Hashira.StageSystem;
using System;
using UnityEngine;

namespace Hashira.Accessories
{
    public class AccessoryExecuter : MonoBehaviour
    {
        [SerializeField]
        private InputReaderSO _inputReader;

        [Header("====DEBUG====")]
        [SerializeField]
        private AccessorySO _passiveAccessory;
        [SerializeField]
        private AccessorySO _activeAccessory;

        private void Awake()
        {
#if UNITY_EDITOR
            if (_passiveAccessory != null)
                Accessory.EquipAccessory(EAccessoryType.Passive, _passiveAccessory);
            if (_activeAccessory != null)
                Accessory.EquipAccessory(EAccessoryType.Active, _activeAccessory);
#endif

            _inputReader.OnAccessoryActiveEvent += ActiveSkill;
            StageGenerator.Instance.OnGeneratedStageEvent += HandleOnGeneratedStage;
            StageGenerator.Instance.OnNextStageEvent += HandleOnNextStage;
        }

        private void HandleOnNextStage()
        {
            foreach (var accessory in Accessory.Accessories)
            {
                if (accessory != null && accessory.CurrentEffect is IInitializeOnNextStage nextStage)
                    nextStage.OnNextStage();
            }
        }

        private void HandleOnGeneratedStage()
        {
            foreach (var accessory in Accessory.Accessories)
            {
                if (accessory != null && accessory.CurrentEffect is IInitializeOnStageStart startStage)
                    startStage.OnStageStart();
            }
        }

        private void Update()
        {
            foreach(var effector in Accessory.Accessories)
            {
                if (effector == null)
                    continue;
                if(effector.CurrentEffect is IUpdatableEffect updatableEffect)
                {
                    updatableEffect.OnUpdate();
                }
            }
        }

        private void ActiveSkill()
        {
            Debug.Log("님ㅇㅏ");
            Accessory.GetAccessoryEffect(EAccessoryType.Active)?.ActiveSkill();
        }

        private void OnDestroy()
        {
            if (StageGenerator.Instance != null)
            {
                StageGenerator.Instance.OnNextStageEvent -= HandleOnNextStage;
                StageGenerator.Instance.OnGeneratedStageEvent -= HandleOnGeneratedStage;
            }
            _inputReader.OnAccessoryActiveEvent -= ActiveSkill;
        }
    }
}
