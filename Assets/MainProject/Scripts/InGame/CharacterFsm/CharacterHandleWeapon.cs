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
    public class CharacterHandleWeapon : CharacterAbility ,IStateCallback
    {
        [SerializeField]protected eActorState myState;
        [Header("무기 세팅")]
        public Weapon InitialWeapon;
        public Weapon secondWeapon;
        public Transform WeaponAttachment;
        public Weapon CurrentWeapon;

        [HideInInspector] public bool Shoot;
        
        [HideInInspector]public float speed = 0.5f;
        [Header("스피드값(기본값은 1.2)")]
        public float SpeedForChange=1.2f;

        protected List<string> attackAnimStateName = new List<string>();
        public bool CanMove;

        //능력치 상속용으로 써서 데코레이터 패턴으로 사용
        protected float criticalRate;
        protected float atkSpeed;
        protected BigInteger AtkDmg;

        public Action OnEnter => onEnter;
        public Action OnExit => onExit;
        public Action OnUpdate => onUpdate;
        
        protected override void Start()
        {
            Shoot = false;

            base.Start();
            InitWeapon();

            _character._state.AddState(myState, this);

            speed = 1.2f;
            speed = SpeedForChange;

            attackAnimStateName.Add("attack1");
            attackAnimStateName.Add("attack2");
            attackAnimStateName.Add("attack3");
            attackAnimStateName.Add("attack4");

            SpineAnimationEventSetting();
        }

        public virtual void SpineAnimationEventSetting()
        {
            if(_character==null)
            {
                _character = this.GetComponent<Character>();
            }
            _character._Skeletonanimator.state.Complete += OnSpineAnimationEndInAttack;
            _character._Skeletonanimator.state.Event += AttackEvent;
        }

        protected virtual void onEnter()
        {
            if (_character.TargetDead== true)
            {
                CanMove = true;
            }
            else
            {
                CanMove = false;
                ShootStart();
            }
        }

        protected virtual void onExit()
        {

        }

        protected virtual void onUpdate()
        {
            if (CanMove)
            {
                if (_character.CurrentEnemyCharacter.playertype==CharacterType.Boss)
                {
                    //_character._state.ChangeState(eActorState.EventAfterKillBoss);
                    Common.InGameManager.Instance.WaveInfoUpdate();
                }
                else if (_character.CurrentEnemyCharacter.playertype == CharacterType.NormalMonster)
                {
                    _character._state.ChangeState(eActorState.Move);
                }
                else if (_character.CurrentEnemyCharacter.playertype == CharacterType.Mimic)
                {
                    _character._state.ChangeState(eActorState.Move);
                }
                else
                {
                    _character._state.ChangeState(eActorState.Move);
                }
           
            }
        }

        protected virtual void InitWeapon()
        {
            if(InitialWeapon)
            {
                ChangeWeapon(InitialWeapon);
            }
        }
        
        //ai업데이트 문을 통해 매 프레임 들어옴
        public virtual void ShootStart()
        {
            if (_character._state.IsCurrentState(eActorState.BaseAttack))
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
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_RATE, CharacterType.Player));
                        return;
                    }
                }

                if (rate_1 <= Crihitrate)
                {
                    if (InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_CRITICAL_RATE].skill.UnLocked)
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_CRITICAL_RATE, CharacterType.Player));
                        return;
                    }

                }
            }
            AttackCo();
        }

        public virtual void ShootStop()
        {
            _character._state.ChangeState(eActorState.Idle);
        }
        protected virtual void ChangeWeapon(Weapon newWeapon )
        {
            ShootStop();
            if(CurrentWeapon!=null)
            {
                Destroy(CurrentWeapon);
            }

            if(newWeapon!=null)
            {
                CurrentWeapon = (Weapon)Instantiate(newWeapon);
                CurrentWeapon.transform.SetParent(WeaponAttachment, false);
                CurrentWeapon.transform.localPosition = Vector3.zero;
                if (_character.FacingRight==false)
                {
                    Vector3 scale = CurrentWeapon.transform.localScale;
                    scale.x *= -1;
                    CurrentWeapon.transform.localScale = scale;
                }
            }
        }
         
        protected float aniTime;
        protected void AttackCo()
        {
#if UNITY_EDITOR
            Debug.Log("공격 1회 시작");
#endif
            if (CurrentWeapon == null)
                return;

            if(SpeedForChange!=speed)
            {
                speed = SpeedForChange;
            }
            atkSpeed = (float)(CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAttackSpeed());

            if (_character.playertype == CharacterType.Player || _character.playertype == CharacterType.PetPlayer)
            {
                _character._Skeletonanimator.timeScale = atkSpeed * speed;
            }
            else
            {
                _character._Skeletonanimator.timeScale = 0.5f;
            }
                
            _character._Skeletonanimator.state.SetAnimation(0, "attack1", false);
            _character.CharacterPetChangeAnim("idle");
        }

        protected virtual void OnSpineAnimationEndInAttack(TrackEntry trackentry)
        {
            if (_character._state.IsCurrentState(myState) == false)
                return;

            if (_character.TargetDead == false)
            {
                if (_character._state.IsCurrentState(eActorState.BaseAttack))
                {
                    int rate_0 = UnityEngine.Random.Range(0, 100);
                    int rate_1 = UnityEngine.Random.Range(0, 100);
                    float hitrate =
                        InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_RATE].specialskilldata.skillInfo.active_rate;
                    float Crihitrate =
                        InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_CRITICAL_RATE].specialskilldata.skillInfo.active_rate;
                    if (rate_0 <= hitrate)
                    {
                        if(InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_RATE].skill.UnLocked)
                        {
                            Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_RATE, CharacterType.Player));
                            return;
                        }
                    }

                    if (rate_1 <= Crihitrate)
                    {
                        if(InGameManager.Instance.SpecialSkillInventory.ActiveSkills[SkillType.SKILL_AUTO_HIT_CRITICAL_RATE].skill.UnLocked)
                        {
                            Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(SkillType.SKILL_AUTO_HIT_CRITICAL_RATE, CharacterType.Player));
                            return;
                        }
                     
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

        protected void AttackEvent(TrackEntry trackentry,Spine.Event _event)
        {
            if (_character._state.IsCurrentState(myState) == false)
                return;
            if(_event.Data.Name=="tag_atk")
            {
                AttackShootTrigger();
            }
        }

        protected void AttackShootTrigger()
        {
            CalculateProjectileData();
        }

        protected virtual void CalculateProjectileData()
        {
            float critical = UnityEngine.Random.Range(0, 100);
            criticalRate = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalRate();
            if (critical > criticalRate)
            {
                AtkDmg = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage();
            }
            else
            {
                AtkDmg = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage() * CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalDamageRate();
            }
            CurrentWeapon.WeaponDamageSetting(AtkDmg);
            CurrentWeapon.ShootProjectile(_character.FacingRight);

            Health targethealth = _character.CurrentEnemyCharacter._health;
            bool cri = (critical < criticalRate);
            targethealth.Damage(AtkDmg, cri);

            DebugExtention.ColorLog("blue", "발사");
        }
    }

}
