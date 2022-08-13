using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DLL_Common.Common;

namespace BlackTree
{
    #region 스킬
    public class PassiveSkillPrime
    {
        public int idx;
        public string name;
        public string desc;
        public string icon;
        public string level_up_soul_type;
        public int level_max;
        public int unlock_enchant;
        public int ability_idx;
    }

    public class PassiveSkillLevelupNeedSoul
    {
        public int level;
        public string level_up_soul_1000;
        public string level_up_soul_500;
        public string level_up_soul_100;
    }
    #endregion

    #region DT_KR_Quest

    public class QuestInfo
    {
        public int idx;
        public string name;
        public string icon;
        public string base_gold;
        public string level_up_gold;
        public int cool_time;
        public int level_max;
    }

    public class QuestGainInfo
    {
        public int level;
        public float gold_rate;
        public float level_up_gold_rate;
    }

    #endregion

    #region DT_KR_Dungeon

    [System.Serializable]
    public class DungeonPrime
    {
        public int dg_stage;
        public string dg_stage_name;
        public string open_stage_script_id;
        public int open_scenario;
        public int open_chapter;
        public int open_stage;
        public int reward_soul;
        public int reward_enchant_stone;
        public int reward_magic_stone;
        public string monster_hp;
        public string boss_hp;
    }
    [System.Serializable]
    public class PetDungeonPrime
    {
        public int pet_chapter;
        public int pet_stage;
        public string pet_reward_icon;
        public int reward_pet_group;
        public int reward_item_rate;
        public float reward_item_rate_stage;
        public int even_stage_reward_item_group;
        public int odd_stage_reward_item_group;
        public string monster_hp;
        public string stage_hp_gain;
    }

    public class PetReward
    {
        public int idx;
        public string reward_type;
        public int pet_idx;
        public int rarity;
    }

    public class PetDungeonItemReward
    {
        public int idx;
        public string item_reward_icon;
        public string reward_type;
        public int reward_idx;
        public int reward_count;
        public int rarity;
    }

    public class PetDungeonGainInfo
    {
        public int stage;
        public float pet_stage_hp_gain_rate;
    }
    #endregion

    #region DT_KR_Characters

    public class CharacterEnchant
    {
        public string awake;
        public int enchant;
        public int enchant_ui;
        public float enchant_gain_rate;
        public string need_soul;
        public int need_;
    }

    public class CharacterBasicStat
    {
        public int level;
        public string attack;
        public string hp;
        public float critical;
        public float critical_damage;
        public int attack_speed;
        public int move_speed;
    }

    public class LevelUpInfo
    {
        public int level;
        public string need_gold;
        public string need_gold_10;
        public string need_gold_100;
    }

    public class ETCUserInfo
    {
        public int level;
        public int exp;
        public string gold;
    }

    #endregion

    #region DT_KR_Stage
    [System.Serializable]
    public class StageMonster
    {
        public int scenario;
        public int chapter;
        public int stage;
        public string monster_hp;
        public string boss_hp;
        public string bg_01;
        public int monster_group_id;
    }
    public class StageGroup
    {
        public int monster_group;
        public string monster_type;
        public string resource_id;
        public string spinename;
        public string skinname;
        public float resource_size;
    }
    [System.Serializable]
    public class StageReward
    {
        public int scenario;
        public int chapter;
        public int reward;
        public int base_stage_magic_pottion;
        public int stage_gain_magic_potion;
        public int user_exp;
        public int reward_idx;
    }

    public class StageBoxReward
    {
        public int idx;
        public RewardType reward_type;
        public int box_idx;
        public int reward_count;
        public int rarity;
    }

    #endregion

    #region DT_KR_Relic
    public class SRelicInfo
    {
        public int idx;
        public int category;
        public string name;
        public string desc;
        public string icon;
        public string need_item_type;
        public string level_up_potion_type;
        public int level_max;
        public int ability_idx;
        public int skill_idx;
    }
    public class RelicInfo
    {
        public int idx;
        public string name;
        public string desc;
        public string icon;
        public string level_up_potion_type;
        public int level_max;
        public int ability_idx;
    }

    public class RelicNeedCurrency
    {
        public int level;
        public string level_up_potion_10000;
        public string level_up_potion_5000;
        public string level_up_potion_1500;
        public string level_up_potion_500;
        public string level_up_potion_200;
        public string level_up_potion_100;
        public string level_up_potion_5;
        public string level_up_s_relic;
        public int level_up_item_1;
    }
    public class ActiveSkillInfo
    {
        public int idx;
        public int category;
        public string name;
        public string desc;
        public SkillType skill_type;
        public string active_type;
        public string active_type_desc;
        public float active_time;
        public float active_rate;
        public float cool_time;
        public int unlock_level;
        public float skill_lock_time;
        public float hit_time;
        public float skill_ability;
        public int skill_ability_level_up;
        public int level_max;
        public string icon;
    }

