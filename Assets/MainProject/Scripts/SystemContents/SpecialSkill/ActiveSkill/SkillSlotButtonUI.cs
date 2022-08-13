using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using UnityEngine.UI;
namespace BlackTree
{
    public class SkillSlotButtonUI : MonoBehaviour
    {
        [SerializeField] SkillType ButtonSkillType;
        [SerializeField] Button MyButton;
        [SerializeField] Image CoolTimeInActiveImage;
        [SerializeField] GameObject NotAuto;
        [SerializeField] GameObject LockedImage;
        bool SkillUsepossible=true;
        float CurrentCoolTime;

        SpecialSkillInventorySlot skillslot;
        // Start is called before the first frame update
        void Start()
        {
            MyButton.onClick.AddListener(ButtonPush);
            Init();
        }
        /// <summary>
        /// 스킬매니저가 지금 스킬의 인벤토리 역할을 하므로 나중에 콜백함수를 만들든 하여 초기화 해줄것.
        /// </summary>
        public void Init()
        {
            MyButton.GetComponent<Image>().fillAmount = 1;
            skillslot = Common.InGameManager.Instance.SpecialSkillInventory.ActiveSkills[ButtonSkillType];

            NotAuto.SetActive(!skillslot.skill.IsAuto);
        }

        // Update is called once per frame
        void Update()
        {
            if (skillslot == null)
                return;
            if (skillslot.skill.IsAuto == false)
                return;
            if (skillslot.skill.UnLocked == false)
            {
                return;
            }
            if (LockedImage.activeInHierarchy == true)
                LockedImage.SetActive(false);



            MyButton.GetComponent<Image>().fillAmount = Common.InGameManager.Instance.SpecialSkillInventory.ActiveSkills[ButtonSkillType].skill.LeftCoolTime
                   / Common.InGameManager.Instance.SpecialSkillInventory.ActiveSkills[ButtonSkillType].specialskilldata.skillInfo.cool_time;
        }
        private void OnDestroy()
        {
            MyButton.onClick.RemoveAllListeners();
        }
        void ButtonPush()
        {
            skillslot.skill.IsAuto = !skillslot.skill.IsAuto;
            NotAuto.SetActive(!skillslot.skill.IsAuto);
        }

    }
}
