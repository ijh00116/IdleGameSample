using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BlackTree
{
    public class CostumData
    {
        public int Max_lv;

        public Costum MyCostum;
        public CostumInformation CostumInfo;

        BigInteger abilityValue;
        AbilitiesType abiltype;
        public AbilityInfo abilinfo;
        public BigInteger Value
        {
            get
            {
                return abilityValue;
            }
        }

        public BigInteger CostPotion_1;
        public BigInteger CostPotion_10;
        public BigInteger CostPotion_100;

        public BigInteger DefaultCostPotion;

        public int PossibleLvUpCount_1;
        public int PossibleLvUpCount_10;
        public int PossibleLvUpCount_100;

        public CostumData(Costum _costum)
        {
            MyCostum = _costum;
            Init();
        }

        void Init()
        {
            CostumInfo = null;
            CostumInfo = InGameDataTableManager.ItemTableList.costum.Find(o => o.idx == MyCostum.idx);

            DefaultCostPotion = new BigInteger(CostumInfo.enchant_potion);
            abilinfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == CostumInfo.ability_idx);

            if (abilinfo != null)
            {
                abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abilinfo.abtype);
                abilityValue = abilinfo.level_unit;
                abilityValue = abilityValue * MyCostum.Level;//데미지
            }
            // UpdateData();
        }
        //addlevel하면서 updatedata이거 호출하니까 data클래스엔 addlevel은 따로 필요하지 않음
        public void UpdateData()
        {
            if (MyCostum.Level > CostumInfo.max_lv)
                MyCostum.Level = CostumInfo.max_lv;
            abilityValue = abilinfo.level_unit * MyCostum.Level;//데미지
            FindNeedCost();

            CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.Costume,
                  abiltype, CostumInfo.ability_idx, abilityValue);
            // Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.SkinName = CostumInfo.sprite;
        }

        public void FindNeedCost()
        {
            BigInteger LevelupTotalCurrency;
            BigInteger PlusCurrency;
            CostPotion_1 = DefaultCostPotion * InGameDataTableManager.ItemTableList.gain[MyCostum.Level-1].cos_potion_gain_rate;
            PossibleLvUpCount_1 = 1;
            LevelupTotalCurrency = BigInteger.Zero;
            PlusCurrency = BigInteger.Zero;
            for (int i = 0; i < 10; i++)
            {
                if (MyCostum.Level + i >= CostumInfo.max_lv)
                {
                    CostPotion_1 = 0;
                    break;
                }
                PlusCurrency =DefaultCostPotion*InGameDataTableManager.ItemTableList.gain[MyCostum.Level - 1 + i].cos_potion_gain_rate;
                if (LevelupTotalCurrency+PlusCurrency > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value)
                {
                    if(LevelupTotalCurrency==0)
                    {
                        LevelupTotalCurrency = CostPotion_1;
                        PossibleLvUpCount_10 = 1;
                    }
                    break;
                }
                LevelupTotalCurrency += PlusCurrency;
                PossibleLvUpCount_10 = i + 1;
            }
            CostPotion_10 = new BigInteger(LevelupTotalCurrency);

            LevelupTotalCurrency = BigInteger.Zero;
            PlusCurrency = BigInteger.Zero;
            CostPotion_1 = DefaultCostPotion * InGameDataTableManager.ItemTableList.gain[MyCostum.Level - 1].cos_potion_gain_rate;
            for (int i = 0; i < 100; i++)
            {
                if (MyCostum.Level + i >= CostumInfo.max_lv)
                {
                    CostPotion_1 = 0;
                    break;
                }
                PlusCurrency = DefaultCostPotion * InGameDataTableManager.ItemTableList.gain[MyCostum.Level - 1 + i].cos_potion_gain_rate;
                if (LevelupTotalCurrency + PlusCurrency > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value)
                {
                    if (LevelupTotalCurrency == 0)
                    {
                        LevelupTotalCurrency = CostPotion_1;
                        PossibleLvUpCount_100 = 1;
                    }
                    break;
                }
              
                LevelupTotalCurrency += PlusCurrency;
                PossibleLvUpCount_100 = i + 1;
            }
            CostPotion_100 = new BigInteger(LevelupTotalCurrency);

        }
        public void UnEquipItem()
        {
            if (MyCostum.Equiped == true)
            {
                MyCostum.Equiped = false;
                UpdateData();
            }
        }

        public void EquipItem()
        {
            if (MyCostum.Equiped == true)
            {
                UpdateData();
                //장착시 증가
                if (MyCostum.Equiped == true)
                {
                    Message.Send<UI.Event.ShapeChange>(new UI.Event.ShapeChange(ItemType.Costum, CostumInfo.SkinName, MyCostum.Equiped)
                    { _character = Common.InGameManager.Instance.mainplayerCharacter, SpineName = CostumInfo.SpineName });

                    Message.Send<UI.Event.ShapeChange>(new UI.Event.ShapeChange(ItemType.Costum, CostumInfo.SkinName, MyCostum.Equiped)
                    { _character = Common.InGameManager.Instance.PetPlayerCharacter, SpineName = CostumInfo.SpineName });

                }
                //BackendManager.Instance.UpdateEquipedCostumList(Common.InGameManager.Instance.CostumInventory);
            }
        }
    
    }

}
