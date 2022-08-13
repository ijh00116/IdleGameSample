using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class StoryBookInterface : MonoBehaviour
    {
        [Header("스테이지 목록")]
        [SerializeField] public UserInfoUI userinfoUI;
        [Header("스토리 목록")]
        [SerializeField] public StoryWindow StoryWindow;
        [Header("메인스토리 보상목록")]
        [SerializeField] public MainstoryRewardWindow MainStoryRewardWindow;
        [Header("메인스토리 대화창")]
        [SerializeField] public MainstoryDialogWindow MainStoryDialogWindow;

        [SerializeField] Button CloseBtn;
        public void Init()
        {
            userinfoUI.Init(this);
            StoryWindow.Init(this);
            MainStoryRewardWindow.Init(this);
            MainStoryDialogWindow.Init(this);

            userinfoUI.gameObject.SetActive(true);
            StoryWindow.gameObject.SetActive(false);
            MainStoryRewardWindow.gameObject.SetActive(false);
            MainStoryDialogWindow.gameObject.SetActive(false);

            CloseBtn.onClick.AddListener(() => this.gameObject.SetActive(false));
        }

        public void Release()
        {
            userinfoUI.Release();
        }
       
      
        public void PopupStoryDialogWindow()
        {
            userinfoUI.gameObject.SetActive(false);
            StoryWindow.gameObject.SetActive(false);
            MainStoryRewardWindow.gameObject.SetActive(false);
            MainStoryDialogWindow.gameObject.SetActive(true);
        }
    }
}
