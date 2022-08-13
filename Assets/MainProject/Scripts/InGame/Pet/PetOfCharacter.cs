using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

namespace BlackTree
{
    public class PetOfCharacter : MonoBehaviour
    {
        [HideInInspector] public PetData petData;
        SkeletonAnimation _skeletonanimation;
        public void DataSet()
        {
            if (petData == null)
            {
                this.gameObject.SetActive(false);
                return;
            }

            this.gameObject.SetActive(true);

            //스케일
            this.transform.localScale = new Vector3(petData.petInfo.Scale*2.28f, petData.petInfo.Scale * 2.28f);
            this.transform.localPosition = Vector3.zero;
            if(_skeletonanimation==null)
            {
                _skeletonanimation = this.GetComponent<SkeletonAnimation>();
            }

            string currentname = _skeletonanimation.AnimationName;
            if (currentname == null)
            {
                currentname = "run";
            }

            if (_skeletonanimation.skeletonDataAsset!=petData.SpineData
                || _skeletonanimation.skeletonDataAsset==null)
            {
                _skeletonanimation.skeletonDataAsset = petData.SpineData;
                _skeletonanimation.Initialize(true);
            }
           

        }

        public void SetAnimation(string animName)
        {
            if (_skeletonanimation == null)
            {
                _skeletonanimation = this.GetComponent<SkeletonAnimation>();
            }
            if (_skeletonanimation.state == null)
                return;

            _skeletonanimation.state.SetAnimation(0, animName, true);
        }
    }

}
