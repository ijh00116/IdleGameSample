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
    public class CharacterHandleWeaponInPVP : CharacterHandleWeapon, IStateCallback
    {
        [HideInInspector] public GameObject Target;
        bool TargetNull
        {
            get { return (Target == null || Target.GetComponent<HealthInPVP>().CurrentHealth <= 0); }
        }

        public bool CanIdle;
        public GameObject Hitparticle;

        protected override void Start()
        {
            base.Start();
            _character = this.GetComponent<CharacterInPVP>();
        }
        
        protected override void onEnter()
        {
            Target = _character.CurrentEnemy;
            if (TargetNull == true)
            {
                CanIdle = true;
            }
            else
            {
                CanIdle = false;
                ShootStart();
            }
        }

        protected override void onExit()
        {

        }
        protected override void onUpdate()
        {
            if (Common.InGameManager.Instance._PvpsceneFsm._State.IsCurrentState(ePlaySubScene.pvpDunGeonUpdate) == false)
            {
                _character._state.ChangeState(eActorState.Idle);
            }
            if (CanIdle)
            {
                _character._state.ChangeState(eActorState.Idle);
            }
        }

        //ai업데이트 문을 통해 매 프레임 들어옴
        public override void ShootStart()
        {
            AttackCo_pvp();
        }
          
        void AttackCo_pvp()
        {
            if (CurrentWeapon == null)
                return;
            if (SpeedForChange != speed)
            {
                speed = SpeedForChange;
            }
            float atkSpeed = (float)(CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAttackSpeed());

            if (_character.playertype == CharacterType.PvpPlayer)
            {
                _character._Skeletonanimator.timeScale = 0.8f;
            }
            else
            {
                _character._Skeletonanimator.timeScale = 0.5f;
            }
            _character._Skeletonanimator.timeScale =float.Parse((_character as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_ATTACK_SPEED_UP]);

            _character._Skeletonanimator.state.SetAnimation(0, "attack1", false);
        }
        protected override void OnSpineAnimationEndInAttack(TrackEntry trackentry)
        {
            if (_character._state.IsCurrentState(myState) == false)
                return;
            if (_character.Skill_Wrath)
            {
                Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_RATE, _character.playertype));
                return;
            }
            else if (_character.Skill_Judge)
            {
                Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_CRITICAL_RATE, _character.playertype));
                return;
            }

            int rate_0 = UnityEngine.Random.Range(0, 100);
            if (rate_0 < 15)
            {
                Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_RATE, _character.playertype));
                return;
            }
            else if (rate_0 >= 15 && rate_0 < 30)
            {
                Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_CRITICAL_RATE, _character.playertype));
                return;
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
        protected override void CalculateProjectileData()
        {
            BigInteger AtkDmg;
            float critical = UnityEngine.Random.Range(0, 100);
            float criticalRate = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalRate();
            if (critical > criticalRate)
            {
                AtkDmg = (_character as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_ATTACK];
            }
            else
            {
                AtkDmg = (_character as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_ATTACK];
                AtkDmg = AtkDmg * (_character as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_CRITICAL_DAMAGE_UP];
            }
            HealthInPVP targethealth = Target.GetComponent<HealthInPVP>();
            targethealth.Damage(AtkDmg,false);

            DebugExtention.ColorLog("blue", "발사");
        }
    }
}