using Hashira.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Accessories
{
    public enum EAccessoryType
    {
        None,
        Passive,
        Active,
    }

    public static class Accessory
    {
        private static Dictionary<EAccessoryType, AccessoryEffect> _AccessoryDict;
        public static IEnumerable<AccessoryEffect> Accessories => _AccessoryDict.Values;

        static Accessory()
        {
            _AccessoryDict = new Dictionary<EAccessoryType, AccessoryEffect>
            {
                { EAccessoryType.Passive, null },
                { EAccessoryType.Active, null },
            };
        }


        /// <summary>
        /// 장신구 효과 클래스 가져오는 함수
        /// </summary>
        /// <param name="type">None을 넣으려 하지마셈. 자꾸 슬롯에 장착이 안된걸 갖고오려하면 곤란함.</param>
        /// <returns></returns>
        public static AccessoryEffect GetAccessoryEffect(EAccessoryType type)
            => _AccessoryDict[type];

        /// <summary>
        /// 장신구 장착 함수
        /// </summary>
        /// <param name="type">Passive나 Active. None은 쓰지 마셈</param>
        /// <param name="accessory">대상 SO</param>
        /// <param name="broadcastChange">Change됐다는걸 장신구들에게 알려줄거냐는 뜻. 인게임 실시간 변경 아니면 false 유지</param>
        public static void EquipAccessory(EAccessoryType type, AccessorySO accessory, bool broadcastChange = false)
        {
            if (broadcastChange)
                _AccessoryDict[type]?.OnAccessoryTypeChange(EAccessoryType.None);
            _AccessoryDict[type] = accessory.GetEffectInstance();
            if (broadcastChange)
                _AccessoryDict[type]?.OnAccessoryTypeChange(type);
        }

        /// <summary>
        /// 장신구 장착해제 함수
        /// </summary>
        /// <param name="type">Passive나 Active. None은 쓰지 마셈</param>
        /// <param name="broadcastChange">Change됐다는걸 장신구들에게 알려줄거냐는 뜻. 인게임 실시간 변경 아니면 false 유지</param>
        public static void UnequipAccessory(EAccessoryType type, bool broadcastChange = false)
        {
            if (broadcastChange)
                _AccessoryDict[type]?.OnAccessoryTypeChange(EAccessoryType.None);
            _AccessoryDict[type] = null;
        }

        /// <summary>
        /// 장신구가 인게임이 아닌 다른 곳에서 수정 됐을 때(정비 등) 장신구가 작동할 수 있도록 해주는 함수.
        /// 그냥 인게임 들어가면 ApplyAll 한번씩 해줘야함. 플레이어가 해줄거임 아마.
        /// </summary>
        /// <param name="owner">장신구 적용 대상. Entity긴 한데 웬만해선 Player일거임.</param>
        public static void ApplyAll(Entity owner)
        {
            foreach (var pair in _AccessoryDict)
            {
                pair.Value?.Initialize(owner);
                pair.Value?.OnAccessoryTypeChange(pair.Key);
            }
        }

        public static void ResetAccessory()
        {
            _AccessoryDict = new Dictionary<EAccessoryType, AccessoryEffect>
            {
                { EAccessoryType.Passive, null },
                { EAccessoryType.Active, null },
            };
        }
    }
}
