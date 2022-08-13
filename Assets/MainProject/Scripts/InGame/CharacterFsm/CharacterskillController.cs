using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;


namespace BlackTree
{
    public class CharacterskillController : MonoBehaviour
    {
        Dictionary<SkillType, SpecialSkillInventorySlot> skilllist;
        Character _character;
        // Start is called before the first frame update
        void Start()
        {
            _character = this.GetComponent<Character>();
            skilllist = Common.InGameManager.Instance.SpecialSkillInventory.ActiveSkills;
        }

        // Update is called once per frame
        void Update()
        {
            foreach(var data in skilllist)
            {
                if (data.Key == SkillType.SKILL_USE_FEVERTIME)
                    continue;
                if (data.Key == SkillType.SKILL_AUTO_HIT_CRITICAL_RATE)
                    continue;
                if (data.Key == SkillType.SKILL_AUTO_HIT_RATE)
                    continue;
                if (data.Value.skill.IsAuto == false)
                    continue;
                if (data.Value.skill.UnLocked== false)
                    continue;

                data.Value.skill.LeftCoolTime += Time.deltaTime;
                if(data.Value.skill.LeftCoolTime >= data.Value.specialskilldata.skillInfo.cool_time)
                {
                    if (_character.TargetDead == true)
                        continue;
                    if (_character._state.IsCurrentState(eActorState.BaseAttack))
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(data.Key, CharacterType.Player));
                        data.Value.skill.LeftCoolTime = 0;
                    }
                }
            }
        }
    }

}
