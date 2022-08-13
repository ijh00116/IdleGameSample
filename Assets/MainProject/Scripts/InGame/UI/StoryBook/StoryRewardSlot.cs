using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class StoryRewardSlot : MonoBehaviour
    {
        [SerializeField] Button SeeStoryBtn;
        [SerializeField] Button SeeStoryAgainImage;
        [SerializeField] Image ClearImage;
        [SerializeField] Image RewardIcon;
        [SerializeField] Text RewardCount;
        [SerializeField] Text ChapterNumber;
        [SerializeField] Text ChapterName;

        [HideInInspector] public StoryBookInterface storybookinterface;
        
        StoryBookMain storyslotdata;
        StoryHistory history;
        public void Init(StoryBookInterface _interface,StoryBookMain storyRewarddata)
        {
            storybookinterface = _interface;
            storyslotdata = storyRewarddata;
            SeeStoryBtn.onClick.AddListener(ShowStoryDialog);

            history = Common.InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.storyhistoryInfo.Find(o => o.idx == storyslotdata.idx);

            SeeStoryBtn.gameObject.SetActive(!history.AlreadySeen);
            SeeStoryAgainImage.gameObject.SetActive(history.AlreadySeen);
            ClearImage.gameObject.SetActive(history.AlreadySeen);

            RewardCount.text = string.Format("{0}", storyslotdata.reward_gem);

            LocalValue _chapternumber= InGameDataTableManager.LocalizationList.story.Find(o => o.id == storyslotdata.name);
            ChapterNumber.text= string.Format("{0}", _chapternumber.kr);
            LocalValue _chaptername = InGameDataTableManager.LocalizationList.story.Find(o => o.id == storyslotdata.desc);
            ChapterName.text = string.Format("{0}", _chaptername.kr);
        }

        void ShowStoryDialog()
        {
            storybookinterface.MainStoryRewardWindow.gameObject.SetActive(false);
            storybookinterface.MainStoryDialogWindow.gameObject.SetActive(true);
            storybookinterface.MainStoryDialogWindow.PopupStoryDialog(storyslotdata.idx);
        }
    }

}
