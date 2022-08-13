using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class GuideMissionInfo : MonoBehaviour
    {
        [SerializeField] Text MissionName;
        [SerializeField] Text ValueAmountText;
        [SerializeField] Text RewardText;
        [SerializeField] Button ClearBtn;

        public void Init()
        {
            LocalValue _localdata = InGameDataTableManager.LocalizationList.mission.Find(o => o.id == 
            Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.baseInfo.name);
            MissionName.text = string.Format("<color=yellow>{0}</color>", _localdata.kr); 

            ValueAmountText.text = string.Format("<color=cyan>{0}</color>/{1}",
                Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.curCount,
                Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.GetMaxCount());

            LocalValue rewardtype = InGameDataTableManager.LocalizationList.currency.Find(o => o.id ==
              Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.baseInfo.reward_type.ToString());
            RewardText.text = string.Format("보상 : <color=cyan>{0} {1}</color>", rewardtype.GetStringForLocal(true),
            Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.baseInfo.reward_count);

            ClearBtn.onClick.AddListener(PushRewardBtn);

            Message.AddListener<InGame.Event.MissionValueUpdate>(MissionDataUpdate);
        }

        public void Release()
        {
            Message.RemoveListener<InGame.Event.MissionValueUpdate>(MissionDataUpdate);
        }
       
        void PushRewardBtn()
        {
            Data_Mission.GuideMission currentguidemission = Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission;
            if (currentguidemission.IsComplete())
            {
                CurrencyType currency = CurrencyType.Gem;
                if (currentguidemission.baseInfo.reward_type == RewardType.REWARD_BOX)
                {
                    currency = Common.InGameManager.Instance.GetPlayerData.rewardinfo.RewardtypeToCurrencyType(currentguidemission.baseInfo.reward_type,
                        currentguidemission.baseInfo.reward_count);
                }
                else
                {
                    currency = Common.InGameManager.Instance.GetPlayerData.rewardinfo.RewardtypeToCurrencyType(currentguidemission.baseInfo.reward_type);
                }

                Common.InGameManager.Instance.GetPlayerData.AddCurrency(currency, currentguidemission.baseInfo.reward_count);
                currentguidemission.CompleteMissionData();
                currentguidemission = Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission;
                LocalValue _localdata = InGameDataTableManager.LocalizationList.mission.Find(o => o.id == currentguidemission.baseInfo.name);
                MissionName.text = _localdata.kr;

                ValueAmountText.text = string.Format("{0}/{1}", currentguidemission.curCount,
                    currentguidemission.GetMaxCount());

                LocalValue rewardtype = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == currentguidemission.baseInfo.reward_type.ToString());
                RewardText.text = string.Format("보상 : <color=yellow>{0} {1}</color>", rewardtype.GetStringForLocal(true), currentguidemission.baseInfo.reward_count);
                Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.GuideQuest, false));
                MissionCheck();
            }
        } 

        void MissionDataUpdate(InGame.Event.MissionValueUpdate msg)
        {
            if (msg.missiontype != Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.GetMissionType())
                return;
            MissionCheck();
        }
        void MissionCheck()
        {
            if(Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.GetMissionType()==MissionType.CHAPTER_1_STAGE)
            {
                Model.PlayerDataModel playerData = Common.InGameManager.Instance.GetPlayerData;
                int TotalStageCount = (playerData.stage_Info.BestScenario - 1) * 1000 + (playerData.stage_Info.Bestchapter - 1) * 100 + playerData.stage_Info.Stage;
                Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.SetMissionValue(MissionType.CHAPTER_1_STAGE, TotalStageCount,false);
            }
            if(Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.GetMissionType()==MissionType.RELIC_COUNT)
            {
                int count=(int)Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo._playingRecord.GetMissionValue(MissionType.RELIC_COUNT);
                Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.SetMissionValue(MissionType.RELIC_COUNT, count,false);
            }

            ValueAmountText.text = string.Format("{0}/{1}", Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.curCount,
     Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.GetMaxCount());

            if (Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.CurrentGuideMission.IsComplete())
            {
                Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.GuideQuest, true));
            }
            else
            {
                Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.GuideQuest, false));
            }
        }
    }

}
