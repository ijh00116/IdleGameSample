using DLL_Common.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public class UserInfo
    {
        public int CurrentNewbiePackageIdx=-1;
        [System.NonSerialized] public Newbieinfo currentnewbieinfo=null;
        [System.NonSerialized] public int NeedExp;
        [System.NonSerialized] public BigInteger RewardGold;//몬스터 죽였을때의 골드는 레벨에 따라 조정되서 캐릭터데이터가 소유하고 있음
        private int level;
        public int CurrentExp;
        public int CharacterLevel;
        //뽑기 레벨
        public int GachaWeaponLevel;
        public int GachaWingLevel;
        public int GachaPetLevel;
        public int GachaSrelicLevel;
        //뽑기 경치
        public int GachaWeaponExp;
        public int GachaWingExp;
        public int GachaPetExp;
        public int GachaSrelicExp;

        public float GachaWeaponAdLeftTime;
        public float GachaWingAdLeftTime;
        public float GachaPetAdLeftTime;
        public float GachaSrelicAdLeftTime;

        public bool PetQuickStartOn;

        public bool CanSeeRouletAd;
        public float RoulettAdLeftTime;

        public bool CanSeeAdGem;
        public float AdGemLeftTime;

        public float ZoomValue=0.2f;

        #region 일일 출쳌 보상
        //받아야할 출석보상 인덱스
        public int RewardIndex=-1;
        public int GetRewardDay=-1;
        
        #endregion

        #region 오프라인 보상
        public DateTime LoginTime;
        public DateTime LogoutTime;
        #endregion

        //일일임무 체크 위함
        public DateTime CurrentPlayingDay;//게임최근 접속 날짜(한번 갱신 되면 더이상 갱신되지 않음)

        public string LOGOUTTIME
        {
            get { return LogoutTime.ToString(); }
            set { LogoutTime = DateTime.Parse(value); }
        }
        public string CurrentPlayingTime
        {
            get { return CurrentPlayingDay.ToString(); }
            set { CurrentPlayingDay = DateTime.Parse(value); }
        }
        public int LEVEL
        {
            get { return level; }
            set
            {
                level = value;
                UpdateData();
                Message.Send<UI.Event.GlobalLvUp>(globallvup);
            }
        }

        public float PlayingTime { get; set; }
        public float TotalPlayingTime { get; set; }


        public int LeftAttackPowerBuffTime { get; set; }
        public int LeftAttackSpeedBuffTime { get; set; }
        public int LeftMonsterGoldBuffTime { get; set; }
        public int LeftMoveSpeedBuffTime { get; set; }
        public int LeftMonsterPotionBuffTime { get; set; }

        public bool LeftAttackPowerBuffActivate { get; set; }
        public bool LeftAttackSpeedBuffActivate { get; set; }
        public bool LeftMonsterGoldBuffActivate { get; set; }
        public bool LeftMoveSpeedBuffActivate { get; set; }
        public bool LeftMonsterPotionBuffActivate { get; set; }

        [System.NonSerialized]
        UI.Event.GlobalLvUp globallvup = new UI.Event.GlobalLvUp();
        public void Init()
        {
            CurrentNewbiePackageIdx = -1;
            LEVEL = 1;
            GachaWeaponLevel = 1;
            GachaWingLevel = 1;
            GachaPetLevel = 1;
            GachaSrelicLevel = 1;
            GachaWeaponExp = 0;
            GachaWingExp = 0;
            GachaPetExp = 0;
            GachaSrelicExp = 0;

            CurrentExp = 0;
            CharacterLevel = 1;
            PlayingTime = 0;

            LeftAttackPowerBuffTime = 0;
            LeftAttackSpeedBuffTime = 0;
            LeftMonsterGoldBuffTime = 0;
            LeftMoveSpeedBuffTime = 0;

            LeftAttackPowerBuffActivate = false;
            LeftAttackSpeedBuffActivate = false;
            LeftMonsterGoldBuffActivate = false;
            LeftMoveSpeedBuffActivate = false;

            GachaWeaponAdLeftTime=0;
            GachaWingAdLeftTime = 0;
            GachaPetAdLeftTime = 0;
            GachaSrelicAdLeftTime = 0;

            PetQuickStartOn = true;

            CanSeeRouletAd=true;
            RoulettAdLeftTime=DTConstraintsData.AD_ROULET_COOLTIME;

            CanSeeAdGem=true;
            AdGemLeftTime=DTConstraintsData.AD_GEM_COOLTIME;

            LogoutTime=System.DateTime.Now;
           // LogoutTime = Common.InGameManager.Instance.GetPlayerData.GetServertime();
        }
        
        public void BuffActive(InGame.BuffType _type,bool active)
        {
            switch (_type)
            {
                case InGame.BuffType.AttackPower:
                    LeftAttackPowerBuffTime = active? DTConstraintsData.BuffTime:0;
                    LeftAttackPowerBuffActivate = active;
                    break;
                case InGame.BuffType.AttackSpeed:
                    LeftAttackSpeedBuffTime = active ? DTConstraintsData.BuffTime : 0;
                    LeftAttackSpeedBuffActivate = active;
                    break;
                case InGame.BuffType.MonsterGold:
                    LeftMonsterGoldBuffTime = active ? DTConstraintsData.BuffTime : 0;
                    LeftMonsterGoldBuffActivate = active;
                    break;
                case InGame.BuffType.MoveSpeed:
                    LeftMoveSpeedBuffTime = active ? DTConstraintsData.BuffTime : 0;
                    LeftMoveSpeedBuffActivate = active;
                    break;
                case InGame.BuffType.MonsterPotion:
                    LeftMonsterPotionBuffTime = active ? DTConstraintsData.BuffTime : 0;
                    LeftMonsterPotionBuffActivate = active;
                    break;
                case InGame.BuffType.End:
                    break;
                default:
                    break;
            }
        }

        public void UpdateBuffTime(InGame.BuffType _type, int time)
        {
            switch (_type)
            {
                case InGame.BuffType.AttackPower:
                    LeftAttackPowerBuffTime = time;
                    break;
                case InGame.BuffType.AttackSpeed:
                    LeftAttackSpeedBuffTime = time;
                    break;
                case InGame.BuffType.MonsterGold:
                    LeftMonsterGoldBuffTime = time;
                    break;
                case InGame.BuffType.MoveSpeed:
                    LeftMoveSpeedBuffTime = time;
                    break;
                case InGame.BuffType.MonsterPotion:
                    LeftMonsterPotionBuffTime= time;
                    break;
                case InGame.BuffType.End:
                    break;
                default:
                    break;
            }
        }

        public void UpdateData()
        {
            //필요 경험치
            for (int i = 0; i < InGameDataTableManager.CharacterList.user.Count; i++)
            {
                if (InGameDataTableManager.CharacterList.user[i].level == this.level)
                {
                    this.NeedExp = InGameDataTableManager.CharacterList.user[i].exp;
                    string gold = InGameDataTableManager.CharacterList.user[i].gold;
                    string ToflotGold = gold.Replace(",", "");

                    this.RewardGold = new BigInteger(ToflotGold);
                    break;
                }
            }

            currentnewbieinfo = InGameDataTableManager.NewbiePackage.newbie.Find(o => o.idx == CurrentNewbiePackageIdx);
        }


    }

}
