using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.InGame
{
    public class DamageOnTouch : MonoBehaviour
    {
        public BigInteger DamageCaused;
        public LayerMask targetLayer;
        public bool IsDisappear;
        Health TriggeredHealth;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.gameObject.activeInHierarchy == false)
                return;
            if (BTLayers.LayerInLayerMask(collision.gameObject.layer, targetLayer) == false)
                return;
            if(collision.GetComponent<Health>()!=null)
            {
                TriggeredHealth = collision.GetComponent<Health>();
                DamageToHealth(TriggeredHealth);
                if (IsDisappear)
                    this.gameObject.SetActive(false);
            }
        }

        void DamageToHealth(Health _health)
        {

        }
    }

}
