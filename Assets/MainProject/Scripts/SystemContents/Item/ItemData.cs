using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class ItemData 
    {
        public ItemType itemtype = ItemType.Relic;

        //강화석관련
        public BigInteger enchantStone_Value;
        public int MaxLevel;
        //분해
        public int Recycle_StoneCount;

        //능력치
        public Dictionary<AbilitiesType,Dictionary<int, BigInteger>> B_abilityList = new Dictionary<AbilitiesType, Dictionary<int, BigInteger>>();
        public Dictionary<AbilitiesType, Dictionary<int, BigInteger>> A_abilityList = new Dictionary<AbilitiesType, Dictionary<int, BigInteger>>();
        public Dictionary<string, string> A_AbilityGainTypelist = new Dictionary<string, string>();
        public Dictionary<string, string> B_AbilityGainTypelist = new Dictionary<string, string>();
        //아이템테이블 데이터
        public ItemInformation itemInfo;
        public Item myItem;

        
        public Sprite mySprite_R;
        public Sprite mySprite_L;

        public Sprite MyUIIcon;
        public ItemData(ItemType _type,Item _item)
        {
            itemtype = _type;
            myItem = _item;
            //테이블에서 정보 가져와서 세팅(로컬,어빌리티 베이스 값 등)
            Init();
        }

        void Init()
        {
            itemInfo=null;
            if (itemtype == ItemType.weapon)
                itemInfo = InGameDataTableManager.ItemTableList.weapon.Find(o => o.idx == myItem.idx);
            else if (itemtype == ItemType.wing)
                itemInfo = InGameDataTableManager.ItemTableList.wing.Find(o => o.idx == myItem.idx);

            //스프라이트
            string path = null;
            if (itemtype == ItemType.weapon)
                path = string.Format("Images/Texture/InGameWeapon");
            else if (itemtype == ItemType.wing)
                path = string.Format("Images/Texture/InGameWing");

            string itemname = itemInfo.sprite;
            string fullPath = null;
            if (itemtype == ItemType.weapon)
                fullPath = string.Format("{0}/{1}", path, itemname);
            else if (itemtype == ItemType.wing)
                fullPath = string.Format("{0}/{1}", path, itemname+"_R");

            mySprite_R = Resources.Load<Sprite>(fullPath);

            fullPath = null;
            if (itemtype == ItemType.weapon)
                fullPath = string.Format("{0}/{1}", path, itemname);
            else if (itemtype == ItemType.wing)
                fullPath = string.Format("{0}/{1}", path, itemname + "_L");

            mySprite_L = Resources.Load<Sprite>(fullPath);

            if (itemtype == ItemType.weapon)
                path = string.Format("Images/InGameWeapon");
            else if (itemtype == ItemType.wing)
                path = string.Format("Images/InGameWing");

            itemname = itemInfo.icon;
            fullPath = string.Format("{0}/{1}", path, itemname);

            MyUIIcon = Resources.Load<Sprite>(fullPath);
        }
        //addlevel하면서 updatedata이거 호출하니까 data클래스엔 addlevel은 따로 필요하지 않음
        public void UpdateData()
        {
            //강화석 관련
            BigInteger costStone = new BigInteger(itemInfo.enchant_stone);
            costStone = costStone * InGameDataTableManager.ItemTableList.gain[myItem.Level-1].ent_stone_gain_rate;
            enchantStone_Value = costStone;

            //능력치
            AbilitySetting(itemInfo.a_aidx_1, itemInfo.a_aidx_1_gain);
            AbilitySetting(itemInfo.a_aidx_2, itemInfo.a_aidx_2_gain);
            AbilitySetting(itemInfo.a_aidx_3, itemInfo.a_aidx_3_gain);
            AbilitySetting(itemInfo.a_aidx_4, itemInfo.a_aidx_4_gain);
            AbilitySetting(itemInfo.b_aidx_1, itemInfo.b_aidx_1_gain, true);
            AbilitySetting(itemInfo.b_aidx_2, itemInfo.b_aidx_2_gain, true);
            AbilitySetting(itemInfo.b_aidx_3, itemInfo.b_aidx_3_gain, true);

            if (myItem.Unlocked == false)
                return;

            foreach (KeyValuePair<AbilitiesType, Dictionary<int, BigInteger>> data in B_abilityList)
            {
                foreach(KeyValuePair<int, BigInteger> _data in data.Value)
                {
                    CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(
                  (itemtype == ItemType.weapon) ? AbilityValueType.HoldWeapon : AbilityValueType.HoldWing,
                  data.Key,_data.Key, _data.Value);
                }
            }

           // Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.weaponName = itemInfo.sprite;
   
        }

        public void UnEquipItem()
        {
            if (myItem.Equiped == true)
            {
                myItem.Equiped = false;
                UpdateData();
                foreach (KeyValuePair<AbilitiesType, Dictionary<int, BigInteger>> data in A_abilityList)
                {
                    foreach (KeyValuePair<int, BigInteger> _data in data.Value)
                    {
                        CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(
                      (itemtype == ItemType.weapon) ? AbilityValueType.EquipWeapon : AbilityValueType.EquipWing,
                      data.Key, _data.Key, 0);
                    }
                }
            }
        }

        public void EquipItem()
        {
            if(myItem.Equiped==true)
            {
                UpdateData();
                foreach (KeyValuePair<AbilitiesType, Dictionary<int, BigInteger>> data in A_abilityList)
                {
                    foreach (KeyValuePair<int, BigInteger> _data in data.Value)
                    {
                        CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(
                      (itemtype == ItemType.weapon) ? AbilityValueType.EquipWeapon : AbilityValueType.EquipWing,
                      data.Key, _data.Key, _data.Value);
                    }
                }

                Message.Send<UI.Event.ShapeChange>(new UI.Event.ShapeChange(itemtype, itemInfo.sprite, myItem.Equiped)
                { _character = Common.InGameManager.Instance.mainplayerCharacter });
                Message.Send<UI.Event.ShapeChange>(new UI.Event.ShapeChange(itemtype, itemInfo.sprite, myItem.Equiped)
                { _character = Common.InGameManager.Instance.PetPlayerCharacter });
            }
        }

        void AbilitySetting(string abilIdx,string abilGainvalue,bool b_Abil=false)
        {
            if(abilIdx=="none")
            {
                return;
            }
            AbilitiesType _abiltype;
            BigInteger value;
            AbilityInfo abilinfo;
            int _abilIdx = int.Parse(abilIdx);
            abilinfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == _abilIdx);
            if (abilinfo != null)
            {
                _abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abilinfo.abtype);
                value = GetAbilityvalue(abilinfo.level_unit, abilGainvalue);
                if(b_Abil==false)
                {
                    if (A_abilityList.ContainsKey(_abiltype))
                    {
                        Dictionary<int, BigInteger> abilValue = A_abilityList[_abiltype];
                        if (abilValue.ContainsKey(_abilIdx))
                        {
                            abilValue[_abilIdx] = value;
                        }
                        else
                        {
                            abilValue.Add(_abilIdx, value);
                            if (A_AbilityGainTypelist.ContainsKey(_abilIdx.ToString()) == false)
                                A_AbilityGainTypelist.Add(_abilIdx.ToString(), abilGainvalue);
                        }
                    }
                    else
                    {
                        Dictionary<int, BigInteger> abilValue = new Dictionary<int, BigInteger>();
                        abilValue.Add(_abilIdx, value);
                        A_abilityList.Add(_abiltype, abilValue);
                        if(A_AbilityGainTypelist.ContainsKey(_abilIdx.ToString())==false)
                            A_AbilityGainTypelist.Add(_abilIdx.ToString(), abilGainvalue);
                    }
                }
                else
                {
                    if (B_abilityList.ContainsKey(_abiltype))
                    {
                        Dictionary<int, BigInteger> abilValue = B_abilityList[_abiltype];
                        if (abilValue.ContainsKey(_abilIdx))
                        {
                            abilValue[_abilIdx] = value;
                        }
                        else
                        {
                            abilValue.Add(_abilIdx, value);
                            if (B_AbilityGainTypelist.ContainsKey(_abilIdx.ToString()) == false)
                                B_AbilityGainTypelist.Add(_abilIdx.ToString(), abilGainvalue);
                        }
                    }
                    else
                    {
                        Dictionary<int, BigInteger> abilValue = new Dictionary<int, BigInteger>();
                        abilValue.Add(_abilIdx, value);
                        B_abilityList.Add(_abiltype, abilValue);
                        if (B_AbilityGainTypelist.ContainsKey(_abilIdx.ToString()) == false)
                            B_AbilityGainTypelist.Add(_abilIdx.ToString(), abilGainvalue);
                    }
                }
                    
            }
        }

        BigInteger GetAbilityvalue(float basevalue,string gaintype)
        {
            BigInteger big = new BigInteger(basevalue);
            switch (gaintype)
            {
                case "weapon_gain_1":
                        big = big * InGameDataTableManager.ItemTableList.gain[myItem.Level-1].weapon_gain_1;
                    break;
                case "weapon_gain_2":
                        big = big * InGameDataTableManager.ItemTableList.gain[myItem.Level - 1].weapon_gain_2;
                    break;
                case "weapon_gain_3":
                        big = big * InGameDataTableManager.ItemTableList.gain[myItem.Level - 1].weapon_gain_3;
                    break;
                case "weapon_gain_4":
                        big = big * InGameDataTableManager.ItemTableList.gain[myItem.Level - 1].weapon_gain_4;
                    break;
                case "weapon_gain_5":
                        big = big * InGameDataTableManager.ItemTableList.gain[myItem.Level - 1].weapon_gain_5;
                    break;
                default:
                    break;
            }
            return big;
        }
    }

}
