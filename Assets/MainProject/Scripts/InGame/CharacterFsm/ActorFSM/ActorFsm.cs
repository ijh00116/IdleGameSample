using BlackTree.InGame;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public enum eActorState
    {
        DoNothing = -1,
        
        Idle,
        Move,
        BaseAttack,
        Hit,
        Skill,
        Dead,
        EventAfterKillBoss,

        End
    }
    public class ActorFsm : MonoBehaviour,IRegistStateCallback
    {
        public ActorStateMachine<eActorState> _State = null;

        Character myCharacter;

        public void Initialize(Character _character)
        {
            if (_State == null)
                _State = ActorStateMachine<eActorState>.Initialize(this);
            else
                _State.ChangeState(eActorState.DoNothing);

            myCharacter = _character;
            _State.ChangeState(eActorState.DoNothing);
        }

        public void RegistCallback()
        {
            StateCallback _statecallback = new StateCallback() { OnEnter = Idle_Enter, Update = Idle_Update, OnExit = Idle_Exit };
            _State.stateLookup.Add(eActorState.Idle, _statecallback);
            _statecallback = new StateCallback() { OnEnter = Attack_Enter, Update = Attack_Enter, OnExit = Attack_Enter };
            _State.stateLookup.Add(eActorState.BaseAttack, _statecallback);
            _statecallback = new StateCallback() { OnEnter = Move_Enter, Update = Move_Update, OnExit = Move_Exit};
            _State.stateLookup.Add(eActorState.Move, _statecallback);
            _statecallback = new StateCallback() { OnEnter = Hit_Enter, Update = Hit_Update, OnExit = Hit_Exit};
            _State.stateLookup.Add(eActorState.Hit, _statecallback);
            _statecallback = new StateCallback() { OnEnter = SkillAttack_Enter, Update = SkillAttack_Update, OnExit = SkillAttack_Exit };
            _State.stateLookup.Add(eActorState.Skill, _statecallback);

            _statecallback = new StateCallback() { OnEnter = null, Update = null, OnExit = null };
            _State.stateLookup.Add(eActorState.DoNothing, _statecallback);
        }

        #region 아이들상태
        void Idle_Enter()
        {

        }
        void Idle_Update()
        {

        }
        void Idle_Exit()
        {

        }
        #endregion

        #region 이동상태(적 추적)
        void Move_Enter()
        {

        }
        void Move_Update()
        {

        }
        void Move_Exit()
        {

        }
        #endregion

        #region 어택상태(적 공격)
        void Attack_Enter()
        {

        }
        void Attack_Update()
        {

        }
        void Attack_Exit()
        {

        }
        #endregion

        #region 맞는 상태(적 공격)
        void Hit_Enter()
        {
            myCharacter._Skeletonanimator.skeleton.SetColor(Color.red);
        }
        void Hit_Update()
        {

        }
        void Hit_Exit()
        {
            myCharacter._Skeletonanimator.skeleton.SetColor(Color.white);
        }
        #endregion

        #region 스킬 공격상태(적 공격)
        void SkillAttack_Enter()
        {

        }
        void SkillAttack_Update()
        {

        }
        void SkillAttack_Exit()
        {

        }
        #endregion

    }

}
