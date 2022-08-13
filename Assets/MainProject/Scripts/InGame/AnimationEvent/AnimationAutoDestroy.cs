using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class AnimationAutoDestroy : MonoBehaviour
    {
        [SerializeField]float  DestroyTime=-1;
        public void AutoDestroyWhenAnimEnd()
        {
            if(DestroyTime>0)
            {
                Invoke("DestroyObject", DestroyTime);
            }
            else
            {
                DestroyObject();
            }
          
        }

        void DestroyObject()
        {
            Destroy(this.gameObject);
        }
    }


}
