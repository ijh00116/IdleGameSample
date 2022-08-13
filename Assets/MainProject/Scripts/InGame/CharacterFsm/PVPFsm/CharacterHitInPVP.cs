using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using System;
using Spine;
using Spine.Unity;

namespace BlackTree.InGame
{
    public class CharacterHitInPVP : CharacterHit, HitforAnimationEvent, IStateCallback
    {
        protected override void Start()
        {
            base.Start();
            _character = this.GetComponent<CharacterInPVP>();
            health = this.GetComponent<HealthInPVP>();
        }


        protected override void onEnter()
        {
            if (WaitHitEnd != null)
            {
                StopCoroutine(WaitHitEnd);
                WaitHitEnd = null;
            }

           // _character._Skeletonanimator.state.SetAnimation(0, "idle", false);

            //hiteffect
            GameObject particle = Instantiate(Hitparticle);
            int hitposindex = UnityEngine.Random.Range(0, hitEffectPosList.Count);
            particle.transform.position = hitEffectPosList[hitposindex].transform.position;
            particle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)));

            //bloodeffect
            float bloodRate = UnityEngine.Random.Range(0, 10);
            if (bloodRate < 3 && Bloodparticle != null)
            {
                GameObject blood = Instantiate(Bloodparticle);
                int bloodposindex = UnityEngine.Random.Range(0, hitEffectPosList.Count);
                blood.transform.position = hitEffectPosList[bloodposindex].transform.position;
            }

            if (health.GetPercentageHealth < 0.2f && DropBloodparticle != null && dropblood == null)
            {
                dropblood = Instantiate(DropBloodparticle);
                dropblood.transform.position = this.transform.position;
            }

            StopAllCoroutines();
            StartCoroutine(HitEffectDissapear());
        }

        IEnumerator HitEffectDissapear()
        {
            meshRenderer.material.SetFloat("_FillPhase", 1);
            yield return null;
            yield return null;

            meshRenderer.material.SetFloat("_FillPhase", 0);
            GotoIdle();
        }

        protected override void onUpdate()
        {
            animationLength -= Time.deltaTime;
            if (animationLength < 0)
            {
                HitEnd();
            }
        }

    }

}
