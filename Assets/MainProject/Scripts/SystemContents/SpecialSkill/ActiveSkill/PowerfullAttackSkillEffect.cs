using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using DLL_Common.Common;

namespace BlackTree
{
    public class PowerfullAttackSkillEffect : ActiveSkillBaseEffect
    {
        protected override void Process()
        {
            //throw new System.NotImplementedException();
        }

        public override void ActivateEffect(ActiveBaseSkill _skill)
        {
            Common.InGameManager.Instance.CameraShake(0.7f, 0.2f);
            base.ActivateEffect(_skill);
            this.transform.position = Actor.CurrentEnemy.transform.position;
            CalculateProjectileData();
        }

        void CalculateProjectileData()
        {
            BigInteger AtkDmg;
            if (Actor.playertype == CharacterType.Player || Actor.playertype == CharacterType.PetPlayer)
            {
                AtkDmg = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage() *
                   ( (baseSkill.skillInfo.skill_ability + (baseSkill.specialskill.Level * baseSkill.skillInfo.skill_ability_level_up)) / 100.0f);
            }
            else
            {
                AtkDmg=(Actor as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_SKILL_POWERFULL_ATTACK_DAMGAGE];
            }

            Health targethealth = TargetActor._health;
            targethealth.Damage(AtkDmg, true);
        }
    }

}
