using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using UnityEngine.UI;

namespace BlackTree
{
    public class PVPSkillSlotButton : MonoBehaviour
    {
        [SerializeField] CharacterType characterType;
        [SerializeField] SkillType ButtonSkillType;
        [SerializeField] Image CoolTimeInActiveImage;

        bool SkillUsepossible = true;
        float CurrentCoolTime;

        SpecialSkillInventorySlot skillslot;
        bool CombatStart = false;

        void Start()
        {
        }
        /// <summary>
        /// 스킬매니저가 지금 스킬의 인벤토리 역할을 하므로 나중에 콜백함수를 만들든 하여 초기화 해줄것.
        /// </summary>
        public void Init()
        {
            SkillUsepossible = false;
            CoolTimeInActiveImage.fillAmount = 1;

            skillslot = Common.InGameManager.Instance.SpecialSkillInventory.ActiveSkills[ButtonSkillType];
            CombatStart = false;
        }

        public void StartCombat()
        {
            CurrentCoolTime = skillslot.specialskilldata.skillInfo.cool_time - 3.0f;
            CoolTimeInActiveImage.fillAmount = 1;
            SkillUsepossible = false;
            CombatStart = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (skillslot == null)
                return;
            if (CombatStart == false)
                return;
            if (SkillUsepossible == false)
            {
                CurrentCoolTime += Time.deltaTime;
                if ( CurrentCoolTime >= skillslot.specialskilldata.skillInfo.cool_time )
                {
                    CurrentCoolTime = 0;
                    SkillUsepossible = true;
                    return;
                }
            }
            else
            {
                if(characterType==CharacterType.PvpPlayer)
                {
                    if(Common.InGameManager.Instance._PvpsceneFsm.pvpDungeonController.PlayerCharacter._state.IsCurrentState(eActorState.BaseAttack))
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(ButtonSkillType, characterType));
                        SkillUsepossible = false;
                    }
                }
                else
                {
                    if (Common.InGameManager.Instance._PvpsceneFsm.pvpDungeonController.EnemyCharacter._state.IsCurrentState(eActorState.BaseAttack))
                    {
                        Message.Send<UI.Event.UsingSkillButtonPush>(new UI.Event.UsingSkillButtonPush(ButtonSkillType, characterType));
                        SkillUsepossible = false;
                    }
                }
            }
        }
        private void OnDestroy()
        {

        }
    }
}
