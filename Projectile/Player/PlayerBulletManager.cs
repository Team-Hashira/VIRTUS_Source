//using Hashira.Core;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Hashira.Projectiles.Player
//{
//    public class PlayerBulletManager : MonoSingleton<PlayerBulletManager>
//    {
//        protected List<ProjectileModifier> _projectileModifiers = new List<ProjectileModifier>();
//        private Attacker _playerAttacker;

//        public int HandleOnModifier { get; private set; }

//        public List<ProjectileModifier> GetModifierList => _projectileModifiers;

//        public void EquipBulletModifier(ProjectileModifier projectileModifier)
//        {
//            _playerAttacker ??= PlayerManager.Instance.Player.Attacker;
//            _projectileModifiers.Add(projectileModifier);
//            projectileModifier.OnEquip(_playerAttacker);
//        }

//        public void UnEquipBulletModifier(ProjectileModifier projectileModifier)
//        {
//            _projectileModifiers.Remove(projectileModifier);
//            projectileModifier.OnUnEquip();
//        }
//    }
//}
