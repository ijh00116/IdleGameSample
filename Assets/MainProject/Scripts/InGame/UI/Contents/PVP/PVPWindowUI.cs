using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DLL_Common.Common;

namespace BlackTree
{
    public class PVPWindowUI : MonoBehaviour
    {
        [Header("시작화면")]
        [SerializeField] Button StartButton;
        [SerializeField] Text Userinfo;
        [SerializeField] Text EnemyInfo;
        //pvp정보
        [SerializeField] Button InfoWindowButton;
        [SerializeField] Button InfoWindowCloseButton;
        [SerializeField] GameObject InfoWindow;
        [SerializeField] Text InfoWindowTop;
        [SerializeField] Text InfoWindowBottom;

        [Header("PVP진입상태")]
        //상단 전투 ui
        [SerializeField] GameObject InPVPWindow;
        [SerializeField] PVPHpbar hpbar;
        [SerializeField] Image CombatUserProfile;
        [SerializeField] Image CombatEnemyProfile;
        [SerializeField] Text CombatUserNickname;
        [SerializeField] Text CombatEnemyNickname;
        [SerializeField] Text LeftTime;
        //하탄 정보 표시 ui
        [SerializeField] Text InPVPUserinfo;
        [SerializeField] Text InPVPEnemyInfo;
        [SerializeField] Text UserRank;
        [SerializeField] Text EnemyRank;
        [SerializeField] Text UserNickName;
        [SerializeField] Text EnemyNickName;
        [SerializeField] Image UserProfileImage;
        [SerializeField] Image EnemyProfileImage;

        [SerializeField] List<PVPSkillSlotButton> CharactersSkillSlot;
        UserInfoForPVP EnemyUserInfo;
        UserInfoForPVP myInfo;
        [Header("결과창")]
        [SerializeField] GameObject Result;
        [SerializeField] GameObject Winwindow;
        [SerializeField] GameObject Losewindow;
        [SerializeField] Text WinResultInfo;
        [SerializeField] Text LoseResultInfo;
        [SerializeField] Button WinResultConfirmButton;
        [SerializeField] Button LoseResultConfirmButton;
        [SerializeField] Text WinResultBeforeScore;
        [SerializeField] Text WinResultAfterScore;
        [SerializeField] Text LoseResultBeforeScore;
        [SerializeField] Text LoseResultAfterScore;


        public void Init()
        {
            hpbar.Init();
            StartButton.onClick.AddListener(CombatWithEnemy);
            WinResultConfirmButton.onClick.AddListener(ConfirmPVPEnd);
            LoseResultConfirmButton.onClick.AddListener(ConfirmPVPEnd);

            Result.SetActive(false);
            Message.AddListener<UI.Event.PVPEnd>(PvpEndUIPopup);

            InfoWindowButton.onClick.AddListener(() => InfoWindow.SetActive(true));
            InfoWindowCloseButton.onClick.AddListener(() => InfoWindow.SetActive(false));

            InfoWindow.SetActive(false);

            LocalValue infotop = InGameDataTableManager.LocalizationList.pvp.Find(o => o.id == "pvp_help_desc_01");
            LocalValue infoBottom = InGameDataTableManager.LocalizationList.pvp.Find(o => o.id == "pvp_help_desc_02");

            InfoWindowTop.text = infotop.kr;
            InfoWindowBottom.text = infoBottom.kr;

            EnemyUserInfo = new UserInfoForPVP();
            EnemyUserInfo.Init();

            myInfo = Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp;

            foreach (var _data in CharactersSkillSlot)
            {
                _data.Init();
            }

            
        }

        public void Release()
        {
            hpbar.Release();
            StartButton.onClick.RemoveAllListeners();
            Message.RemoveListener<UI.Event.PVPEnd>(PvpEndUIPopup);
        }

        //시작화면
        public void PvpUIPopup()
        {
             EnemyRefresh();
             
             Userinfo.text = string.Format("점수: {0}\n닉네임:{1}", myInfo.RankScore, myInfo.NickName);
             EnemyInfo.text = string.Format("점수: {0}\n닉네임:{1}", EnemyUserInfo.RankScore, EnemyUserInfo.NickName);

             InPVPWindow.SetActive(false);
        }

