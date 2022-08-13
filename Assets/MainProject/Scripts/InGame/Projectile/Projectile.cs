using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using DLL_Common.Common;

namespace BlackTree.InGame
{
    public class Projectile : PoolableObject
    {
        bool Speed;
        public bool Right;

        public bool NotMove = false;
        public void ShootStart(bool right,BigInteger damage)
        {
            GetComponent<DamageOnTouch>().DamageCaused = damage;
            ShootStart(right);
        }
        public void ShootStart(bool right)
        {
            Right = right;
            Vector3 localscale = transform.localScale;

            if (Right && localscale.x < 0)
                localscale *= -1;
            if (Right == false && localscale.x > 0)
                localscale *= -1;
            transform.localScale = localscale;
        }

        private void FixedUpdate()
        {
            if (NotMove)
                return;

            if(Right)
                transform.Translate(Vector3.right);
            else
                transform.Translate(Vector3.left);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawSphere(this.transform.position, 1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

        }
    }

}
