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
    public class CharacterIdleInPet : CharacterIdle, IStateCallback
    {
        [Header("적 감지")]
        public LayerMask TargetLayer;
        public float distanceTotarget;
        public Color GizmoColor;
        public Transform DetectPosition;
        [HideInInspector]public GameObject CurrentEnemy;

        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;
            //Gizmos.DrawWireSphere(this.transform.position, distanceTotarget);
            if (DetectPosition != null)
            {
                Gizmos.DrawLine(DetectPosition.position, (Vector2)DetectPosition.position
            + ((this.transform.localScale.x < 0) ? Vector2.left : Vector2.right)
            * distanceTotarget);
            }

        }

        protected override void Start()
        {
            base.Start();
            _character = this.GetComponent<Character>();

            _character._state.ChangeState(eActorState.Idle);
        }

        protected override void onEnter()
        {
            if (meshRenderer == null)
                meshRenderer = _character._Skeletonanimator.GetComponent<MeshRenderer>();

            meshRenderer.material.SetFloat("_FillPhase", 0);
            _character._Skeletonanimator.state.SetAnimation(0, "idle_1", true);
        }
        protected override void onUpdate()
        {
            if (Common.InGameManager.Instance._PetsceneFsm._State.IsCurrentState(ePlaySubScene.PetDunGeonUpdate) == false)
                return;
            Vector2 dir = (transform.localScale.x < 0) ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(DetectPosition.position, dir, distanceTotarget, TargetLayer);

            if (hit.collider == null)
            {

            }
            else
            {
                //CurrentEnemy = hit.collider.gameObject;
                //_character.CurrentEnemy= hit.collider.gameObject;
                //_character.CurrentEnemyCharacter = _character.CurrentEnemy.GetComponent<Character>();
                //if (_character.CurrentEnemyCharacter._health.CurrentHealth >0)
                //{
                //    _character._state.ChangeState(eActorState.BaseAttack);
                //}
            }
        }

        protected override void onExit()
        {

        }
    }
}


