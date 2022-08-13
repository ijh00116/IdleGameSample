using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlackTree.State;
using System;

namespace BlackTree.InGame
{
    public class CharacterMoveAfterBossKill : CharacterAbility,IStateCallback
    {
        [SerializeField] eActorState Mystate;

        Character character;
        [HideInInspector] public bool isMoving=false;

        //FSM
        public Action OnEnter { get { return onEnter; } }
        public Action OnExit { get { return onExit; } }
        public Action OnUpdate { get { return onUpdate; } }

        protected override void Start()
        {
            base.Start();
            character = this.GetComponent<Character>();

            _State.AddState(Mystate, this);
        }

        void onEnter()
        {
            if (isMoving == true)
                return;
            character._Skeletonanimator.state.SetAnimation(0, "run", true);
            isMoving = true;
            Common.InGameManager.Instance.CinemachineCamActive(false);
        }
        void onExit()
        {
            isMoving = false;
        }
        float currenttime;
        void onUpdate()
        {
            if (isMoving == false)
                return;
            
            character.transform.Translate(Vector3.right * 10 * Time.deltaTime);

            
        }
    }

}
