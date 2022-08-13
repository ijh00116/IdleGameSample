using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public enum MissionDatatype
    {
        Daily,
        Guide,
        Repeate,
        End
    }
    public class MissionUIInterface : MonoBehaviour
    {
        public MissionUISlot missionuiPrefab;

        public Button DailyBtn;
        public Image DailySelected;
        public Button AchievementBtn;
        public Image AchievementSelected;

        public Transform Dailyparent;
        public Transform Repeatparent;

        public GameObject DailyWindow;
        public GameObject RepeatWindow;

        public Dictionary<MissionDatatype,List<MissionUISlot>> missionUIList = new Dictionary<MissionDatatype, List<MissionUISlot>>();
        public void Awake()
        {
            DailyBtn.onClick.AddListener(() => {
                DailySelected.gameObject.SetActive(true);
                DailyWindow.gameObject.SetActive(true);
                RepeatWindow.gameObject.SetActive(false);
                AchievementSelected.gameObject.SetActive(false);
            });

            AchievementBtn.onClick.AddListener(() => {
                DailySelected.gameObject.SetActive(false);
                DailyWindow.gameObject.SetActive(false);
                RepeatWindow.gameObject.SetActive(true);
                AchievementSelected.gameObject.SetActive(true);
            });
            
            RepeatWindow.gameObject.SetActive(false);
            AchievementSelected.gameObject.SetActive(false);
            DailyWindow.gameObject.SetActive(true);
            DailySelected.gameObject.SetActive(true);

            List<MissionUISlot> dmissionuislotlist = new List<MissionUISlot>();
            for(int i=0; i<InGameDataTableManager.MissionTableList.daily_mission.Count; i++)
            {
                var obj = Instantiate(missionuiPrefab);
                obj.transform.SetParent(Dailyparent, false);
                int idx = InGameDataTableManager.MissionTableList.daily_mission[i].idx;
                BaseMission basemission = Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.dailyMission.Find(o => o.baseInfo.idx == idx);
                obj.Init(MissionDatatype.Daily, basemission);
                dmissionuislotlist.Add(obj);
            }

            List<MissionUISlot> rmissionuislotlist = new List<MissionUISlot>();
            for (int i = 0; i < InGameDataTableManager.MissionTableList.repeat_mission.Count; i++)
            {
                var obj = Instantiate(missionuiPrefab);
                obj.transform.SetParent(Repeatparent, false);
                int idx = InGameDataTableManager.MissionTableList.repeat_mission[i].idx;
                BaseMission basemission = Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.repeatMissions.Find(o => o.baseInfo.idx == idx);
                obj.Init(MissionDatatype.Repeate, basemission);
                rmissionuislotlist.Add(obj);
            }
            missionUIList.Add(MissionDatatype.Daily, dmissionuislotlist);
            missionUIList.Add(MissionDatatype.Repeate, rmissionuislotlist);

            Message.AddListener<InGame.Event.MissionValueUpdate>(MissionDataUpdate);
        }

        public void Release()
        {
            foreach(var _data in missionUIList)
            {
                foreach(var _mdata in _data.Value)
                {
                    _mdata.Release();
                }
            }

            Message.RemoveListener<InGame.Event.MissionValueUpdate>(MissionDataUpdate);
        }

        void MissionDataUpdate(InGame.Event.MissionValueUpdate msg)
        {
            bool IsCompleteMissionExist = false;
            foreach(var _data in missionUIList)
            {
                for(int i=0; i<_data.Value.Count; i++)
                {
                    if(_data.Value[i].mymission.IsComplete())
                    {
                        IsCompleteMissionExist = true;
                        break;
                    }
                }
                if (IsCompleteMissionExist)
                    break;
            }

            Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.Mission, IsCompleteMissionExist));
        }
    }

}
