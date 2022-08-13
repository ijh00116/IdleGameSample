using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using DLL_Common.Common;

namespace BlackTree
{
    public class AutoHitSkillEffect : ActiveSkillBaseEffect
    {
        protected override void Process()
        {
            
        }

        public override void ActivateEffect(ActiveBaseSkill _skill)
        {
            base.ActivateEffect(_skill);
            this.transform.position =new Vector3(_skill.Target.transform.position.x, _skill.Target.transform.position.y+4.7f,0);
            CalculateProjectileData();
        }

        void CalculateProjectileData()
        {
            float skilllevel = 3;
            BigInteger AtkDmg;
            if (Actor.playertype == CharacterType.Player || Actor.playertype == CharacterType.PetPlayer)
            {
                AtkDmg = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage()
                    * ((baseSkill.skillInfo.skill_ability + (baseSkill.specialskill.Level * baseSkill.skillInfo.skill_ability_level_up)) / 100.0f);
            }
            else
            {
                AtkDmg = (Actor as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_SKILL_HIT_DAMGAGE];
            }
            //AtkDmg = AtkDmg * skilllevel * (baseSkill.skillInfo.skill_ability / 100.0f);

            Health targethealth = TargetActor._health;
            targethealth.Damage(AtkDmg, true);
        }
    }

}
