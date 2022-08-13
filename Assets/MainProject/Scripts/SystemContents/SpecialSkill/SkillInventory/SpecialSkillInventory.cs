using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System;
using System.Linq;

namespace BlackTree
{
    public class SpecialSkillInventoryObject
    {
        public ItemType type;
        private SpecialSkillInventory Container = new SpecialSkillInventory();

        public List<SpecialSkillInventorySlot> GetSlots => Container.Slots;
        public Dictionary<SkillType, SpecialSkillInventorySlot> ActiveSkills = new Dictionary<SkillType, SpecialSkillInventorySlot>();

        public void Init(ItemType _type)
        {
            type = _type;
         
            SkillSetting();
        }
        public void SkillSetting()
        {
            List<ActiveSkillInfo> activeSkilltableList;
            activeSkilltableList = InGameDataTableManager.RelicList.active_skill;
            for (int i = 0; i < activeSkilltableList.Count; i++)
            {
                SkillType st = activeSkilltableList[i].skill_type;
                ActiveBaseSkill baseskill=null;
                baseskill = new ActiveBaseSkill();
                baseskill.Init(activeSkilltableList[i]);

                SpecialSkillInventorySlot slot = new SpecialSkillInventorySlot();
                slot.skill = new SpecialSkill(activeSkilltableList[i].idx);
                slot.specialskilldata = baseskill;
                slot.specialskilldata.specialskill = slot.skill;

                slot.UpdateSlot();
                ActiveSkills.Add(st,slot);
                GetSlots.Add(slot);
            }
        }

        UI.Event.SrelicLevelUpdate srelicmsg = new UI.Event.SrelicLevelUpdate();
        public void SkillLevelUpdate(int skillIdx)
        {
            SkillType st = (SkillType)skillIdx;
            if (ActiveSkills.ContainsKey(st))
            {
                SRelicInventoryObject srelicinven = Common.InGameManager.Instance.SRelicInventory;

                if (srelicinven.GetslotList.ContainsKey(st))
                {
                    List<SRelicInventorySlot> invenslot = srelicinven.GetslotList[st];
                    int _skilllevel = invenslot[0].srelic.Level;
                    for (int i = 0; i < invenslot.Count; i++)
                    {
                        if (invenslot[i].srelic.Level < _skilllevel)
                        {
                            _skilllevel = invenslot[i].srelic.Level;
                        }
                    }
                    ActiveSkills[st].skill.Level= _skilllevel;
                    //특수유물 업데이트 된거 보고 모두 unlock이면 스킬 unlock
                    bool unlocked = false;
                    for (int i = 0; i < invenslot.Count; i++)
                    {
                        if (invenslot[i].srelic.Unlocked)
                            unlocked = true;
                        else
                        {
                            unlocked = false;
                            break;
                        }
                    }
                    ActiveSkills[st].skill.UnLocked = unlocked;
                    //pvp정보에 스킬정보 업데이트
                    switch (st)
                    {
                        case SkillType.SKILL_USE_LIGHTNING_SLASH:
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_LIGHTNING_LV] = _skilllevel.ToString();
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_LIGHTNING_DAMGAGE] =
                                (CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage() * CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalDamageRate()).ToString();
                            break;
                        case SkillType.SKILL_USE_POWERFULL_ATTACK:
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_POWERFULL_ATTACK_LV] = _skilllevel.ToString();
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_POWERFULL_ATTACK_DAMGAGE] =
                                (CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage() * CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalDamageRate()).ToString();
                            break;
                        case SkillType.SKILL_USE_BUFF_ATK_MOVE_SPEED:
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_BUFF_ATK_MOVE_SPEED_LV] = _skilllevel.ToString();
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_BUFF_ATK_MOVE_SPEED_VALUE] =
                                ActiveSkills[st].specialskilldata.skillInfo.active_time.ToString();
                            break;
                        case SkillType.SKILL_AUTO_HIT_RATE:
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_HIT_RATE_LV] = _skilllevel.ToString();
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_HIT_DAMGAGE] =
                                (CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage() * CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalDamageRate()).ToString();
                            break;
                        case SkillType.SKILL_AUTO_HIT_CRITICAL_RATE:
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_POWERFULL_ATTACK_LV] = _skilllevel.ToString();
                            Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_SKILL_POWERFULL_ATTACK_DAMGAGE] =
                                (CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage() * CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCriticalDamageRate()).ToString();
                            break;
                        case SkillType.SKILL_USE_FEVERTIME:
                            break;
                        default:
                            break;
                    }


                    Message.Send<UI.Event.SrelicLevelUpdate>(srelicmsg);
                }
                else
                {
                    Debug.Log("스킬 존재하지 않음 ");
                    return;
                }
            }
        }

    }

    [System.Serializable]
    public class SpecialSkillInventory
    {
        public List<SpecialSkillInventorySlot> Slots = new List<SpecialSkillInventorySlot>();
    }
    [System.Serializable]
    public class SpecialSkillInventorySlot
    {
        [System.NonSerialized] public SpecialSkillInventoryObject parent;
        [System.NonSerialized] public Action onAfterUpdated;
        [System.NonSerialized] public Action onBeforeUpdated;

        public SpecialSkill skill;
        public ActiveBaseSkill specialskilldata;
        public void UpdateSlot()
        {
            onBeforeUpdated?.Invoke();

            //Statusobj.Data.UpdateData();

            onAfterUpdated?.Invoke();
        }

    }
}
