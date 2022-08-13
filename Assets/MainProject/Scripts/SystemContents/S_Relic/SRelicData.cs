using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class SRelicData
    {
        public AbilitiesType abilitytype;
        public ItemType type = ItemType.s_relic;

        public SRelicInfo srelicInfo;
        public RelicNeedCurrency srelicNeedCurrency;

        public LevelUpType MaxLevelType = LevelUpType.LevelUp_100;
        public BigInteger NeedPotionData = BigInteger.Zero;
        public BigInteger NeedPotionData_10;
        public BigInteger NeedPotionData_100;

        public int PossibleLvUpCount_1;
        public int PossibleLvUpCount_10;
        public int PossibleLvUpCount_100;

        public float BaseRelicvalue;
        public string srelicName;
        public string abilName;

        public SRelic myItem;
        BigInteger abilityValue;

        public int NeedMyItemForLevelup;
        public BigInteger Value
        {
            get
            {
                return abilityValue;
            }
        }

        public Sprite MyUIIcon;
        public SRelicData(SRelic srelic)
        {
            NeedPotionData = BigInteger.Zero;
            abilitytype = AbilitiesType.End;
            myItem = srelic;
            abilityValue = 0;
            //테이블에서 정보 가져와서 세팅(로컬,어빌리티 베이스 값 등)
            Init();
        }

        void Init()
        {
            srelicInfo = null;
            for (int i = 0; i < InGameDataTableManager.RelicList.s_relic.Count; i++)
            {
                if (InGameDataTableManager.RelicList.s_relic[i].idx == myItem.idx)
                {
                    srelicInfo = InGameDataTableManager.RelicList.s_relic[i];
                    break;
                }
            }

            if (srelicInfo == null)
                return;

            int abidx = srelicInfo.ability_idx;
            AbilityInfo abInfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == abidx);
            AbilitiesType abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abInfo.abtype);
            BaseRelicvalue = abInfo.level_unit;
            abilitytype = abiltype;
            abilityValue = BaseRelicvalue * myItem.Level;

            LocalValue relicLocalization = InGameDataTableManager.LocalizationList.relic.Find(o => o.id == srelicInfo.name);
            LocalValue abilLocalization = InGameDataTableManager.LocalizationList.relic.Find(o => o.id == srelicInfo.desc);

            srelicName = relicLocalization.kr;
            abilName = abilLocalization.kr;

            //스프라이트
            string path = string.Format("Images/GUI/Srelic");
            string itemname = srelicInfo.icon;
            string fullPath = string.Format("{0}/{1}", path, itemname);

            MyUIIcon = Resources.Load<Sprite>(fullPath);

            //UpdateData();
        }
        public void UpdateData()
        {
            if (abilitytype == AbilitiesType.End)
                return;

            if (myItem.Level >= srelicInfo.level_max)
            {
                PossibleLvUpCount_1 = 0;
                PossibleLvUpCount_10 = 0;
                PossibleLvUpCount_100 = 0;
                NeedPotionData = 0;
                NeedPotionData_10 = 0;
                NeedPotionData_100 = 0;
            }
            else
            {
                srelicNeedCurrency = InGameDataTableManager.RelicList.gain.Find(o => o.level == (myItem.Level));
                FindNeedPotion();
            }
            //Message.Send<UI.Event.CharacterInfoUIUpdate>(new UI.Event.CharacterInfoUIUpdate());

            //if (myItem.Unlocked == false)
            //    return;

            abilityValue = BaseRelicvalue * myItem.Level;

            CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.SRelic, abilitytype, srelicInfo.ability_idx, abilityValue);

            Common.InGameManager.Instance.SpecialSkillInventory.SkillLevelUpdate(srelicInfo.skill_idx);
        }


        void FindNeedPotion()
        {
            BigInteger LevelupTotalCurrency;
            BigInteger PlusGold;
            switch (srelicInfo.level_up_potion_type)
            {
                case "level_up_potion_5":
                    MaxLevelType = LevelUpType.LevelUp_5;
                    NeedPotionData = srelicNeedCurrency.level_up_potion_5;
                    break;
                case "level_up_potion_100":
                    MaxLevelType = LevelUpType.LevelUp_100;
                    NeedPotionData = srelicNeedCurrency.level_up_potion_100;
                    break;
                case "level_up_potion_200":
                    MaxLevelType = LevelUpType.LevelUp_200;
                    NeedPotionData = srelicNeedCurrency.level_up_potion_200;
                    break;
                case "level_up_potion_500":
                    MaxLevelType = LevelUpType.LevelUp_500;
                    NeedPotionData = srelicNeedCurrency.level_up_potion_500;
                    break;
                case "level_up_potion_1500":
                    MaxLevelType = LevelUpType.LevelUp_1500;
                    NeedPotionData = srelicNeedCurrency.level_up_potion_1500;
                    break;
                case "level_up_potion_5000":
                    MaxLevelType = LevelUpType.LevelUp_5000;
                    NeedPotionData = srelicNeedCurrency.level_up_potion_5000;
                    break;
                case "level_up_s_relic":
                    MaxLevelType = LevelUpType.LevelUp_100;
                    NeedPotionData = srelicNeedCurrency.level_up_s_relic;
                    break;
                default:
                    break;
            }

            PossibleLvUpCount_1 = 1;
            LevelupTotalCurrency = BigInteger.Zero;
            PlusGold = BigInteger.Zero;
            for (int i = 0; i < 10; i++)
            {
                if (myItem.Level + i >= srelicInfo.level_max)
                    break;
              
                switch (srelicInfo.level_up_potion_type)
                {
                    case "level_up_potion_5":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5;
                        break;
                    case "level_up_potion_100":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_100;
                        break;
                    case "level_up_potion_200":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_200;
                        break;
                    case "level_up_potion_500":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_500;
                        break;
                    case "level_up_potion_1500":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_1500;
                        break;
                    case "level_up_potion_5000":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5000;
                        break;
                    case "level_up_s_relic":
                        PlusGold= InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_s_relic;
                        break;
                    default:
                        break;
                }

                if (LevelupTotalCurrency + PlusGold > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value)
                    break;

                LevelupTotalCurrency += PlusGold;
                PossibleLvUpCount_10 = i + 1;
            }
            if (LevelupTotalCurrency == 0)
            {
                LevelupTotalCurrency = PlusGold;
                PossibleLvUpCount_10 = 1;
            }
            NeedPotionData_10 = new BigInteger(LevelupTotalCurrency);
            LevelupTotalCurrency = BigInteger.Zero;
            PlusGold = BigInteger.Zero;

            for (int i = 0; i < 100; i++)
            {
                if (myItem.Level + i >= srelicInfo.level_max)
                    continue;
              
                switch (srelicInfo.level_up_potion_type)
                {
                    case "level_up_potion_5":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5;
                        break;
                    case "level_up_potion_100":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_100;
                        break;
                    case "level_up_potion_200":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_200;
                        break;
                    case "level_up_potion_500":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_500;
                        break;
                    case "level_up_potion_1500":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_1500;
                        break;
                    case "level_up_potion_5000":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5000;
                        break;
                    case "level_up_s_relic":
                        PlusGold = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_s_relic;
                        break;
                    default:
                        break;
                }
                if (LevelupTotalCurrency + PlusGold > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value)
                    break;
                LevelupTotalCurrency += PlusGold;
                PossibleLvUpCount_100 = i + 1;
            }

            if (LevelupTotalCurrency == 0)
            {
                LevelupTotalCurrency = PlusGold;
                PossibleLvUpCount_100 = 1;
            }
            NeedPotionData_100 = new BigInteger(LevelupTotalCurrency);

            //특수유물일경우 내 아이템 갯수 필요
            //if(srelicInfo.need_item_type== "level_up_item_1")
            {
                NeedMyItemForLevelup = InGameDataTableManager.RelicList.gain.Find(o => o.level == myItem.Level).level_up_item_1;
            }
        }
    }
}
