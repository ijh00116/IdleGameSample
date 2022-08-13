using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace BlackTree
{
    public class AnimationCallback : MonoBehaviour
    {
        public Dictionary<int, Action> animExitcallbacklist = new Dictionary<int, Action>();
        public delegate void ExitTrigger();
        public delegate void Trigger();

        public ExitTrigger exitTrigger;
        public Trigger attackTrigger;

        public ExitTrigger hitexitTrigger;

        private void Awake()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        public void AnimationOnExit(int Hashvalue)
        {
            if(animExitcallbacklist.ContainsKey(Hashvalue))
            {
                var _event = animExitcallbacklist[Hashvalue];
                _event?.Invoke();
            }
         
        }

        public void tag_atk()
        {
            attackTrigger?.Invoke();
        }

    }

}
