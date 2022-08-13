using BlackTree.Common;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

namespace BlackTree.UI
{
    public class SideDialog : IDialog
    {
        [Header("아이템 인벤토리")]
        [SerializeField] Button CurrencyInvenCloseBtn;
        [SerializeField] GameObject CurrencyInvenWindow;

        [Header("뽑기 상점")]
        [SerializeField] Button GachaWindowCloseBtn;
        [SerializeField] GameObject GachaWindow;

        [Header("출석 보상")]
        [SerializeField] Button DailyRewardCloseBtn;
        [SerializeField] DailyRewardUI DailyRewardWindow;

        [Header("치트 인벤토리")]
        [SerializeField] Button CheatInvenCloseBtn;
        [SerializeField] GameObject CheatInvenWindow;
        [SerializeField] CurrencyInventory CurrencyInven;
        [SerializeField] CheatInven cheatInven;

        [Header("버프")]
        [SerializeField] BuffInterface buffinven;

        [Header("출석보상 슬롯")]
        public GameObject[] DailyRewardSlot;

        [Header("미션")]
        public MissionUIInterface missionuinterface;
        public Button MissionCloseBtn;
        [Header("roulette")]
        public RouletteController roulettecontroller;
        public Button roulettecloseBtn;

        [Header("배틀패스")]
        public BattlepassInterface battlepassinterface;
        public GameObject BattlePassWindow;
        public Button closebutton;

        [Header("우편함")]
        public MailBoxInterface mailboxInterface;
        public Button mailCloseButton;

        [Header("랭킹")]
        public RankingInterface rankinginterface;
        public Button rankingclose;

        [Header("스토리북")]
        public StoryBookInterface storybookinterface;
        public Button storybookclose;

        [Header("뉴비패키지")]
        public NewbieWindow newbieWindow;
        public Button newbieWindowclose;
        [Header("오프보상")]
        public OfflineReward offlineReward;

        [Header("리뷰")]
        public ReviewWindow Reviewwindow;
        EnemyReward offlineRewardValue;

        [Header("절전모드")]
        [SerializeField] SavingBatteryWindow savingWindow;

        [SerializeField] GameObject testWindow;
        protected override void Awake()
        {
            base.Awake();
            buffinven.Init();

            CurrencyInvenCloseBtn.onClick.AddListener(() => CurrencyInvenWindow.SetActive(false));
            CheatInvenCloseBtn.onClick.AddListener(() => CheatInvenWindow.SetActive(false));
            GachaWindowCloseBtn.onClick.AddListener(() => GachaWindow.SetActive(false));
            DailyRewardCloseBtn.onClick.AddListener(() => DailyRewardWindow.gameObject.SetActive(false));
            roulettecloseBtn.onClick.AddListener(() => roulettecontroller.gameObject.SetActive(false));
            closebutton.onClick.AddListener(() => BattlePassWindow.SetActive(false));
            MissionCloseBtn.onClick.AddListener(() => missionuinterface.gameObject.SetActive(false));
            mailCloseButton.onClick.AddListener(() => mailboxInterface.gameObject.SetActive(false));
            rankingclose.onClick.AddListener(() => rankinginterface.gameObject.SetActive(false));
            storybookclose.onClick.AddListener(() => storybookinterface.gameObject.SetActive(false));
            newbieWindowclose.onClick.AddListener(() => newbieWindow.gameObject.SetActive(false));

            Message.AddListener<UI.Event.SideWindowPopup>(WindowPopup);

            CurrencyInvenWindow.SetActive(false);
            CheatInvenWindow.SetActive(false);
            buffinven.gameObject.SetActive(false);
            roulettecontroller.gameObject.SetActive(false);
            

            
            //rankinginterface.Init();
            GachaWindow.SetActive(false);

            storybookinterface.Init();

            newbieWindow.Init();
            offlineRewardValue = new EnemyReward(InGame.CharacterType.NormalMonster);

            offlineReward.Init();
            Reviewwindow.Init();

            savingWindow.Init();
            savingWindow.gameObject.SetActive(false);

            

            CurrencyInvenWindow.SetActive(false);
            CheatInvenWindow.SetActive(false);
            GachaWindow.SetActive(false);
            DailyRewardWindow.gameObject.SetActive(false);
            roulettecontroller.gameObject.SetActive(false);
            BattlePassWindow.SetActive(false);
            missionuinterface.gameObject.SetActive(false);
            mailboxInterface.gameObject.SetActive(false);
            rankinginterface.gameObject.SetActive(false);
            storybookinterface.gameObject.SetActive(false);
            newbieWindow.gameObject.SetActive(false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            buffinven.Release();
            GachaWindow.GetComponent<GachaWindowUI>().Release();
            CurrencyInven.Release();
            missionuinterface.Release();
            storybookinterface.Release();
            newbieWindow.Release();
            rankinginterface.Release();
            offlineReward.Release();
            Reviewwindow.Release();
            savingWindow.Release();

            Message.RemoveListener<UI.Event.SideWindowPopup>(WindowPopup);
            
        }

        protected override void OnEnter()
        {
            base.OnEnter();

            CheckDailyReward();
            GachaWindow.GetComponent<GachaWindowUI>().Init();
            mailboxInterface.Init();

            Message.AddListener<UI.Event.DungeonStart>(StartDungeon);
            Message.AddListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);
            Message.AddListener<UI.Event.SavingBatteryMessage>(PopupSavingBatteryWindow);
        }

