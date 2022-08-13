using BlackTree.InGame;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlackTree
{
    //최종 데미지는 이것이므로  여기의 데이터를 써서 게임 세팅 할것
    public class CharacterDataAbility
    {
        public Data_Character Characterdata;
        public CharacterDataAbility(Data_Character _character)
        {
            Characterdata = _character;
        }
        public BigInteger GetBaseAttackPower()
        {
            return Characterdata.characterBaseData.attack;
        }

        
        public BigInteger GetAtkDamage()
        {
            BigInteger damage=BigInteger.Zero;
            damage = (Characterdata.characterBaseData.attack * Characterdata.GetAbilityValue(AbilitiesType.ENFORCE_Gain_Rate));
            damage= damage+ damage*(Characterdata.GetAbilityValue(AbilitiesType.CHA_BAGIC_ATTACK_UP).ToFloat() / 100.0f);
            damage = damage + damage * ((Characterdata.GetAbilityValue(AbilitiesType.CHA_ATTACK_UP).ToFloat() / 100.0f));
            damage = damage * Characterdata.GetAbilityValue(AbilitiesType.CHA_AD_ATTACK_BUFF).ToFloat();

            return damage;
        }

        public BigInteger GetNormalAtkDamage()
        {
            BigInteger damage = BigInteger.Zero;
            damage = Characterdata.characterBaseData.attack;
            damage = damage + damage * (Characterdata.GetAbilityValue(AbilitiesType.CHA_BAGIC_ATTACK_UP).ToFloat() / 100.0f);
            damage = damage + damage * ((Characterdata.GetAbilityValue(AbilitiesType.CHA_ATTACK_UP).ToFloat() / 100.0f));
            damage = damage * Characterdata.GetAbilityValue(AbilitiesType.CHA_AD_ATTACK_BUFF).ToFloat();

            return damage;
        }

        public float GetAttackSpeed()
        {
            float value = 0;
            value = ((Characterdata.characterBaseData.attack_speed) + (int)Characterdata.GetAbilityValue(AbilitiesType.CHA_ATTACK_SPEED_UP).ToFloat());
            value = value * Characterdata.GetAbilityValue(AbilitiesType.CHA_AD_ATTACKSPEED_BUFF).ToFloat();
            value = value + DTConstraintsData.ActiveSkillData_forAtkSpeed;
            if (value>=DTConstraintsData.ABILITY_ATK_SPEED_MAX)
            {
                value = DTConstraintsData.ABILITY_ATK_SPEED_MAX;
            }
            value = value / (float)DTConstraintsData.SPEED_REVISION_VALUE;
            return value;
        }


        public float GetCriticalRate()
        {
            float value = 0;
            value = Characterdata.characterBaseData.critical + (Characterdata.GetAbilityValue(AbilitiesType.CHA_CRITICAL_PER).ToFloat());
            return value;
        }

        public float GetCriticalDamageRate()
        {
            float value = 0;
            value = Characterdata.characterBaseData.critical_damage+ (Characterdata.GetAbilityValue(AbilitiesType.CHA_CRITICAL_DAMAGE_UP).ToFloat())/100.0f;
            return value;
        }

        //크리 뎀지 다운은 나중에 서버로 적캐릭터 정보 받아서 GetCriticalDamageRate에서 상대의 GetCriticalDamageArmor값을 뺀다음에 크리티컬 데미지 적용
        public float GetCriticalDamageArmor()
        {
            float value = 0;
            value = Characterdata.characterBaseData.critical_damage + Characterdata.GetAbilityValue(AbilitiesType.CHA_CRITICAL_DAMAGE_DOWN).ToFloat();
            return value;
        }

        public float GetMoveSpeed()
        {
            float value = 0;
           
            value = Characterdata.characterBaseData.move_speed + (int)Characterdata.GetAbilityValue(AbilitiesType.CHA_MOVE_SPEED_UP).ToFloat();
            value =value* Characterdata.GetAbilityValue(AbilitiesType.CHA_AD_MOVESPEED_BUFF).ToFloat();
            value = value + DTConstraintsData.ActiveSkillData_forMoveSpeed;
            if (Characterdata.GetAbilityValue(AbilitiesType.CHA_MOVE_SPEED_UP).ToFloat() > 2000)
            {
                value = 3000;
            }
            value = value / (float)DTConstraintsData.SPEED_REVISION_VALUE;
            return value;
        }

        public int GetCharacterLevel()
        {
            int value = 0;
            value = Characterdata.characterBaseData.Level + int.Parse(Characterdata.GetAbilityValue(AbilitiesType.CHA_LV_UP).ToString());
            return value;
        }

        public void GetMonsterHp(ref BigInteger baseMonsterHp)
        {
            BigInteger rate = Characterdata.GetAbilityValue(AbilitiesType.MON_HP_DOWN);
            if(rate>=99)
            {
                rate = 99;
            }
            float decreaseRate = (1.0f - (rate.ToFloat() / 100.0f));
            BigInteger hp= baseMonsterHp* decreaseRate;
            baseMonsterHp = hp;
        }

        public void GetBossMonsterHp(ref BigInteger basebossMonsterHp)
        {
            BigInteger rate = Characterdata.GetAbilityValue(AbilitiesType.BOSS_HP_DOWN);
            if (rate >= 99)
            {
                rate = 99;
            }
            float decreaseRate = (1.0f - (rate.ToFloat() / 100.0f));
            BigInteger hp = basebossMonsterHp * decreaseRate;
            basebossMonsterHp = hp;
        }

        public int GetWaveMonsterCount()
        {
            //기본 웨이브  수 일단 12로 해놓고 나중에 constraints로 변경
            int value = 0;
            value = DTConstraintsData.BATTLE_STAGE_WAVE - (int)Characterdata.GetAbilityValue(AbilitiesType.STAGE_WAVE_MON_COUNT_DOWN).ToFloat();
            return value;
        }

        public BigInteger GetLevelUpCost(BigInteger cost)
        {
            BigInteger value = BigInteger.Zero;
            BigInteger rate = Characterdata.GetAbilityValue(AbilitiesType.CHA_LEVEL_UP_COST_DOWN);
            
            value = (cost*100.0f) / (100 + rate);
            return value;
        }

        public BigInteger GetQuestReward(BigInteger EarnGold)
        {
            BigInteger value = BigInteger.Zero;
            value = EarnGold + (EarnGold * (Characterdata.GetAbilityValue(AbilitiesType.QUEST_REWARD_UP).ToFloat()/100.0f));
            return value;
        }

        public BigInteger GetQuestLevelUpCost(BigInteger cost)
        {
            BigInteger value = BigInteger.Zero;
            float rate = Characterdata.GetAbilityValue(AbilitiesType.QUEST_LEVEL_UP_COST_DOWN).ToFloat();
            if(rate>=90)
            {
                rate = 90;
            }
            value = cost - (cost * (rate/100.0f));
            return value;
        }

        public int GetQuestTime(int basequestTime)
        {
            int value = 0;
            value = basequestTime - (int)Characterdata.GetAbilityValue(AbilitiesType.QUEST_REWARD_TIME_DOWN).ToFloat();
            if(value<=1)
            {
                value = 1;
            }
            return value;
        }

        public BigInteger GetMonsterKillGoldReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward * (Characterdata.GetAbilityValue(AbilitiesType.MONS_KILL_REWARD_GOLD_UP).ToFloat()/100.0f));
            value = value * Characterdata.GetAbilityValue(AbilitiesType.CHA_AD_REWARDGOLD_BUFF).ToFloat();
            return value;
        }

        public BigInteger GetMonsterKillPotionReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward * (Characterdata.GetAbilityValue(AbilitiesType.MONS_POTION_REWARD_UP).ToFloat()/100.0f));
            value = value * Characterdata.GetAbilityValue(AbilitiesType.CHA_AD_REWARDPOTION_BUFF).ToFloat();
            return value;
        }

        public BigInteger GetDungeonMonsterKillSoulReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward * (Characterdata.GetAbilityValue(AbilitiesType.DUNGEON_REWARD_POTION_UP).ToFloat() / 100.0f));
            return value;
        }
        public BigInteger GetDungeonMonsterKillEnforceStoneReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward * (Characterdata.GetAbilityValue(AbilitiesType.DUNGEON_REWARD_MAGIC_STONE_UP).ToFloat() / 100.0f));
            return value;
        }

        public BigInteger GetBossKillGoldReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward *
                (( Characterdata.GetAbilityValue(AbilitiesType.MONS_KILL_REWARD_GOLD_UP).ToFloat()+ Characterdata.GetAbilityValue(AbilitiesType.BOSS_KILL_GOLD_UP).ToFloat())/100.0f) );
            return value;
        }

        public BigInteger GetBossKillPotionReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward *
        ((Characterdata.GetAbilityValue(AbilitiesType.MONS_POTION_REWARD_UP).ToFloat() + Characterdata.GetAbilityValue(AbilitiesType.BOSS_POTION_REWARD_UP).ToFloat())/100.0f));
            return value;
        }

        public double GetMimicAppearRate()
        {
            double value;
            value = (DTConstraintsData.BATTLE_STAGE_MIMIC_GEN_RATE + (double)Characterdata.GetAbilityValue(AbilitiesType.MIMIC_GEN_RATE_UP).ToFloat())/100.0f;
            return value;
        }

        public BigInteger GetMimicKillGoldReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward *
                ((Characterdata.GetAbilityValue(AbilitiesType.MONS_KILL_REWARD_GOLD_UP).ToFloat() + Characterdata.GetAbilityValue(AbilitiesType.MIMIC_KILL_REWARD_GOLD_UP).ToFloat())/100.0f));
            return value;
        }

        public BigInteger GetMimicKillPotionReward(BigInteger reward)
        {
            BigInteger value = BigInteger.Zero;
            value = reward + (reward *
        ((Characterdata.GetAbilityValue(AbilitiesType.MONS_POTION_REWARD_UP).ToFloat() + Characterdata.GetAbilityValue(AbilitiesType.MIMIC_POTION_REWARD_UP).ToFloat())/100.0f));
            return value;
        }

        public int GetCowDungeonTime()
        {
            int value;
            value = (DTConstraintsData.DG_BATTLE_TIME_SEC + (int)Characterdata.GetAbilityValue(AbilitiesType.DUNGEON_TIME_LIMIT_UP).ToFloat());
            return value;
        }

        public float GetCowKingRate()
        {
            float value;
            value = ((float)DTConstraintsData.DG_BOSS_GEN_RATE + Characterdata.GetAbilityValue(AbilitiesType.DUNGEON_BOSS_GEN_RATE_UP).ToFloat());
            return value;
        }

        public float GetTenTimesGoldRate()
        {
            float value;
            value = ((float)DTConstraintsData.BATTLE_KILL_GOLD_10X_RATE + Characterdata.GetAbilityValue(AbilitiesType.KILL_REWARD_GOLD_10X_RATE).ToFloat()) / 100.0f;
            return value;
        }

        public float GetTenTimesPotionRate()
        {
            float value;
            value = ((float)DTConstraintsData.BATTLE_KILL_POTION_10X_RATE + Characterdata.GetAbilityValue(AbilitiesType.KILL_REWARD_POTION_10X_RATE).ToFloat()) / 100.0f;
            return value;
        }

        public BigInteger GetCharacterHp()
        {
            BigInteger hpValue;
            hpValue = (Characterdata.characterBaseData.hp + (Characterdata.characterBaseData.hp * Characterdata.GetAbilityValue(AbilitiesType.CHA_HP_UP) / 100.0f));
            return hpValue;
        }


    }

}
