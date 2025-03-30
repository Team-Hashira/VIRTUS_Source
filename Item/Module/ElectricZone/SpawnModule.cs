using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Items.Cards
{ 
    //public class SpawnModule : Card, ICloneable, IStatable
    //{
    //    /// <summary>
    //    /// 처음 실행될때까지의 딜레이
    //    /// </summary>
    //    [SerializeField] private float _cooldownStartDelay;
    //    /// <summary>
    //    /// 쿨타임
    //    /// </summary>
    //    [SerializeField] private float _cooldown;
    //    /// <summary>
    //    /// 쿨이 다시 돌기 시작할때까지의 시간
    //    /// </summary>
    //    [SerializeField] private float _cooldownDelay;
    //    [SerializeField] private OtherPoolType _poolingObject;
    //    private bool _isFirst;

    //    public override void Equip(Player player)
    //    {
    //        base.Equip(player);
    //        CooldownUtillity.StartCooldown("SpawnModuleStartDelay");
    //        _isFirst = true;
    //    }

    //    public override void ItemUpdate()
    //    {
    //        base.ItemUpdate();


    //        if (CooldownUtillity.CheckCooldown("SpawnModuleStartDelay", _cooldownStartDelay))
    //        {
    //            if (CooldownUtillity.CheckCooldown("SpawnModule", _cooldown, _isFirst))
    //            {
    //                _isFirst = false;
    //                _player.gameObject.Pop(_poolingObject, _player.transform.position, Quaternion.identity);
    //                CooldownUtillity.StartCooldown("SpawnModuleDelay");
    //            }

    //            if (CooldownUtillity.CheckCooldown("SpawnModuleDelay", _cooldownDelay, false))
    //                CooldownUtillity.StartCooldown("SpawnModule");
    //        }

                
    //    }

    //    public override void UnEquip()
    //    {
    //        base.UnEquip();
    //    }
    //}
}