        protected override void OnExit()
        {
            base.OnExit();

            Message.RemoveListener<UI.Event.DungeonStart>(StartDungeon);
            Message.RemoveListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);
            Message.RemoveListener<UI.Event.SavingBatteryMessage>(PopupSavingBatteryWindow);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.S))
            {
                testWindow.gameObject.SetActive(!testWindow.activeInHierarchy);
            }
#endif
        }

        void CheckDailyReward()
        {
            bool IsGetReward = InGameManager.Instance.GetPlayerData.CheckForDailyReward();
            if(IsGetReward)
            {
#if UNITY_EDITOR
                Debug.Log("출쳌보상 획득가능");
#endif
                DailyRewardWindow.Init(true);
                Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(SideButtonType.DailyReward, true));
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("출쳌보상 획득 불가");
#endif
                DailyRewardWindow.Init(false);
            }
        }

        void WindowPopup(UI.Event.SideWindowPopup msg)
        {
            switch (msg.type)
            {
                case SideButtonType.CurrencyInventory:
                    CurrencyInvenWindow.SetActive(true);
                    break;
                case SideButtonType.GachaWindow:
                    GachaWindow.SetActive(true);
                    break;
                case SideButtonType.DailyReward:
                    DailyRewardWindow.gameObject.SetActive(true);
                    break;
                case SideButtonType.CheatInven:
                    CheatInvenWindow.SetActive(true);
                    break;
                case SideButtonType.BuffInven:
                    buffinven.gameObject.SetActive(true);
                    break;
                case SideButtonType.Roulette:
                    roulettecontroller.gameObject.SetActive(true);
                    break;
                case SideButtonType.Battlepass:
                    BattlePassWindow.SetActive(true);
                    break;
                case SideButtonType.End:
                    break;
                case SideButtonType.NewbieEvent:
                    newbieWindow.gameObject.SetActive(true);
                    break;
                case SideButtonType.Mission:
                    missionuinterface.gameObject.SetActive(true);
                    break;
                case SideButtonType.MailBox:
                    mailboxInterface.gameObject.SetActive(true);
                    mailboxInterface.PopupMailWindow();
                    break;
                case SideButtonType.Ranking:
                    rankinginterface.gameObject.SetActive(true);
                   // rankinginterface.PopupMailWindow();
                    break;
                case SideButtonType.StoryBook:
                    storybookinterface.gameObject.SetActive(true);
                    //storybookinterface.PopupWindow();
                    break;
                default:
                    break;
            }
        }

        #region 오프보상 체크/출석보상 체크
        void CheckOfflineReward()
        {
            float elapsedtime = 0;
            bool IsGetReward = InGameManager.Instance.GetPlayerData.CheckForOfflineReward(out elapsedtime);
            if(IsGetReward)
            {
#if UNITY_EDITOR
                Debug.Log("오프보상 획득가능");
#endif
                //오프보상 로직 ==>1회 보상 로직
                int chapter=InGameManager.Instance.GetPlayerData.stage_Info.chapter;

                List<OfflineRewardData> offlineRewarddata = new List<OfflineRewardData>();
                for(int i=0; i< InGameDataTableManager.OfflineRewardTableList.reward.Count; i++)
                {
                    if(InGameDataTableManager.OfflineRewardTableList.reward[i].chapter==chapter)
                    {
                        offlineRewarddata.Add(InGameDataTableManager.OfflineRewardTableList.reward[i]);
                    }
                }
                
                List<OfflineRewardData> sortedrewarddata = offlineRewarddata.OrderBy(x => x.rarity).ToList();
                //횟수 지정
                int count = (int)(elapsedtime / 60.0f);
                if (count > 20)
                    count = 20;
                //골드 및 포션 정보
                BigInteger gold = offlineRewardValue.CalculateGold();
                BigInteger potion= offlineRewardValue.CalculateMagicpotion();

                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gold, gold);
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, potion);
                //골드 및 포션 정보

                //아이템 박스 정보
                Dictionary<CurrencyType, int> currencylist = new Dictionary<CurrencyType, int>();
                List<int> boxidList = new List<int>();
                while(count>0)
                {
                    float rate = Random.Range(0, 1100);
                    float rarity = 0;

                    for (int i = 0; i < sortedrewarddata.Count; i++)
                    {
                        rarity += sortedrewarddata[i].rarity;
                        if (rate <= rarity)
                        {
                            boxidList.Add(sortedrewarddata[i].box_idx);
                            break;
                        }
                    }
                    count--;
                }
                for (int i=0; i< boxidList.Count; i++)
                {
                    CurrencyType _type= Common.InGameManager.Instance.GetPlayerData.Playercurrency.GetIdxToType(boxidList[i]);
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(_type, 1);
                    if(currencylist.ContainsKey(_type))
                        currencylist[_type]+=1;
                    else
                        currencylist.Add(_type, 1);
                    
                }
                //아이템 박스 정보
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("오프보상 획득 불가능");
#endif
            }
        }

        #endregion
        void StartDungeon(UI.Event.DungeonStart msg)
        {
            DialogView.SetActive(false);
        }

        void ReturnToMainFromDungeon(UI.Event.DungeonEndStartMain msg)
        {
            DialogView.SetActive(true);
        }

        void PopupSavingBatteryWindow(UI.Event.SavingBatteryMessage msg)
        {
            savingWindow.gameObject.SetActive(true);
            savingWindow.Popup();
        }
    }


}
