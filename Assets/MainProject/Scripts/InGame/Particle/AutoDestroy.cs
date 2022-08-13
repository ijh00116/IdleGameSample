using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.InGame
{
    public class AutoDestroy : MonoBehaviour
    {
        [Header("0보다 작으면 2초 생존")]
        public float LifeTime=2;
        [BTReadOnly]
        public float CurrentTime;

        [SerializeField] bool IsDelete=false;
        private void OnEnable()
        {
            CurrentTime = 0.0f;
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (this.gameObject.activeInHierarchy == false)
                return;
            CurrentTime += Time.deltaTime;
            if(CurrentTime>((LifeTime<=0)?2:LifeTime))
            {
                CurrentTime = 0;
                this.gameObject.SetActive(false);
                if (IsDelete)
                    Destroy(this.gameObject);
            }
        }
    }

}
