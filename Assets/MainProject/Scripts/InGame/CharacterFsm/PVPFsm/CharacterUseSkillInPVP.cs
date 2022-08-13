using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using DLL_Common.Common;
using System;
using Spine.Unity;
using Spine;

namespace BlackTree.InGame
{
    public class CharacterUseSkillInPVP : CharacterUseSkill,IStateCallback
    {
        public Dictionary<SkillType, ActiveBaseSkill> ActiveSkills = new Dictionary<SkillType, ActiveBaseSkill>();

        protected override void Start()
        {
            base.Start();
            _character = this.GetComponent<CharacterInPVP>();

            List<ActiveSkillInfo> activeSkilltableList;
            activeSkilltableList = InGameDataTableManager.RelicList.active_skill;
            for (int i = 0; i < activeSkilltableList.Count; i++)
            {
                SkillType st = activeSkilltableList[i].skill_type;
                ActiveBaseSkill baseskill = null;
                baseskill = new ActiveBaseSkill();
                baseskill.Init(activeSkilltableList[i]);
                ActiveSkills.Add(st, baseskill);
            }
        }

        protected override void OnDestroy()
        {

        }

        protected override void onEnter()
        {
            UseSkill();
        }
        protected override void onUpdate()
        {
            if (CurrentSkill.IsSkillEnd == true)
            {
                eActorState _previousState =_character._state.PreviousState;
                _character._state.ChangeState(eActorState.BaseAttack);
            }
        }

        protected override void onExit()
        {

        }

        protected override void UseSkillForButtonpush(UI.Event.UsingSkillButtonPush msg)
        {
            if (msg.charactertype != _character.playertype)
                return;
            if (_character._state.IsCurrentState(eActorState.BaseAttack) == false)
                return;

            if (ActiveSkills.ContainsKey(msg.skillType))
            {
                CurrentSkill = ActiveSkills[msg.skillType];
            }
            if (CurrentSkill == null)
                return;

            _character._state.ChangeState(eActorState.Skill);
        }
    }

}
