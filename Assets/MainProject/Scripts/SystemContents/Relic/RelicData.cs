using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
   
    public class RelicData
    {
        //public GameBuff[] buffs;
        public AbilitiesType abilitytype;
        public ItemType type=ItemType.Relic;

        public RelicInfo relicInfo;
        public RelicNeedCurrency relicNeedPotion;

        //필요 골드
        public LevelUpType LevelupType = LevelUpType.LevelUp_5000;

        public BigInteger NeedPotionData=BigInteger.Zero;
        public BigInteger NeedPotionData_10;
        public BigInteger NeedPotionData_100;

        public int PossibleLvUpCount_1;
        public int PossibleLvUpCount_10;
        public int PossibleLvUpCount_100;

        //능력치 등
        public float BaseRelicvalue;
        public string relicName;
        public string abilName;

        public Relic myItem;
        BigInteger abilityValue;
        public BigInteger Value
        {
            get
            {
                return abilityValue;
            }
        }

        public RelicData(Relic _item)
        {
            NeedPotionData = BigInteger.Zero;
            abilitytype = AbilitiesType.End;
            myItem = _item;
            abilityValue = 0;
            //테이블에서 정보 가져와서 세팅(로컬,어빌리티 베이스 값 등)
            Init();
        }

        void Init()
        {
            relicInfo = null;
            for (int i = 0; i < InGameDataTableManager.RelicList.relic.Count; i++)
            {
                if (InGameDataTableManager.RelicList.relic[i].idx == myItem.idx)
                {
                    relicInfo = InGameDataTableManager.RelicList.relic[i];
                    break;
                }
            }
           
            if (relicInfo == null)
                return;

            int abidx = relicInfo.ability_idx;
            AbilityInfo abInfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == abidx);
            AbilitiesType abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abInfo.abtype);
            BaseRelicvalue = abInfo.level_unit;
            abilitytype = abiltype;
            abilityValue = BaseRelicvalue * myItem.Level;

            LocalValue relicLocalization= InGameDataTableManager.LocalizationList.relic.Find(o => o.id == relicInfo.name);
            LocalValue abilLocalization = InGameDataTableManager.LocalizationList.relic.Find(o => o.id == relicInfo.desc);

            relicName = relicLocalization.kr;
            abilName = abilLocalization.kr;

            //UpdateData();
        }

        public void UpdateData()
        {
            if (abilitytype == AbilitiesType.End)
                return;

            if(myItem.Level>= relicInfo.level_max)
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
                relicNeedPotion = InGameDataTableManager.RelicList.gain.Find(o => o.level == (myItem.Level + 1));
                FindNeedPotion();
            }
            //Message.Send<UI.Event.CharacterInfoUIUpdate>(new UI.Event.CharacterInfoUIUpdate());

            if (myItem.Unlocked==false)
                return;

            abilityValue = BaseRelicvalue * myItem.Level;

            CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.Relic, abilitytype, relicInfo.ability_idx, abilityValue);
        }
        

        void FindNeedPotion()
        {
            BigInteger LevelupTotalCurrency;
            BigInteger PlusCurrency;
            switch (relicInfo.level_max)
            {
                case 5:
                    LevelupType = LevelUpType.LevelUp_5;
                    NeedPotionData = relicNeedPotion.level_up_potion_5;
                    break;
                case 100:
                    LevelupType = LevelUpType.LevelUp_100;
                    NeedPotionData = relicNeedPotion.level_up_potion_100;
                    break;
                case 200:
                    LevelupType = LevelUpType.LevelUp_200;
                    NeedPotionData = relicNeedPotion.level_up_potion_200;
                    break;
                case 500:
                    LevelupType = LevelUpType.LevelUp_500;
                    NeedPotionData = relicNeedPotion.level_up_potion_500;
                    break;
                case 1500:
                    LevelupType = LevelUpType.LevelUp_1500;
                    NeedPotionData = relicNeedPotion.level_up_potion_1500;
                    break;
                case 5000:
                    LevelupType = LevelUpType.LevelUp_5000;
                    NeedPotionData = relicNeedPotion.level_up_potion_5000;
                    break;
                case 10000:
                    LevelupType = LevelUpType.LevelUp_10000;
                    NeedPotionData = relicNeedPotion.level_up_potion_10000;
                    break;
                default:
                    break;
            }

            PossibleLvUpCount_1 = 1;
            LevelupTotalCurrency = BigInteger.Zero;
            PlusCurrency = BigInteger.Zero;
            for (int i = 0; i < 10; i++)
            {
                if (myItem.Level + i >= relicInfo.level_max)
                    break;
                if (relicInfo.level_max==5)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5;
                else if (relicInfo.level_max == 100)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_100;
                else if (relicInfo.level_max == 200)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_200;
                else if (relicInfo.level_max == 500)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_500;
                else if (relicInfo.level_max == 1500)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_1500;
                else if (relicInfo.level_max == 5000)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5000;
                else if (relicInfo.level_max == 10000)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_10000;

                if (LevelupTotalCurrency + PlusCurrency
                    > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value)
                {
                    if (LevelupTotalCurrency == 0)
                    {
                        LevelupTotalCurrency = NeedPotionData;
                        PossibleLvUpCount_10 = 1;
                    }
                    break;
                }
                LevelupTotalCurrency += PlusCurrency;
                PossibleLvUpCount_10 = i + 1;
            }
                
            NeedPotionData_10 = new BigInteger(LevelupTotalCurrency);

            LevelupTotalCurrency = BigInteger.Zero;
            PlusCurrency = BigInteger.Zero;
            for (int i = 0; i < 100; i++)
            {
                if (myItem.Level + i >= relicInfo.level_max)
                    break;
                if (relicInfo.level_max == 5)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5;
                else if (relicInfo.level_max == 100)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_100;
                else if (relicInfo.level_max == 200)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_200;
                else if (relicInfo.level_max == 500)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_500;
                else if (relicInfo.level_max == 1500)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_1500;
                else if (relicInfo.level_max == 5000)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_5000;
                else if (relicInfo.level_max == 10000)
                    PlusCurrency = InGameDataTableManager.RelicList.gain[myItem.Level + i].level_up_potion_10000;

                if (LevelupTotalCurrency + PlusCurrency
                    > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value)
                {
                    if (LevelupTotalCurrency == 0)
                    {
                        LevelupTotalCurrency = NeedPotionData;
                        PossibleLvUpCount_100 = 1;
                    }
                    break;
                }

                LevelupTotalCurrency += PlusCurrency;
                PossibleLvUpCount_100 = i + 1;
            }
                
            NeedPotionData_100 = new BigInteger(LevelupTotalCurrency);
        }
        
    }

}
