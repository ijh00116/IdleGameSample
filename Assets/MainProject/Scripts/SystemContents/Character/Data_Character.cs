using DLL_Common.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class Data_Character
    {
        public Data_CharacterInfo characterBaseData =new Data_CharacterInfo();

        public CharacterDataAbility ability;

        Dictionary<AbilityValueType, AbilityValueCache> _cachedAbilityValue;

        Dictionary<AbilitiesType, BigInteger> _cachedAbilityNumericValue;

        AbilityValueCache _cachedCostumeAbilityValue;

        AbilityValueCache _cachedEquipWeaponAbilityValue;
        AbilityValueCache _cachedEquipWingAbilityValue;
        AbilityValueCache _cachedHoldWeaponAbilityValue;
        AbilityValueCache _cachedHoldWingAbilityValue;

        AbilityValueCache _cachedEnforceAbilityValue;

        AbilityValueCache _cachedRelicAbilityValue;

        AbilityValueCache _cachedSRelicAbilityValue;

        AbilityValueCache _cachedEquipPetAbilityValue;
        AbilityValueCache _cachedCollectPetAbilityValue;

        //광고버프 어빌리티
        //테이블 나오면 추가 작업해야하나 먼저 세팅함//2021_03_11
        AbilityValueCache _cachedBuffAbilityValue;
        //뉴비패키지 어빌리티
        AbilityValueCache _cachedNewbieAbilityValue;
        public Data_Character()
        {
            ability = new CharacterDataAbility(this);
            _cachedAbilityValue = new Dictionary<AbilityValueType, AbilityValueCache>();
            _cachedAbilityNumericValue = new Dictionary<AbilitiesType, BigInteger>();

            _cachedCostumeAbilityValue = new AbilityValueCache();
            _cachedHoldWeaponAbilityValue = new AbilityValueCache();
            _cachedEquipWeaponAbilityValue = new AbilityValueCache();
            _cachedHoldWingAbilityValue = new AbilityValueCache();
            _cachedEquipWingAbilityValue = new AbilityValueCache();
            _cachedEnforceAbilityValue = new AbilityValueCache();
            _cachedRelicAbilityValue = new AbilityValueCache();
            _cachedSRelicAbilityValue = new AbilityValueCache();
            _cachedBuffAbilityValue = new AbilityValueCache();

            _cachedEquipPetAbilityValue = new AbilityValueCache();
            _cachedCollectPetAbilityValue = new AbilityValueCache();
            _cachedNewbieAbilityValue = new AbilityValueCache();

            //강화 세팅(능력치 아이디값 없어서 임시 세팅
            _cachedEnforceAbilityValue.Add(8300001, AbilitiesType.ENFORCE_Gain_Rate);

            //유물 어빌리티 세팅
            for (int i=0; i< InGameDataTableManager.RelicList.relic.Count; i++)
            {
                int abidx = InGameDataTableManager.RelicList.relic[i].ability_idx;
                AbilityInfo abInfo= InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == abidx);
                if(abInfo==null)
                {
#if UNITY_EDITOR
                    Debug.Log(abInfo.abtype);
#endif
                }
                AbilitiesType abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abInfo.abtype);
                _cachedRelicAbilityValue.Add(abidx,abiltype);
            }

            //특수 어빌리티 세팅
            for(int i=0; i<InGameDataTableManager.RelicList.s_relic.Count; i++)
            {
                int abidx = InGameDataTableManager.RelicList.s_relic[i].ability_idx;
                AbilityInfo abInfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == abidx);
                if (abInfo == null)
                {
#if UNITY_EDITOR
                    Debug.Log(abInfo.abtype);
#endif
                }
                AbilitiesType abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abInfo.abtype);
                _cachedSRelicAbilityValue.Add(abidx, abiltype);
            }

            //스킬 어빌리티 세팅
            //for (int i=0; i<InGameDataTableManager.PassiveSkillList.passive.Count; i++)
            //{
            //    int abidx = InGameDataTableManager.PassiveSkillList.passive[i].ability_idx;
            //    AbilityInfo abInfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == abidx);
            //    AbilitiesType abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abInfo.abtype);
            //    _cachedSkillAbilityValue.Add(abidx,abiltype);
            //}

            //아템 어빌 세팅
            for (int i = 0; i < InGameDataTableManager.ItemTableList.weapon.Count; i++)
            {
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].a_aidx_1, _cachedEquipWeaponAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].a_aidx_2, _cachedEquipWeaponAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].a_aidx_3, _cachedEquipWeaponAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].a_aidx_4, _cachedEquipWeaponAbilityValue);

                AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].b_aidx_1, _cachedHoldWeaponAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].b_aidx_2, _cachedHoldWeaponAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].b_aidx_3, _cachedHoldWeaponAbilityValue);
                //AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].b_aidx_4, _cachedHoldWeaponAbilityValue);
            }

            for (int i = 0; i < InGameDataTableManager.ItemTableList.wing.Count; i++)
            {
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.wing[i].a_aidx_1, _cachedEquipWingAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.wing[i].a_aidx_2, _cachedEquipWingAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.wing[i].a_aidx_3, _cachedEquipWingAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.wing[i].a_aidx_4, _cachedEquipWingAbilityValue);

                AbilityTypeSetting(InGameDataTableManager.ItemTableList.wing[i].b_aidx_1, _cachedHoldWingAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.wing[i].b_aidx_2, _cachedHoldWingAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.wing[i].b_aidx_3, _cachedHoldWingAbilityValue);
                //AbilityTypeSetting(InGameDataTableManager.ItemTableList.weapon[i].b_aidx_4, _cachedHoldWeaponAbilityValue);
            }
            //코스튬 어빌 세팅
            for(int i = 0; i < InGameDataTableManager.ItemTableList.costum.Count; i++)
            {
                AbilityTypeSetting(InGameDataTableManager.ItemTableList.costum[i].ability_idx.ToString(), _cachedCostumeAbilityValue);
            }

            //펫 어빌
            for(int i=0; i<InGameDataTableManager.PetTableList.pet.Count; i++)
            {
                AbilityTypeSetting(InGameDataTableManager.PetTableList.pet[i].use_aidx.ToString(), _cachedEquipPetAbilityValue);
            }
            for (int i = 0; i < InGameDataTableManager.PetTableList.pet.Count; i++)
            {
                AbilityTypeSetting(InGameDataTableManager.PetTableList.pet[i].collect_aidx.ToString(), _cachedCollectPetAbilityValue);
            }

            //버프 어빌리티 세팅
            _cachedBuffAbilityValue.Add(100031, AbilitiesType.CHA_AD_ATTACK_BUFF);
            _cachedBuffAbilityValue.Add(100235, AbilitiesType.CHA_AD_ATTACKSPEED_BUFF);
            _cachedBuffAbilityValue.Add(100613, AbilitiesType.CHA_AD_REWARDGOLD_BUFF);
            _cachedBuffAbilityValue.Add(101715, AbilitiesType.CHA_AD_MOVESPEED_BUFF);
            _cachedBuffAbilityValue.Add(101716, AbilitiesType.CHA_AD_REWARDPOTION_BUFF);

            for (int i=0; i<InGameDataTableManager.NewbiePackage.newbie.Count;i++)
            {
                AbilityTypeSetting(InGameDataTableManager.NewbiePackage.newbie[i].a_aidx_1.ToString(), _cachedNewbieAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.NewbiePackage.newbie[i].a_aidx_2.ToString(), _cachedNewbieAbilityValue);
                AbilityTypeSetting(InGameDataTableManager.NewbiePackage.newbie[i].a_aidx_3.ToString(), _cachedNewbieAbilityValue);
            }



            _cachedAbilityValue.Add(AbilityValueType.Enforce, _cachedEnforceAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.Relic, _cachedRelicAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.SRelic, _cachedSRelicAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.HoldWeapon, _cachedHoldWeaponAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.EquipWeapon, _cachedEquipWeaponAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.HoldWing, _cachedHoldWingAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.EquipWing, _cachedEquipWingAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.Costume, _cachedCostumeAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.EquipPet, _cachedEquipPetAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.HoldPet, _cachedCollectPetAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.AdBuff, _cachedBuffAbilityValue);
            _cachedAbilityValue.Add(AbilityValueType.Newbie, _cachedNewbieAbilityValue);

            for (int i=0; i<(int)AbilitiesType.End; i++)
            {
                AbilitiesType _type = (AbilitiesType)i;
                if (_cachedAbilityNumericValue.ContainsKey(_type))
                {
                    _cachedAbilityNumericValue[_type] = ReGainAbilityValue(_type);
                }
                else
                {
                    BigInteger data = ReGainAbilityValue(_type);
                    _cachedAbilityNumericValue.Add(_type, data);
                }
            }
           
        }

        void AbilityTypeSetting(string abilityidx, AbilityValueCache abilCache)
        {
            if (abilityidx != "none")
            {
                int abidx = int.Parse(abilityidx);
                AbilityInfo abInfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == abidx);
                AbilitiesType abiltype = EnumExtention.ParseToEnum<AbilitiesType>(abInfo.abtype);
                abilCache.Add(abidx,abiltype);
            }
        }

        public void InitBaseData()
        {
            //기본정보 세팅
            //기본정보 로드하여 불러오기
            characterBaseData.Init();
            //기본정보 세팅
        }

        public void ReleaseBaseData()
        {
            characterBaseData.Release();
        }

        InGame.Event.CharacterAbilityUpdate Abilitymsg = new InGame.Event.CharacterAbilityUpdate(AbilitiesType.End);
        public void SetAbilityValue(AbilityValueType valueType ,AbilitiesType _type,int abilIndex,BigInteger value)
        {
            if(_cachedAbilityValue.ContainsKey(valueType))
            {
                if(_cachedAbilityValue[valueType].HasValue(_type))
                {
                    _cachedAbilityValue[valueType].SetValue(_type, abilIndex, value);
                    Abilitymsg.abilType = _type;
                }
            }
            if(_cachedAbilityNumericValue.ContainsKey(_type))
            {
                _cachedAbilityNumericValue[_type] = ReGainAbilityValue(_type);
            }

            //타입에 따라 메세지 보낼것!
            if (Abilitymsg.abilType!=AbilitiesType.End)
            {
                Message.Send<InGame.Event.CharacterAbilityUpdate>(Abilitymsg);
            }

            Message.Send<UI.Event.CharacterInfoUIUpdate>(new UI.Event.CharacterInfoUIUpdate());
        }

        public BigInteger GetAbilityValue(AbilitiesType _type)
        {
            if (_cachedAbilityNumericValue.ContainsKey(_type))
                return _cachedAbilityNumericValue[_type];
            else
                return BigInteger.Zero;
        }

        public BigInteger ReGainAbilityValue(AbilitiesType _type)
        {
            BigInteger totalValue = BigInteger.Zero;

            foreach (KeyValuePair<AbilityValueType, AbilityValueCache> data in _cachedAbilityValue)
            {
                if (data.Value.HasValue(_type))
                {
                    totalValue += data.Value.GetValue(_type);
                }
            }
            if (totalValue == 0)
            {
                switch (_type)
                {
                    case AbilitiesType.ENFORCE_Gain_Rate:
                        totalValue = 1;
                        break;
                    case AbilitiesType.CHA_ATTACK_UP:
                        totalValue = 1;
                        break;
                    case AbilitiesType.CHA_BAGIC_ATTACK_UP:
                        totalValue = 1;
                        break;
                    case AbilitiesType.QUEST_REWARD_UP:
                        break;
                    case AbilitiesType.QUEST_LEVEL_UP_COST_DOWN:
                        break;
                    case AbilitiesType.QUEST_REWARD_TIME_DOWN:
                        break;
                    case AbilitiesType.CHA_ATTACK_SPEED_UP:
                        break;
                    case AbilitiesType.CHA_CRITICAL_PER:
                        break;
                    case AbilitiesType.CHA_CRITICAL_DAMAGE_UP:
                        break;
                    case AbilitiesType.CHA_CRITICAL_DAMAGE_DOWN:
                        break;
                    case AbilitiesType.CHA_MOVE_SPEED_UP:
                        break;
                    case AbilitiesType.CHA_LV_UP:
                        break;
                    case AbilitiesType.MON_HP_DOWN:
                        break;
                    case AbilitiesType.BOSS_HP_DOWN:
                        break;
                    case AbilitiesType.SKILL_ONE_COOLTIME_DOWN:
                        break;
                    case AbilitiesType.SKILL_TWO_COOLTIME_DOWN:
                        break;
                    case AbilitiesType.STAGE_WAVE_MON_COUNT_DOWN:
                        break;
                    case AbilitiesType.CHA_LEVEL_UP_COST_DOWN:
                        break;
                    case AbilitiesType.MONS_KILL_REWARD_GOLD_UP:
                        break;
                    case AbilitiesType.MONS_POTION_REWARD_UP:
                        break;
                    case AbilitiesType.BOSS_KILL_GOLD_UP:
                        break;
                    case AbilitiesType.BOSS_POTION_REWARD_UP:
                        break;
                    case AbilitiesType.MIMIC_GEN_RATE_UP:
                        break;
                    case AbilitiesType.MIMIC_KILL_REWARD_GOLD_UP:
                        break;
                    case AbilitiesType.MIMIC_POTION_REWARD_UP:
                        break;
                    case AbilitiesType.DUNGEON_TIME_LIMIT_UP:
                        break;
                    case AbilitiesType.DUNGEON_BOSS_GEN_RATE_UP:
                        break;
                    case AbilitiesType.DUNGEON_REWARD_POTION_UP:
                        break;
                    case AbilitiesType.DUNGEON_REWARD_MAGIC_STONE_UP:
                        break;
                    case AbilitiesType.KILL_REWARD_GOLD_10X_RATE:
                        break;
                    case AbilitiesType.KILL_REWARD_POTION_10X_RATE:
                        break;
                    case AbilitiesType.CHA_HP_UP:
                        break;
                    case AbilitiesType.CHA_AD_ATTACK_BUFF:
                        totalValue = 1;
                        break;
                    case AbilitiesType.CHA_AD_ATTACKSPEED_BUFF:
                        totalValue = 1;
                        break;
                    case AbilitiesType.CHA_AD_FEVERTIME_BUFF:
                        break;
                    case AbilitiesType.CHA_AD_REWARDGOLD_BUFF:
                        totalValue = 1;
                        break;
                    case AbilitiesType.CHA_AD_MOVESPEED_BUFF:
                        totalValue = 1;
                        break;
                    case AbilitiesType.CHA_AD_REWARDPOTION_BUFF:
                        totalValue = 1;
                        break;
                    case AbilitiesType.End:
                        break;
                    default:
                        break;
                }
            }

            if (totalValue <= 1)
            {

                switch (_type)
                {
                    case AbilitiesType.BOSS_POTION_REWARD_UP:
                        totalValue = 1;
                        break;
                    case AbilitiesType.BOSS_KILL_GOLD_UP:
                        totalValue = 1;
                        break;

                }
            }
            return totalValue;
        }

    }

}