    public class FeverReward
    {
        public string reward_type;
        public int box_idx;
        public int reward_count;
        public float rarity;
    }
    #endregion

    #region DT_KR_Abillities
    public class AbilityInfo
    {
        public int idx;
        public string abtype;
        public float level_unit;
        public string name;
        public string desc;
    }
    #endregion

    #region DT_KR_Localization
    public class LocalValue
    {
        public string id;
        public string kr;
        public string en;

        public string GetStringForLocal(bool iskr)
        {
            return (iskr) ? kr : en;
        }
    }
    #endregion

    #region DT_KR_Weapons
    public class ItemInformation
    {
        public int idx;
        public string icon;
        public string outline_sprite;
        public string sprite;
        public string name;
        public string grade;
        public string grade_name;
        public int enchant_stone;
        public int max_lv;
        public int awake_gem_cost;
        public int awake_max_lv;
        public int awake2_max_lv;
        public int awake3_max_lv;
        public string a_aidx_1;
        public string a_aidx_1_gain;
        public string a_aidx_2;
        public string a_aidx_2_gain;
        public string a_aidx_3;
        public string a_aidx_3_gain;
        public string a_aidx_4;
        public string a_aidx_4_gain;
        public string b_aidx_1;
        public string b_aidx_1_gain;
        public string b_aidx_2;
        public string b_aidx_2_gain;
        public string b_aidx_3;
        public string b_aidx_3_gain;
        public int awake_aidx;
        public string awake_aidx_gain;
    }

    public class CostumInformation
    {
        public int idx;
        public string icon;
        public string SpineName;
        public string SkinName;
        public string name;
        public string grade;
        public int enchant_potion;
        public int max_lv;
        public int ability_idx;
        public int buy_gem;
    }

    public class ItemGain
    {
        public int lv;
        public float cos_potion_gain_rate;
        public float ent_stone_gain_rate;
        public float weapon_gain_1;
        public float weapon_gain_2;
        public float weapon_gain_3;
        public float weapon_gain_4;
        public float weapon_gain_5;

    }
    #endregion

    #region DT_KR_Gacha
    public class Gacha
    {
        public int idx;
        public ItemType category;
        public string name;
        public string price_type;
        public int price_value;
        public int reward_count;
        public string reward_box_id;
        public int reward_enchant_stone_min;
        public int reward_enchant_stone_max;
        public int reward_pet_food_min;
        public int reward_pet_food_max;
        public int reward_potion_min;
        public int reward_potion_max;
    }

    public class GachapotionGainRate
    {
        public int level;
        public float potion_rate;
    }

    public class GachaRewardBoxInfo
    {
        public int grade;
        public int need_point;
        public int reward_box_1;
        public int reward_box_2;
        public int reward_box_3;
        public int reward_box_4;
    }

    public class GachaPointReward
    {
        public int point;
        public int weapon_box;
        public int wing_box;
        public int pet_box;
        public int s_relic_box;
    }
    #endregion

    #region 펫관련 데이터
    public class PetInfo
    {
        public int idx;
        public int grade;
        public string icon;
        public string name;
        public int max_level;
        public float Scale;
        public string SpineName;
        public int use_aidx;
        public string use_aidx_gain;
        public int collect_aidx;
        public string collect_aidx_gain;
        public int mix_count;
        public int mix_reward;
        public int pet_levelup_cost;
        public string pet_levelup_cost_gain;
        public int pet_sell_get_food;
    }

    public class PetGain
    {
        public int lv;
        public float aidx_gain_1;
        public float aidx_gain_2;
        public float aidx_gain_3;
        public float aidx_gain_4;
        public float aidx_gain_5;
        public string pet_food_gain_rate;
    }
    #endregion

    #region 보상 관련

    public class OfflineRewardData
    {
        public int chapter;
        public string reward_type;
        public int box_idx;
        public int reward_count;
        public int rarity;
    }

    public class DailyRewardData
    {
        public int day;
        public RewardType reward_type;
        public int count;
        public int box_idx;
    }

    public class BoxData
    {
        public int idx;
        public string name;
        public string desc;
        public CurrencyType boxType;
        public ItemType itemtype;
        public string icon;
        public int box_group_idx;
    }

    public class Box_Group
    {
        public int idx;
        public ItemType reward_type;
        public int reward_idx;
        public int count_min;
        public int count_max;
    }

    public class Reward_Item
    {
        public int idx;
        public string reward_type;
        public int item_idx;
        public int rarity;
    }
    #endregion

    #region 미션
    public class GuideMissionDesc
    {
        public int idx;
        public int next_idx;
        public string name;

        public string name_complete;
        public string desc_reward;

        public MissionType m_type;
        public int m_value;
        public RewardType reward_type;
        public int reward_count;
        public string clear_type;
    }

