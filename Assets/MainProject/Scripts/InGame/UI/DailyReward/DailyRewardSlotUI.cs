using BlackTree.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class DailyRewardSlotUI : MonoBehaviour
    {
        [SerializeField] Button RewardButton;
        [SerializeField] Text AmountInfo;
        [SerializeField] Text DayText;
        [SerializeField] Image CheckIcon;
        [SerializeField] Image IconImage;
        [SerializeField] Animator anim;

        int DayIndex;

        DailyRewardData rewardInfo;
        DailyRewardUI dailyrewardWindow;
        public void Init(DailyRewardUI dailywindow,int dayindex,bool Canreward)
        {
            DayIndex = dayindex;
            dailyrewardWindow = dailywindow;
            rewardInfo = InGameDataTableManager.DailyRewardTableList.attendance.Find(o => o.day == (DayIndex + 1));
            AmountInfo.text = string.Format("x{0}", rewardInfo.count);
            DayText.text = (DayIndex + 1).ToString();
            anim.enabled = false;

            int CanRewardIndex = Common.InGameManager.Instance.GetPlayerData.GlobalUser.RewardIndex + 1;
            if (CanRewardIndex > DTConstraintsData.RewardDay)
                CanRewardIndex = 0;
            RewardButton.onClick.AddListener(TouchRewardButton);

            if (Canreward)
            {
                if(dayindex<CanRewardIndex)
                {
                    RewardButton.enabled = false;
                    CheckIcon.gameObject.SetActive(true);
                }
                else if(dayindex==CanRewardIndex)
                {
                    RewardButton.enabled = true;
                    dailyrewardWindow.RewardAttendCallback = TouchRewardButton;
                    anim.enabled = true;
                    CheckIcon.gameObject.SetActive(false);
                }
                else
                {
                    RewardButton.enabled = false;
                    CheckIcon.gameObject.SetActive(false);
                }
            }
            else
            {
                if (dayindex < CanRewardIndex)
                {
                    RewardButton.enabled = false;
                    CheckIcon.gameObject.SetActive(true);
                }
                else
                {
                    RewardButton.enabled = false;
                    CheckIcon.gameObject.SetActive(false);
                }
            }
            IconImage.sprite = Common.InGameManager.Instance.UIIconImageList[rewardInfo.reward_type];
        }

        void TouchRewardButton()
        {
            if (RewardButton.enabled == false)
                return;
            switch (rewardInfo.reward_type)
            {
                case RewardType.REWARD_GEM:
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem, rewardInfo.count);
                    break;
                case RewardType.REWARD_ESSENCE:
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.SuperMagicPotion, rewardInfo.count);
                    break;
                case RewardType.REWARD_POTION:
                    break;
                case RewardType.REWARD_SOUL:
                    break;
                case RewardType.REWARD_ENCHANT_STONE:
                    break;
                case RewardType.REWARD_MAGIC_STONE:
                    break;
                case RewardType.REWARD_DG_TICKET:
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Ticket_Dungeon, rewardInfo.count);
                    break;
                case RewardType.REWARD_PVP_TICKET:
                    break;
                case RewardType.REWARD_PET_TICKET:
                    break;
                case RewardType.REWARD_MILEAGE:
                    break;
                case RewardType.REWARD_BOX:
                    AddBoxtoItem();
                    break;
                case RewardType.REWARD_BUFF_ATK:
                    break;
                case RewardType.REWARD_GACHA_TICKET:
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Ticket_Gacha, rewardInfo.count);
                    break;
                default:
                    break;
            }
            

            RewardButton.enabled = false;
            CheckIcon.gameObject.SetActive(true);
            anim.enabled = false;
            IconImage.color = new Color(1, 1, 1, 1);

            Common.InGameManager.Instance.GetPlayerData.GlobalUser.GetRewardDay = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LoginTime.DayOfYear;
            Common.InGameManager.Instance.GetPlayerData.GlobalUser.RewardIndex++;

            Common.InGameManager.Instance.Localdata.SaveData(Common.InGameManager.Instance.GetPlayerData.saveData);
            //BackendManager.Instance.SaveDBData(DTConstraintsData.UserData,Common.InGameManager.Instance.GetPlayerData.GlobalUser);

            SoundManager.Instance.PlaySound((int)SoundType.Fire);

            LocalValue typename = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == rewardInfo.reward_type.ToString());

            Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(string.Format("{0} {1}획득", typename.GetStringForLocal(true), rewardInfo.count)));
            Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(SideButtonType.DailyReward, false));

        }

        void AddBoxtoItem()
        {
            CurrencyType ctype= Common.InGameManager.Instance.GetPlayerData.Playercurrency.GetIdxToType(rewardInfo.box_idx);
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(ctype, rewardInfo.count);
        }
    }

}
