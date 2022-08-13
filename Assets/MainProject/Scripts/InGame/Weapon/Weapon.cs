using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using DLL_Common.Common;

namespace BlackTree.InGame
{
    public class Weapon : MonoBehaviour
    {
        public StateMachine<WeaponState> weaponState;
        
        public int AttackFrame;
        public int AttackingFrame;
        public int AttackEndFrame;

        //public GameObject Player;

        public bool IsDontmoveWhenShoot;

        public BigInteger Damage;
        protected virtual void Start()
        {
            weaponState = new StateMachine<WeaponState>(this.gameObject, true);
            weaponState.ChangeState(WeaponState.Idle);
        }

        public virtual void WeaponDamageSetting(BigInteger damage)
        {
            Damage = damage;
            
        }

        public virtual void StartAttack()
        {

        }

        public virtual void ShootProjectile(bool right)
        {

        }

        public virtual void EndOfShootProjectile()
        {

        }

        public virtual void EndAttack()
        {

        }
    }

}
