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
    public class CharacterHit : CharacterAbility, HitforAnimationEvent,IStateCallback
    {
        [SerializeField]protected eActorState Mystate;
        public float InvinsibleTime;
        public bool IsInvinsible=true;
        public bool IsGetstun = true;

        public GameObject Hitparticle;
        public GameObject Bloodparticle;
        public GameObject DropBloodparticle;
        public GameObject Criticalparticle;

        public GameObject HitSkillParticle;
        public GameObject HitCriticalSkillParticle;

        [Header("hit이펙트 터질 위치 배열")]
        [SerializeField] public List<GameObject> hitEffectPosList = new List<GameObject>();

        [HideInInspector]public bool Invinsible;
        protected float CurrentInvinsibleTime;

        public Action OnEnter => onEnter;

        public Action OnExit => onExit;

        public Action OnUpdate => onUpdate;

        protected Coroutine WaitHitEnd;

        protected MeshRenderer meshRenderer;
        protected float animationLength;
        protected Health health;

        protected GameObject dropblood;
        protected override void Start()
        {
            base.Start();
            Invinsible = false;

            _character._state.AddState(Mystate, this);

            _character._Skeletonanimator.state.End += OnSpineAnimationEndInHit;

            meshRenderer = _character._Skeletonanimator.GetComponent<MeshRenderer>();

            health = this.GetComponent<Health>();
        }

        protected void OnEnable()
        {
            Invinsible = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected virtual void onEnter()
        {
            if(WaitHitEnd!=null)
            {
                StopCoroutine(WaitHitEnd);
                WaitHitEnd = null;
            }

            _character._Skeletonanimator.state.SetAnimation(0, "hit", false);

            animationLength = _character._Skeletonanimator.state.GetCurrent(0).Animation.Duration;
            HitEffectInstantiate();

            if (health.GetPercentageHealth<0.2f &&DropBloodparticle!=null && dropblood==null)
            {
                dropblood = Instantiate(DropBloodparticle);
                dropblood.transform.position= this.transform.position;
            }

            StopAllCoroutines();
            
        }

        public void HitEffectInstantiate()
        {
            if (health.hitType != HitType.Skill)
            {
                //hiteffect
                GameObject particle = Instantiate(Hitparticle);
                int hitposindex = UnityEngine.Random.Range(0, hitEffectPosList.Count);
                particle.transform.position = hitEffectPosList[hitposindex].transform.position;
                particle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)));
            }

            //bloodeffect
            float bloodRate = UnityEngine.Random.Range(0, 10);
            if (bloodRate < 3 && Bloodparticle != null)
            {
                GameObject blood = Instantiate(Bloodparticle);
                int bloodposindex = UnityEngine.Random.Range(0, hitEffectPosList.Count);
                blood.transform.position = hitEffectPosList[bloodposindex].transform.position;
            }
        }

        protected virtual void onExit()
        {

        }
        protected virtual void onUpdate()
        {
            animationLength -= Time.deltaTime;
            if(animationLength<0)
            {
                HitEnd();
            }
        }

        protected override void ProcessAbility()
        {
            CurrentInvinsibleTime += Time.deltaTime;
        }

        protected override void Animating()
        {
            base.Animating();
        }

        public void HitByattack()
        {
            if (IsInvinsible)
            {
                CurrentInvinsibleTime = 0;
                Invinsible = true;
            }
        }

        protected void OnSpineAnimationEndInHit(TrackEntry trackentry)
        {
            if(trackentry.Animation.Name=="hit")
            {
               
            }
        }
        public void HitEnd()
        {
            if (_character._state.IsCurrentState(eActorState.Dead))
                return;

            GotoIdle();
        }

        protected void GotoIdle()
        {
            _character._state.ChangeState(eActorState.Idle);
        }
    }

}
