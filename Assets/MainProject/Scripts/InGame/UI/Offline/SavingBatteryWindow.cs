using BlackTree.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BlackTree
{
    public class SavingBatteryWindow : MonoBehaviour
    {
        [Header("오른쪽 핸드폰데이터")]
        [SerializeField] GameObject WifiIcon;
        [SerializeField] GameObject DataIcon;
        [SerializeField] Text BatteryInfo;
        [SerializeField] Image BatteryIcon;
        [SerializeField] Text TimeText;
        [SerializeField] List<Sprite> batterysprite = new List<Sprite>();

        [Header("왼쪽 재화,스테이지 텍스트")]
        [SerializeField] Text StageText;
        [SerializeField] List<EarnCurrencyText> currencyText=new List<EarnCurrencyText>();

        [Header("잠금해제,절전시간")]
        [SerializeField] Slider UnlockSlider;
        [SerializeField] Text SavingTime; 
        
        [Header("펫")]
        [SerializeField] PetRewardSlotUI rewardpetprefab;
        [SerializeField] Transform parent;
        [HideInInspector] List<PetRewardSlotUI> petslotList = new List<PetRewardSlotUI>();

        float CurrentTime = 0;
        float CurrentShowingMinute=0;
        private void Start()
        {
            for (int i = 0; i < InGameManager.Instance.petInventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(rewardpetprefab);
                obj.transform.SetParent(parent, false);
                obj.slot = InGameManager.Instance.petInventory.GetSlots[i];
                obj.Initialize();
                petslotList.Add(obj);
            }
        }

        private void OnDestroy()
        {
            
        }

        public void Init()
        {
            Message.AddListener<UI.Event.PetAmountAdded>(PetAddedUpdate);
            Message.AddListener<UI.Event.CurrencyChange>(CurrencyUpdate);
            UnlockSlider.onValueChanged.AddListener(UnlockSliderCallback);

            Common.InGameManager.Instance.GetPlayerData.WaveChange += StageTextSetting;
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.PetAmountAdded>(PetAddedUpdate);
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyUpdate);
            UnlockSlider.onValueChanged.RemoveAllListeners();
        }

        public void Popup()
        {
            for(int i=0; i< petslotList.Count; i++)
            {
                petslotList[i].Initialize();
            }
            SavingTime.text = string.Format("{0}시간{1}분", 0, 0);
            for (int i = 0; i < currencyText.Count; i++)
            {
                currencyText[i].Initialize();
            }
            StageTextSetting();
            MobileInfoUpdate();

            UnlockSlider.value = 0.5f;
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;

            if(CurrentTime>=60)//1minute update
            {
                CurrentShowingMinute += 1;
                CurrentTime = 0;
                int h = (int)(CurrentShowingMinute / 60.0f);
                int m = (int)(CurrentShowingMinute % 60.0f);
                SavingTime.text = string.Format("{0}시간{1}분", h, m);

                MobileInfoUpdate();
            }
        }
        void MobileInfoUpdate()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("인터넷 종료로 시스템이 종료됩니다.");
                Application.Quit();
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//데이터요금
            {
                WifiIcon.SetActive(false);
                DataIcon.SetActive(true);
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)//와이파이
            {
                WifiIcon.SetActive(true);
                DataIcon.SetActive(false);
            }

            //배터리
            float batteryLevel = SystemInfo.batteryLevel;
            BatteryStatus batterystatus = SystemInfo.batteryStatus;
            if (batterystatus == BatteryStatus.Charging)
            {
                BatteryIcon.sprite = batterysprite[0];
            }
            else
            {
                if (batteryLevel > 0 && batteryLevel < 0.35f)
                {
                    BatteryIcon.sprite = batterysprite[1];
                }
                else if (batteryLevel >= 0.35f && batteryLevel < 0.65f)
                {
                    BatteryIcon.sprite = batterysprite[2];
                }
                else if (batteryLevel >= 0.65f && batteryLevel < 0.9f)
                {
                    BatteryIcon.sprite = batterysprite[3];
                }
                else if (batteryLevel >= 0.9f)
                {
                    BatteryIcon.sprite = batterysprite[4];
                }

            }
            int batteryper = (int)(batteryLevel * 100.0f);
            BatteryInfo.text = string.Format("{0}%", batteryper);

            //BackendReturnObject servertime = Backend.Utils.GetServerTime();
            //string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
            DateTime parsedDate = DateTime.Now;
            TimeText.text = string.Format("{0}:{1}", parsedDate.Hour, parsedDate.Minute);
        }
        void StageTextSetting()
        {
            if (this.gameObject.activeInHierarchy == false)
                return;

            int chapter = Common.InGameManager.Instance.GetPlayerData.stage_Info.chapter;
            int stage = Common.InGameManager.Instance.GetPlayerData.stage_Info.Stage;
            int maxwave = Common.InGameManager.Instance.GetPlayerData.stage_Info.MaxWave();
            int currentwave = Common.InGameManager.Instance.GetPlayerData.CurrentWave;

            StageText.text = string.Format("{0}-{1}  {2}/{3}", chapter, stage, currentwave, maxwave);
        }

        void CurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (this.gameObject.activeInHierarchy == false)
                return;
            for(int i=0; i< currencyText.Count; i++)
            {
                currencyText[i].CurrencyTextSet(msg.Type, msg.value);
            }
        }

        void PetAddedUpdate(UI.Event.PetAmountAdded msg)
        {
            if (this.gameObject.activeInHierarchy == false)
                return;

            PetRewardSlotUI slotui = this.petslotList.Find(o => o.slot.pet.idx == msg.slot.pet.idx);
            if(slotui!=null)
                slotui.AddAmount(msg.AddCount);
        }

        public void UnlockSliderCallback(float value)
        {
            if(value>=0.95f)
            {
                Application.targetFrameRate = 50;
                this.gameObject.SetActive(false);
            }
        }

    }
}
