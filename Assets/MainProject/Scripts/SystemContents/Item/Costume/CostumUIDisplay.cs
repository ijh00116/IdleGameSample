using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class CostumUIDisplay : MonoBehaviour
    {
        [SerializeField] UI.BottomDialogType _type;
        public CostumInventorySlot slot;

        [SerializeField] Button EquipButton;
        [SerializeField] Button SuperLevelupButton;
        [SerializeField] Button LevelupButton;
        [SerializeField] Button CantLevelupButton;
        [SerializeField] Button BuyButton;
        [SerializeField] Text BuyText;

        [SerializeField] Text ItemLevel;
        [SerializeField] Text AbillityValue;
        [SerializeField] Text CostumName;
        [SerializeField] Image UIImage;

        [SerializeField] Text PossibleLvup;
        [SerializeField] Text NeedCost;

        Sprite currentSpriteImage;

        LevelUpUIType levelupUIType;

        public void Init(CostumInventorySlot _slot)
        {
            Message.AddListener<UI.Event.CurrencyChange>(CurrencyUpdated);
            slot = _slot;
            if (EquipButton != null)
                EquipButton.onClick.AddListener(EquipItem);
            LevelupButton.onClick.AddListener(AddLevel);
            SuperLevelupButton.onClick.AddListener(SuperAddLevel);
            BuyButton.onClick.AddListener(BuyCostum);
            SuperLevelupButton.gameObject.SetActive(false);

            slot.onAfterUpdated += SlotUIUpdate;
            slot.onBeforeUpdated += SlotUIBeforUpdate;

            levelupUIType = LevelUpUIType.Levelup_1;
            //SlotUIUpdate(null);

            LocalValue costumnamelocal = InGameDataTableManager.LocalizationList.weapon.Find(o => o.id == slot.itemData.CostumInfo.name);
            CostumName.text = costumnamelocal.GetStringForLocal(true);

            LocalValue lv = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == slot.itemData.abilinfo.name);
            //능력치
            AbillityValue.text = string.Format("{0} {1}%", lv.GetStringForLocal(true), slot.itemData.Value.ToDisplay());
        }

        private void Awake()
        {
        }

        private void OnDestroy()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyUpdated);
        }

        void EquipItem()
        {
            if (slot == null)
                return;
            if (slot.item.Unlocked==false)
                return;
            if(Common.InGameManager.Instance.mainplayerCharacter._state.CurrentState!=eActorState.Move
                &&Common.InGameManager.Instance.mainplayerCharacter._state.CurrentState != eActorState.Idle)
            {
                Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup("전투중에는 코스튬을 입을수 없습니다"));
                return;
            }

            slot.item.Equiped = true;
            slot.EquipItem();

            EquipButton.transform.GetChild(0).gameObject.SetActive(true);
            Common.InGameManager.Instance.Localdata.SaveCostumData();
        }

        void BuyCostum()
        {
            if(Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value>= slot.itemData.CostumInfo.buy_gem)
            {
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem, -slot.itemData.CostumInfo.buy_gem);
                slot.AddAmount();
                Common.InGameManager.Instance.Localdata.SaveCostumData();
            }
        }

        void AddLevel()
        {
            if (slot == null)
                return;
            if (slot.item.Unlocked == false)
                return;

            BigInteger costLevelUp;
            switch (levelupUIType)
            {
                case LevelUpUIType.Levelup_1:
                    costLevelUp = slot.itemData.CostPotion_1;
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    slot.AddLevel(slot.itemData.PossibleLvUpCount_1);
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.COSTUM_LEVELUP, 1);
                    break;
                case LevelUpUIType.Levelup_10:
                    costLevelUp = slot.itemData.CostPotion_10;
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    slot.AddLevel(slot.itemData.PossibleLvUpCount_10);
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.COSTUM_LEVELUP, 10);
                    break;
                case LevelUpUIType.Levelup_100:
                    costLevelUp = slot.itemData.CostPotion_100;
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    slot.AddLevel(slot.itemData.PossibleLvUpCount_100);
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.COSTUM_LEVELUP, 100);
                    break;
                default:
                    break;
            }
            Common.InGameManager.Instance.Localdata.SaveCostumData();
        }

        void SuperAddLevel()
        {
            if (slot.item.Unlocked == false)
                return;

            slot.AddLevel(1);
            Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.COSTUM_LEVELUP, 1);
        }

        public void SlotUIUpdate(CostumInventorySlot _slot)
        {
            if (gameObject.activeInHierarchy == false)
                this.gameObject.SetActive(true);

            if (slot.itemData.CostumInfo.buy_gem == 0 && slot.item.Unlocked==false)
            {
                BuyCostum();
            }

            //이미지
            if (currentSpriteImage == null)
            {
                currentSpriteImage = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Costum/{0}", slot.itemData.CostumInfo.icon));
            }
            UIImage.sprite = currentSpriteImage;

            if (slot.item.Unlocked)
                UIImage.color = Color.white;
            else
                UIImage.color = Color.black;
            //UIImage.SetNativeSize();
            //구매되어 잠겨있나 안잠겨있나 확인
            if (slot.item.Unlocked==false)
            {
                BuyButton.gameObject.SetActive(true);
                EquipButton.gameObject.SetActive(false);
                LevelupButton.gameObject.SetActive(false);
                SuperLevelupButton.gameObject.SetActive(false);
                BuyText.text = string.Format("{0}", slot.itemData.CostumInfo.buy_gem);
                return;
            }
            else
            {
                BuyButton.gameObject.SetActive(false);
                EquipButton.gameObject.SetActive(true);
                LevelupButton.gameObject.SetActive(true);
                //SuperLevelupButton.gameObject.SetActive(true);
            }
           
            //장착
            EquipButton.transform.GetChild(0).gameObject.SetActive(slot.item.Equiped);
            //레벨
            ItemLevel.text =string.Format("LV.{0}", slot.item.Level.ToString());

            DebugExtention.ColorLog("red", Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value.ToDisplay());
            //레벨업버튼
            LevelUpUISetting(levelupUIType);

            LocalValue lv= InGameDataTableManager.LocalizationList.ability.Find(o => o.id == slot.itemData.abilinfo.name);
            //능력치
            AbillityValue.text = string.Format("{0} {1}%", lv.GetStringForLocal(true),slot.itemData.Value.ToDisplay());
        }

        public void SlotUIBeforUpdate(CostumInventorySlot _slot)
        {
            EquipButton.transform.GetChild(0).gameObject.SetActive(slot.item.Equiped);
        }

        void CurrencyUpdated(UI.Event.CurrencyChange msg)
        {
            //if (_type != Common.InGameManager.Instance.BottomDialogtype)
           //     return;
            if (msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.MagicPotion)
                    return;
            }
                

            LevelUpUISetting(levelupUIType);
        }
        
        public void LevelUpUISetting(LevelUpUIType lvupType)
        {
            if(lvupType!=LevelUpUIType.None)
                levelupUIType = lvupType;

            slot.itemData.FindNeedCost();

            switch (levelupUIType)
            {
                case LevelUpUIType.Levelup_1:
                    PossibleLvup.text = "Lv+" + slot.itemData.PossibleLvUpCount_1.ToString();
                    NeedCost.text = "-" + slot.itemData.CostPotion_1.ToDisplay();
                    if (slot.item.Level < slot.itemData.CostumInfo.max_lv)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.itemData.CostPotion_1)
                        {
                            LevelupButton.enabled = true;
                            CantLevelupButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            LevelupButton.enabled = false;
                            CantLevelupButton.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        LevelupButton.enabled = false;
                        CantLevelupButton.gameObject.SetActive(true);
                    }
                    break;
                case LevelUpUIType.Levelup_10:
                    PossibleLvup.text = "Lv+" + slot.itemData.PossibleLvUpCount_10.ToString();
                    NeedCost.text = "-" + slot.itemData.CostPotion_10.ToDisplay();
                    if (slot.item.Level < slot.itemData.CostumInfo.max_lv)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.itemData.CostPotion_10)
                        {
                            LevelupButton.enabled = true;
                            CantLevelupButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            LevelupButton.enabled = false;
                            CantLevelupButton.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        LevelupButton.enabled = false;
                        CantLevelupButton.gameObject.SetActive(true);
                    }
                    break;
                case LevelUpUIType.Levelup_100:
                    PossibleLvup.text ="Lv+" +slot.itemData.PossibleLvUpCount_100.ToString();
                    NeedCost.text = "-" + slot.itemData.CostPotion_100.ToDisplay();
                    if (slot.item.Level < slot.itemData.CostumInfo.max_lv)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.itemData.CostPotion_100)
                        {
                            LevelupButton.enabled = true;
                            CantLevelupButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            LevelupButton.enabled = false;
                            CantLevelupButton.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        LevelupButton.enabled = false;
                        CantLevelupButton.gameObject.SetActive(true);
                    }
                    break;
                default:
                    break;
            }

        }

    }

}