        void EnemyRefresh()
        {
        }

        private void Update()
        {
            if(InPVPWindow.activeInHierarchy==true)
            {
                LeftTime.text = string.Format("{0}", (int)Common.InGameManager.Instance._PvpsceneFsm.pvpDungencurrentTime);
            }
        }
        //전투중&시작
        void CombatWithEnemy()
        {
            Common.InGameManager.Instance.StartPVP(EnemyUserInfo);
            hpbar.StartPvp();
            InPVPWindow.SetActive(true);
            InPVPWindowSetup();
            foreach (var _data in CharactersSkillSlot)
            {
                _data.StartCombat();
            }
            Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.PVP_BATTLE, 1);
        }

        void InPVPWindowSetup()
        {
            CombatUserNickname.text = myInfo.NickName;
            CombatEnemyNickname.text = EnemyUserInfo.NickName;
            UserNickName.text = myInfo.NickName;
            EnemyNickName.text = EnemyUserInfo.NickName;

            UserRank.text = string.Format("점수\n{0}", myInfo.RankScore);
            EnemyRank.text = string.Format("점수\n{0}", EnemyUserInfo.RankScore);

            string info = null;
            for(int i=0; i<InGameDataTableManager.PVPTableList.battle_ability.Count; i++)
            {
                LocalValue _value = InGameDataTableManager.LocalizationList.pvp.Find(o => o.id ==
                InGameDataTableManager.PVPTableList.battle_ability[i].name);
                if (InGameDataTableManager.PVPTableList.battle_ability[i].abtype <PVPAbilityType.CHA_SKILL_LIGHTNING_LV)
                {
                    info += string.Format(_value.kr + "\n", myInfo.AbilityList[InGameDataTableManager.PVPTableList.battle_ability[i].abtype]);
                }
                else
                {
                    info += string.Format(_value.kr + "\n", myInfo.AbilityList[InGameDataTableManager.PVPTableList.battle_ability[i].abtype],
                        myInfo.AbilityList[InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype]);
                }
            }
            InPVPUserinfo.text = info;

            string enemyinfo = null;
            for (int i = 0; i < InGameDataTableManager.PVPTableList.battle_ability.Count; i++)
            {
                LocalValue _value = InGameDataTableManager.LocalizationList.pvp.Find(o => o.id ==
                InGameDataTableManager.PVPTableList.battle_ability[i].name);
                if (InGameDataTableManager.PVPTableList.battle_ability[i].abtype < PVPAbilityType.CHA_SKILL_LIGHTNING_LV)
                {
                    enemyinfo += string.Format(_value.kr + "\n", EnemyUserInfo.AbilityList[InGameDataTableManager.PVPTableList.battle_ability[i].abtype]);
                }
                else
                {
                    enemyinfo += string.Format(_value.kr + "\n", EnemyUserInfo.AbilityList[InGameDataTableManager.PVPTableList.battle_ability[i].abtype],
                        EnemyUserInfo.AbilityList[InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype]);
                }
            }

            InPVPEnemyInfo.text = enemyinfo;
        }


        //전투 끝
        void PvpEndUIPopup(UI.Event.PVPEnd msg)
        {
            int BeforeScore = Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.RankScore;
            if (msg.MeWin)
                Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.RankScore += 10;
            else
                Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.RankScore -= 5;

            int AfterScore= Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.RankScore;
            //BackendManager.Instance.RegisterRank();
            Result.SetActive(true);
            Winwindow.SetActive(msg.MeWin);
            Losewindow.SetActive(!msg.MeWin);
            if (msg.MeWin)
            {
                WinResultInfo.text = msg.MeWin == true ? "승리" : "패배";
                WinResultBeforeScore.text = BeforeScore.ToString();
                WinResultAfterScore.text = AfterScore.ToString();
            }
            else
            {
                LoseResultInfo.text = msg.MeWin == true ? "승리" : "패배";
                LoseResultBeforeScore.text = BeforeScore.ToString();
                LoseResultAfterScore.text = AfterScore.ToString();
            }
        }

        void ConfirmPVPEnd()
        {
            Result.SetActive(false);
            InPVPWindow.SetActive(false);
        }

    }

}
