using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;

namespace BlackTree.InGame
{
    public class ProjectileWeapon : Weapon
    {
        protected BTSimpleObjectPooler objectPooler;
      

        protected override void Start()
        {
            base.Start();
            objectPooler = GetComponent<BTSimpleObjectPooler>();
        }

        public override void ShootProjectile(bool right)
        {
            if (objectPooler == null)
            {
                objectPooler = GetComponent<BTSimpleObjectPooler>();
                objectPooler.FillObjects();
            }
                
            base.ShootProjectile(right);
            Projectile projectile =(Projectile)objectPooler.GetPooledObject();
            projectile.ShootStart(right);
        }

        public override void EndOfShootProjectile()
        {
            base.EndOfShootProjectile();
        }
    }

}
