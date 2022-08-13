using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class SpecialSkillSlotUI : MonoBehaviour
    {
        [HideInInspector] public SpecialSkillInventorySlot slot;
        [SerializeField] Button InfoButton;
        [SerializeField] Text SkillLevel;

        [HideInInspector] public Items.SRelicUserInterface SrelicUI;

        [HideInInspector]public Sprite SkillIconSprite;

        [HideInInspector] public SpecialSkillInterface skillinterface;
        [SerializeField] public GameObject PushedImage;
        public void Init(SpecialSkillInventorySlot _slot)
        {
            slot = _slot;

            slot.onAfterUpdated += SlotUIUpdate;

            InfoButton.onClick.AddListener(PushButton);
            Message.AddListener<UI.Event.SrelicLevelUpdate>(SkillLevelUpdated);

            if(SkillIconSprite==null)
            {
                string path = "Images/Artifact/ActiveSkill";
                SkillIconSprite = Resources.Load<Sprite>(string.Format("{0}/{1}", path, slot.specialskilldata.skillInfo.name));
            }
        }
        public void Release()
        {
            InfoButton.onClick.RemoveAllListeners();
            Message.RemoveListener<UI.Event.SrelicLevelUpdate>(SkillLevelUpdated);
        }
        public void PushButton()
        {
            for (int i = 0; i < skillinterface.specialSkillUIList.Count; i++)
                skillinterface.specialSkillUIList[i].PushedImage.SetActive(false);

            PushedImage.SetActive(true);
            SrelicUI.SkillUIButtonSet(this);
        }

        void SlotUIUpdate()
        {
            if(gameObject.activeInHierarchy==false)
                this.gameObject.SetActive(true);
            if(slot.skill.UnLocked)
                SkillLevel.text = string.Format("레벨 : {0}", slot.skill.Level);
            else
                SkillLevel.text = string.Format("레벨 : {0}", 0);
            if (SkillIconSprite!=null)
            {
                InfoButton.GetComponent<Image>().sprite = SkillIconSprite;
            }
        }

        void SkillLevelUpdated(UI.Event.SrelicLevelUpdate msg)
        {
            if (Common.InGameManager.Instance.SpecialSkillInventory.ActiveSkills.ContainsKey(slot.specialskilldata.skillType))
            {
                if (slot.skill.UnLocked)
                    SkillLevel.text = string.Format("레벨 : {0}", slot.skill.Level);
                else
                    SkillLevel.text = string.Format("레벨 : {0}", 0);
            }
        }

    }

}
