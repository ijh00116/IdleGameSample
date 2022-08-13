using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using DLL_Common.Common;

namespace BlackTree.InGame
{
    public class MeleeWeapon : Weapon
    {
        public DamageOnTouch MeleeCollider;
        protected override void Start()
        {
            base.Start();
            
            MeleeCollider.gameObject.SetActive(false);
        }

        public override void WeaponDamageSetting(BigInteger damage)
        {
            base.WeaponDamageSetting(damage);
            MeleeCollider.DamageCaused = Damage;
        }

        public override void ShootProjectile(bool right)
        {
            base.ShootProjectile(right);
            //MeleeCollider.gameObject.SetActive(true);
        }

        public override void EndOfShootProjectile()
        {
            base.EndOfShootProjectile();
            //MeleeCollider.gameObject.SetActive(false);
        }
    }

}
