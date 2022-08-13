using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class UserInfoUI : MonoBehaviour
    {
        //상단
        [Header("상단")]
        [SerializeField] Text Level;
        [SerializeField] Text nickName;
        [SerializeField] Text UserId;
        [SerializeField] Button StoryButton;

        [HideInInspector]public StoryBookInterface storybookinterface;
        [SerializeField] List<ScenarioButtonUI> ScenarioButtonList = new List<ScenarioButtonUI>();
        [SerializeField] List<ChapterButtonUI> ChapterEarnGemButtonList = new List<ChapterButtonUI>();
        [Header("센터 시나리오 이름 및 화살표")]
        [SerializeField] Button NextScenario;
        [SerializeField] Button PrevScenario;
        [SerializeField] Text Scenario;
        int CurrentScenarioIndex;

        List<int> ScenarioIndexList = new List<int>();
        StageInfo stageInfo;

        [Header("챕터 이동 팝업")]
        [SerializeField] GameObject chapterMoveWindow;
        [SerializeField] Button ConfirmChaptermove;
        [SerializeField] Button CancelChpaterMove;
        [SerializeField] Text ChapterChangeText;
        ChapterReward currentChapterinfo;
        public void Init(StoryBookInterface _interface)
        {
            storybookinterface = _interface;
            StoryButton.onClick.AddListener(PopupStoryWindow);

            stageInfo = InGameManager.Instance.GetPlayerData.stage_Info;
            for (int i=0; i< ScenarioButtonList.Count; i++)
            {
                ScenarioIndexList.Add(i);
                ScenarioButtonList[i].Setting(i,ChapterButtonSet);
            }
            CurrentScenarioIndex = 0;
            ScenarioButtonList[CurrentScenarioIndex].Push();
            NextScenario.onClick.AddListener(NextScenarioPush);
            PrevScenario.onClick.AddListener(PrevScenarioPush);

           // nickName.text = BackendManager.Instance.GetNickName();
           // UserId.text = BackendManager.Instance.authorization.current.serialized;

            CancelChpaterMove.onClick.AddListener(() => chapterMoveWindow.SetActive(false));
            ConfirmChaptermove.onClick.AddListener(ChangeChapterAndRestart);
        }

        void ChapterButtonSet(int scenarioIndex)
        {
            CurrentScenarioIndex = scenarioIndex;
            Scenario.text = string.Format("{0}시나리오",CurrentScenarioIndex+1);
            for (int i = 0; i < ChapterEarnGemButtonList.Count; i++)
            {
                ChapterEarnGemButtonList[i].userinfoUI = this;
                ChapterEarnGemButtonList[i].ChapterInfoSetUp(stageInfo.stagesubinfo.RewardInfo.Find(o=>o.ScenarioIndex==(scenarioIndex)&&o.ChapterIndex==i));
            }
        }

        void NextScenarioPush()
        {
            CurrentScenarioIndex++;
            if(CurrentScenarioIndex>=10)
                CurrentScenarioIndex = 0;
            Scenario.text = string.Format("{0}시나리오", CurrentScenarioIndex+1);
            for (int i = 0; i < ChapterEarnGemButtonList.Count; i++)
            {
                ChapterEarnGemButtonList[i].ChapterInfoSetUp(stageInfo.stagesubinfo.RewardInfo.Find(o => o.ScenarioIndex == (CurrentScenarioIndex) && o.ChapterIndex == i));
            }
        }
        void PrevScenarioPush()
        {
            CurrentScenarioIndex--;
            if (CurrentScenarioIndex < 0)
                CurrentScenarioIndex = 10;
            Scenario.text = string.Format("{0}시나리오", CurrentScenarioIndex+1);
            for (int i = 0; i < ChapterEarnGemButtonList.Count; i++)
            {
                ChapterEarnGemButtonList[i].ChapterInfoSetUp(stageInfo.stagesubinfo.RewardInfo.Find(o => o.ScenarioIndex == (CurrentScenarioIndex) && o.ChapterIndex == i));
            }
        }

        public void Release()
        {

        }

        void PopupStoryWindow()
        {
            Level.text = string.Format("LV.{0}",Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL); 
            this.gameObject.SetActive(false);
            storybookinterface.StoryWindow.gameObject.SetActive(true);
            ChapterButtonSet(Common.InGameManager.Instance.GetPlayerData.stage_Info.BestScenario);
        }

        public void PopupChapterChangeWindow(ChapterReward cr)
        {
            currentChapterinfo = cr;
            chapterMoveWindow.SetActive(true);

            ChapterChangeText.text =
                string.Format("<color=#FDA600>챕터.{0}</color> <color=#D35664>로 이동하시겠습니다?</color>", currentChapterinfo.ChapterIndex+1);
        }

        void ChangeChapterAndRestart()
        {
            if (currentChapterinfo.Rewarded == true)
            {
                InGameManager.Instance.GetPlayerData.stage_Info.scenario = currentChapterinfo.ScenarioIndex + 1;
                InGameManager.Instance.GetPlayerData.stage_Info.chapter = currentChapterinfo.ChapterIndex + 1;
                InGameManager.Instance.GetPlayerData.stage_Info.Stage = 1;
                InGameManager.Instance._sceneFsm._State.ChangeState(ePlayScene.MainInit);

                chapterMoveWindow.SetActive(false);
                storybookinterface.gameObject.SetActive(false);
            }
        }
    }

}
