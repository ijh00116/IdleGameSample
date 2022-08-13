using DLL_Common.Common;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree
{
    public class ItemEnforceWindow : MonoBehaviour
    {
        [SerializeField] Text GradeText;
        [SerializeField] Image[] StarImages;
        [SerializeField] Image OutLineIconImage;
        [SerializeField] Image IconImage;

        [SerializeField] List<EffectText> EquipEffectList=new List<EffectText>();
        [SerializeField] List<EffectText> PossessionEffectList = new List<EffectText>();

        [SerializeField] Button EquipButton;
        [SerializeField] Button AlreadyEquipButton;
        [SerializeField] Button EnforceButton;
        [SerializeField] Button CantEnforceButton;
        [SerializeField] Text EnforceCost;
        [SerializeField] Text LevelText;

        [SerializeField] Slider amountslider;
        [SerializeField] Text amountText;

        private ItemUIDisplay MyInventoryslotui;

        BigInteger EnforceJewelCount=BigInteger.Zero;

        [SerializeField] Image amountFillImage;
        [SerializeField] GameObject PossibleLvUpArrow;
        [SerializeField] Sprite FillYellowImage;
        [SerializeField] Sprite FillGreenImage;

        bool LvBtnPushed = false;
        WaitForSeconds btnpushdelay = new WaitForSeconds(0.5f);
        Coroutine Btnpushevent = null;

        public void Init()
        {
            Message.AddListener<UI.Event.CurrencyChange>(CurrencyUpdated);
            
            EquipButton.onClick.AddListener(PushEquip);
            //EnforceButton.onClick.AddListener(PushEnforce);

            EventTrigger trigger = EnforceButton.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => LvBtnDown());
            trigger.triggers.Add(pointerDown);

            var pointerDown_1 = new EventTrigger.Entry();
            pointerDown_1.eventID = EventTriggerType.PointerUp;
            pointerDown_1.callback.AddListener((e) => LvBtnUp());
            trigger.triggers.Add(pointerDown_1);
            LvBtnPushed = false;
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyUpdated);
        }
        public void PopupSetting(ItemUIDisplay slot)
        {
            MyInventoryslotui = slot;


            for (int i = 0; i < EquipEffectList.Count; i++)
                EquipEffectList[i].gameObject.SetActive(false);
            for (int i = 0; i < PossessionEffectList.Count; i++)
                PossessionEffectList[i].gameObject.SetActive(false);

            UpdateData();
        }

        IEnumerator LvBtnPush()
        {
            yield return btnpushdelay;

            while (LvBtnPushed)
            {
                PushEnforce();
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
            PushEnforce();
            Btnpushevent = StartCoroutine(LvBtnPush());
        }

        void PushEnforce()
        {
            if (MyInventoryslotui.slot == null)
                return;
            if (MyInventoryslotui.slot.item.Unlocked==false)
                return;
            if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.EnchantStone).value < EnforceJewelCount)
                return;

            int maxlevel = 100;
            switch (MyInventoryslotui.slot.item.AwakeLv)
            {
                case 0:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.max_lv;
                    break;
                case 1:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake_max_lv;
                    break;
                case 2:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake2_max_lv;
                    break;
                case 3:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake3_max_lv;
                    break;
            }
            if (MyInventoryslotui.slot.item.Level < maxlevel)
            {
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.EnchantStone, -EnforceJewelCount);
                MyInventoryslotui.slot.AddLevel(1);
                if (MyInventoryslotui.slot.itemData.itemtype == ItemType.weapon)
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.WEAPON_ENCHANT, 1);
                if (MyInventoryslotui.slot.itemData.itemtype == ItemType.wing)
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.WING_ENCHANT, 1);
                UpdateData();
            }
            if(MyInventoryslotui.slot.parent.type==ItemType.weapon)
                Common.InGameManager.Instance.Localdata.SaveweaponData();
            else
                Common.InGameManager.Instance.Localdata.SavewingData();
        }

        void PushEquip()
        {
            if (MyInventoryslotui.slot == null)
                return;
            if (MyInventoryslotui.slot.item.Unlocked==false)
                return;

            MyInventoryslotui.slot.item.Equiped=true;
            MyInventoryslotui.slot.EquipItem();

            EquipButton.gameObject.SetActive(!MyInventoryslotui.slot.item.Equiped);
            AlreadyEquipButton.gameObject.SetActive(MyInventoryslotui.slot.item.Equiped);

            if (MyInventoryslotui.slot.parent.type == ItemType.weapon)
                Common.InGameManager.Instance.Localdata.SaveweaponData();
            else
                Common.InGameManager.Instance.Localdata.SavewingData();
        }

        void UpdateData()
        {
            ItemData _itemdata = MyInventoryslotui.slot.itemData;

            amountslider.value = (float)MyInventoryslotui.slot.item.amount / 5.0f;
            amountText.text = string.Format("{0}/5", MyInventoryslotui.slot.item.amount);

            if(MyInventoryslotui.slot.item.Unlocked)
            {
                EquipButton.gameObject.SetActive(!MyInventoryslotui.slot.item.Equiped);
                AlreadyEquipButton.gameObject.SetActive(MyInventoryslotui.slot.item.Equiped);
            }
            else
            {
                EquipButton.gameObject.SetActive(false);
                AlreadyEquipButton.gameObject.SetActive(false);
            }
           
            int EffectCount = 0;
            //보유 효과 텍스트 세팅
            foreach (KeyValuePair<AbilitiesType, Dictionary<int, BigInteger>> data in _itemdata.B_abilityList)
            {
                AbilityInfo abilinfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.abtype == data.Key.ToString());
                LocalValue name = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo.name);
                
                foreach (KeyValuePair<int, BigInteger> _abildata in data.Value)
                {
                    BigInteger _currentValue = _abildata.Value;
                    AbilityInfo _abilinfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == _abildata.Key);
                    LocalValue lv = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo.desc);
                    BigInteger _nextvalue = GetNextvalue(_abilinfo.level_unit, _itemdata.B_AbilityGainTypelist[_abildata.Key.ToString()]);
                    string currentvalue;
                    string nextvalue;
                    if (abilinfo.desc== "ability_value_06"|| abilinfo.desc == "ability_value_07"|| abilinfo.desc == "ability_value_08"|| abilinfo.desc == "ability_value_09")
                    {
                        currentvalue = string.Format(lv.GetStringForLocal(true), _currentValue.ToFloat());
                        nextvalue = string.Format(lv.GetStringForLocal(true), _nextvalue.ToFloat());
                    }
                    else
                    {
                        currentvalue = string.Format(lv.GetStringForLocal(true), _currentValue.ToDisplay());
                        nextvalue = string.Format(lv.GetStringForLocal(true), _nextvalue.ToDisplay());
                    }
                    PossessionEffectList[EffectCount++].SetText(name.kr, currentvalue, nextvalue, true);
                }
            }
            EffectCount = 0;
            //장착시 적용
            foreach (KeyValuePair<AbilitiesType, Dictionary<int, BigInteger>> data in _itemdata.A_abilityList)
            {
                AbilityInfo abilinfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.abtype == data.Key.ToString());
                LocalValue name = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo.name);
                foreach (KeyValuePair<int, BigInteger> _abildata in data.Value)
                {
                    BigInteger _currentValue = _abildata.Value;
                    AbilityInfo _abilinfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == _abildata.Key);
                    LocalValue lv = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo.desc);
                    BigInteger _nextvalue = GetNextvalue(_abilinfo.level_unit, _itemdata.A_AbilityGainTypelist[_abildata.Key.ToString()]);
                    string currentvalue;
                    string nextvalue;
                    if (abilinfo.desc == "ability_value_06" || abilinfo.desc == "ability_value_07" || abilinfo.desc == "ability_value_08" || abilinfo.desc == "ability_value_09")
                    {
                        currentvalue = string.Format(lv.GetStringForLocal(true), _currentValue.ToFloat());
                        nextvalue = string.Format(lv.GetStringForLocal(true), _nextvalue.ToFloat());
                    }
                    else
                    {
                        currentvalue = string.Format(lv.GetStringForLocal(true), _currentValue.ToDisplay());
                        nextvalue = string.Format(lv.GetStringForLocal(true), _nextvalue.ToDisplay());
                    }

                    EquipEffectList[EffectCount++].SetText(name.kr, currentvalue,nextvalue, true);
                }
            }

            //맥스레벨에 따른 강화 버튼 활성화
            int maxlevel = 100;
            switch(MyInventoryslotui.slot.item.AwakeLv)
            {
                case 0:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.max_lv;
                    break;
                case 1:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake_max_lv;
                    break;
                case 2:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake2_max_lv;
                    break;
                case 3:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake3_max_lv;
                    break;
            }

            if (MyInventoryslotui.slot.item.Level< maxlevel)
            {
                EnforceJewelCount = _itemdata.enchantStone_Value;
                EnforceCost.text = string.Format(EnforceJewelCount.ToDisplay());
                if(Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.EnchantStone).value >= EnforceJewelCount)
                {
                    EnforceButton.enabled=true;
                    CantEnforceButton.gameObject.SetActive(false);
                }
                else
                {
                    EnforceButton.enabled = false;
                    CantEnforceButton.gameObject.SetActive(true);
                }
            }
            else
            {
                EnforceButton.enabled = false;
                CantEnforceButton.gameObject.SetActive(true);
            }
            LevelText.text = "LV." + MyInventoryslotui.slot.item.Level.ToString();

            IconImage.sprite = MyInventoryslotui.currentSpriteImage;
            OutLineIconImage.sprite = MyInventoryslotui.currentOutlineSpriteImage;
            GradeText.text = MyInventoryslotui._GradeText;

            for (int i = 0; i < StarImages.Length; i++)
            {
                StarImages[i].gameObject.SetActive(i < MyInventoryslotui.slot.item.AwakeLv);
            }

            if (MyInventoryslotui.slot.item.Unlocked == false)
                IconImage.color = new Color(0, 0, 0, 1);
            else
                IconImage.color = new Color(1, 1, 1, 1);

            //레벨업 가능할때 이미지 교체
            if (amountslider.value >= 1)
            {
                amountFillImage.sprite = FillGreenImage;
                PossibleLvUpArrow.SetActive(true);
            }
            else
            {
                amountFillImage.sprite = FillYellowImage;
                PossibleLvUpArrow.SetActive(false);
            }
        }

        void CurrencyUpdated(UI.Event.CurrencyChange msg)
        {
            if(msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.EnchantStone && msg.Type != CurrencyType.Gem)
                    return;
            }
            

            if (MyInventoryslotui == null)
                return;
            if (EnforceJewelCount == 0)
                return;
            ItemData _itemdata = MyInventoryslotui.slot.itemData;

            //맥스레벨에 따른 강화 버튼 활성화
            int maxlevel = 100;
            switch (MyInventoryslotui.slot.item.AwakeLv)
            {
                case 0:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.max_lv;
                    break;
                case 1:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake_max_lv;
                    break;
                case 2:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake2_max_lv;
                    break;
                case 3:
                    maxlevel = MyInventoryslotui.slot.itemData.itemInfo.awake3_max_lv;
                    break;
            }

            if (MyInventoryslotui.slot.item.Level < maxlevel)
            {
                EnforceJewelCount = _itemdata.enchantStone_Value;
                EnforceCost.text = string.Format(EnforceJewelCount.ToDisplay());
                if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.EnchantStone).value >= EnforceJewelCount)
                {
                    EnforceButton.enabled = true;
                    CantEnforceButton.gameObject.SetActive(false);
                }
                else
                {
                    EnforceButton.enabled = false;
                    CantEnforceButton.gameObject.SetActive(true);
                }
            }
        }



        BigInteger GetNextvalue(BigInteger currentValue, string gaintype)
        {
            BigInteger big = new BigInteger(currentValue);
            switch (gaintype)
            {
                case "weapon_gain_1":
                        big = big * InGameDataTableManager.ItemTableList.gain[MyInventoryslotui.slot.item.Level].weapon_gain_1;
                    break;
                case "weapon_gain_2":
                        big = big * InGameDataTableManager.ItemTableList.gain[MyInventoryslotui.slot.item.Level].weapon_gain_2;
                    break;
                case "weapon_gain_3":
                        big = big * InGameDataTableManager.ItemTableList.gain[MyInventoryslotui.slot.item.Level].weapon_gain_3;
                    break;
                case "weapon_gain_4":
                        big = big * InGameDataTableManager.ItemTableList.gain[MyInventoryslotui.slot.item.Level].weapon_gain_4;
                    break;
                case "weapon_gain_5":
                        big = big * InGameDataTableManager.ItemTableList.gain[MyInventoryslotui.slot.item.Level].weapon_gain_5;
                    break;
                default:
                    break;
            }
            return big;
        }

    }

}
