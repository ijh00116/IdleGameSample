using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class PetDungeonResultInfo : MonoBehaviour
    {
        [SerializeField] GameObject backBtn;
        [Header("자동시작,닫기버튼")]
        [SerializeField] Button AutoRestart;
        [SerializeField] Button AutoNextStart;
        [SerializeField] GameObject NextOn;
        [SerializeField] GameObject NextOff;
        [SerializeField] GameObject CurrentOn;
        [SerializeField] GameObject CurrentOff;
        [SerializeField] Button BackButton;
        [SerializeField] Text TimeText;

        [Header("스테이지")]
        [SerializeField] Button StartStage;
        [SerializeField] Button NextStage;
        [SerializeField] Button PrevStage;
        [SerializeField] Text Chapter_Stage;

        [SerializeField] PetRewardSlotUI RewardPrefab;
        [SerializeField] GameObject RewardListParent;

        List<PetRewardSlotUI> rewardPetList = new List<PetRewardSlotUI>();
        List<PetRewardSlotUI> rewardTempList=new List<PetRewardSlotUI>();
        float currentTime;
        const float NextDungeonTime = 3.0f;

        bool StartingCountActive;

        int SelectedChapter = 1;
        int SelectedStage = 1;

        PetDungeonWindowUI petdungeonUI;
        public void Init(PetDungeonWindowUI petdungeonui)
        {
            petdungeonUI = petdungeonui;
            AutoRestart.onClick.AddListener(AutoActivate);
            AutoNextStart.onClick.AddListener(NextPetDungeonActivate);
            BackButton.onClick.AddListener(CloseResultWindow);

            AutoRestart.transform.GetChild(0).gameObject.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart);
            NextOn.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            NextOff.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOn.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOff.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);

            NextStage.onClick.AddListener(TouchStageNextButton);
            PrevStage.onClick.AddListener(TouchStagePrevButton);

            StartingCountActive = false;

            SlotSetting();
        }

        public void SlotSetting()
        {
            for(int i=0; i< InGameManager.Instance.petInventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(RewardPrefab);
                obj.transform.SetParent(RewardListParent.transform, false);
                obj.slot = InGameManager.Instance.petInventory.GetSlots[i];
                obj.Initialize();
                rewardPetList.Add(obj);
            }
        }
        public void PopupResult()
        {
            currentTime = NextDungeonTime;

            List<int> rewardpetidList = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeonRewardPetList;
            Dictionary<int, int> rewardpetdic = new Dictionary<int, int>();
            for(int i=0; i< rewardpetidList.Count; i++)
            {
                if(rewardpetdic.ContainsKey(rewardpetidList[i]))
                {
                    rewardpetdic[rewardpetidList[i]]+=1;
                }
                else
                {
                    rewardpetdic.Add(rewardpetidList[i], 1);
                }
            }
            foreach(var _data in rewardpetdic)
            {
                PetRewardSlotUI slotui = this.rewardPetList.Find(o => o.slot.pet.idx == _data.Key);
                if (slotui != null)
                {
                    InGameManager.Instance.petInventory.AddAmount(slotui.slot.pet, _data.Value);
                    slotui.Setting(_data.Value,false);
                }
            }

            List<int> rewardItemList =InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeonRewardItemList;

            if (InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage % 2 == 0)
            {
                for(int i=0; i<rewardItemList.Count; i++)
                {
                    InventorySlot slot = InGameManager.Instance.WingInventory.GetSlots.Find(o => o.item.idx == rewardItemList[i]);
                    slot.AddAmount(1);
                    var obj = Instantiate(RewardPrefab);
                    obj.Setting(1, false);
                    obj.itemimage.sprite = slot.display.currentSpriteImage;
                    obj.transform.SetParent(RewardListParent.transform, false);
                    rewardTempList.Add(obj);
                }
            }
            else
            {
                for (int i = 0; i < rewardItemList.Count; i++)
                {
                    SRelicInventorySlot slot = InGameManager.Instance.SRelicInventory.GetSlots.Find(o => o.srelic.idx == rewardItemList[i]);
                    slot.AddAmount(1);
                    var obj = Instantiate(RewardPrefab);
                    obj.Setting(1, false);
                    obj.itemimage.sprite = slot.slotDisplay.currentSpriteImage;
                    obj.transform.SetParent(RewardListParent.transform, false);
                    rewardTempList.Add(obj);
                }
            }

            AutoRestart.transform.GetChild(0).gameObject.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart);
            NextOn.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            NextOff.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOn.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOff.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);

            SelectedChapter = InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentChapter;
            SelectedStage = InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage;

            if (InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart)
                TouchStageNextButton();

         
            Chapter_Stage.text = string.Format("던전레벨 {0} - {1}", SelectedChapter, SelectedStage);
        }

        private void Update()
        {
            if(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart)
            {
                if(StartingCountActive==false)
                    StartingCountActive = true;

                currentTime -= Time.deltaTime;
                if(currentTime<0)
                {
                    //던전 자동 시작
                    
                    CloseResultWindow();
                    petdungeonUI.StartCombatPetDungeon();
                }
                if (TimeText.gameObject.activeInHierarchy==false)
                    TimeText.gameObject.SetActive(true);
                TimeText.text = string.Format("{0:F0}초 후 자동 시작됩니다.", currentTime);
            }
            else
            {
                if (TimeText.gameObject.activeInHierarchy)
                    TimeText.gameObject.SetActive(false);
            }
        }

        void AutoActivate()
        {
            currentTime = NextDungeonTime;
            InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart = !InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart;
            AutoRestart.transform.GetChild(0).gameObject.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart);
        }

        void NextPetDungeonActivate()
        {
            currentTime = NextDungeonTime;
            InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart = !InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart;
            NextOn.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            NextOff.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOn.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOff.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
        }

        void CloseResultWindow()
        {
            backBtn.SetActive(true);
            currentTime = NextDungeonTime;
            StartingCountActive = false;
            for(int i=0; i< rewardPetList.Count; i++)
            {
                rewardPetList[i].Initialize();
            }
            foreach(var _date in rewardTempList)
            {
                Destroy(_date.gameObject);
            }
            rewardTempList.Clear();
            this.gameObject.SetActive(false);

        }

        void TouchStageNextButton()
        {
            currentTime = NextDungeonTime;
            if (SelectedChapter >= InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestChapter + 1)
                return;
            if (SelectedStage >= InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage + 1)
                return;
            SelectedStage++;

            if (SelectedStage > 50)
            {
                SelectedChapter++;
                SelectedStage = 1;
            }

            Chapter_Stage.text = string.Format("던전레벨 {0} - {1}", SelectedChapter, SelectedStage);

            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentChapter=SelectedChapter;
            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage= SelectedStage;
        }
        void TouchStagePrevButton()
        {
            currentTime = NextDungeonTime;
            if (SelectedChapter == 1)
            {
                SelectedStage = 1;
            }
            else
            {
                SelectedChapter--;
                SelectedStage = 50;
            }
            Chapter_Stage.text = string.Format("던전레벨 {0} - {1}", SelectedChapter, SelectedStage);

            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentChapter = SelectedChapter;
            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage = SelectedStage;
        }
    }

}
