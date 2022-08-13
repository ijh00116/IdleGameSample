using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BlackTree
{
    public class MainstoryDialogWindow : MonoBehaviour
    {
        [HideInInspector] public StoryBookInterface storybookinterface;

        [SerializeField] Text title_Name;
        [SerializeField] Image LeftCharacter;
        [SerializeField] Image RightCharacter;

        [SerializeField] Text Cha_name;
        [SerializeField] Text Script;

        [SerializeField] Button DialogNextBtn;
        [SerializeField] Button BackBtn;

        int currentscriptstepindex;
        StoryBookScript currentstory;
        List<StoryBookScript> storyDialogList;

        
        public void Init(StoryBookInterface _interface)
        {
            DialogNextBtn.onClick.AddListener(dialogButtonPush);
            BackBtn.onClick.AddListener(BacktoStoryRewardWindow);
            storybookinterface = _interface;
        }

        public void PopupStoryDialog(int storyidx)
        {
            BackBtn.gameObject.SetActive(false);
            storyDialogList = InGameDataTableManager.StoryTableList.script.FindAll(o => o.idx == storyidx).OrderBy(o=>o.step).ToList();

            currentscriptstepindex = 0;
            currentstory = storyDialogList[currentscriptstepindex];
            LocalValue titlename = InGameDataTableManager.LocalizationList.story.Find(o => o.id == currentstory.title_name);
            title_Name.text = string.Format("{0}", titlename.kr);

            if(currentstory.cha_position=="CHA_LEFT")
            {
                LeftCharacter.color = new Color(1,1,1,1.0f);
                RightCharacter.color = new Color(1, 1, 1, 0.6f);
            }
            else
            {
                LeftCharacter.color = new Color(1, 1, 1, 0.6f);
                RightCharacter.color = new Color(1, 1, 1, 1.0f);
            }

            LocalValue chaname = InGameDataTableManager.LocalizationList.story.Find(o => o.id == currentstory.cha_name);
            Cha_name.text = string.Format("{0}", chaname.kr);

            LocalValue scriptdata = InGameDataTableManager.LocalizationList.story.Find(o => o.id == currentstory.script);
            Script.text = string.Format("{0}", scriptdata.kr);
        }
        void dialogButtonPush()
        {
            currentscriptstepindex++;
            if(currentscriptstepindex>= storyDialogList.Count)
            {
                BackBtn.gameObject.SetActive(true);
                return;
            }
            currentstory = storyDialogList[currentscriptstepindex];
            if (currentstory.cha_position == "CHA_LEFT")
            {
                LeftCharacter.color = new Color(1, 1, 1, 1.0f);
                RightCharacter.color = new Color(1, 1, 1, 0.6f);
            }
            else
            {
                LeftCharacter.color = new Color(1, 1, 1, 0.6f);
                RightCharacter.color = new Color(1, 1, 1, 1.0f);
            }

            LocalValue chaname = InGameDataTableManager.LocalizationList.story.Find(o => o.id == currentstory.cha_name);
            Cha_name.text = string.Format("{0}", chaname.kr);

            LocalValue scriptdata = InGameDataTableManager.LocalizationList.story.Find(o => o.id == currentstory.script);
            Script.text = string.Format("{0}", scriptdata.kr);
        }

        void BacktoStoryRewardWindow()
        {
            this.gameObject.SetActive(false);
            storybookinterface.MainStoryRewardWindow.gameObject.SetActive(true);
        }
    }

}
