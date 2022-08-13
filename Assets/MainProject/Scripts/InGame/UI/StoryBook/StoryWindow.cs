using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class StoryWindow : MonoBehaviour
    {
        [HideInInspector] public StoryBookInterface storybookinterface;
        [SerializeField] Button MainStoryButton;

        [SerializeField] Button BackBtn;
        public void Init(StoryBookInterface _interface)
        {
            storybookinterface = _interface;
            MainStoryButton.onClick.AddListener(PopupStoryRewardWindow);
            BackBtn.onClick.AddListener(BackToStage);
        }

        public void PopupStoryRewardWindow()
        {
            this.gameObject.SetActive(false);
            storybookinterface.MainStoryRewardWindow.gameObject.SetActive(true);
        }

        public void BackToStage()
        {
            this.gameObject.SetActive(false);
            storybookinterface.userinfoUI.gameObject.SetActive(true);
        }
    }

}
