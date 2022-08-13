using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class BattlePassSlot : MonoBehaviour
    {
        [SerializeField] Button Normalrewardbtn;
        [SerializeField] Image NormalRewardImage;
        [SerializeField] Image NormalCantRewardImage;
        [SerializeField] Text Normalrewardtext;
        [SerializeField] Image NormalRewardIconImage;
        [SerializeField] GameObject NormalRewardLock;

        [SerializeField] Button Payrewardbtn;
        [SerializeField] Image PayRewardImage;
        [SerializeField] Image PayCantRewardImage;
        [SerializeField] Text Payrewardtext;
        [SerializeField] Image PayRewardIconImage;
        [SerializeField] GameObject PayRewardLock;
        [SerializeField] Text BattlePassCount;

        BattlePassDay myBattlePassInfo;

        BattlePassType _BpType;
        public bool CanReward;
        public void Init(BattlePassDay battle,BattlePassType _type)
        {
            CanReward = false;
            Normalrewardbtn.onClick.AddListener(PushNormalBtn);
            Payrewardbtn.onClick.AddListener(PushPaidBtn);

            myBattlePassInfo = battle;
            _BpType = _type;

            LocalValue normalrewardtype = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == myBattlePassInfo.tableData.free_reward_type.ToString());
            LocalValue payrewardtype = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == myBattlePassInfo.tableData.pass_reward_type.ToString());
            Normalrewardtext.text = string.Format("{0}:{1}", normalrewardtype.GetStringForLocal(true), myBattlePassInfo.tableData.free_reward);
            Payrewardtext.text = string.Format("{0}:{1}", payrewardtype.GetStringForLocal(true), myBattlePassInfo.tableData.pass_reward);

            if(myBattlePassInfo.IsFreeTaken)
                Normalrewardbtn.gameObject.SetActive(false);

            if (myBattlePassInfo.IsPaidTaken)
                Payrewardbtn.gameObject.SetActive(false);

            switch (_BpType)
            {
                case BattlePassType.UserLevel:
                    BattlePassCount.text = string.Format("LV.{0}", myBattlePassInfo.tableData.Count);
                    break;
                case BattlePassType.Fairy:
                    BattlePassCount.text = string.Format("{0}", myBattlePassInfo.tableData.Count);
                    break;
                case BattlePassType.PlayingTime:
                    BattlePassCount.text = string.Format("{0}s", myBattlePassInfo.tableData.Count);
                    break;
                default:
                    break;
            }
           
        }

        public void UpdateCount(int count)
        {
            if (count >= myBattlePassInfo.tableData.Count)
            {
                if (myBattlePassInfo.IsFreeTaken==false || myBattlePassInfo.IsPaidTaken==false)
                {
                    CanReward = true;
                }
                else
                    CanReward = false;
            }
                
            else
                CanReward = false;

            if (CanReward)
            {
                NormalCantRewardImage.gameObject.SetActive(false);
                PayCantRewardImage.gameObject.SetActive(false);
                //NormalRewardLock.SetActive(false);
                //PayRewardLock.SetActive(false);
          
            }
            else
            {
                NormalCantRewardImage.gameObject.SetActive(true);
                PayCantRewardImage.gameObject.SetActive(true);
                //NormalRewardLock.SetActive(true);
                //PayRewardLock.SetActive(true);
                //Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.Battlepass, false));
            }
            //현재 배틀패스 결제는 일단 잠금
            //PayRewardLock.SetActive(true);

            if (myBattlePassInfo.IsFreeTaken)
                NormalRewardImage.color = Color.red;
            else
                NormalRewardImage.color = Color.white;

            if (myBattlePassInfo.IsPaidTaken)
                PayRewardImage.color = Color.red;
            else
                PayRewardImage.color = Color.white;
        }

        void PushNormalBtn()
        {
            if (CanReward == false)
                return;
            if (myBattlePassInfo.IsFreeTaken)
                return;

            Normalrewardbtn.gameObject.SetActive(false);
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(myBattlePassInfo.tableData.free_reward_type, myBattlePassInfo.tableData.free_reward);
            SoundManager.Instance.PlaySound((int)SoundType.Fire);
            myBattlePassInfo.IsFreeTaken = true;
            NormalRewardImage.color = Color.red;

            Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.Battlepass, false));
        }

        void PushPaidBtn()
        {
            if (CanReward == false)
                return;
            if (myBattlePassInfo.IsPaidTaken)
                return;

            Payrewardbtn.gameObject.SetActive(false);
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(myBattlePassInfo.tableData.pass_reward_type, myBattlePassInfo.tableData.pass_reward);
            SoundManager.Instance.PlaySound((int)SoundType.Fire);
            PayRewardImage.color = Color.red;
            myBattlePassInfo.IsPaidTaken = true;

            Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.Battlepass, false));
        }
    }

}
