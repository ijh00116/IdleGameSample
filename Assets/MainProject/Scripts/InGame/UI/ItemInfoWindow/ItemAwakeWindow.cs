using DLL_Common.Common;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ItemAwakeWindow : MonoBehaviour
    {
        private ItemUIDisplay MyInventoryslot;

        [Header("현재 이미지")]
        [SerializeField] Text CurrentItemMaxLevelText;
        [SerializeField] Image CurrentItemIconImage;
        [SerializeField] Image CurrentItemOutLineImage;
        [SerializeField] Image[] CurrentItemAwakeLvImages;
        [SerializeField] Text CurrentGradeText;
        [SerializeField] Text CurrentLevelText;

        [Header("다음 이미지")]
        [SerializeField] Text NextItemMaxLevelText;
        [SerializeField] Image NextItemIconImage;
        [SerializeField] Image NextItemOutLineImage;
        [SerializeField] Image[] NextItemAwakeLvImages;
        [SerializeField] Text NextGradeText;
        [SerializeField] Text NextLevelText;

        [SerializeField] Button AwakeButton;
        [SerializeField] Text AwakeGemCount;
        [SerializeField] Button CantAwakeButton;

        int NeedGemCost;
        public void Init()
        {
            Message.AddListener<UI.Event.CurrencyChange>(CurrencyUpdate);

            AwakeButton.onClick.AddListener(PushAwake);
            AwakeButton.gameObject.SetActive(true);
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyUpdate);
        }

        public void PopupSetting(ItemUIDisplay slot)
        {
            MyInventoryslot = slot;
            UpdateData();
        }

        void PushAwake()
        {
            int awakeLv = MyInventoryslot.slot.item.AwakeLv;
            int maxLv=100;
            switch (awakeLv)
            {
                case 0:
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.max_lv;
                    break;
                case 1:
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.awake_max_lv;
                    break;
                case 2:
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.awake2_max_lv;
                    break;
                case 3:
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.awake3_max_lv;
                    break;
            }
            if (awakeLv >= 3)
                return;

            bool canAwake = (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicStone).value >= NeedGemCost);
            if (canAwake)
            {
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicStone, -NeedGemCost);
                MyInventoryslot.slot.AwakeItem();

                //BackendManager.Instance.SaveItemList(MyInventoryslot.slot.parent);
                if (MyInventoryslot.slot.parent.type == ItemType.weapon)
                    Common.InGameManager.Instance.Localdata.SaveweaponData();
                else
                    Common.InGameManager.Instance.Localdata.SavewingData();

                UpdateData();
            }
         
        }

        void UpdateData()
        {
            int awakeLv = MyInventoryslot.slot.item.AwakeLv;
            int maxLv = 0;
            int NextMaxLv = 0;
            switch (awakeLv)
            {
                case 0:
                    NeedGemCost = MyInventoryslot.slot.itemData.itemInfo.awake_gem_cost;
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.max_lv;
                    NextMaxLv = MyInventoryslot.slot.itemData.itemInfo.awake_max_lv;
                    break;
                case 1:
                    NeedGemCost = MyInventoryslot.slot.itemData.itemInfo.awake_gem_cost;
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.awake_max_lv;
                    NextMaxLv = MyInventoryslot.slot.itemData.itemInfo.awake2_max_lv;
                    break;
                case 2:
                    NeedGemCost = MyInventoryslot.slot.itemData.itemInfo.awake_gem_cost;
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.awake2_max_lv;
                    NextMaxLv = MyInventoryslot.slot.itemData.itemInfo.awake3_max_lv;
                    break;
                case 3:
                    NeedGemCost = 0;
                    maxLv = MyInventoryslot.slot.itemData.itemInfo.awake3_max_lv;
                    NextMaxLv = 0;
                    break;
            }

            CurrentItemMaxLevelText.text = string.Format("LV.{0}", maxLv);
            CurrentItemIconImage.sprite = MyInventoryslot.currentSpriteImage;
            if (MyInventoryslot.slot.item.Unlocked)
                CurrentItemIconImage.color = new Color(1, 1, 1, 1);
            else
                CurrentItemIconImage.color = new Color(0, 0, 0, 1);
            CurrentItemOutLineImage.sprite = MyInventoryslot.currentOutlineSpriteImage;
            for (int i = 0; i < CurrentItemAwakeLvImages.Length; i++)
                CurrentItemAwakeLvImages[i].gameObject.SetActive(i < MyInventoryslot.slot.item.AwakeLv);
            CurrentGradeText.text = MyInventoryslot._GradeText;
            CurrentLevelText.text = string.Format("Lv.{0}", MyInventoryslot.slot.item.Level);

            NextItemMaxLevelText.text = string.Format("LV.{0}", NextMaxLv);
            NextItemIconImage.sprite = MyInventoryslot.currentSpriteImage;
            if (MyInventoryslot.slot.item.Unlocked)
                NextItemIconImage.color = new Color(1, 1, 1, 1);
            else
                NextItemIconImage.color = new Color(0, 0, 0, 1);
            NextItemOutLineImage.sprite = MyInventoryslot.currentOutlineSpriteImage;
            for (int i = 0; i < NextItemAwakeLvImages.Length; i++)
                NextItemAwakeLvImages[i].gameObject.SetActive(i < MyInventoryslot.slot.item.AwakeLv + 1);
            NextGradeText.text = MyInventoryslot._GradeText;
            NextLevelText.text = string.Format("Lv.{0}", MyInventoryslot.slot.item.Level);

            if (awakeLv < 3)
            {
                bool isAwake = (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicStone).value >= NeedGemCost) &&
                                (MyInventoryslot.slot.item.amount >= 1);
                AwakeButton.enabled = isAwake;
                CantAwakeButton.gameObject.SetActive(!isAwake);
                AwakeGemCount.text = NeedGemCost.ToString();
            }
            else
            {
                AwakeButton.enabled = false;
                CantAwakeButton.gameObject.SetActive(true);
                AwakeGemCount.text = NeedGemCost.ToString();
            }


            
        }
        void CurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.MagicStone)
                    return;
            }
            if (MyInventoryslot == null)
                return;

            int awakeLv = MyInventoryslot.slot.item.AwakeLv;
            switch(awakeLv)
            {
                case 0:
                    NeedGemCost = MyInventoryslot.slot.itemData.itemInfo.awake_gem_cost;
                    break;
                case 1:
                    NeedGemCost = MyInventoryslot.slot.itemData.itemInfo.awake_gem_cost;
                    break;
                case 2:
                    NeedGemCost = MyInventoryslot.slot.itemData.itemInfo.awake_gem_cost;
                    break;
                case 3:
                    NeedGemCost=0;
                    break;
            }
            bool isAwake = (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicStone).value >= NeedGemCost);
            if (awakeLv < 3)
            {
                AwakeButton.enabled = isAwake;
                CantAwakeButton.gameObject.SetActive(!isAwake);
            }
            else
            {
                AwakeButton.enabled = false;
                CantAwakeButton.gameObject.SetActive(true);
            }
             
            AwakeGemCount.text = NeedGemCost.ToString();
            for (int i = 0; i < CurrentItemAwakeLvImages.Length; i++)
                CurrentItemAwakeLvImages[i].gameObject.SetActive(i < MyInventoryslot.slot.item.AwakeLv);
            for (int i = 0; i < NextItemAwakeLvImages.Length; i++)
                NextItemAwakeLvImages[i].gameObject.SetActive(i < MyInventoryslot.slot.item.AwakeLv + 1);

        }
    }

}
