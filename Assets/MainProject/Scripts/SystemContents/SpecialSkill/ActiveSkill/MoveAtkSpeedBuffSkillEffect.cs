using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;

namespace BlackTree
{
    public class MoveAtkSpeedBuffSkillEffect : ActiveSkillBaseEffect
    {
        public float LifeTime;
        public float currentTime;
        protected override void Process()
        {
            currentTime += Time.deltaTime;
            if(currentTime>=LifeTime)
            {
                Expired();
            }
        }
        void Expired()
        {
            if (Actor.playertype == CharacterType.Player || Actor.playertype == CharacterType.PetPlayer)
            {
                DTConstraintsData.ActiveSkillData_forMoveSpeed = 0;
                DTConstraintsData.ActiveSkillData_forAtkSpeed = 0;
            }
            else
            {
                //(Actor as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_ATTACK_SPEED_UP] -= 0.5f;
            }
            this.gameObject.SetActive(false);
        }
        public override void ActivateEffect(ActiveBaseSkill _skill)
        {
            base.ActivateEffect(_skill);
            LifeTime = _skill.skillInfo.active_time;
            currentTime = 0;

            this.transform.localPosition = new Vector3(0.23f, 2.18f, 0);

            if (Actor.playertype == CharacterType.Player || Actor.playertype == CharacterType.PetPlayer)
            {
                int value = (int)baseSkill.skillInfo.skill_ability + (baseSkill.specialskill.Level * baseSkill.skillInfo.skill_ability_level_up);
                DTConstraintsData.ActiveSkillData_forMoveSpeed = value;
                DTConstraintsData.ActiveSkillData_forAtkSpeed = value;
                //AtkDmg = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage() * CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalDamageRate();
            }
            else
            {
                //(Actor as CharacterInPVP).userInfo.AttackSpeed+=0.5f;
            }

        }
    }

}