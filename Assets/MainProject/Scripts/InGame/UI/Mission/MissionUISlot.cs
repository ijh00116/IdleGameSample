using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class MissionUISlot : MonoBehaviour
    {
         [HideInInspector]public BaseMission mymission;
        [SerializeField] Text title;
        [SerializeField] Text count;
        [SerializeField] Text RewardText;
        [SerializeField] Slider slider;
        [SerializeField] Button confirmbtn_Red;
        [SerializeField] Button confirmbtn_Green;
        [SerializeField] GameObject AlreadyComplete;
        [SerializeField] Image NotCompleteBtn;
        [SerializeField] Image RewardIcon;

        [SerializeField] GameObject RewardedCheck;
        
        [Header("스프라이트 색")]
        [SerializeField] Sprite RedBar;
        [SerializeField] Sprite greenBar;
        [SerializeField] Image FillBar;

        [Header("슬롯 아웃라인 스프라이트 색")]
        [SerializeField] Sprite PurpleOutline;
        [SerializeField] Sprite RedOutline;
        [SerializeField] Image OutlineImage;

        MissionDatatype datatype;

        public void Init(MissionDatatype _type,BaseMission mission)
        {
            mymission = mission;
            datatype = _type;
            RewardedCheck.SetActive(false);

            slider.value = (float)((float)mymission.GetCurCount() / (float)mymission.GetMaxCount());
            count.text = string.Format("{0}/{1}", mymission.GetCurCount(), mymission.GetMaxCount());
            title.text = string.Format(mymission.GetMissionTitle(),mymission.GetMaxCount());
            RewardText.text = mymission.GetRewardValue().ToString();

            if(mymission.GetMissionType()==MissionType.CLEAR_COUNT)
            {
                FillBar.sprite = RedBar;
                confirmbtn_Red.gameObject.SetActive(true);
                confirmbtn_Green.gameObject.SetActive(false);
                confirmbtn_Red.onClick.AddListener(GetRewardItem);
            }
            else
            {
                FillBar.sprite = greenBar;
                confirmbtn_Red.gameObject.SetActive(false);
                confirmbtn_Green.gameObject.SetActive(true);
                confirmbtn_Green.onClick.AddListener(GetRewardItem);
            }
            RewardIcon.sprite = Common.InGameManager.Instance.UIIconImageList[mission.GetRewardType()];

            ButtonUpdate();

            if(mymission.GetMissionType()==MissionType.CLEAR_COUNT)
            {
                OutlineImage.sprite = PurpleOutline;
            }
            else
            {
                OutlineImage.sprite = RedOutline;
            }
           

            Message.AddListener<InGame.Event.MissionValueUpdate>(MissionDataUpdate);
        }

        public void Release()
        {
            Message.RemoveListener<InGame.Event.MissionValueUpdate>(MissionDataUpdate);
        }

        void GetRewardItem()
        {
            if (mymission.IsComplete() == false)
                return;

            CurrencyType currency = CurrencyType.Gem;
            if (mymission.GetRewardType() == RewardType.REWARD_BOX)
            {
                currency = Common.InGameManager.Instance.GetPlayerData.rewardinfo.RewardtypeToCurrencyType(mymission.GetRewardType(),
                    mymission.GetRewardValue());
            }
            else
            {
                currency = Common.InGameManager.Instance.GetPlayerData.rewardinfo.RewardtypeToCurrencyType(mymission.GetRewardType());
            }

            Common.InGameManager.Instance.GetPlayerData.AddCurrency(currency, mymission.GetRewardValue());
            SoundManager.Instance.PlaySound((int)SoundType.Fire);
            mymission.CompleteMissionData();
            slider.value = (float)((float)mymission.GetCurCount() / (float)mymission.GetMaxCount());
            count.text = string.Format("{0}/{1}", mymission.GetCurCount(), mymission.GetMaxCount());

            if(mymission.GetType()==typeof(Data_Mission.DailyMission))
                Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.CLEAR_COUNT, 1);

            ButtonUpdate();
        }

        void MissionDataUpdate(InGame.Event.MissionValueUpdate msg)
        {
            if (msg.missiontype != mymission.GetMissionType())
                return;

            slider.value = (float)((float)mymission.GetCurCount() / (float)mymission.GetMaxCount());
            count.text = string.Format("{0}/{1}", mymission.GetCurCount(), mymission.GetMaxCount());

            ButtonUpdate();
        }

        void ButtonUpdate()
        {
            AlreadyComplete.SetActive(false);
            NotCompleteBtn.gameObject.SetActive(false);
            confirmbtn_Green.gameObject.SetActive(false);
            confirmbtn_Red.gameObject.SetActive(false);
            title.color = Color.yellow;
            //완료되어 보상 받기 가능
            if (mymission.IsComplete())
            {
                if (mymission.GetMissionType() == MissionType.CLEAR_COUNT)
                    confirmbtn_Red.gameObject.SetActive(true);
                else
                    confirmbtn_Green.gameObject.SetActive(true);

             
            }
            else
            {
                //완료되고 이미 받음
                if (mymission.take == true)
                {
                    AlreadyComplete.gameObject.SetActive(true);
                    RewardedCheck.SetActive(true);
                    title.color = Color.grey;
                }
                else
                {
                    NotCompleteBtn.gameObject.SetActive(true);
                }
            }
        }
    }

}
