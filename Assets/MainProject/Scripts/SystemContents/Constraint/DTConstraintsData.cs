using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DTConstraintsData
{
    #region BackEnd서버
    public const string Indate = "inDate";
    #endregion

    #region 서버 테이블 네임
    public const string Abilities = "Abilities";
    public const string item = "item";
    #endregion

    #region 유물
    public const string Relic = "Relic";
    public const string RelicId = "RelicId";
    public const string RelicLevel = "RelicLevel";

    #endregion

    #region 특수 유물
    public const string SRelic = "SRelic";
    public const string SRelicId = "SRelicId";
    public const string SRelicLevel = "SRelicLevel";

    public const string SpecialSkill = "SpecialSkill";
    #endregion

    #region 퀘스트
    public const string Quest = "Quest";
    public const string QuestId = "QuestId";
    public const string QuestLevel = "QuestLevel";
    #endregion

    #region 스킬
    public const string Skill = "Skill";
    public const string SkillId = "SkillId";
    public const string SkillLevel = "SkillLevel";
    #endregion

    #region 아이템
    public const string Weapon = "Weapon";
    public const string WeaponId = "WeaponId";
    public const string WeaponLevel = "WeaponLevel";

    public const string Wing = "Wing";
    public const string WingId = "WingId";
    public const string WingLevel = "WingLevel";

    public const string Costum = "Costum";
    public const string CostumId = "CostumId";
    public const string CostumLevel = "CostumLevel";

    public const string Pet = "Pet";
    public const string PetId = "PetId";
    public const string PetLevel = "PetLevel";
    #endregion

    #region 유저데이터
    public const string PVPAbil = "PVPAbil";
    public const string PVPInfo = "PVPInfo";
    public const string UserData = "UserData";
    public const string UserDataForPvp = "UserDataForPVP";
    public const string Stage = "Stage";
    public const string Currency = "Currency";
    public const string Enforce = "Enforce";
    public const string RewardData = "Reward";
    public const string DungeonData = "Dungeon";
    public const string PetDungeonData = "PetDungeon";
    public const string Tutorial = "Tutorial";
    public const string division = "division";
    public const string step = "step";

    public const string currencyType = "currencyType";
    public const string Value = "VALUE";

    public const string AwakeLevel = "AwakeLevel";
    public const string EnchentLevel = "EnchentLevel";

    public const string chapter = "chapter";
    public const string currentWave = "CURRENTWAVE";
    public const string BestScenario = "BestScenario";
    public const string Bestchapter = "Bestchapter";
    public const string scenario = "scenario";
    public const string CowBestLevel = "CowBestLevel";

    public const string Pet_BestChapter = "Pet_BestChapter";
    public const string Pet_BestStage = "Pet_BestStage";

    public const string CurrentExp = "CurrentExp";

    public const string Rewarded = "Rewarded";
    public const string ChapterIndex = "ChapterIndex";
    public const string ScenarioIndex = "ScenarioIndex";
    public const string KillCount = "KillCount";
    public const string KingKillCount = "KingKillCount";

    public const string GachaWeaponLevel = "GachaWeaponLevel";
    public const string GachaWingLevel = "GachaWingLevel";
    public const string GachaPetLevel = "GachaPetLevel";
    public const string GachaSrelicLevel = "GachaSrelicLevel";

    public const string GachaWeaponExp = "GachaWeaponExp";
    public const string GachaWingExp = "GachaWingExp";
    public const string GachaPetExp = "GachaPetExp";
    public const string GachaSrelicExp = "GachaSrelicExp";

    public const string FreeGachaWeaponLv = "FreeGachaWeaponLv";
    public const string FreeGachaWingLv = "FreeGachaWingLv";
    public const string FreeGachaPetLv = "FreeGachaPetLv";
    public const string FreeGachaSrelicLv = "FreeGachaSrelicLv";
                     
    public const string GachaWeaponAdLeftTime = "GachaWeaponAdLeftTime";
    public const string GachaWingAdLeftTime = "GachaWingAdLeftTime";
    public const string GachaPetAdLeftTime = "GachaPetAdLeftTime";
    public const string GachaSrelicAdLeftTime = "GachaSrelicAdLeftTime";

    public const float GachaAdCoolTime = 1800;

    public const int FreeGachaMaxLv = 6;

    public const string LOGOUTTIME = "LOGOUTTIME";
    public const string RewardIndex = "RewardIndex";
    public const string GetRewardDay = "GetRewardDay";

    public const string Buff = "Buff";
    public const string LeftAttackPowerBuffTime = "LeftAttackPowerBuffTime";
    public const string LeftAttackSpeedBuffTime = "LeftAttackSpeedBuffTime";
    public const string LeftMonsterGoldBuffTime = "LeftMonsterGoldBuffTime";
    public const string LeftAutoSkillBuffTime = "LeftAutoSkillBuffTime";
    public const string LeftMoveSpeedBuffTime = "LeftMoveSpeedBuffTime";
    public const string LeftAttackPowerBuffActivate = "LeftAttackPowerBuffActivate";
    public const string LeftAttackSpeedBuffActivate = "LeftAttackSpeedBuffActivate";
    public const string LeftMonsterGoldBuffActivate = "LeftMonsterGoldBuffActivate";
    public const string LeftAutoSkillBuffActivate = "LeftAutoSkillBuffActivate";
    public const string LeftMoveSpeedBuffActivate = "LeftMoveSpeedBuffActivate";

    public const string CurrentNewbiePackageIdx = "CurrentNewbiePackageIdx";

    public const string PetQuickStartOn = "PetQuickStartOn";
    public const int BuffTime = 900;

    public const string CanSeeRouletAd = "CanSeeRouletAd";
    public const string RoulettAdLeftTime = "RoulettAdLeftTime";

    public const string CanSeeAdGem = "CanSeeAdGem";
    public const string AdGemLeftTime = "AdGemLeftTime";
    public const string ZoomValue = "ZoomValue";
    #endregion

    #region 공통
    public const string idx = "idx";
    public const string QuestIdx = "QuestIdx";
    public const string Equiped = "Equiped";
    public const string LEVEL = "LEVEL";
    public const string Level = "Level";
    public const string PlayingTime = "PlayingTime";
    public const string CharacterLevel = "CharacterLevel";
    public const string amount = "amount";
    public const string Unlocked = "Unlocked";
    public const string Awake = "Awake";
    public const string AwakeLv = "AwakeLv";
    public const int RewardDay = 30;
    public const int MaxEquiptPetCount = 5;
    public const string LeftCoolTime = "LeftCoolTime";
    #endregion

    #region 펫
    public const string PETFOOD = "PETFOOD";

    #endregion

    #region Constraints
    public const int BATTLE_STAGE_WAVE = 12;
    public const int BATTLE_STAGE_BOSS_TIME_LIMIT = 10;
    public const double BATTLE_STAGE_MIMIC_GEN_RATE = 5;
    public const int REWARD_GAIN_RATE_MIMIC = 3;
    public const int REWARD_GAIN_RATE_BOSS = 10;
    public const int RELIC_BUY_COST_GEM = 300;
    public const int ABILITY_ATK_SPEED_MAX = 10000;
    public const int DG_BATTLE_TIME_SEC = 30;
    public const double DG_BOSS_GEN_RATE = 10;
    public const int DG_REWARD_GAIN_RATE_BOSS = 3;
    public const double BATTLE_KILL_GOLD_10X_RATE = 2.5;
    public const double BATTLE_KILL_POTION_10X_RATE = 2.5;

    public const int SPEED_REVISION_VALUE = 1000;

    public const int PLAYER_MAX_LEVEL = 10000;

    public const float UI_DAMAGETEXT_SPEED = 5.0f;
    #endregion

    #region 스킬관련 고정 수치
    public static int ActiveSkillData_forAtkSpeed = 0;
    public static int ActiveSkillData_forMoveSpeed = 0;
    #endregion

    #region 보상
    public const int MaxDailyRewardIndex = 30;//임시로 일일보상 체크 여기다가 해놈(나중에 일일보상 테이블 나오면 테이블에 따라 달라질것.)
    #endregion

    #region 랭크
    public const string PVPRTRankTableUUID = "5bc8bc10-7bd3-11eb-86bb-21788bad5d71";
    public const string STAGERTRankTableUUID = "f25fb1e0-9124-11eb-b6d4-93acbee47e66";
    public const string RankScore = "RankScore";
    public const string Scenario = "Scenario";
    public const string Chapter = "Chapter";
    public const string StageScore = "StageScore";
    public const string AttackPower = "AttackPower";
    public const string AttackSpeed = "AttackSpeed";
    public const string CriRate = "CriRate";
    public const string CriDamage = "CriDamage";
    public const string HealthPoint = "HealthPoint";
    public const string SkinName = "SkinName";
    public const string weaponName = "weaponName";
    public const string wingName = "wingName";
    public const string Lightning_LV = "Lightning_LV";
    public const string Lightning_Damage = "Lightning_Damage";
    public const string PowerfullAttack_LV = "PowerfullAttack_LV";
    public const string PowerfullAttack_Damage = "PowerfullAttack_Damage";
    public const string HITRate_LV = "HITRate_LV";
    public const string HITRate_Damage = "HITRate_Damage";
    public const string CriticalRate_LV = "CriticalRate_LV";
    public const string CriticalRate_Damage = "CriticalRate_Damage";
    public const string MoveAtkSpeed_LV = "MoveAtkSpeed_LV";
    public const string MoveAtkSpeed_Value = "MoveAtkSpeed_Damage";
    #endregion

    #region 미션
    public const string GuideMISSION = "GuideMISSION";
    public const string DailyMISSION = "DailyMISSION";
    public const string RepeatMISSION = "RepeatMISSION";
    public const string PlayingRecord = "PlayingRecord";
    public const string IsDone = "IsDone";
    public const string curCount = "curCount";
    public const string take = "take";
    public const string CurrentPlayingTime = "CurrentPlayingTime";
    #endregion

    public const int NormalMaxScenario = 10;
    public const int NormalMaxChapter = 10;
    public const int NormalMaxStage = 100;

    public const int DungeonMaxStage = 15;

    public const int PVPMaxTime = 15;

    public const int AD_GEM_REWARD_COUNT = 50;
    public const int AD_GEM_COOLTIME = 600;
    public const int AD_ROULET_COOLTIME = 1200;

    public const int OFFLINfE_MON_KILL_TIME = 30;

    public const int BATTLE_STAGE_ITEM_MON_KILL = 30;
    public const int GIFT_FAIRY_GEN_COOLTIME = 20;
    public const int GIFT_FAIRY_REWARD_DAILY_MAX = 200;

    public const string IsFreeTaken = "IsFreeTaken";
    public const string IsPaidTaken = "IsPaidTaken";
}
