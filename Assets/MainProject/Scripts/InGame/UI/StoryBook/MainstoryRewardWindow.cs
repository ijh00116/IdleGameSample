using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class MainstoryRewardWindow : MonoBehaviour
    {
        [HideInInspector] public StoryBookInterface storybookinterface;
        [SerializeField] StoryRewardSlot RewardSlotPrefab;
        [SerializeField] Transform parent;

        [SerializeField] Button BackBtn;
        public void Init(StoryBookInterface _interface)
        {
            storybookinterface = _interface;

            StoryTable storytable = InGameDataTableManager.StoryTableList;
            for(int i=0; i< storytable.main.Count; i++)
            {
                var _rewardslot = Instantiate(RewardSlotPrefab);
                _rewardslot.transform.SetParent(parent);
                _rewardslot.Init(storybookinterface, storytable.main[i]);
            }

            BackBtn.onClick.AddListener(BackToStage);
        }

        void BackToStage()
        {
            storybookinterface.StoryWindow.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

    }

}
