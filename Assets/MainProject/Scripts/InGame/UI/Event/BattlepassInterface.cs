using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public enum BattlePassType
    {
        UserLevel,
        Fairy,
        PlayingTime,
        End
    }
    public class BattlepassInterface : MonoBehaviour
    {
        [SerializeField] BattlePassSlot battlepassPrefab;

        [Header("유저레벨 배틀패스")]
        [SerializeField] Button UserLevelBPBtn;
        [SerializeField] GameObject UserLevelBPBtnSelected;
        [SerializeField] GameObject UserLevelBattlepassWindow;
        [SerializeField] Transform UserLevelparent;
        [SerializeField] Slider UserLevelSlider;
        [SerializeField] RectTransform UserLevelTotalContent;
        int UserLevelMaxcount;
        List<BattlePassSlot> userlevelslotlist = new List<BattlePassSlot>();
        [SerializeField] Text userlevelbattlepassName;
        [SerializeField] Text userlevelbattlepassdesc;
        [SerializeField] Text ulProgressText;
        [Header("페어리 배틀패스")]
        [SerializeField] Button FairyBPBtn;
        [SerializeField] GameObject FairyBPBtnSelected;
        [SerializeField] GameObject FairyBattlepassWindow;
        [SerializeField] Transform Fairyparent;
        [SerializeField] Slider FairySlider;
        [SerializeField] RectTransform FairyTotalContent;
        int FairyMaxcount;
        List<BattlePassSlot> fairyslotlist = new List<BattlePassSlot>();
        [SerializeField] Text fairybattlepassName;
        [SerializeField] Text fairybattlepassdesc;
        [SerializeField] Text fairyProgressText;
        [Header("타임 배틀패스")]
        [SerializeField] Button PlayingTimeBPBtn;
        [SerializeField] GameObject PlayingTimeBPBtnSelected;
        [SerializeField] GameObject PlayingTimeBattlepassWindow;
        [SerializeField] Transform PlayingTimeparent;
        [SerializeField] Slider PlayingTimeSlider;
        [SerializeField] RectTransform PlayingTimeTotalContent;
        int TimeMaxcount;
        List<BattlePassSlot> PlayingTimeslotlist = new List<BattlePassSlot>();
        [SerializeField] Text PtbattlepassName;
        [SerializeField] Text Ptbattlepassdesc;
        [SerializeField] Text PtProgressText;

        const int UISlotWidthSize = 240;
        const int UIScrollviewSpace = 20;

        public void Awake()
        {
            UserLevelBPBtn.onClick.AddListener(()=> { ActiveWindow(UserLevelBattlepassWindow, UserLevelBPBtnSelected); });
            FairyBPBtn.onClick.AddListener(() => { ActiveWindow(FairyBattlepassWindow, FairyBPBtnSelected); });
            PlayingTimeBPBtn.onClick.AddListener(() => { ActiveWindow(PlayingTimeBattlepassWindow, PlayingTimeBPBtnSelected); });

            ActiveWindow(UserLevelBattlepassWindow, UserLevelBPBtnSelected);
            //유저레벨 배틀패스
            for (int i=0;i<Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo.Count; i++)
            {
                var obj = Instantiate(battlepassPrefab);
                obj.transform.SetParent(UserLevelparent, false);
                obj.Init(Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo[i],BattlePassType.UserLevel);
                UserLevelMaxcount = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo[i].tableData.Count;
                userlevelslotlist.Add(obj);
            }
            foreach (var _data in userlevelslotlist)
            {
                _data.UpdateCount(Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL);
            }
            //UI길이 조정
            int slotcount = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo.Count;
            int height = slotcount * (UISlotWidthSize + UIScrollviewSpace);
            UserLevelSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(UserLevelSlider.GetComponent<RectTransform>().sizeDelta.x, height);
            UserLevelTotalContent.GetComponent<RectTransform>().sizeDelta = new Vector2(UserLevelTotalContent.GetComponent<RectTransform>().sizeDelta.x, height+ UISlotWidthSize + UIScrollviewSpace);

            //페어리 배틀패스
            for (int i = 0; i < Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo.Count; i++)
            {
                var obj = Instantiate(battlepassPrefab);
                obj.transform.SetParent(Fairyparent, false);
                obj.Init(Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo[i], BattlePassType.Fairy);
                FairyMaxcount = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo[i].tableData.Count;
                fairyslotlist.Add(obj);
            }
            foreach (var _data in fairyslotlist)
            {
                _data.UpdateCount((int)Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.GetRecordValue(MissionType.FAIRY_GETTING_COUNT));
            }
            //UI길이 조정
            slotcount = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo.Count;
            height = slotcount * (UISlotWidthSize + UIScrollviewSpace);
            FairySlider.GetComponent<RectTransform>().sizeDelta = new Vector2(FairySlider.GetComponent<RectTransform>().sizeDelta.x, height);
            FairyTotalContent.GetComponent<RectTransform>().sizeDelta = new Vector2(FairyTotalContent.GetComponent<RectTransform>().sizeDelta.x, height + UISlotWidthSize + UIScrollviewSpace);

            //타임 배틀패스
            for (int i = 0; i < Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo.Count; i++)
            {
                var obj = Instantiate(battlepassPrefab);
                obj.transform.SetParent(PlayingTimeparent, false);
                obj.Init(Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo[i], BattlePassType.PlayingTime);
                TimeMaxcount = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo[i].tableData.Count;
                PlayingTimeslotlist.Add(obj);
            }
            foreach (var _data in PlayingTimeslotlist)
            {
                _data.UpdateCount((int)Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime);
            }
            //UI길이 조정
            slotcount = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo.Count;
            height = slotcount * (UISlotWidthSize + UIScrollviewSpace);
            PlayingTimeSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(PlayingTimeSlider.GetComponent<RectTransform>().sizeDelta.x, height);
            PlayingTimeTotalContent.GetComponent<RectTransform>().sizeDelta = new Vector2(PlayingTimeTotalContent.GetComponent<RectTransform>().sizeDelta.x, height + UISlotWidthSize + UIScrollviewSpace);

            //슬라이더 밸류값이 각 상황의 맥스값마다 다르므로 따로 설정해줘야함
            UserLevelSlider.value = (float)((float)Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL / (float)UserLevelMaxcount);
            PlayingTimeSlider.value = (float)((float)Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime / (float)TimeMaxcount);
            float currentcount = Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.GetRecordValue(MissionType.FAIRY_GETTING_COUNT);
            FairySlider.value = (float)(currentcount / FairyMaxcount);

            //제목 설명 설정
            string ulname = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo[0].tableData.name;
            string uldesc= Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo[0].tableData.desc;
            string fname = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo[0].tableData.name;
            string fdesc = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo[0].tableData.desc;
            string ptname = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo[0].tableData.name;
            string ptdesc = Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo[0].tableData.desc;

            LocalValue namelv = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == ulname);
            LocalValue desclv = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == uldesc);
            userlevelbattlepassName.text = string.Format("{0}", namelv.GetStringForLocal(true));
            userlevelbattlepassdesc.text = string.Format("{0}", desclv.GetStringForLocal(true));

            namelv = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == fname);
            desclv = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == fdesc);
            fairybattlepassName.text = string.Format("{0}", namelv.GetStringForLocal(true));
            fairybattlepassdesc.text = string.Format("{0}", desclv.GetStringForLocal(true));

            namelv = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == ptname);
            desclv = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == ptdesc);
            PtbattlepassName.text = string.Format("{0}", namelv.GetStringForLocal(true));
            Ptbattlepassdesc.text = string.Format("{0}", desclv.GetStringForLocal(true));

            Message.AddListener<InGame.Event.MissionValueUpdate>(FairyGettingMsg);

            ulProgressText.text = string.Format("LV.{0}",Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL);
            int m = (int)(Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime / 60.0f);
            int s = (int)(Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime % 60.0f);
            PtProgressText.text = string.Format("{0:D2}:{1:D2}",m,s);
            fairyProgressText.text = string.Format(Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.GetRecordValue(MissionType.FAIRY_GETTING_COUNT).ToString());
        }

        
        public void OnDestroy()
        {
            Message.RemoveListener<InGame.Event.MissionValueUpdate>(FairyGettingMsg);
        }

        void ActiveWindow(GameObject Window,GameObject selectedimage)
        {
            UserLevelBattlepassWindow.SetActive(false);
            PlayingTimeBattlepassWindow.SetActive(false);
            FairyBattlepassWindow.SetActive(false);

            UserLevelBPBtnSelected.SetActive(false);
            FairyBPBtnSelected.SetActive(false);
            PlayingTimeBPBtnSelected.SetActive(false);

            Window.SetActive(true);
            selectedimage.SetActive(true);
        }

        UI.Event.SideBtnNewIconActivate sidebtnNewIcon = new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.Battlepass, false);

        float currenttime = 0;
        private void Update()
        {
            currenttime += Time.deltaTime;
            if (currenttime < 1)
                return;
            currenttime = 0;
            int index = -1;
            for(int i=0; i< Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo.Count; i++)
            {
                if (Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo[i].tableData.Count <= Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL)
                    index = i;
                else
                    break;
            }
            UserLevelSlider.value = (index>=0)?
                (float)(index +1)/ (float)(Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.UserLevelBattlePassinfo.Count): 0;
            index = -1;
            for (int i = 0; i < Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo.Count; i++)
            {
                if (Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo[i].tableData.Count <= Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime)
                    index = i;
                else
                    break;
            }
            PlayingTimeSlider.value= (index >= 0)?
            (float)(index + 1) / (float)(Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.PlayingTimeBattlePassinfo.Count) : 0;

            foreach (var _data in userlevelslotlist)
            {
                _data.UpdateCount(Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL);
            }
            foreach (var _data in PlayingTimeslotlist)
            {
                _data.UpdateCount((int)Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime);
            }

            bool Isnew = false;

            for(int i=0; i<userlevelslotlist.Count; i++)
            {
                if(userlevelslotlist[i].CanReward)
                {
                    Isnew = true;
                    break;
                }
            }
            for (int i = 0; i < PlayingTimeslotlist.Count; i++)
            {
                if (Isnew)
                    break;
                if (PlayingTimeslotlist[i].CanReward)
                {
                    Isnew = true; 
                    break;
                }
                    
            }
            for (int i = 0; i < fairyslotlist.Count; i++)
            {
                if (Isnew)
                    break;
                if (fairyslotlist[i].CanReward)
                {
                    Isnew = true;
                    break;
                }
            }
            if(Isnew)
            {
                sidebtnNewIcon.IsHaveSomethingNew = Isnew;
                Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.Battlepass, true));
            }
            ulProgressText.text = string.Format("LV.{0}", Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL);
            int m = (int)(Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime / 60.0f);
            int s = (int)(Common.InGameManager.Instance.GetPlayerData.GlobalUser.PlayingTime % 60.0f);
            PtProgressText.text = string.Format("{0:D2}:{1:D2}", m, s);

        }

        void FairyGettingMsg(InGame.Event.MissionValueUpdate msg)
        {
            if (msg.missiontype != MissionType.FAIRY_GETTING_COUNT)
                return;
            float currentcount = Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.GetRecordValue(MissionType.FAIRY_GETTING_COUNT);
            int index = -1;
            for (int i = 0; i < Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo.Count; i++)
            {
                if (Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo[i].tableData.Count <= currentcount)
                    index = i;
                else
                    break;
            }
            FairySlider.value = (index >= 0) ?
                (float)(index + 1) / (float)(Common.InGameManager.Instance.GetPlayerData.saveData.battlepass.FairyCountBattlePassinfo.Count) : 0;
            foreach (var _data in fairyslotlist)
            {
                _data.UpdateCount((int)Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.GetRecordValue(MissionType.FAIRY_GETTING_COUNT));
            }
            fairyProgressText.text = string.Format(Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.GetRecordValue(MissionType.FAIRY_GETTING_COUNT).ToString());
        }
    }

}
