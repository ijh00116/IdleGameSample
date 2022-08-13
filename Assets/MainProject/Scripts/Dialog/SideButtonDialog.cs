using BlackTree.Common;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Spine.Unity;

namespace BlackTree.UI
{
    public enum SideButtonType
    {
        //오른쪽
        QuickPetButton=0,
        GachaWindow,
        NewbieEvent,
        BossAndInfinity,
        Battlepass,
        StoryBook,
        MailBox,
        CurrencyInventory,

        //왼쪽
        BuffInven,
        Ranking,
        DailyReward,
        Mission,
        AdvertiseRewardGem,
        Roulette,
        GuideQuest,

        //에디터용
        CheatInven,

        End
    }

    public class SideButtonDialog : IDialog
    {
        [Header("펫바로가기 버튼")]
        [SerializeField] Button PetquickBtn;
        [Header("뽑기 상점")]
        [SerializeField] Button GachaWindowBtn;
        [Header("이벤트 버튼")]
        [SerializeField] Button EventBtn;
        [Header("보스 무한 버튼")]
        [SerializeField] Button BossAndInfinityBtn;
        [SerializeField] Image InfinitImage;
        [SerializeField] Image BossTryImage;
        [Header("배틀패스 버튼")]
        [SerializeField] Button BattlepassBtn;
        [SerializeField] GameObject BattlepassnewIcon;
        [Header("스토리북 버튼")]
        [SerializeField] Button StoryBookBtn;
        [SerializeField] GameObject StorynewIcon;
        [Header("메일함 버튼")]
        [SerializeField] Button MailboxBtn;
        [SerializeField] GameObject MailnewIcon;
        [Header("아이템 인벤토리")]
        [SerializeField] Button CurrencyInvenBtn;

        [Header("버프 인벤")]
        [SerializeField] Button BuffInvenBtn;
        [SerializeField] Button BuffArrowBtn;
        [SerializeField] GameObject BuffIconBar;
        [SerializeField] BuffTimeIcon[] bufficonlist;
        [Header("랭킹 인벤")]
        [SerializeField] Button RankingBtn;
        [Header("출석 보상")]
        [SerializeField] Button DailyRewardBtn;
        [SerializeField] GameObject DailynewIcon;
        [Header("미션 버튼")]
        [SerializeField] Button MissionBtn;
        [SerializeField] GameObject MissionnewIcon;
        [Header("광고 젬 버튼")]
        [SerializeField] Button AdvertiseGemRewardBtn;
        [SerializeField] Text LeftText;
  
        [Header("룰렛 버튼")]
        [SerializeField] Button RouletteBtn;
        [Header("가이드 버튼")]
        [SerializeField] Button GuideQuestBtn;
        [SerializeField] GameObject GuidenewIcon;
        [SerializeField] Image GuidArrow;
        [SerializeField] GuideMissionInfo guideMissionWindow;
        [SerializeField] Button GuideHideBtn;

        [Header("치트 인벤토리")]
        [SerializeField] Button CheatInvenBtn;

        [Header("바 버튼")]
        [SerializeField] Button leftBarArrowBtnup;
        [SerializeField] Button RightBarArrowBtnup;
        [SerializeField] Button leftBarArrowBtndown;
        [SerializeField] Button RightBarArrowBtndown;

        bool IsLeftBarStartpos;
        bool IsRightBarStartpos;

        [SerializeField] RectTransform leftBar;
        [SerializeField] RectTransform RightBar;

        [SerializeField] Vector3 LeftBarStartPos;
        [SerializeField] Vector3 LeftBarEndPos;

        [SerializeField] Vector3 RightBarStartPos;
        [SerializeField] Vector3 RightBarEndPos;

        [Header("스테이지 UI")]
        public Text StageInfo;
        public Text StageNum;
        public GameObject StageInfinite;

