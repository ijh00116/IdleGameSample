using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;

namespace BlackTree.InGame
{
    public class PoolableObject : MonoBehaviour
    {
        public float LifeTime;

        protected float currenttime;

        public bool NotDead = false;
        protected virtual void OnEnable()
        {
            currenttime = 0.0f;
        }
        protected virtual void Update()
        {
            if (NotDead)
                return;

            currenttime += Time.deltaTime;
            if(currenttime>LifeTime)
            {
                this.gameObject.SetActive(false);
            }
        }

        protected virtual void OnDisable()
        {
            
        }
    }

}
