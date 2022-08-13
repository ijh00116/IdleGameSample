using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlackTree.State;
using System;
using Spine;
using Spine.Unity;

namespace BlackTree.InGame
{
    public class PetControllerInPetdungeon : MonoBehaviour, IStateCallback
    {
        [SerializeField] eActorState Mystate;
        public Action OnEnter => onEnter;
        public Action OnExit => onExit;
        public Action OnUpdate => onUpdate;

        [HideInInspector]public Vector3 startPos;
        [HideInInspector] public Vector3 endPos;

        Character character;

        SkeletonDataAsset spineasset;
        void Awake()
        {
            character = this.GetComponent<Character>();

            character._state.AddState(Mystate, this);
        }

        void Start()
        {
     
        }

        void OnEnable()
        {
         
        }
        void onEnter()
        {
            MoveNextPosition();
        }

        void onExit()
        {

        }

        void onUpdate()
        {

        }

        public void SetPositionData(Transform startingpoint,Transform endpoint,SkeletonDataAsset _spineasset)
        {
            Vector2 randomstart= UnityEngine.Random.insideUnitCircle;
            Vector2 randomend = UnityEngine.Random.insideUnitCircle * 1.4f;
            startPos = startingpoint.position+(Vector3)randomstart;
            endPos = endpoint.position + (Vector3)randomend;

            if(endPos.x-startPos.x <0 &&this.transform.localScale.x>0)
            {
                flip();
            }
            if (endPos.x - startPos.x > 0 && this.transform.localScale.x < 0)
            {
                flip();
            }
            this.transform.position = startPos;

            spineasset = _spineasset;
            if (spineasset != null)
            {
                character._Skeletonanimator.skeletonDataAsset = spineasset;
                //Debug.LogError(spineasset.name);
                character._Skeletonanimator.Initialize(true);
            }
        }

        void MoveNextPosition()
        {
            character._Skeletonanimator.state.SetAnimation(0, "run", true);
            character.CharacterPetChangeAnim("run");
            if (CharacterDataManager.Instance.PlayerCharacterdata == null)
                return;

            float moveTime =3.0f;
            StartCoroutine(Movec(moveTime));
        }

        IEnumerator Movec(float moveTime)
        {
            float Lerptime = 0;
            Vector3 _position = transform.position;
            //이동중
            while (Lerptime < 1)
            {
                Lerptime += Time.deltaTime/moveTime;
                transform.position = Vector3.Lerp(_position, endPos, Lerptime);

                yield return null;
            }

            flip();
            character._state.ChangeState(eActorState.Idle);
        }


        void flip()
        {
            Vector2 localscale = transform.localScale;
            localscale.x *= -1;
            transform.localScale = localscale;
        }
    }
}
