using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class PetDungeonWindowUI : MonoBehaviour
    {
        public Text Chapter_Stage;

        public Button StartButton;
        [SerializeField] GameObject backBtn;

        public GameObject CountDownObj;
        public Text CountDownButton;

        public Button PetQuickStart;
        public GameObject QuickStartOndarkImage;
        public GameObject QuickStartOffdarkImage;

        [Header("스테이지 바꾸기")]
        public Button NextStage;
        public Button PrevStage;
        public Text ItemFarmingInfo;
        public Text ItemFarmingrateInfo;

        [Header("재시작버튼")]
        [SerializeField] Button AutoRestart;
        [SerializeField] Button AutoNextStart;
        [SerializeField] GameObject NextOn;
        [SerializeField] GameObject NextOff;
        [SerializeField] GameObject CurrentOn;
        [SerializeField] GameObject CurrentOff;

        [Header("던전 진행시간")]
        [SerializeField] GameObject TimeObj;
        [SerializeField] Text DungeonTimetxt;

        public GameObject ResultWindow;
        public GameObject StartWindow;

        int SelectedChapter=1;
        int SelectedStage=1;

        [SerializeField] PetDungeonResultInfo resultInfo;

        const float StartTime = 3.0f;
        float StartingCurrentTime;

        [SerializeField] Text DungeonTicketText;

        [SerializeField] Button ButtonHider;

        [SerializeField] Button SaveEnergyBtn;

        PetDungeonPrime CurrentPetDungeondata;
        public void Init()
        {
            StartButton.onClick.AddListener(StartCombatPetDungeon);
            NextStage.onClick.AddListener(TouchStageNextButton);
            PrevStage.onClick.AddListener(TouchStagePrevButton);

            SelectedChapter = InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestChapter;
            SelectedStage = InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage;

            if(SelectedChapter >= 21)
            {
                InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestChapter = 20;
                InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage = 50;
                SelectedChapter = InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestChapter;
                SelectedStage = InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage;
            }
            Message.AddListener<UI.Event.PetDungeonEnd>(PetCombatResultPopup);


            ResultWindow.SetActive(false);
            StartWindow.SetActive(true);

            Chapter_Stage.text = string.Format("던전레벨\n<color=yellow>{0}-{1}</color>", SelectedChapter, SelectedStage);

            PetDungeonPrime petinfo = InGameDataTableManager.DungeonTableList.Dungeon_pet.Find(o => o.pet_chapter == SelectedChapter);
            CurrentPetDungeondata = petinfo;
            float rate = petinfo.reward_item_rate + (petinfo.reward_item_rate_stage * (SelectedStage - 1));
            ItemFarmingrateInfo.text = string.Format("{0:N2}%", rate);
            
            if(SelectedStage%2==0)
            {
                LocalValue lv= InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_216");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }
            else
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_215");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }

            AutoRestart.transform.GetChild(0).gameObject.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart);
           
            NextOn.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            NextOff.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOn.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOff.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);

            AutoRestart.onClick.AddListener(PushAutoRestart);
            AutoNextStart.onClick.AddListener(PushAutoNextStart);

            TimeObj.SetActive(false);

            resultInfo.Init(this);

            Message.AddListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);

            PetQuickStart.onClick.AddListener(PetDungeonQuickStartOnOff);

            if (InGameManager.Instance.GetPlayerData.GlobalUser.PetQuickStartOn)
            {
                QuickStartOndarkImage.SetActive(false);
                QuickStartOffdarkImage.SetActive(true);
            }
            DungeonTicketText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_pet).value.ToDisplay();

            ButtonHider.gameObject.SetActive(false);
            ButtonHider.onClick.AddListener(PopupmsgIFPetFight);
            SaveEnergyBtn.onClick.AddListener(PopupSavingBatteryWindow);
        }
        public void Release()
        {
            Message.RemoveListener<UI.Event.PetDungeonEnd>(PetCombatResultPopup);
            AutoRestart.onClick.RemoveAllListeners();
            AutoNextStart.onClick.RemoveAllListeners();
            Message.RemoveListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
            PetQuickStart.onClick.RemoveListener(PetDungeonQuickStartOnOff);
            ButtonHider.onClick.RemoveAllListeners();
            SaveEnergyBtn.onClick.RemoveAllListeners();
        }

        private void Update()
        {
           
        }

        void TouchStageNextButton()
        {
            if (SelectedChapter >= InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestChapter + 1)
                return;
            if (SelectedStage >= InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage + 1)
                return;

            SelectedStage++;

            if(SelectedStage>50)
            {
                SelectedChapter++;
                SelectedStage = 1;
            }
            if(SelectedChapter>20)
            {
                SelectedChapter = 20;
                SelectedStage = 50;
            }
            Chapter_Stage.text = string.Format("던전레벨\n{0} - {1}", SelectedChapter, SelectedStage);

            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentChapter = SelectedChapter;
            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage = SelectedStage;
            PetDungeonPrime petinfo = InGameDataTableManager.DungeonTableList.Dungeon_pet.Find(o => o.pet_chapter == SelectedChapter);
            CurrentPetDungeondata = petinfo;
            float rate = petinfo.reward_item_rate + (petinfo.reward_item_rate_stage * (SelectedStage - 1));
            ItemFarmingrateInfo.text = string.Format("{0:N2}%", rate);

            if (SelectedStage % 2 == 0)
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_216");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }
            else
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_215");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }
        }
        void TouchStagePrevButton()
        {
            if(SelectedChapter>1)
            {
                if(SelectedStage==1)
                {
                    SelectedStage = 50;
                    SelectedChapter--;
                }
                else
                {
                    SelectedStage--;
                }
            }
            else
            {
                if (SelectedStage > 1)
                {
                    SelectedStage--;
                }
            }
            Chapter_Stage.text = string.Format("던전레벨\n{0} - {1}", SelectedChapter, SelectedStage);

            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentChapter = SelectedChapter;
            InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage = SelectedStage;
            PetDungeonPrime petinfo = InGameDataTableManager.DungeonTableList.Dungeon_pet.Find(o => o.pet_chapter == SelectedChapter);
            CurrentPetDungeondata = petinfo;
            float rate = petinfo.reward_item_rate + (petinfo.reward_item_rate_stage * (SelectedStage - 1));
            ItemFarmingrateInfo.text = string.Format("{0:N2}%", rate);

            if (SelectedStage % 2 == 0)
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_216");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }
            else
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_215");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }
        }

        void PushAutoRestart()
        {
            InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart = !InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart;
            AutoRestart.transform.GetChild(0).gameObject.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart);
        }

        void PushAutoNextStart()
        {
            InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart = !InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart;
            NextOn.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            NextOff.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOn.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOff.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
        }

        public void PopupMyWindow()
        {
            backBtn.gameObject.SetActive(true);
            StartButton.gameObject.SetActive(true);
            CountDownObj.SetActive(false);
        }


        public void StartCombatPetDungeon()
        {
            ButtonHider.gameObject.SetActive(true);
            StartButton.gameObject.SetActive(false);
            backBtn.SetActive(false);

            CountDownObj.SetActive(true);

            //결과창에서 PetDungeonAutoNextStart 해준거때매 여기서도 setactive 해주어야 함
            AutoRestart.transform.GetChild(0).gameObject.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoRestart);
            NextOn.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            NextOff.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOn.SetActive(!InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);
            CurrentOff.SetActive(InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart);

            SelectedChapter = InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentChapter;
            SelectedStage = InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage;

            Chapter_Stage.text = string.Format("던전레벨\n{0} - {1}", SelectedChapter, SelectedStage);

            PetDungeonPrime petinfo = InGameDataTableManager.DungeonTableList.Dungeon_pet.Find(o => o.pet_chapter == SelectedChapter);
            CurrentPetDungeondata = petinfo;
            float rate = petinfo.reward_item_rate + (petinfo.reward_item_rate_stage * (SelectedStage - 1));
            ItemFarmingrateInfo.text = string.Format("{0:N2}%", rate);

            if (SelectedStage % 2 == 0)
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_216");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }
            else
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_215");
                ItemFarmingInfo.text = lv.GetStringForLocal(true);
            }

            StartButton.enabled = false;
            StartingCurrentTime = StartTime;
            StartCoroutine(PetCombatstart());

            InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.PET_BATTLE, 1);
        }

        IEnumerator PetCombatstart()
        {
            //while(StartingCurrentTime>0)
            //{
            //    StartingCurrentTime -= Time.deltaTime;
            //    CountDownButton.text = string.Format("{0}", (int)StartingCurrentTime);
            //    yield return null;
            //}
            InGameManager.Instance.StartPetDungeon(SelectedChapter, SelectedStage);
            StartButton.gameObject.SetActive(false);
            CountDownObj.SetActive(false);
            TimeObj.SetActive(true);
            while (InGameManager.Instance._PetsceneFsm.PetDungencurrentTime<=10)
            {
                int lefttime = 10 - (int)InGameManager.Instance._PetsceneFsm.PetDungencurrentTime;
                int m = lefttime / 60;
                int s = lefttime % 60;

                DungeonTimetxt.text = string.Format("{0:D2}:{1:D2}", m, s);

                yield return null;
            }
            
            yield break;
        }

        void PetCombatResultPopup(UI.Event.PetDungeonEnd msg)
        {
            if (Common.InGameManager.Instance.GetPlayerData.stage_Info.PetCurrentKillCount<=0)
            {
                TouchStagePrevButton();
                InGameManager.Instance.GetPlayerData.stage_Info.PetDungeonAutoNextStart = false;
            }
                

            TimeObj.SetActive(false);
            ResultWindow.SetActive(true);
            resultInfo.PopupResult();
            StartButton.gameObject.SetActive(true);
            StartButton.enabled = true;
            //CountDownButton.text = "구출하기";

            StartButton.gameObject.SetActive(true);
            CountDownObj.SetActive(false);

            ButtonHider.gameObject.SetActive(false);
        }

        void GetCurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.Type == CurrencyType.Ticket_pet)
            {
                DungeonTicketText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_pet).value.ToDisplay();
            }
        }

        void PetDungeonQuickStartOnOff()
        {
            InGameManager.Instance.GetPlayerData.GlobalUser.PetQuickStartOn = !InGameManager.Instance.GetPlayerData.GlobalUser.PetQuickStartOn;
            bool on = InGameManager.Instance.GetPlayerData.GlobalUser.PetQuickStartOn;
            Message.Send<UI.Event.PetQuickStartButtonOnOff>(new UI.Event.PetQuickStartButtonOnOff(on));
                QuickStartOndarkImage.SetActive(!on);
                QuickStartOffdarkImage.SetActive(on);
        }

        void PopupmsgIFPetFight()
        {
            Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup("펫던전 진행중에는 할 수 없습니다."));
        }

        void PopupSavingBatteryWindow()
        {
            Application.targetFrameRate = 10;
            Message.Send<UI.Event.SavingBatteryMessage>(new UI.Event.SavingBatteryMessage(true));
        }
    }

}