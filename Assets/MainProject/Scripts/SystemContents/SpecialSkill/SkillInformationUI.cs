using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class SkillInformationUI : MonoBehaviour
    {
        [SerializeField] Button Closebtn;
        [SerializeField] Image skillIcon;
        [SerializeField] Text LvText;
        [SerializeField] Text NameText;
        [SerializeField] Text DescText;

        //스킬 특성
        [SerializeField] Text SkillTypeTxt;
        [SerializeField] Text ActiveTime;//지속시간
        [SerializeField] Text ActiveRate;// 발동확률
        [SerializeField] Text CoolTimeTxt;
        [SerializeField] Text Damage;

        SpecialSkillSlotUI Currentslot;
        public void Init()
        {
            this.gameObject.SetActive(false);
            Closebtn.onClick.AddListener(() => this.gameObject.SetActive(false));
        }

        public void Release()
        {

        }

        public void PopupActiveSkillUI(SpecialSkillSlotUI skill)
        {
            this.gameObject.SetActive(true);
            Currentslot = skill;

            skillIcon.sprite = Currentslot.SkillIconSprite;
            if(Currentslot.slot.specialskilldata.specialskill.UnLocked)
            {
                LvText.text = string.Format("Lv.{0}", Currentslot.slot.specialskilldata.specialskill.Level.ToString());

            }
            else
            {
                LvText.text = string.Format("Lv.{0}", 0);
            }
                

            LocalValue name = InGameDataTableManager.LocalizationList.skill.Find(o => o.id == Currentslot.slot.specialskilldata.skillInfo.name);
            LocalValue desc= InGameDataTableManager.LocalizationList.skill.Find(o => o.id == Currentslot.slot.specialskilldata.skillInfo.desc);

            NameText.text = name.kr;
            DescText.text = desc.kr;

            LocalValue activetype= InGameDataTableManager.LocalizationList.info.Find(o => o.id == Currentslot.slot.specialskilldata.skillInfo.active_type_desc);
            SkillTypeTxt.text =string.Format("타입 : <color=cyan>{0}</color>", activetype.kr);

            if ((int)Currentslot.slot.specialskilldata.skillInfo.active_time != 0)
            {
                ActiveTime.gameObject.SetActive(true);
                ActiveTime.text = string.Format("지속시간 : <color=yellow>{0}</color>s", (int)Currentslot.slot.specialskilldata.skillInfo.active_time);
            }
            else
                ActiveTime.gameObject.SetActive(false);
        
            if ((int)Currentslot.slot.specialskilldata.skillInfo.active_rate != 0)
            {
                ActiveRate.gameObject.SetActive(true);
                ActiveRate.text = string.Format("발동확률 : <color=yellow>{0}</color>%", (int)Currentslot.slot.specialskilldata.skillInfo.active_rate);
            }
            else
                ActiveRate.gameObject.SetActive(false);

            CoolTimeTxt.text = string.Format("쿨타임 : <color=yellow>{0}</color>s", (int)Currentslot.slot.specialskilldata.skillInfo.cool_time);

            BigInteger damage=
                   (Currentslot.slot.specialskilldata.skillInfo.skill_ability + 
                    (Currentslot.slot.specialskilldata.specialskill.Level * Currentslot.slot.specialskilldata.skillInfo.skill_ability_level_up));

            Damage.text = string.Format("데미지 : +<color=yellow>{0}</color>%", damage.ToDisplay());

        }
    }

}
