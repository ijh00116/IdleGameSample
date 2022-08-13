using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using DLL_Common.Common;
using System;
using Spine;

namespace BlackTree.InGame
{
    public class CharacterHandleWeaponInPet : CharacterHandleWeapon, IStateCallback
    {
        [HideInInspector] public GameObject Target;
        bool TargetNull
        {
            get { return (Target == null || Target.GetComponent<Health>().CurrentHealth <= 0); }
        }

        public bool CanIdle;
       
        protected override void Start()
        {
            base.Start();
        }



        protected override void onEnter()
        {
            CanMove = false;
            Target = _character.CurrentEnemy;
            if (TargetNull == true)
            {
                CanMove = true;
            }
            else
            {
                CanMove = false;
                ShootStart();
            }

        }

        public override void ShootStart()
        {
            if (_character.TargetDead == true)//적 죽음
            {
                CanMove = true;
            }
            else
            {
                int rate_0 = UnityEngine.Random.Range(0, 100);
                int rate_1 = UnityEngine.Random.Range(0, 100);
                float hitrate =
                    InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_RATE].specialskilldata.skillInfo.active_rate;
                float Crihitrate =
                    InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_CRITICAL_RATE].specialskilldata.skillInfo.active_rate;
                if (rate_0 <= hitrate)
                {
                    if (InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_RATE].skill.UnLocked)
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_RATE, CharacterType.PetPlayer));
                        return;
                    }
                }

                if (rate_1 <= Crihitrate)
                {
                    if (InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_CRITICAL_RATE].skill.UnLocked)
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_CRITICAL_RATE, CharacterType.PetPlayer));
                        return;
                    }

                }

            }
         
            AttackCo();
        }
        protected override void OnSpineAnimationEndInAttack(TrackEntry trackentry)
        {
            if (_character._state.IsCurrentState(myState) == false)
                return;

            if (_character.TargetDead == true)//적 죽음
            {
                CanMove = true;
            }
            else
            {
                int rate_0 = UnityEngine.Random.Range(0, 100);
                int rate_1 = UnityEngine.Random.Range(0, 100);
                float hitrate =
                    InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_RATE].specialskilldata.skillInfo.active_rate;
                float Crihitrate =
                    InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_CRITICAL_RATE].specialskilldata.skillInfo.active_rate;
                if (rate_0 <= hitrate)
                {
                    if (InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_RATE].skill.UnLocked)
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_RATE, CharacterType.PetPlayer));
                        return;
                    }
                }

                if (rate_1 <= Crihitrate)
                {
                    if (InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_CRITICAL_RATE].skill.UnLocked)
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_CRITICAL_RATE, CharacterType.PetPlayer));
                        return;
                    }

                }
            }
          


            if (trackentry.Animation.Name == "attack1")
            {
                if (_character.TargetDead == true)//적 죽음
                {
                    CanMove = true;
                }
                else
                {
                    _character._Skeletonanimator.state.SetAnimation(0, "attack2", false);
                }
            }
            else if (trackentry.Animation.Name == "attack2")
            {
                if (_character.TargetDead == true)//적 죽음
                {
                    CanMove = true;
                }
                else
                {
                    _character._Skeletonanimator.state.SetAnimation(0, "attack3", false);
                }
            }
            else if (trackentry.Animation.Name == "attack3")
            {
                if (_character.TargetDead == true)//적 죽음
                {
                    CanMove = true;
                }
                else
                {
                    _character._Skeletonanimator.state.SetAnimation(0, "attack4", false);
                }
            }
            else if (trackentry.Animation.Name == "attack4")
            {
                if (_character.TargetDead == true)//적 죽음
                {
                    CanMove = true;
                }
                else
                {
                    _character._Skeletonanimator.state.SetAnimation(0, "attack1", false);
                }
            }
        }

        protected override void onExit()
        {

        }
        protected override void onUpdate()
        {
            if (Common.InGameManager.Instance._PetsceneFsm._State.IsCurrentState(ePlaySubScene.PetDunGeonUpdate) == false)
            {
                _character._state.ChangeState(eActorState.Idle);
            }
            if (CanMove)
            {
                _character._state.ChangeState(eActorState.Idle);
            }
        }


        protected override void CalculateProjectileData()
        {
            base.CalculateProjectileData();
        }
    }
}
