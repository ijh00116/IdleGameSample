using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree.UI
{
    public enum BottomDialogType
    {
        Dungeon,
        Character,
        Relic,
        SRelic,
        Pet,
        Item,
        Shop,
        Chat,
    }
    public class MainDialog : IDialog
    {
        [Header("하단 버튼")]
        public Button CowButton;
        public Button CharacterButton;
        public Button PetButton;
        public Button EquipButton;
        public Button SArtifactButton;
        public Button ArtifactButton;
        public Button ShopButton;

        public GameObject CowButtonOn;
        public GameObject CharacterButtonOn;
        public GameObject PetButtonOn;
        public GameObject EquipButtonOn;
        public GameObject SArtifactButtonOn;
        public GameObject ArtifactButtonOn;
        public GameObject ShopButtonOn;

        [SerializeField]
        MainDialogButtonText buttonText;

        [Header("업그레이드 UI")]
        public GameObject ContentsWindow;
        public GameObject CharacterWindow;
        public GameObject PetWindow;
        public GameObject EquipWindow;
        public GameObject SArtifactWindow;
        public GameObject ArtifactWindow;
        public GameObject ShopWindow;

        [Header("윈도우 내부 시스템")]
        public CharacterStatUI characterStat;
        public EnforcementUI enforceUI;

        public PetInterface petinterface;

       
        [Header("채팅 오브젝트")]
        public Button ChatButton;

        [Header("boss이벤트")]
        public GameObject BossHpobj;
        public Slider BossHPSlider;
        public Slider TimeSlider;
        bool InBossEvent;
        protected override void OnEnter()
        {
            CowButton.onClick.AddListener(() => { PopupWindow(ContentsWindow, BottomDialogType.Dungeon); });
            CharacterButton.onClick.AddListener(() => { PopupWindow(CharacterWindow, BottomDialogType.Character); });
            PetButton.onClick.AddListener(() => { PopupWindow(PetWindow, BottomDialogType.Pet); });
            EquipButton.onClick.AddListener(() => { PopupWindow(EquipWindow, BottomDialogType.Item);  });
            SArtifactButton.onClick.AddListener(() => { PopupWindow(SArtifactWindow, BottomDialogType.SRelic); });
            ArtifactButton.onClick.AddListener(() => { PopupWindow(ArtifactWindow, BottomDialogType.Relic); });
            ShopButton.onClick.AddListener(() => { PopupWindow(ShopWindow, BottomDialogType.Shop); });

            ContentsWindow.SetActive(true);
            CharacterWindow.SetActive(true);
            PetWindow.SetActive(true);
            EquipWindow.SetActive(true);
            SArtifactWindow.SetActive(true);
            ArtifactWindow.SetActive(true);
            ShopWindow.SetActive(false);
            //chatController.gameObject.SetActive(true);

            ArtifactWindow.GetComponent<RelicCategoryUI>().Init();

            characterStat.Init();
            enforceUI.Init();

            petinterface.Init();
            
            //chatController.Initialize();
            ContentsWindow.GetComponent<ContentTitle>().Init();
            ShopWindow.GetComponent<ShopUI>().Init();

            //처음 시작시 영웅탭 활성화
            PopupWindow(CharacterWindow, BottomDialogType.Character);

            Message.AddListener<UI.Event.SideWindowPopup>(OpenPetDungeon);
            Message.AddListener<UI.Event.DungeonStart>(StartDungeon);
            Message.AddListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);


            BossHpobj.SetActive(false);
            InBossEvent = false;
            
            Message.AddListener<InGame.Event.EnemyKilled>(EnemyKilled);
            Message.AddListener<UI.Event.MainSceneBosshpPopupEvent>(BossHpPopup);
            buttonText.Init();

            SArtifactWindow.GetComponent<SRelicCategoryUI>().Init();
        }

        protected override void OnExit()
        {
            CowButton.onClick.RemoveAllListeners();
            CharacterButton.onClick.RemoveAllListeners();
            PetButton.onClick.RemoveAllListeners();
            EquipButton.onClick.RemoveAllListeners();
            SArtifactButton.onClick.RemoveAllListeners();
            ArtifactButton.onClick.RemoveAllListeners();
            ShopButton.onClick.RemoveAllListeners();

            ArtifactWindow.GetComponent<RelicCategoryUI>().Release();

            characterStat.Release();
            enforceUI.Release();
            petinterface.Release();

            ContentsWindow.GetComponent<ContentTitle>().Release();
            ShopWindow.GetComponent<ShopUI>().Release();
            Message.RemoveListener<UI.Event.SideWindowPopup>(OpenPetDungeon);

            Message.RemoveListener<UI.Event.DungeonStart>(StartDungeon);
            Message.RemoveListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);

            Message.RemoveListener<InGame.Event.EnemyKilled>(EnemyKilled);
            Message.RemoveListener<UI.Event.MainSceneBosshpPopupEvent>(BossHpPopup);
            buttonText.Release();
            SArtifactWindow.GetComponent<SRelicCategoryUI>().Release();
        }
        float BossCurrentTime;
        private void Update()
        {
            if (InBossEvent)
            {
                BossCurrentTime -= Time.deltaTime;
                GameObject Enemyobj = Common.InGameManager.Instance.mainplayerCharacter.CurrentEnemy;
                if (Enemyobj.GetComponent<InGame.Character>().playertype == InGame.CharacterType.Boss)
                {
                    BossHPSlider.value = Common.InGameManager.Instance.mainplayerCharacter.CurrentEnemyCharacter._health.Rate;
                    if (BossHPSlider.value <= 0)
                    {
                        BossHpobj.SetActive(false);
                        InBossEvent = false;
                    }
                }

                TimeSlider.value = (float)(BossCurrentTime / (float)DTConstraintsData.BATTLE_STAGE_BOSS_TIME_LIMIT);
                if(BossCurrentTime<0)
                {
                    InGameManager.Instance.GetPlayerData.stage_Info.Stage--;
                    if(InGameManager.Instance.GetPlayerData.stage_Info.Stage<=0)
                    {
                        InGameManager.Instance.GetPlayerData.stage_Info.chapter--;
                        InGameManager.Instance.GetPlayerData.stage_Info.Stage = 100;
                        if(InGameManager.Instance.GetPlayerData.stage_Info.chapter<=0)
                        {
                            InGameManager.Instance.GetPlayerData.stage_Info.scenario--;
                            InGameManager.Instance.GetPlayerData.stage_Info.chapter = 10;
                            if(InGameManager.Instance.GetPlayerData.stage_Info.scenario<=0)
                            {
                                InGameManager.Instance.GetPlayerData.stage_Info.scenario = 1;
                                InGameManager.Instance.GetPlayerData.stage_Info.chapter = 1;
                                InGameManager.Instance.GetPlayerData.stage_Info.Stage = 1;
                            }
                        }
                    }
                    LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_214");
                    Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(lv.GetStringForLocal(true)));
                    BossHpobj.SetActive(false);
                    InBossEvent = false;
                    InGameManager.Instance._sceneFsm._State.ChangeState(ePlayScene.MainInit);
                }
            }
            if(Common.InGameManager.Instance._sceneFsm._State.IsCurrentState(ePlayScene.MainUpdate)==false)
            {
                BossHpobj.SetActive(false);
                InBossEvent = false;
            }
        }

        WaitForSeconds WaitForBossEvent = new WaitForSeconds(1.0f);

        void BossHpPopup(UI.Event.MainSceneBosshpPopupEvent msg)
        {
            BossHpobj.SetActive(true);
            InBossEvent = true;
            BossCurrentTime = DTConstraintsData.BATTLE_STAGE_BOSS_TIME_LIMIT;
        }
     
        void EnemyKilled(InGame.Event.EnemyKilled msg)
        {
            if(msg.charactertype==InGame.CharacterType.Boss)
            {
                BossHpobj.SetActive(false);
            }
        }

    
        void SetLastChild(GameObject myWindow)
        {
            myWindow.transform.SetAsLastSibling();
            Message.Send<UI.Event.OtherUIPopup>(new UI.Event.OtherUIPopup(myWindow));
        }


        UI.Event.CurrencyChange currencyupdate = new Event.CurrencyChange();
        void PopupWindow(GameObject obj,BottomDialogType _type)
        {
            CowButtonOn.SetActive(false);
            CharacterButtonOn.SetActive(false);
            PetButtonOn.SetActive(false);
            EquipButtonOn.SetActive(false);
            SArtifactButtonOn.SetActive(false);
            ArtifactButtonOn.SetActive(false);
            ShopButtonOn.SetActive(false);
            ShopWindow.SetActive(false);
            switch (_type)
            {
                case BottomDialogType.Dungeon:
                    CowButtonOn.SetActive(true);
                    break;
                case BottomDialogType.Character:
                    CharacterButtonOn.SetActive(true);
                    break;
                case BottomDialogType.Relic:
                    ArtifactButtonOn.SetActive(true);
                    break;
                case BottomDialogType.SRelic:
                    SArtifactButtonOn.SetActive(true);
                    break;
                case BottomDialogType.Pet:
                    PetButtonOn.SetActive(true);
                    break;
                case BottomDialogType.Item:
                    EquipButtonOn.SetActive(true);
                    break;
                case BottomDialogType.Shop:
                    ShopButtonOn.SetActive(true);
                    ShopWindow.SetActive(true);
                    break;
                default:
                    break;
            }
            SetLastChild(obj);
            Common.InGameManager.Instance.BottomDialogtype = _type;
            currencyupdate.CurrencyTypeSummarize = false;
        }

        void OpenPetDungeon(UI.Event.SideWindowPopup msg)
        {
            if(msg.type==SideButtonType.QuickPetButton)
                PopupWindow(ContentsWindow, BottomDialogType.Dungeon);
        }

        void StartDungeon(UI.Event.DungeonStart msg)
        {
            DialogView.SetActive(false);
        }

        void ReturnToMainFromDungeon(UI.Event.DungeonEndStartMain msg)
        {
            DialogView.SetActive(true);
        }
    }

}
