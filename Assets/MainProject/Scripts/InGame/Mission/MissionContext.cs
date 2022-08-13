using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace BlackTree
{
    [System.Serializable]
    public class PlayingRecord
    {
        public long MONSTER_KILL     { get; set; }
        public long QUEST_LEVELUP    { get; set; }
        public long GACHA_COUNT      { get; set; }
        public long WEAPON_ENCHANT   { get; set; }
        public long WEAPON_MIX       { get; set; }
        public long CHA_LEVELUP      { get; set; }
        public long CHA_ENCHANT      { get; set; }
        public long CHAPTER_1_STAGE  { get; set; }
        public long RELIC_COUNT      { get; set; }
        public long RELIC_LEVELUP    { get; set; }
        public long PET_LEVELUP      { get; set; }
        public long PET_MIX             { get; set; }
        public long PET_GET           { get; set; }
        public long CHA_EVOLUTION     { get; set; }
        public long PASSIVE_LEVELUP   { get; set; }
        public long COSTUM_LEVELUP    { get; set; }
        public long WING_ENCHANT      { get; set; }
        public long WING_MIX          { get; set; }
        public long RELIC_ENCHANT     { get; set; }
        public long S_RELIC_ENCHANT   { get; set; }
        public long GACHA_WEAPON      { get; set; }
        public long GACHA_WING        { get; set; }
        public long GACHA_PET         { get; set; }
        public long GACHA_S_RELIC     { get; set; }
        public long GAME_REVIEW     { get; set; }
        public long CLEAR_COUNT    { get; set; }
        public long DUNGEON_BATTLE { get; set; }
        public long PET_BATTLE     { get; set; }
        public long PVP_BATTLE     { get; set; }
        public long FAIRY_GETTING_COUNT { get; set; }

        public long GetMissionValue(MissionType _MissionType)
        {
            var t = this.GetType();
            var field = t.GetProperty(_MissionType.ToString());
            if (null == field) return -1;

            object o = field.GetValue(this);
            if (null == o) return -1;

            return (long)o;
        }
        public long SetMissionValue(MissionType _MissionType, int _IncValue)
        {
            var t = this.GetType();
            var field = t.GetProperty(_MissionType.ToString());
            if (null == field) return -1;

            object o = field.GetValue(this);
            if (null == o) return -1;

            long curval = (long)o;
            curval = _IncValue;

            field.SetValue(this, _IncValue);

            return curval;
        }

        public long IncMissionValue(MissionType _MissionType, int _IncValue)
        {
            var t = this.GetType();
            var field = t.GetProperty(_MissionType.ToString());
            if (null == field) return -1;

            object o = field.GetValue(this);
            if (null == o) return -1;

            long curval = (long)o;
            curval += _IncValue;

            field.SetValue(this, curval);

            return curval;
        }

        /// <summary>
        /// 높은값으로 교체
        /// </summary>
        /// <param name="_MissionType"></param>
        /// <param name="_TargetValue"></param>
        /// <returns>수정했는지여부</returns>
        public bool HighValueExchange(MissionType _MissionType, int _TargetValue)
        {
            var t = this.GetType();
            var field = t.GetProperty(_MissionType.ToString());
            if (null == field) return false;

            object o = field.GetValue(this);
            if (null == o) return false;

            long curval = (long)o;

            if (_TargetValue <= curval) return false;

            field.SetValue(this, _TargetValue);
            return true;
        }
    }

}
