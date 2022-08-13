
namespace BlackTree
{
    public enum AbilitiesType
    {
        //플레이어
        #region 강화
        ENFORCE_Gain_Rate=0,
        #endregion

        #region 공격력
        CHA_ATTACK_UP, ///공격력 증가
        CHA_BAGIC_ATTACK_UP, //추가 공격력
        #endregion

        #region 공격속도

        #endregion

        #region 이동속도

        #endregion

        #region 크리티컬

        #endregion

        #region 레벨증가

        #endregion

        #region 적 hp 관련

        #endregion

        #region 레벨업 골드 감소

        #endregion

        #region 퀘스트 관련
        QUEST_REWARD_UP,// 퀘스트 보상 증가
        QUEST_LEVEL_UP_COST_DOWN, //퀘스트 강화 비용 감소
        QUEST_REWARD_TIME_DOWN,//퀘스트 시간 감소
        #endregion

        #region 적 hp 관련

        #endregion

        CHA_ATTACK_SPEED_UP, //공격 속도 증가
        CHA_CRITICAL_PER, //크리티컬 확률 증가
        CHA_CRITICAL_DAMAGE_UP, //크리티컬 데미지 증가
        CHA_CRITICAL_DAMAGE_DOWN, //크리티컬 데미지 방어
        CHA_MOVE_SPEED_UP, //이동 속도 증가
        CHA_LV_UP, //데스나이트 레벨 증가
        MON_HP_DOWN, //몬스터 HP 감소
        BOSS_HP_DOWN, //보스 몬스터 HP 감소
        SKILL_ONE_COOLTIME_DOWN, //스킬(돌격진격) 지속 시간 증가
        SKILL_TWO_COOLTIME_DOWN,//스킬(돌격진격) 쿨타임 감소
        STAGE_WAVE_MON_COUNT_DOWN, //몬스터 출현 감소
        CHA_LEVEL_UP_COST_DOWN, //데스나이트 레벨업 비용 감소

        MONS_KILL_REWARD_GOLD_UP,//몬스터 처치 골드 증가
        MONS_POTION_REWARD_UP,//건전지 획득량 증가
        BOSS_KILL_GOLD_UP,//보스 처치시 골드 획득량 증가
        BOSS_POTION_REWARD_UP,//보스 처리시 건전지 획득량 증가
        MIMIC_GEN_RATE_UP,//미믹 출현 확률 증가
        MIMIC_KILL_REWARD_GOLD_UP,//미믹 골드 획득 증가
        MIMIC_POTION_REWARD_UP,//미믹 건전지 획득 증가
        DUNGEON_TIME_LIMIT_UP,//카우방 시간 증가
        DUNGEON_BOSS_GEN_RATE_UP,//카우킹 등장 확률 증가
        DUNGEON_REWARD_POTION_UP,//카우방 우유 획득량 증가
        DUNGEON_REWARD_MAGIC_STONE_UP,//카우방 암흑 구슬 획득량 증가
        KILL_REWARD_GOLD_10X_RATE,//적 처치 시 골드 10배 확률
        KILL_REWARD_POTION_10X_RATE,//적 처치 시 건전지 10배 확률
        CHA_HP_UP,//체력 증가


        #region 광고버프
        CHA_AD_ATTACK_BUFF,
        CHA_AD_REWARDGOLD_BUFF,
        CHA_AD_ATTACKSPEED_BUFF,
        CHA_AD_FEVERTIME_BUFF,
        CHA_AD_MOVESPEED_BUFF,
        CHA_AD_REWARDPOTION_BUFF,
        #endregion

        #region 뉴비패키지
        NEWBIE_ATTACK_UP,
        NEWBIE_GOLD_UP,
        NEWBIE_POTION_UP,
        #endregion

        End
    }

    public enum PVPAbilityType
    {
        CHA_LV,
        CHA_ATTACK,
        CHA_ATTACK_UP,
        CHA_BAGIC_ATTACK_UP,
        CHA_ATTACK_SPEED_UP,
        CHA_CRITICAL_PER,
        CHA_CRITICAL_DAMAGE_UP,
        CHA_SKILL_LIGHTNING_LV,
        CHA_SKILL_LIGHTNING_DAMGAGE,
        CHA_SKILL_POWERFULL_ATTACK_LV,
        CHA_SKILL_POWERFULL_ATTACK_DAMGAGE,
        CHA_SKILL_HIT_RATE_LV,
        CHA_SKILL_HIT_DAMGAGE,
        CHA_SKILL_HIT_RATE,
        CHA_SKILL_HIT_CRITICAL_RATE_LV,
        CHA_SKILL_HIT_CRITICAL_DAMGAGE,
        CHA_SKILL_HIT_CRITICAL_RATE,
        CHA_SKILL_BUFF_ATK_MOVE_SPEED_LV,
        CHA_SKILL_BUFF_ATK_MOVE_SPEED_VALUE,

        End
    }
}
