using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    /// <summary>
    /// 값이 중간에 추가되거나 변경되면 안됨!
    /// </summary>
    public enum MissionType
    {
        MONSTER_KILL         ,
        QUEST_LEVELUP        ,
        GACHA_COUNT          ,
        WEAPON_ENCHANT       ,
        WEAPON_MIX           ,
        CHA_LEVELUP          ,
        CHA_ENCHANT          ,
        CHAPTER_1_STAGE      ,
        RELIC_COUNT          ,
        RELIC_LEVELUP        ,
        PET_LEVELUP          ,
        PET_MIX              ,
                             
                             
        PET_GET              ,
        CHA_EVOLUTION        ,
        PASSIVE_LEVELUP      ,
        COSTUM_LEVELUP       ,
        WING_ENCHANT         ,
        WING_MIX             ,
        RELIC_ENCHANT        ,
        S_RELIC_ENCHANT      ,
        GACHA_WEAPON         ,
        GACHA_WING           ,
        GACHA_PET            ,
        GACHA_S_RELIC        ,
        GAME_REVIEW          ,
        CLEAR_COUNT          ,
        DUNGEON_BATTLE       ,
        PET_BATTLE           ,
        PVP_BATTLE           ,
        FAIRY_GETTING_COUNT,


        End
    }

    public enum AchivementType
    {
        NONE = 0,
        STAGE_CLEAR = 1,
    }
}