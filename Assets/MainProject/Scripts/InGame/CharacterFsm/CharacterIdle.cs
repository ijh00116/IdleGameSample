using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using DLL_Common.Common;
using System;
using Spine.Unity;

namespace BlackTree.InGame
{
    public class CharacterIdle : CharacterAbility, IStateCallback
    {
        [SerializeField]protected eActorState Mystate;
        public Action OnEnter => onEnter;
        public Action OnExit => onExit;
        public Action OnUpdate => onUpdate;

        protected MeshRenderer meshRenderer;

        
        protected override void Start()
        {
               base.Start();
               if(meshRenderer==null)
                   meshRenderer = _character._Skeletonanimator.GetComponent<MeshRenderer>();
               _State.AddState(Mystate, this);
        }

        protected virtual void onEnter()
        {
            if (meshRenderer == null)
                meshRenderer = _character._Skeletonanimator.GetComponent<MeshRenderer>();

            meshRenderer.material.SetFloat("_FillPhase", 0);
            if(_character.playertype==CharacterType.Player)
            {
                _character._Skeletonanimator.state.SetAnimation(0, "idle_1", true);
            }
            else
            {
                _character._Skeletonanimator.state.SetAnimation(0, "idle", true);
            }

            Spine.Animation[] animlist = _character._Skeletonanimator.skeleton.Data.Animations.ToArray();
            Spine.Skin[] skinlist= _character._Skeletonanimator.skeleton.Data.Skins.ToArray();
            int a = 0;
            string name;

            for(int i=0; i<animlist.Length; i++)
            {
                name = animlist[i].Name;
            }
            for (int i = 0; i < skinlist.Length; i++)
            {
                name = skinlist[i].Name;
            }
        }
        protected virtual void onUpdate()
        {
            
        }
        protected virtual void onExit()
        {

        }
    }
}
