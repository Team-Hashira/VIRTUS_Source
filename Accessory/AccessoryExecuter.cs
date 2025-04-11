using UnityEngine;

namespace Hashira.Accessories
{
    public class AccessoryExecuter : MonoBehaviour
    {
        [SerializeField]
        private InputReaderSO _inputReader;

        [Header("====DEBUG====")]
        [SerializeField]
        private AccessorySO _accessory;
        [SerializeField]
        private EAccessoryType _type;

        private void Awake()
        {
            Accessory.EquipAccessory(_type, _accessory);

            _inputReader.OnAccessoryActiveEvent += ActiveSkill;
        }

        private void Update()
        {
            Accessory.GetAccessoryEffect(EAccessoryType.Passive)?.PassiveSkill();
        }

        private void ActiveSkill()
        {
            Accessory.GetAccessoryEffect(EAccessoryType.Active)?.ActiveSkill();
        }

        private void OnDestroy()
        {
            _inputReader.OnAccessoryActiveEvent -= ActiveSkill;
        }
    }
}
