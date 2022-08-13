using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ChapterButtonUI : MonoBehaviour
    {
        [SerializeField] Text Chapter;
        [SerializeField] Button GetGem;
        [SerializeField] Button ChapterRestart;
        [SerializeField] GameObject ProgressText;
        [SerializeField] GameObject CompleteText;
        [SerializeField] GameObject Locked;

        ChapterReward RewardInfo;

        public UserInfoUI userinfoUI;
        private void Awake()
        {
            GetGem.onClick.AddListener(AddGem);
            ChapterRestart.onClick.AddListener(ChangeChapterAndRestart);
        }

        void AddGem()
        {
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem,100);
            RewardInfo.Rewarded = true;
            GetGem.gameObject.SetActive(!RewardInfo.Rewarded);
            ButtonChange(true);

            InGameManager.Instance.Localdata.SaveData(InGameManager.Instance.GetPlayerData.saveData);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData, InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.RewardInfo);
        }

        void ChangeChapterAndRestart()
        {
            if (RewardInfo.Rewarded == true)
                userinfoUI.PopupChapterChangeWindow(this.RewardInfo);
            //if(RewardInfo.Rewarded==true)
            //{
            //    InGameManager.Instance.GetPlayerData.stage_Info.scenario = RewardInfo.ScenarioIndex + 1;
            //    InGameManager.Instance.GetPlayerData.stage_Info.chapter = RewardInfo.ChapterIndex + 1;
            //    InGameManager.Instance.GetPlayerData.stage_Info.Stage = 1;
            //    InGameManager.Instance._sceneFsm._State.ChangeState(ePlayScene.MainInit);
            //}
        }

        public void ChapterInfoSetUp(ChapterReward _rewardInfo)
        {
            StageInfo stageinfo  = InGameManager.Instance.GetPlayerData.stage_Info;
            RewardInfo = _rewardInfo;
            Chapter.text = string.Format("{0}/{1} 챕터\n1~100", RewardInfo.ScenarioIndex+1, RewardInfo.ChapterIndex+1);

            if (RewardInfo.ScenarioIndex+1 < stageinfo.BestScenario)
            {
                ButtonChange(true);
            }
            else if (RewardInfo.ScenarioIndex+1 == stageinfo.BestScenario)
            {
                if (RewardInfo.ChapterIndex+1<= stageinfo.Bestchapter-1)
                {
                    ButtonChange(true);
                }
                else
                {
                    ButtonChange(false);
                }
            }
            else
            {
                ButtonChange(false);
            }
            
        }

        void ButtonChange(bool chapterArrived)
        {
            //보상 받을수 있음(챕터 완료)
            if(chapterArrived)
            {
                Locked.SetActive(false);
                ProgressText.SetActive(false);
                CompleteText.SetActive(true);
                if (RewardInfo.Rewarded == false)
                {
                    GetGem.gameObject.SetActive(true);
                    ChapterRestart.enabled = false;
                }
                else
                {
                    GetGem.gameObject.SetActive(false);
                    ChapterRestart.enabled = true;
                }
            }
            else
            {
                Locked.SetActive(true);
                GetGem.gameObject.SetActive(false);
                ChapterRestart.enabled = false;
                ProgressText.SetActive(true);
                CompleteText.SetActive(false);
            }
          
        }
    }

}
