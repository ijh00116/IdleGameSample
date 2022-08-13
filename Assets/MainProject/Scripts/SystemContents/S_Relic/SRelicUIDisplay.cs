using BlackTree.Common;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class SRelicUIDisplay : MonoBehaviour
    {
        [SerializeField] UI.BottomDialogType _type;
        [HideInInspector] public SRelicInventorySlot slot;

        [SerializeField] Button LevelupBtn_Essence;
        [SerializeField] Image CantLevelupBtn_Essence;
        [SerializeField] Button LevelupButton;
        [SerializeField] Image CantLevelupButton;

        [SerializeField] Text ItemLevel;
        [SerializeField] Image UIImage;

        [SerializeField] Text RelicName;
        [SerializeField] Text AbilName;

        [SerializeField] Text NeedPotion;
        //[SerializeField] Text PossiblePlusLevel;

        [SerializeField] Slider AmountBar;
        [SerializeField] Text AmountBartext;

        [SerializeField] Image amountFillImage;
        [SerializeField] GameObject PossibleLvUpArrow;
        [SerializeField] Sprite FillYellowImage;
        [SerializeField] Sprite FillGreenImage;
        public Sprite currentSpriteImage;

        public LevelUpUIType levelupUIType;

        public string LocalName;
        public void Init(SRelicInventorySlot _slot)
        {
            slot = _slot;

            LevelupButton.onClick.AddListener(AddLevel);
            LevelupBtn_Essence.onClick.AddListener(AddlevelForEssence);
            slot.onAfterUpdated += SlotUIUpdate;
            slot.onBeforeUpdated += SlotUIBeforUpdate;
            Message.AddListener<UI.Event.CurrencyChange>(CurrencyUpdated);
        }

        void AddLevel()
        {
            if (slot == null)
                return;

            BigInteger costLevelUp;
            switch (levelupUIType)
            {
                case LevelUpUIType.Levelup_1:
                    costLevelUp = slot.srelicData.NeedPotionData;
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value < costLevelUp
                        || slot.srelic.amount< slot.srelicData.NeedMyItemForLevelup)
                        return;
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    slot.AddAmount(-slot.srelicData.NeedMyItemForLevelup);
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.S_RELIC_ENCHANT, 1);
                    slot.AddLevel(slot.srelicData.PossibleLvUpCount_1);
                    break;
                case LevelUpUIType.Levelup_10:
                    costLevelUp = slot.srelicData.NeedPotionData_10;
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value < costLevelUp
                        || slot.srelic.amount < slot.srelicData.NeedMyItemForLevelup)
                        return;
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    slot.AddAmount(-slot.srelicData.NeedMyItemForLevelup);
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.S_RELIC_ENCHANT, 10);
                    slot.AddLevel(slot.srelicData.PossibleLvUpCount_10);
                    break;
                case LevelUpUIType.Levelup_100:
                    costLevelUp = slot.srelicData.NeedPotionData_100;
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value < costLevelUp
                        || slot.srelic.amount < slot.srelicData.NeedMyItemForLevelup)
                        return;
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    slot.AddAmount(-slot.srelicData.NeedMyItemForLevelup);
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.S_RELIC_ENCHANT, 100);
                    slot.AddLevel(slot.srelicData.PossibleLvUpCount_100);
                    break;
                default:
                    break;
            }
            Common.InGameManager.Instance.Localdata.SavesrelicData();
        }

        void AddlevelForEssence()
        {
            if (slot.srelic.amount < slot.srelicData.NeedMyItemForLevelup)
                return;
            if(Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.SuperMagicPotion).value>=1)
            {
                slot.AddLevel(1);
            }
        }

        public void LevelUpUISetting(LevelUpUIType lvupType)
        {
            levelupUIType = lvupType;
            Data_Character _character = CharacterDataManager.Instance.PlayerCharacterdata;
            switch (levelupUIType)
            {
                case LevelUpUIType.Levelup_1:
                    NeedPotion.text = slot.srelicData.NeedPotionData.ToDisplay();
                    //PossiblePlusLevel.text = "+Lv." + slot.srelicData.PossibleLvUpCount_1.ToString();
                    if (slot.srelic.Level < slot.srelicData.srelicInfo.level_max)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.srelicData.NeedPotionData
                                && slot.srelic.amount >= slot.srelicData.NeedMyItemForLevelup)
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
                    NeedPotion.text = slot.srelicData.NeedPotionData_10.ToDisplay();
                   // PossiblePlusLevel.text = "+Lv." + slot.srelicData.PossibleLvUpCount_10.ToString();
                    if (slot.srelic.Level < slot.srelicData.srelicInfo.level_max)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.srelicData.NeedPotionData_10
                            && slot.srelic.amount >= slot.srelicData.NeedMyItemForLevelup)
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
                    NeedPotion.text =slot.srelicData.NeedPotionData_100.ToDisplay();
                    //PossiblePlusLevel.text = "+Lv." + slot.srelicData.PossibleLvUpCount_100.ToString();
                    if (slot.srelic.Level < slot.srelicData.srelicInfo.level_max)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.srelicData.NeedPotionData_100
                            && slot.srelic.amount >= slot.srelicData.NeedMyItemForLevelup)
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

        public void SlotUIUpdate()
        {
            if (currentSpriteImage == null)
            {
                currentSpriteImage = Resources.Load<Sprite>(string.Format("Images/GUI/Srelic/{0}", slot.srelicData.srelicInfo.icon));
                UIImage.sprite = currentSpriteImage;
            }
            if (slot.srelic.Unlocked == false)
                UIImage.color = Color.black;
            else
                UIImage.color = Color.white;
            if (slot.srelicData.NeedMyItemForLevelup >= 1)
                CantLevelupBtn_Essence.gameObject.SetActive(false);
            else
                CantLevelupBtn_Essence.gameObject.SetActive(true);

            ItemLevel.text =string.Format("LV.{0}", slot.srelic.Level.ToString()) ;
            RelicName.text = slot.srelicData.srelicName;
            AbilName.text = string.Format(slot.srelicData.abilName, slot.srelicData.Value.ToFloat());
            NeedPotion.text =slot.srelicData.NeedPotionData.ToDisplay();
            if (slot.srelic.Level >= slot.srelicData.srelicInfo.level_max)
                AmountBar.value = 1;
            else
                AmountBar.value = (slot.srelic.amount / slot.srelicData.NeedMyItemForLevelup);

            AmountBartext.text= string.Format("{0}/{1}", slot.srelic.amount , slot.srelicData.NeedMyItemForLevelup); 

            if(AmountBar.value>=1)
            {
                amountFillImage.sprite = FillGreenImage;
                PossibleLvUpArrow.SetActive(true);
            }
            else
            {
                amountFillImage.sprite = FillYellowImage;
                PossibleLvUpArrow.SetActive(false);
            }
            LevelUpUISetting(levelupUIType);
        }

        public void SlotUIBeforUpdate()
        {
            if (slot.srelicData == null)
                return;
        }

        void CurrencyUpdated(UI.Event.CurrencyChange msg)
        {
            if (msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.MagicPotion)
                    return;
            }
                
            switch (levelupUIType)
            {
                case LevelUpUIType.Levelup_1:
                    NeedPotion.text =slot.srelicData.NeedPotionData.ToDisplay();
                    //PossiblePlusLevel.text = "+Lv." + slot.srelicData.PossibleLvUpCount_1.ToString();
                    if (slot.srelic.Level < slot.srelicData.srelicInfo.level_max)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.srelicData.NeedPotionData
                             && slot.srelic.amount >= slot.srelicData.NeedMyItemForLevelup)
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
                    NeedPotion.text =slot.srelicData.NeedPotionData_10.ToDisplay();
                    if (slot.srelic.Level < slot.srelicData.srelicInfo.level_max)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.srelicData.NeedPotionData_10
                             && slot.srelic.amount >= slot.srelicData.NeedMyItemForLevelup)
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
                    NeedPotion.text = slot.srelicData.NeedPotionData_100.ToDisplay();
                    //PossiblePlusLevel.text = "+Lv." + slot.srelicData.PossibleLvUpCount_100.ToString();
                    if (slot.srelic.Level < slot.srelicData.srelicInfo.level_max)
                    {
                        if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= slot.srelicData.NeedPotionData_100
                             && slot.srelic.amount >= slot.srelicData.NeedMyItemForLevelup)
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