    /// <summary>
    /// 일일미션은 5시에 초기화
    /// </summary>
    public class DailyMissionDesc
    {
        public int idx;
        public string name;
        public MissionType m_type;

        public int m_value;
        public RewardType reward_type;
        public int reward_count;
    }

    /// <summary>
    /// 일일미션은 5시에 초기화
    /// </summary>
    public class RepeatMissionDesc
    {
        public int idx;
        public string name;
        public MissionType m_type;

        public int m_value;
        public int m_value_lvup;
        public RewardType reward_type;
        public int reward_count;
        public bool repeat_type;
    }
    #endregion

    #region 튜토리얼
    public class TutorialTouch
    {
        public eTutorialDivision tutorial_division;
        public int step;
        public string cha_id;
        public string name_id;
        public string desc_id;
        public eTargetUI target_ui;
        public eArrowDirection arrow_direction;
        public int save_step;
    }
    #endregion

    #region PVP
    public class PVP_BattleReward
    {
        public int win_gem;
        public int win_pvp_point;
        public int win_rank_point;
        public int lose_gem;
        public int lose_pvp_point;
        public int lose_rank_point;
    }

    public class PVP_DailyReward
    {
        public int tier;
        public int rank_min;
        public int rank_max;
        public int daily_gem;
        public int daily_pvp_point;
    }
    public class PVP_WeekReward
    {
        public int tier;
        public int rank_min;
        public int rank_max;
        public int week_gem;
        public int week_pvp_point;
    }
    public class PVP_Rankreset
    {
        public int tier;
        public int week_battle_point_reset;
    }

    public class PVP_BattleAbility
    {
        public int idx;
        public string name;
        public PVPAbilityType abtype;
        public PVPAbilityType SkillAbtype;
        public PVPAbilityType SkillAbtype_2;
    }
    #endregion

    #region 상점 배틀패스,룰렛 등
    public class Battlepasstable
    {
        public int idx;
        public int Count;
        public int free_reward;
        public int pass_reward;
        public CurrencyType free_reward_type;
        public CurrencyType pass_reward_type;
        public string name;
        public string desc;
        public int iap_idx;
    }
    public class ShopGoodstable
    {
        public int idx;//고유번호
        public int sort;//정렬값
        public string icon;//이미지
        public string ability;//상승되는 능력치의 id값(광고 등)
        public CycleType cycle_type;//초기화 주기
        public int cycle_count;//초기화 이전 구매 가능 횟수
        public PriceType price_type;// 지불 타입
        public int price_count;//지불가격
        public string price_max;//지불맥스값
        public string ad_cooltime;//광고타임-(광고효과 지속시간과 같음)
        public int iap_idx;//인앱결제 고유번호
        public string name;//로컬라이즈 이름
        public string desc_1;//로컬라이즈 설명
        public string desc_2;//로컬라이즈 설명
        public string message;//로컬라이즈 메시지
        public int reward_idx;//보상 아이디
        public string daily_reward_idx;//일일보상 id
        public LayoutType LayoutType=LayoutType.none;//버튼 레이아웃 타입
        public int event_icon;//1+1등의 이벤트아이콘 존재 유무
        public float value;//능력치값 존재할시 능력치값의 배수
        public RewardType subinfoIcon_0;
        public RewardType subinfoIcon_1;
    }

    public class ShopRewardInfotable
    {
        public int idx;
        public RewardType reward_type;
        public int reward_count;
        public string reward_box_idx;
        public string icon;
    }

    public class Iapinformationtable
    {
        public int iap_idx;
        public string google;
        public string onestore;
        public string ios;
    }

    public class RouletteTabledata
    {
        public int idx;
        public RewardType reward_type;
        public int reward_count;
        public int rarity;
        public string icon;
    }
    #endregion

    #region 메일박스

    #endregion

    #region 스토리북
    public class StoryBookMain
    {
        public int idx;
        public string name;
        public string desc;
        public int unlock_scenario;
        public int ulock_chapter;
        public string unlock_desc;
        public int reward_gem;
        public int script_group_id;
    }

    public class StoryBookScript
    {
        public int idx;
        public int script_group;
        public int step;
        public int next_step;
        public string title_name;
        public string character_filename;
        public string cha_position;
        public string face_ani;
        public string cha_name;
        public string script;
    }

    #endregion

    #region 뉴비 패키지
    public class Newbieinfo
    {
        public int idx;
        public int next_idx;
        public string name;
        public int a_aidx_1;
        public int a_aidx_2;
        public int a_aidx_3;
        public int reward_gem;
        public int reward_mileage;
        public int iap_idx;
    }

    public class Iapinfo
    {
        public int iap_idx;
        public string google;
        public string onestore;
        public string ios;
    }
    #endregion

    #region 요정
    public class FairyInfo
    {
        public int idx;
        public RewardType reward_type;
        public int reward_idx;
        public int reward_count;
        public int rarity;
        public string message_id;
    }
    #endregion
}