        protected override void OnLoad()
        {
            base.OnLoad();
            Message.AddListener<Event.SideBtnNewIconActivate>(ActivateNewIcon);
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            Message.RemoveListener<Event.SideBtnNewIconActivate>(ActivateNewIcon);
        }
        protected override void OnEnter()
        {
            //오른쪽
            PetquickBtn.onClick.AddListener(() => OpenWindow(SideButtonType.QuickPetButton));
            GachaWindowBtn.onClick.AddListener(() => OpenWindow(SideButtonType.GachaWindow));
            //이벤트버튼 테스트
            EventBtn.onClick.AddListener(()=>InGameManager.Instance. Localdata.SaveData(InGameManager.Instance.GetPlayerData.saveData));
            //이벤트버튼 테스트

            BossAndInfinityBtn.onClick.AddListener(PushInfiniteAndBossBtn);
            BattlepassBtn.onClick.AddListener(() => OpenWindow(SideButtonType.Battlepass));
            StoryBookBtn.onClick.AddListener(() => OpenWindow(SideButtonType.StoryBook));
            MailboxBtn.onClick.AddListener(() => OpenWindow(SideButtonType.MailBox));
            CurrencyInvenBtn.onClick.AddListener(() => OpenWindow(SideButtonType.CurrencyInventory));

            //왼쪽
            BuffInvenBtn.onClick.AddListener(() => OpenWindow(SideButtonType.BuffInven));
            RankingBtn.onClick.AddListener(() => OpenWindow(SideButtonType.Ranking));
            DailyRewardBtn.onClick.AddListener(() => OpenWindow(SideButtonType.DailyReward));
            MissionBtn.onClick.AddListener(() => OpenWindow(SideButtonType.Mission));
            AdvertiseGemRewardBtn.onClick.AddListener(PushAdRewardGemBtn);
            RouletteBtn.onClick.AddListener(() => OpenWindow(SideButtonType.Roulette));
            GuideQuestBtn.onClick.AddListener(PushGuideMissionBtn);

            Common.InGameManager.Instance.GetPlayerData.WaveChange += StageTextSetting;
            StageTextSetting();

            //보스
            if (InGameManager.Instance.InfiniteMode)
            {
                InfinitImage.gameObject.SetActive(true);
                BossTryImage.gameObject.SetActive(false);
            }
            else
            {
                InfinitImage.gameObject.SetActive(false);
                BossTryImage.gameObject.SetActive(true);
            }


            CheatInvenBtn.onClick.AddListener(() => OpenWindow(SideButtonType.CheatInven));

            guideMissionWindow.Init();

                LeftText.gameObject.SetActive(!Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeAdGem);

            //BuffIconBar.SetActive(false);
            BuffArrowBtn.onClick.AddListener(PushBuffArrowBtn);
            for(int i=0; i< bufficonlist.Length; i++)
            {
                bufficonlist[i].Init();
            }

            leftBar.anchoredPosition = LeftBarStartPos;
            RightBar.anchoredPosition = RightBarStartPos;

            IsLeftBarStartpos = true;
            IsRightBarStartpos = true;

            leftBarArrowBtnup.onClick.AddListener(LeftBarArrowBtnPush);
            leftBarArrowBtndown.onClick.AddListener(LeftBarArrowBtnPush);
            RightBarArrowBtnup.onClick.AddListener(RightBarArrowBtnPush);
            RightBarArrowBtndown.onClick.AddListener(RightBarArrowBtnPush);

            leftBarArrowBtndown.gameObject.SetActive(!IsLeftBarStartpos);
            leftBarArrowBtnup.gameObject.SetActive(IsLeftBarStartpos);
            RightBarArrowBtndown.gameObject.SetActive(!IsRightBarStartpos);
            RightBarArrowBtnup.gameObject.SetActive(IsRightBarStartpos);

            Message.AddListener<UI.Event.DungeonStart>(StartDungeon);
            Message.AddListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);
            Message.AddListener<Event.PetQuickStartButtonOnOff>(PetQuickStartonoff);
           

            //뉴 아이콘 비활성화
           
