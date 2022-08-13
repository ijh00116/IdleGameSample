using BlackTree.Common;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BlackTree
{
    public class ArtifactUIDisplay : MonoBehaviour
    {
        [SerializeField] UI.BottomDialogType _type;
        [HideInInspector]public RelicInventorySlot slot;

        [SerializeField] Button LevelupButton;
        [SerializeField] Button CantLevelupButton;

        [SerializeField] Text ItemLevel;
        [SerializeField] Image UIImage;

        [SerializeField] Text RelicName;
        [SerializeField] Text AbilName;

        [SerializeField] Text NeedPotion;
        [SerializeField] Text PossiblePlusLevel;

        Sprite currentSpriteImage;

        public LevelUpUIType levelupUIType;

        [Header("구매슬롯")]
        [SerializeField] GameObject Unlockedslot;
        [SerializeField] Button BuyBtn;
        [SerializeField] GameObject CantBuyBtn;

        bool LvBtnPushed = false;
        WaitForSeconds btnpushdelay = new WaitForSeconds(0.5f);
        Coroutine Btnpushevent = null;

        public void Init(RelicInventorySlot _slot)
        {
            slot = _slot;
            slot.uiDisplay = this;

//            LevelupButton.onClick.AddListener(AddLevel);
          
            slot.onAfterUpdated += SlotUIUpdate;
            slot.onBeforeUpdated += SlotUIBeforUpdate;
            Message.AddListener<UI.Event.CurrencyChange>(CurrencyUpdated);
            BuyBtn.onClick.AddListener(BuyButton);

            if(slot.item.Unlocked)
            {
                Unlockedslot.SetActive(false);
            }
            else
            {
                Unlockedslot.SetActive(true);
                if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value >= 300)
                    CantBuyBtn.SetActive(false);
                else
                    CantBuyBtn.SetActive(true);
            }

            BuyBtn.gameObject.SetActive(false);

            EventTrigger trigger = LevelupButton.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => LvBtnDown());
            trigger.triggers.Add(pointerDown);

            var pointerDown_1 = new EventTrigger.Entry();
            pointerDown_1.eventID = EventTriggerType.PointerUp;
            pointerDown_1.callback.AddListener((e) => LvBtnUp());
            trigger.triggers.Add(pointerDown_1);
            LvBtnPushed = false;

            SlotUIUpdate();
        }

        private void Awake()
        {
        }

        private void OnDestroy()
        {
        }

        IEnumerator LvBtnPush()
        {
            yield return btnpushdelay;

            while (LvBtnPushed)
            {
                AddLevel();
                yield return null;
            }
            yield break;
        }

        void LvBtnUp()
        {
            LvBtnPushed = false;
        }
        void LvBtnDown()
        {
            LvBtnPushed = true;
            if (Btnpushevent != null)
                StopCoroutine(Btnpushevent);
            Btnpushevent = null;
            AddLevel();
            Btnpushevent = StartCoroutine(LvBtnPush());
        }

        void AddLevel()
        {
            if (slot == null)
                return;
            if (slot.item.Level >= slot.itemData.relicInfo.level_max)
                return;
            if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value < NeedPotionvalue)
                return;

                BigInteger costLevelUp;
            switch (levelupUIType)
            {
                case LevelUpUIType.Levelup_1:
                    costLevelUp = slot.itemData.NeedPotionData;
                    slot.AddLevel(slot.itemData.PossibleLvUpCount_1);
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.RELIC_LEVELUP, 1);
                    break;
                case LevelUpUIType.Levelup_10:
                    costLevelUp = slot.itemData.NeedPotionData_10;
                    slot.AddLevel(slot.itemData.PossibleLvUpCount_10);
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.RELIC_LEVELUP, 10);
                    break;
                case LevelUpUIType.Levelup_100:
                    costLevelUp = slot.itemData.NeedPotionData_100;
                    slot.AddLevel(slot.itemData.PossibleLvUpCount_100);
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, -costLevelUp);
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.RELIC_LEVELUP, 100);
                    break;
                default:
                    break;
            }
            InGameManager.Instance.Localdata.SaverelicData();
        }

        BigInteger NeedPotionvalue;
        public void LevelUpUISetting(LevelUpUIType lvupType)
        {
            levelupUIType = lvupType;
             
            switch (levelupUIType)
            {
                case LevelUpUIType.Levelup_1:
                    NeedPotion.text = slot.itemData.NeedPotionData.ToDisplay();
                    PossiblePlusLevel.text = "+Lv." + slot.itemData.PossibleLvUpCount_1.ToString();
                    NeedPotionvalue = slot.itemData.NeedPotionData;
                    break;
                case LevelUpUIType.Levelup_10:
                    NeedPotion.text = slot.itemData.NeedPotionData_10.ToDisplay();
                    PossiblePlusLevel.text = "+Lv." + slot.itemData.PossibleLvUpCount_10.ToString();
                    NeedPotionvalue = slot.itemData.NeedPotionData_10;
                    break;
                case LevelUpUIType.Levelup_100:
                    NeedPotion.text =slot.itemData.NeedPotionData_100.ToDisplay();
                    PossiblePlusLevel.text="+Lv."+ slot.itemData.PossibleLvUpCount_100.ToString();
                    NeedPotionvalue = slot.itemData.NeedPotionData_100;
                    break;
                default:
                    break;
            }

            if (slot.item.Level < slot.itemData.relicInfo.level_max)
            {
                if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value >= NeedPotionvalue)
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
        }

        public void SlotUIUpdate()
        {
            if (slot.item.Unlocked)
            {
                Unlockedslot.SetActive(false);
            }
            else
            {
                Unlockedslot.SetActive(true);
                if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value >= 300)
                    CantBuyBtn.SetActive(false);
                else
                    CantBuyBtn.SetActive(true);
            }
            if (currentSpriteImage == null)
            {
                currentSpriteImage = Resources.Load<Sprite>(string.Format("Images/GUI/Relic/{0}", slot.itemData.relicInfo.icon));
                UIImage.sprite = currentSpriteImage;
            }
            //임시 테스트
            ItemLevel.text = string.Format("LV.{0}",slot.item.Level.ToString());
            RelicName.text = slot.itemData.relicName;
            AbilName.text = string.Format(slot.itemData.abilName, slot.itemData.Value.ToFloat());
            NeedPotion.text = slot.itemData.NeedPotionData.ToDisplay();
            if (slot.item.Unlocked == false)
            {
                return;
            }
            LevelUpUISetting(levelupUIType);
        }

        public void SlotUIBeforUpdate()
        {
            if (slot.itemData == null)
                return;
        }

        void CurrencyUpdated(UI.Event.CurrencyChange msg)
        {
            if(msg.Type==CurrencyType.Gem)
            {
                if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value >= 300)
                    CantBuyBtn.SetActive(false);
                else
                    CantBuyBtn.SetActive(true);
            }
            if (msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.MagicPotion)
                    return;
            }
                
            LevelUpUISetting(levelupUIType);

        }

        void BuyButton()
        {
            if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value >= 300)
            {
                slot.AddAmount(1);
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem, -300);
                Unlockedslot.SetActive(false);

                ArtifactUIDisplay nextUI=null;
                for(int i=0; i< InGameManager.Instance.RelicInventory.GetSlots.Count; i++)
                {
                    if(InGameManager.Instance.RelicInventory.GetSlots[i].uiDisplay==this)
                    {
                        if (i + 1 < InGameManager.Instance.RelicInventory.GetSlots.Count)
                        {
                            nextUI = InGameManager.Instance.RelicInventory.GetSlots[i + 1].uiDisplay;
                            break;
                        }
                    }
                }

                if(nextUI!=null)
                {
                    nextUI.ActivateBuyButton();
                }

                InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.RELIC_COUNT, 1);
                InGameManager.Instance.Localdata.SaverelicData();
            }
        }

        public void ActivateBuyButton()
        {
            BuyBtn.gameObject.SetActive(true);
        }
    
    }

}
