using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ItemComposeWindow : MonoBehaviour
    {
        [SerializeField] Text CurrentGradeText;
        [SerializeField] Image[] CurrentStarImages;
        [SerializeField] Image CurrentOutLineIconImage;
        [SerializeField] Image CurrentIconImage;
        [SerializeField] Text CurrentLevelText;
        [SerializeField] Text CurrentAmountText;

        [SerializeField] Text NextGradeText;
        [SerializeField] Image[] NextStarImages;
        [SerializeField] Image NextOutLineIconImage;
        [SerializeField] Image NextIconImage;
        [SerializeField] Text NextLevelText;
        [SerializeField] Text NextAmountText;



        [SerializeField] Button PlusComposeCount;
        [SerializeField] Button MinusComposeCount;
        [SerializeField] Text ItemComposeCount;
        [SerializeField] Button ComposeBtn;
        [SerializeField] Button CantComposeBtn;

        private ItemUIDisplay MyInventoryslot;
        private ItemUIDisplay MyNextInventorySlot;

        int CurrentComposeCount;
        const int ComposeValue = 5;
        public void Init()
        {
            PlusComposeCount.onClick.AddListener(PushPlusButton);
            MinusComposeCount.onClick.AddListener(PushMinusButton);
            ComposeBtn.onClick.AddListener(PushComposeButton);
        }

        public void PopupSetting(ItemUIDisplay slot)
        {
            MyInventoryslot = slot;
            ItemData _itemdata = MyInventoryslot.slot.itemData;

            int index = MyInventoryslot.slot.itemData.itemInfo.idx;
            InventorySlot nextslot = MyInventoryslot.slot.parent.GetSlots.Find(o => o.itemData.itemInfo.idx == index+1);
            if (nextslot != null)
            {
                MyNextInventorySlot = nextslot.display;
            }

            CurrentGradeText.text = MyInventoryslot._GradeText;
            for (int i = 0; i < CurrentStarImages.Length; i++)
                CurrentStarImages[i].gameObject.SetActive(i < MyInventoryslot.slot.item.AwakeLv);
            CurrentOutLineIconImage.sprite = MyInventoryslot.currentOutlineSpriteImage;
            CurrentIconImage.sprite= MyInventoryslot.currentSpriteImage;
            if(MyInventoryslot.slot.item.Unlocked)
                CurrentIconImage.color = new Color(1, 1, 1, 1);
            else
                CurrentIconImage.color = new Color(0, 0, 0, 1);
            CurrentLevelText.text =string.Format("LV.{0}", MyInventoryslot.slot.item.Level);

            if(nextslot!=null)
            {
                NextGradeText.text = MyNextInventorySlot._GradeText;
                for (int i = 0; i < NextStarImages.Length; i++)
                    NextStarImages[i].gameObject.SetActive(i < MyNextInventorySlot.slot.item.AwakeLv);
                NextOutLineIconImage.sprite = MyNextInventorySlot.currentOutlineSpriteImage;
                NextIconImage.sprite = MyNextInventorySlot.currentSpriteImage;
                if (MyNextInventorySlot.slot.item.Unlocked)
                    NextIconImage.color = new Color(1, 1, 1, 1);
                else
                    NextIconImage.color = new Color(0, 0, 0, 1);
                NextLevelText.text = string.Format("LV.{0}", MyNextInventorySlot.slot.item.Level);
            }
        


            CurrentComposeCount = MyInventoryslot.slot.item.amount / ComposeValue;

            UpdateUI();

        }

        void PushPlusButton()
        {
            if(CurrentComposeCount* ComposeValue+ ComposeValue < MyInventoryslot.slot.item.amount)
            {
                CurrentComposeCount++;
                UpdateUI();
            }
        }

        void PushMinusButton()
        {
            if(CurrentComposeCount>0)
            {
                CurrentComposeCount--;
                UpdateUI();
            }
        }

        void PushComposeButton()
        {
            if (MyNextInventorySlot == null)
                return;
            if(MyInventoryslot.slot.item.amount>= ComposeValue)
            {
                MyInventoryslot.slot.AddAmount(-(CurrentComposeCount* ComposeValue));
                MyNextInventorySlot.slot.AddAmount(CurrentComposeCount);

                CurrentComposeCount = 0;
                UpdateUI();

                if (MyNextInventorySlot.slot.itemData.itemtype == ItemType.weapon)
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.WEAPON_MIX, 1);
                if (MyNextInventorySlot.slot.itemData.itemtype == ItemType.wing)
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.WING_MIX, 1);
            }
            if (MyInventoryslot.slot.parent.type == ItemType.weapon)
                Common.InGameManager.Instance.Localdata.SaveweaponData();
            else
                Common.InGameManager.Instance.Localdata.SavewingData();
        }

        void UpdateUI()
        {
            CurrentAmountText.text = string.Format("{0}(-{1})", MyInventoryslot.slot.item.amount, (CurrentComposeCount * ComposeValue).ToString());
            NextAmountText.text = string.Format("{0}(+{1})", MyInventoryslot.slot.item.amount, (CurrentComposeCount).ToString());

            ItemComposeCount.text = CurrentComposeCount.ToString();

            CantComposeBtn.gameObject.SetActive(MyInventoryslot.slot.item.amount < ComposeValue);
        }

    }

}