            BattlepassnewIcon.SetActive(false);
            StorynewIcon.SetActive(false);
            MailnewIcon.SetActive(false);
            DailynewIcon.SetActive(false);
            MissionnewIcon.SetActive(false);
            GuidenewIcon.SetActive(false);

        }

        protected override void OnExit()
        {
            CurrencyInvenBtn.onClick.RemoveAllListeners();
            GachaWindowBtn.onClick.RemoveAllListeners();
            DailyRewardBtn.onClick.RemoveAllListeners();
            CheatInvenBtn.onClick.RemoveAllListeners();
            BuffInvenBtn.onClick.RemoveAllListeners();
            MissionBtn.onClick.RemoveAllListeners();
            RankingBtn.onClick.RemoveAllListeners();
            RouletteBtn.onClick.RemoveAllListeners();
            BattlepassBtn.onClick.RemoveAllListeners();

            guideMissionWindow.Release();

            BuffArrowBtn.onClick.RemoveAllListeners();

            for (int i = 0; i < bufficonlist.Length; i++)
            {
                bufficonlist[i].Release();
            }

            Message.RemoveListener<UI.Event.DungeonStart>(StartDungeon);
            Message.RemoveListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);
            Message.RemoveListener<Event.PetQuickStartButtonOnOff>(PetQuickStartonoff);
        }
        private void Update()
        {
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeAdGem == true)
                return;
            Common.InGameManager.Instance.GetPlayerData.GlobalUser.AdGemLeftTime += Time.deltaTime;

            int lefttime = (DTConstraintsData.AD_GEM_COOLTIME- (int)Common.InGameManager.Instance.GetPlayerData.GlobalUser.AdGemLeftTime);

            int m = lefttime / 60;
            int s = lefttime % 60;

            LeftText.text = string.Format("{0:D2}:{1:D2}", m, s);

            if(Common.InGameManager.Instance.GetPlayerData.GlobalUser.AdGemLeftTime >= DTConstraintsData.AD_GEM_COOLTIME)
            {
                Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeAdGem = true;
                LeftText.gameObject.SetActive(false);
            }
        }
        void OpenWindow(SideButtonType type)
        {
            Message.Send<UI.Event.SideWindowPopup>(new UI.Event.SideWindowPopup(type));
        }

        void StageTextSetting()
        {
            int scenario = Common.InGameManager.Instance.GetPlayerData.stage_Info.scenario;
            int chapter = Common.InGameManager.Instance.GetPlayerData.stage_Info.chapter;
            int stage = Common.InGameManager.Instance.GetPlayerData.stage_Info.Stage;
            int maxwave = Common.InGameManager.Instance.GetPlayerData.stage_Info.MaxWave();
            int currentwave = Common.InGameManager.Instance.GetPlayerData.CurrentWave;

            StageInfo.text = string.Format(
                " <color=yellow>{0}</color>시나리오-<color=yellow>{1}</color>챕터\n " +
                " <color=yellow>{2}</color> <color=white>스테이지</color>         ", scenario,chapter, stage);
            StageNum.text = string.Format("<color=cyan>{0}-{1}</color>", currentwave, maxwave);
            
        }

        void PushAdRewardGemBtn()
        {
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeAdGem == false)
                return;

            Common.InGameManager.Instance.admob.ShowRewardAd(() => Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem, DTConstraintsData.AD_GEM_REWARD_COUNT));
            Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeAdGem = false;
            Common.InGameManager.Instance.GetPlayerData.GlobalUser.AdGemLeftTime= 0;
            LeftText.gameObject.SetActive(true);
        }

        void PushGuideMissionBtn()
        {
            if(guideMissionWindow.gameObject.activeInHierarchy)
            {
                GuidArrow.gameObject.SetActive(true);
                guideMissionWindow.gameObject.SetActive(false);
            }
            else
            {
                GuidArrow.gameObject.SetActive(false);
                guideMissionWindow.gameObject.SetActive(true);
            }
        }

        void StartDungeon(UI.Event.DungeonStart msg)
        {
            DialogView.SetActive(false);
        }

        void ReturnToMainFromDungeon(UI.Event.DungeonEndStartMain msg)
        {
            DialogView.SetActive(true);
        }

        Event.BossInfiniteChange GamemodeChange = new Event.BossInfiniteChange();
        void PushInfiniteAndBossBtn()
        {
            Common.InGameManager.Instance.ChangeModeAndPlay();
            InfinitImage.gameObject.SetActive(Common.InGameManager.Instance.InfiniteMode);
            BossTryImage.gameObject.SetActive(!Common.InGameManager.Instance.InfiniteMode);

            StageNum.gameObject.SetActive(!Common.InGameManager.Instance.InfiniteMode);
            StageInfinite.gameObject.SetActive(Common.InGameManager.Instance.InfiniteMode);

            Message.Send<UI.Event.BossInfiniteChange>(GamemodeChange);
        }
        Vector3 buffarrowscale;
        void PushBuffArrowBtn()
        {
            buffarrowscale = BuffArrowBtn.transform.localScale;
            buffarrowscale.x *= -1;
            BuffArrowBtn.transform.localScale = buffarrowscale;
            BuffIconBar.SetActive(!BuffIconBar.activeInHierarchy);
        }

        Vector3 endrot =new Vector3(0, 0, 180);
        
        void LeftBarArrowBtnPush()
        {
            leftBar.DOAnchorPos((IsLeftBarStartpos)?LeftBarEndPos:LeftBarStartPos, 0.5f);
            IsLeftBarStartpos = !IsLeftBarStartpos;

            leftBarArrowBtndown.gameObject.SetActive(!IsLeftBarStartpos);
            leftBarArrowBtnup.gameObject.SetActive(IsLeftBarStartpos);
        }

        void RightBarArrowBtnPush()
        {
            RightBar.DOAnchorPos((IsRightBarStartpos) ? RightBarEndPos : RightBarStartPos, 0.5f);
            IsRightBarStartpos = !IsRightBarStartpos;
         
            RightBarArrowBtndown.gameObject.SetActive(!IsRightBarStartpos);
            RightBarArrowBtnup.gameObject.SetActive(IsRightBarStartpos);
        }

        void PetQuickStartonoff(UI.Event.PetQuickStartButtonOnOff msg)
        {
            PetquickBtn.gameObject.SetActive(msg.On);
        }

        void ActivateNewIcon(UI.Event.SideBtnNewIconActivate msg)
        {
            switch (msg.buttonType)
            {
                case SideButtonType.QuickPetButton:
                    break;
                case SideButtonType.GachaWindow:
                    break;
                case SideButtonType.NewbieEvent:
                    break;
                case SideButtonType.BossAndInfinity:
                    break;
                case SideButtonType.Battlepass:
                    BattlepassnewIcon.SetActive(msg.IsHaveSomethingNew);
                    break;
                case SideButtonType.StoryBook:
                    StorynewIcon.SetActive(msg.IsHaveSomethingNew);
                    break;
                case SideButtonType.MailBox:
                    MailnewIcon.SetActive(msg.IsHaveSomethingNew);
                    break;
                case SideButtonType.CurrencyInventory:
                    break;
                case SideButtonType.BuffInven:
                    break;
                case SideButtonType.Ranking:
                    break;
                case SideButtonType.DailyReward:
                    DailynewIcon.SetActive(msg.IsHaveSomethingNew);
                    break;
                case SideButtonType.Mission:
                    MissionnewIcon.SetActive(msg.IsHaveSomethingNew);
                    break;
                case SideButtonType.AdvertiseRewardGem:
                    break;
                case SideButtonType.Roulette:
                    break;
                case SideButtonType.GuideQuest:
                    GuidenewIcon.SetActive(msg.IsHaveSomethingNew);
                    break;
                case SideButtonType.CheatInven:
                    break;
                case SideButtonType.End:
                    break;
                default:
                    break;
            }
        }
    }

}
