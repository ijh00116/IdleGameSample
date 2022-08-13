using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BlackTree
{
    [System.Serializable]
    public class BaseMission
    {
        public bool take { get; set; }
        public bool IsDone { get; set; }
        /// <summary>
        /// 현재 미션 달성횟수
        /// </summary>
        /// <returns></returns>
        public virtual long GetCurCount()
        {
            return 0;
        }
        /// <summary>
        /// 미션 필요달성 횟수
        /// </summary>
        /// <returns></returns>
        public virtual long GetMaxCount()
        {
            return 0;
        }
        /// <summary>
        /// 미션 지급보상
        /// </summary>
        /// <returns></returns>
        public virtual int GetRewardValue()
        {
            return 0;
        }
        /// <summary>
        /// 미션완료여부
        /// </summary>
        /// <returns></returns>
        public virtual bool IsComplete()
        {
            long curCount = GetCurCount();
            long maxCount = GetMaxCount();
            Debug.Assert(maxCount > 0);

            return (curCount >= maxCount);
        }
        public virtual MissionType GetMissionType()
        {
            return MissionType.MONSTER_KILL;
        }

        public virtual RewardType GetRewardType()
        {
            return RewardType.REWARD_GEM;
        }

        public virtual bool CompleteMissionData()
        {
            return false;
        }

        public virtual string GetMissionTitle()
        {
            return GetMissionType().ToString();
        }
    }

    [System.Serializable]
    public class Data_Mission
    {
        [System.Serializable]
        public class GuideMission : BaseMission
        {
            [System.NonSerialized]Data_Mission data_mission;
            [System.NonSerialized]public GuideMissionDesc baseInfo;
            public long curCount;
            public int idx;
            public GuideMission()
            {
                curCount = 0;
                take = false;
                IsDone = false;
            }
            public void Init(GuideMissionDesc info, Data_Mission mission)
            {
                data_mission = mission;
                baseInfo = info;
                idx = baseInfo.idx;
            }
            public override long GetCurCount()
            {
                return curCount;
            }

            public override long GetMaxCount()
            {
                return baseInfo.m_value;
            }

            /// <summary>
            /// 미션 지급보상
            /// </summary>
            /// <returns></returns>
            public override int GetRewardValue()
            {
                // 기본보상 + 누적증가보상(현재미션레벨 X 레벨당 보상증가)
                int rewardValue = baseInfo.reward_count;
                return rewardValue;
            }
            public override bool IsComplete()
            {
                if (take == true)//이미 받음
                    return false;

                int maxcount = baseInfo.m_value;
                if (maxcount <= curCount)
                {
                    IsDone = true;
                    return true;
                }
                else
                    return false;
            }

            //받은거 처리
            public override bool CompleteMissionData()
            {
                if (IsDone == false)
                    return false;
                take = true;
                IsDone = true;

                data_mission.UpdateMissionInfo();
                LocalValue typename = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == GetRewardType().ToString());

                Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(string.Format("{0} {1}획득",typename.GetStringForLocal(true) ,baseInfo.reward_count)));

                return true;
            }

            public override MissionType GetMissionType()
            {
                return baseInfo.m_type;
            }

            public override RewardType GetRewardType()
            {
                return baseInfo.reward_type;
            }
            public override string GetMissionTitle()
            {
                LocalValue _data = InGameDataTableManager.LocalizationList.mission.Find(o => o.id == baseInfo.name);
                if(_data==null)
                {
                    return null;
                }
                return _data.kr;
            }
        }
        [System.Serializable]
        public class DailyMission : BaseMission
        {
            [System.NonSerialized] public DailyMissionDesc baseInfo;
            public long curCount;
            public int idx;
            public DailyMission()
            {
                curCount = 0;
                IsDone = false;
                take = false;
            }
            public void Init(DailyMissionDesc info)
            {
                baseInfo = info;
                idx = baseInfo.idx;
            }
            public override long GetCurCount()
            {
                return curCount;
            }

            public override long GetMaxCount()
            {
                return baseInfo.m_value;
            }

            /// <summary>
            /// 미션 지급보상
            /// </summary>
            /// <returns></returns>
            public override int GetRewardValue()
            {
                // 기본보상 + 누적증가보상(현재미션레벨 X 레벨당 보상증가)
                int rewardValue = baseInfo.reward_count;

                return rewardValue;
            }

            public override bool IsComplete()
            {
                if (take == true)//이미 받음
                    return false;

                int maxcount = baseInfo.m_value;
                if (maxcount <= curCount)
                {
                    IsDone = true;
                    return true;
                }
                else
                {
                    IsDone = false;
                    return false;
                }
            }

            public override bool CompleteMissionData()
            {
                if (IsDone == false)
                    return false;
                take = true;
                IsDone = true;

                LocalValue typename = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == GetRewardType().ToString());

                Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(string.Format("{0} {1}획득", typename.GetStringForLocal(true), baseInfo.reward_count)));
                return true;
            }

            public void Initialize()
            {
                take = false;
                IsDone = false;
                curCount = 0;
            }

            public override MissionType GetMissionType()
            {
                return baseInfo.m_type;
            }
            public override RewardType GetRewardType()
            {
                return baseInfo.reward_type;
            }
            public override string GetMissionTitle()
            {
                LocalValue _data = InGameDataTableManager.LocalizationList.mission.Find(o => o.id == baseInfo.name);
                return _data.kr;
            }
        }
        [System.Serializable]
        public class RepeatMission : BaseMission
        {
            [System.NonSerialized]public RepeatMissionDesc baseInfo;
            public long curCount;
            public int LEVEL;
            public int idx;
            public RepeatMission()
            {
                LEVEL = 0;
                IsDone = false;
                take = false;
                curCount = 0;
            }

            public void Init(RepeatMissionDesc info)
            {
                baseInfo = info;
                idx = baseInfo.idx;
            }
            public override long GetCurCount()
            {
                return curCount;
            }

            public override long GetMaxCount()
            {
                int maxcount= baseInfo.m_value;

                for(int i=1; i<=LEVEL; i++)
                {
                    maxcount += maxcount +( baseInfo.m_value_lvup * i);
                }
                //int max = baseInfo.m_value + LEVEL * baseInfo.m_value_lvup;
                return maxcount;
            }

            /// <summary>
            /// 미션 지급보상
            /// </summary>
            /// <returns></returns>
            public override int GetRewardValue()
            {
                // 기본보상 + 누적증가보상(현재미션레벨 X 레벨당 보상증가)
                int rewardValue = baseInfo.reward_count;

                return rewardValue;
            }

            public override bool IsComplete()
            {
                if (take == true)//이미 받음
                    return false;

                int maxcount = (int)GetMaxCount();
                if (maxcount <= curCount)
                {
                    IsDone = true;
                    return true;
                }
                else
                {
                    IsDone = false;
                    return false;
                }
            }
            public override bool CompleteMissionData()
            {
                if (IsDone == false)
                    return false;
                take = true;
                IsDone = true;
                if(baseInfo.repeat_type)
                {
                    take = false;
                    IsDone = false;
                    LEVEL++;
                }
                LocalValue typename = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == GetRewardType().ToString());

                Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(string.Format("{0} {1}획득", typename.GetStringForLocal(true), baseInfo.reward_count)));
                return true;
            }

            public override MissionType GetMissionType()
            {
                return baseInfo.m_type;
            }
            public override RewardType GetRewardType()
            {
                return baseInfo.reward_type;
            }
            public override string GetMissionTitle()
            {
                LocalValue _data = InGameDataTableManager.LocalizationList.mission.Find(o => o.id == baseInfo.name);
                return _data.kr;
            }
        }

        public List<GuideMission> guideMissions;
        public List<DailyMission> dailyMission;
        public List<RepeatMission> repeatMissions;
        public GuideMission CurrentGuideMission;
        public PlayingRecord _playingRecord;
        public Data_Mission()
        {
            guideMissions = new List<GuideMission>();
            dailyMission = new List<DailyMission>();
            repeatMissions = new List<RepeatMission>();

            _playingRecord = new PlayingRecord();

        }

        public void Init()
        {
            List<GuideMission> guidlist = new List<GuideMission>();
            foreach(var mission in InGameDataTableManager.MissionTableList.guide_mission)
            {
                GuideMission gm = new GuideMission();
                gm.Init(mission,this);
                guidlist.Add(gm);
            }
            guideMissions = guidlist.OrderBy(o => o.baseInfo.idx).ToList();

            foreach (var mission in InGameDataTableManager.MissionTableList.daily_mission)
            {
                DailyMission dm = new DailyMission();
                dm.Init(mission);
                dailyMission.Add(dm);
            }
            foreach (var mission in InGameDataTableManager.MissionTableList.repeat_mission)
            {
                RepeatMission rm = new RepeatMission();
                rm.Init(mission);
                repeatMissions.Add(rm);
            }
            LoadSetting();
        }

        public void LoadSetting()
        {
            foreach (var mission in InGameDataTableManager.MissionTableList.guide_mission)
            {
                var _ms = guideMissions.Find(o => o.idx == mission.idx);
                if(_ms!=null)
                    _ms.Init(mission, this);
            }
   
            foreach (var mission in InGameDataTableManager.MissionTableList.daily_mission)
            {
                var _dm= dailyMission.Find(o=>o.idx==mission.idx);
                if(_dm!=null)
                {
                    _dm.Init(mission);
                }
            }
            foreach (var mission in InGameDataTableManager.MissionTableList.repeat_mission)
            {
                var _rm = repeatMissions.Find(o => o.idx == mission.idx);
                if (_rm != null)
                {
                    _rm.Init(mission);
                }
            }
            missionUpdater = new InGame.Event.MissionValueUpdate();
            UpdateMissionInfo();
        }

        //미션 카운트 업데이트
        public void UpdateMissionInfo()
        {
            for(int i=0; i<guideMissions.Count; i++)
            {
                if (guideMissions[i].take == false)
                {
                    CurrentGuideMission = guideMissions[i];
                    break;
                }
            }
        }

        public long GetRecordValue(MissionType missionType)
        {
            return (_playingRecord != null) ? _playingRecord.GetMissionValue(missionType) : 0;
        }

        [System.NonSerialized]
        InGame.Event.MissionValueUpdate missionUpdater = new InGame.Event.MissionValueUpdate();
        public void IncMissionValue(MissionType _type, int value)
        {
            _playingRecord.IncMissionValue(_type, value);
            missionUpdater.missiontype = _type;
            if (CurrentGuideMission.baseInfo.m_type==_type)
            {
                CurrentGuideMission.curCount += value;
            }
            DailyMission _dmission = dailyMission.Find(o => o.baseInfo.m_type == _type);
            if(_dmission != null)
                _dmission.curCount += value;
            RepeatMission _rmission = repeatMissions.Find(o => o.baseInfo.m_type == _type);
            if (_rmission != null)
                _rmission.curCount += value;

            Message.Send<InGame.Event.MissionValueUpdate>(missionUpdater);

            Common.InGameManager.Instance.Localdata.SaveData(Common.InGameManager.Instance.GetPlayerData.saveData);
            //BackendManager.Instance.SaveDBData(DTConstraintsData.UserData, _playingRecord);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData, guideMissions);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData, dailyMission);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData,repeatMissions);
        }

        public void SetMissionValue(MissionType _type, int value,bool sendmsg)
        {
            _playingRecord.SetMissionValue(_type, value);
            missionUpdater.missiontype = _type;
            if (CurrentGuideMission.baseInfo.m_type == _type)
            {
                CurrentGuideMission.curCount = value;
            }
            DailyMission _dmission = dailyMission.Find(o => o.baseInfo.m_type == _type);
            if (_dmission != null)
                _dmission.curCount = value;
            RepeatMission _rmission = repeatMissions.Find(o => o.baseInfo.m_type == _type);
            if (_rmission != null)
                _rmission.curCount = value;

            if(sendmsg)
                Message.Send<InGame.Event.MissionValueUpdate>(missionUpdater);

            Common.InGameManager.Instance.Localdata.SaveData(Common.InGameManager.Instance.GetPlayerData.saveData);
            //BackendManager.Instance.SaveDBData(DTConstraintsData.UserData, _playingRecord);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData, guideMissions);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData, dailyMission);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData,repeatMissions);
        }
    }
}
