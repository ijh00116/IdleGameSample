using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using Spine;

namespace BlackTree
{
    public class ActiveBaseSkill
    {
        public SkillType skillType;
        public ActiveSkillInfo skillInfo;

        //스킬 생성자
        public InGame.Character Actor;
        public GameObject Target;

        public string SkillResourcePath= "Prefabs/Skills/LightningSkill";

        public string SkillSubResourcePath_0 = "Prefabs/Skills/LightningSkill";
        public string SkillSubResourcePath_1 = "Prefabs/Skills/LightningSkill";

        protected bool SkillActivate;
        public int skillLevel;

        public SpecialSkill specialskill;

        MeshRenderer meshRenderer;
     

        public virtual void Init(ActiveSkillInfo _skillInfo)
        {
            skillInfo = _skillInfo;
            skillType = _skillInfo.skill_type;

            _skillInfo.idx = skillInfo.idx;

            SkillResourcePath = string.Format("Prefabs/Skills/{0}", skillType.ToString());
            SkillSubResourcePath_0 = string.Format("Prefabs/Skills/{0}", skillType.ToString()+"_0");
            SkillSubResourcePath_1 = string.Format("Prefabs/Skills/{0}", skillType.ToString() + "_1");
        }

        public virtual void UseSkill(Character actor)
        {
            SkillActivate = true;
            Actor = actor;
            Target = actor.CurrentEnemy;

            if (meshRenderer == null)
                meshRenderer = actor._Skeletonanimator.GetComponent<MeshRenderer>();

            meshRenderer.material.SetFloat("_FillPhase", 0);

            if(skillType==SkillType.SKILL_USE_POWERFULL_ATTACK)
                actor._Skeletonanimator.state.SetAnimation(0, "skill1", true);
            else if (skillType == SkillType.SKILL_USE_LIGHTNING_SLASH)
                actor._Skeletonanimator.state.SetAnimation(0, "skill2", true);
            else
                actor._Skeletonanimator.state.SetAnimation(0, "attack1", true);

            actor._Skeletonanimator.state.Complete += OnSpineAnimationEndInAttack;
            actor._Skeletonanimator.state.Event += AttackEvent;
        }

        public virtual bool IsSkillEnd
        {
            get { return !SkillActivate; }
        }
        
        public virtual void SkillAnimationEnd()
        {
            SkillActivate = false;
        }
        void OnSpineAnimationEndInAttack(TrackEntry trackentry)
        {
            Actor._Skeletonanimator.state.Complete -= OnSpineAnimationEndInAttack;
            Actor._Skeletonanimator.state.Event -= AttackEvent;

            SkillAnimationEnd();
        }

        void AttackEvent(TrackEntry trackentry, Spine.Event _event)
        {
            var UseSkill = Actor.GetComponent<CharacterUseSkill>();

            if (_event.Data.Name=="tag_wait")
            {
                if(skillType==SkillType.SKILL_USE_POWERFULL_ATTACK)
                    UseSkill.InstantiateSubSkillEffect("SkillSubResourcePath_0", SkillSubResourcePath_0);
            }
            if(_event.Data.Name=="tag_atk")
            {
                UseSkill.InstantiateSkillEffect(SkillResourcePath, skillType);
                if (skillType == SkillType.SKILL_USE_POWERFULL_ATTACK)
                    UseSkill.InstantiateSubSkillEffect("SkillSubResourcePath_1", SkillSubResourcePath_1);
            }
            
        }
    }
    


}
