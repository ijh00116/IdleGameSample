using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DLL_Common.Common;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

namespace BlackTree.Model
{
    //서버 세이브용으로 따로 만들어 줘야 함
    [System.Serializable]
    public class SaveData
    {
        public TutorialInfo tutorialInfo;
        public StageSubInfo stage_subinfo;
        public StageInfo stage_Info;
        public GlobalCurrency Playercurrency;
        public UserInfo GlobalUser;
        public CharacterEnforcement Enforcement;
        public UserInfoForPVP userinfoPvp;
        public Data_Mission missionInfo;
        public PaymentInfo battlepass;
    }
    public class PlayerDataModel
    {
        public SaveData saveData;
        public TutorialInfo tutorialInfo { get { return saveData.tutorialInfo; } }
        public StageInfo stage_Info { get { return saveData.stage_Info; } }
        public StageSubInfo stage_subinfo { get { return saveData.stage_subinfo; } }
        public UserInfo GlobalUser { get { return saveData.GlobalUser; } }
        public CharacterEnforcement Enforcement { get { return saveData.Enforcement; } }
        public GlobalCurrency Playercurrency { get { return saveData.Playercurrency; } }
        public PaymentInfo BattlePass { get { return saveData.battlepass; } }
        public RewardInfo rewardinfo;

        string basicSavePath;
        string savePath = "/stage";

        public Action WaveChange;
        public int CurrentWave 
        {
            get { return stage_Info.currentWave; }
            set {
                stage_Info.currentWave = value;
                if(WaveChange != null)
                    WaveChange();
            }
        }
        public int CowWave
        {
            get { return stage_Info.CowWave; }
            set
            {
                stage_Info.CowWave = value;
            }
        }

        #region 통합
        public void TotalSave()
        {
            savePath = "/TotalPlayerData";

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(basicSavePath, savePath), FileMode.Create, FileAccess.Write);
         

            formatter.Serialize(stream, saveData);
            stream.Close();

        }

        public void TotalLoad()
        {
            
        }

        public void LoadDataInit()
        {
            saveData = new SaveData();

            saveData.GlobalUser = new UserInfo();
            saveData.GlobalUser.Init();
            saveData.GlobalUser.UpdateData();

            saveData.stage_subinfo = new StageSubInfo();

            saveData.stage_Info = new StageInfo();
            stage_Info.Init();

            saveData.tutorialInfo = new TutorialInfo();

            saveData.Playercurrency = new GlobalCurrency();
            for (int i = 0; i < (int)CurrencyType.End; i++)
            {
                saveData.Playercurrency.UpdateCurrency((CurrencyType)i, BigInteger.Zero);
            }

            saveData.Enforcement = new CharacterEnforcement();

            saveData.Enforcement.AwakeLevel = 0;
            saveData.Enforcement.EnchentLevel = 0;

            saveData.Enforcement.Init();
            saveData.Enforcement.Update();

            saveData.userinfoPvp = new UserInfoForPVP();
            saveData.userinfoPvp.Init();
            //로드 되고
            rewardinfo = new RewardInfo();

            //미션 정보
            saveData.missionInfo = new Data_Mission();
            saveData.missionInfo.Init();
            //샵 정보
            saveData.battlepass = new PaymentInfo();
            saveData.battlepass.Init();

            
        }

        void SaveDataInit()
        {
        }
        #endregion
        #region 유저데이터
        public void UserInfoSave()
        {
            savePath = "/userInfo";

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(basicSavePath, savePath), FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, GlobalUser);
            stream.Close();
        }

        UI.Event.UserInfoUIUpdate userInfoUIdata = new UI.Event.UserInfoUIUpdate();
        public void AddExp(int exp)
        {
            GlobalUser.CurrentExp += exp;
            if(GlobalUser.CurrentExp>= GlobalUser.NeedExp)
            {
                GlobalUser.CurrentExp = GlobalUser.CurrentExp- GlobalUser.NeedExp;
                GlobalUser.LEVEL++;
            }
            userInfoUIdata.EarnExp = exp;

            Message.Send<UI.Event.UserInfoUIUpdate>(userInfoUIdata);
        }
        #endregion
      
        #region 화폐

        UI.Event.CurrencyChange currencyMsg = new UI.Event.CurrencyChange();
        UI.Event.BoxChange BoxMsg = new UI.Event.BoxChange();

        public void AddCurrency(CurrencyType _cType,BigInteger _value)
        {
            BigInteger _currencyvalue = Playercurrency.GetCurrency(_cType).value;
            _currencyvalue += _value;

            Playercurrency.UpdateCurrency(_cType, _currencyvalue);

            currencyMsg.Set(_cType, _value);
            currencyMsg.CurrencyTypeSummarize = true;
            Message.Send<UI.Event.CurrencyChange>(currencyMsg);
        }

        public Currency GetCurrency(CurrencyType _cType)
        {
            return Playercurrency.GetCurrency(_cType);
        }

        #endregion
        #region 일일보상
        public bool CheckForDailyReward()
        {
            DateTime currentDatetime = GetServertime();
            GlobalUser.LoginTime = currentDatetime;

            int LastRewardedDay = GlobalUser.GetRewardDay;
            if(currentDatetime.DayOfYear!= LastRewardedDay)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 오프 보상
        public bool CheckForOfflineReward(out float elapsedTime)
        {
            DateTime currentDatetime = GetServertime();
            DateTime LastRewardedTime = GlobalUser.LogoutTime;

            elapsedTime = (float)(currentDatetime - LastRewardedTime).TotalSeconds;

            if(elapsedTime > 300)//일단 오프라인 보상 5분 이후부터 제공
            {
                if (elapsedTime > 28800)
                    elapsedTime = 28800;
                return true;
            }
            else
            {
                return false;
            }
        }
        public DateTime GetServertime()
        {
            //BackendReturnObject servertime = Backend.Utils.GetServerTime();
            

            //string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
            DateTime parsedDate = System.DateTime.Now;

            return parsedDate;
        }
        #endregion

        public void Init()
        {
#if UNITY_EDITOR
            basicSavePath = Application.dataPath + "/SaveData";
#else
            basicSavePath = Application.persistentDataPath;
#endif
            LoadDataInit();

            //InGameManager.Instance.Localdata.LoadData(() => BackendManager.Instance.LoadUserData(null),SettingGameTimeInFirst);
            
            //BackendManager.Instance.LoadPVPList(null);

            //게임 첫 시작시 rank세팅
            //BackendManager.Instance.RegisterRank();
        }

        void SettingGameTimeInFirst()
        {
           
        }

        public void Release()
        {
            SaveDataInit();
            GlobalUser.LogoutTime = GetServertime();
            //BackendManager.Instance.SaveDBData(DTConstraintsData.UserData, Common.InGameManager.Instance.GetPlayerData.saveData.GlobalUser);
            InGameManager.Instance.Localdata.SaveData(saveData);

        }
    }

    

}
