using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;

namespace BlackTree.InGame
{
    public class DoubleProjectileWeapon : Weapon
    {
        protected BTMultipleObjectPooler objectPooler;


        protected override void Start()
        {
            base.Start();
            objectPooler = GetComponent<BTMultipleObjectPooler>();
        }

        public override void ShootProjectile(bool right)
        {
            base.ShootProjectile(right);
            PoolableObject[] pooledObjs = objectPooler.GetPooledObject();
            for(int i=0; i< pooledObjs.Length; i++)
            {
                Projectile projectile = (Projectile)pooledObjs[i];
                //projectile.ShootStart(right);
                projectile.ShootStart(right, Damage);
            }
        }

        public override void EndOfShootProjectile()
        {
            base.EndOfShootProjectile();
        }
    }
}