using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

namespace BlackTree
{
    public class DieEffectControll : MonoBehaviour
    {
        [SerializeField]List<SkeletonAnimation> animList=new List<SkeletonAnimation>();
        private void Start()
        {
            for (int i = 0; i < animList.Count; i++)
            {
                animList[i].state.Complete+= OnSpineAnimationEndInAttack;
            }
        }
        public void AppearEffect(string animname)
        {
            this.gameObject.SetActive(true);
            for (int i=0; i< animList.Count; i++)
            {
                animList[i].state.SetAnimation(0, animname, false);
            }
            StartCoroutine(Expired());
        }
        WaitForSeconds wait = new WaitForSeconds(0.833f);
        IEnumerator Expired()
        {
            yield return wait;
            this.gameObject.SetActive(false);
        }
        protected virtual void OnSpineAnimationEndInAttack(TrackEntry trackentry)
        {

        }
    }

}
