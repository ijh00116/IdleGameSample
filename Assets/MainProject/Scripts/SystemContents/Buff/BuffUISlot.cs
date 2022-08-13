using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackTree.InGame;

namespace BlackTree
{
    public class BuffUISlot : MonoBehaviour
    {
        public BuffType BuffType;

        public Button BuffActiveBtn;
        public Button BuffActiveBtnForGem;
        public Image InActiveImage;
        public Text LeftActiveTime;


        public void Init()
        {
            BuffActiveBtnForGem.gameObject.SetActive(true);
            BuffActiveBtn.gameObject.SetActive(true);
            InActiveImage.gameObject.SetActive(false);
            LeftActiveTime.gameObject.SetActive(false);

            bool Activate=false;
            int Time=0;
            switch (BuffType)
            {
                case InGame.BuffType.AttackPower:
                    Time = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftAttackPowerBuffTime;
                    Activate = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftAttackPowerBuffActivate;
                    break;
                case InGame.BuffType.AttackSpeed:
                    Time = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftAttackSpeedBuffTime;
                    Activate = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftAttackSpeedBuffActivate;
                    break;
                case InGame.BuffType.MonsterGold:
                    Time = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftMonsterGoldBuffTime;
                    Activate = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftMonsterGoldBuffActivate;
                    break;
                case InGame.BuffType.MoveSpeed:
                    Time = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftMoveSpeedBuffTime;
                    Activate = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftMoveSpeedBuffActivate;
                    break;
                case InGame.BuffType.MonsterPotion:
                    Time = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftMonsterPotionBuffTime;
                    Activate = Common.InGameManager.Instance.GetPlayerData.GlobalUser.LeftMonsterPotionBuffActivate;
                    break;
                case InGame.BuffType.End:
                    break;
                default:
                    break;
            }
            if(Activate)
            {
                InActiveImage.gameObject.SetActive(true);
                LeftActiveTime.gameObject.SetActive(true);
                BuffActiveBtn.gameObject.SetActive(false);
                BuffActiveBtnForGem.gameObject.SetActive(false);

                Message.Send<InGame.Event.BuffActivate>(new InGame.Event.BuffActivate(BuffType, Time));
            }
       

            BuffActiveBtn.onClick.AddListener(PushButton);
            BuffActiveBtnForGem.onClick.AddListener(PushButtonForGem);

            Message.AddListener<InGame.Event.BuffTimer>(BuffTimer);
        }

        public void Release()
        {
            BuffActiveBtn.onClick.RemoveAllListeners();
            Message.RemoveListener<InGame.Event.BuffTimer>(BuffTimer);
        }

        void PushButton()
        {
            Common.InGameManager.Instance.admob.ShowRewardAd(ActivateBuff);
        }

        void PushButtonForGem()
        {
            if(Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value>=100)
            {
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem, -100);
                ActivateBuff();
            }
        }

        void ActivateBuff()
        {
            InActiveImage.gameObject.SetActive(true);
            LeftActiveTime.gameObject.SetActive(true);
            BuffActiveBtn.gameObject.SetActive(false);
            BuffActiveBtnForGem.gameObject.SetActive(false);

            Message.Send<InGame.Event.BuffActivate>(new InGame.Event.BuffActivate(BuffType, DTConstraintsData.BuffTime));
        }

        void BuffTimer(InGame.Event.BuffTimer msg)
        {
            if (msg.Bufftype != BuffType)
                return;
            if(msg.ElapsedTime<=0)
            {
                InActiveImage.gameObject.SetActive(false);
                LeftActiveTime.gameObject.SetActive(false);
                BuffActiveBtn.gameObject.SetActive(true);
                BuffActiveBtnForGem.gameObject.SetActive(true);
            }
            else
            {
                InActiveImage.gameObject.SetActive(true);
                LeftActiveTime.gameObject.SetActive(true);
                BuffActiveBtn.gameObject.SetActive(false);
                BuffActiveBtnForGem.gameObject.SetActive(false);
            }
            int lefttime =(int)msg.ElapsedTime;

            int m = lefttime / 60;
            int s = lefttime % 60;

            LeftActiveTime.text = string.Format("{0:D2}:{1:D2}", m, s);
        }
    }
}

