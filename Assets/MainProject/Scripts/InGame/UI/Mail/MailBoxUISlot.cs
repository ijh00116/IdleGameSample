using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class MailBoxUISlot : MonoBehaviour
    {
        [SerializeField] Button RewardBtn;
        [SerializeField] Text MailName;
        [SerializeField] Text AmountText;

        System.Action<string> ButtonCallback;
        string inDate;

        int GemRewardAmount;
        public void Init(string title,int amount,string indate,System.Action<string> RecieveMail)
        {
            RewardBtn.onClick.AddListener(PushBtn);

            MailName.text = title;
            AmountText.text = string.Format("x:{0}",amount);
            GemRewardAmount = amount;
            inDate = indate;
            ButtonCallback = RecieveMail;
        }

        void PushBtn()
        {
            ButtonCallback?.Invoke(inDate);
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem, GemRewardAmount);
            this.gameObject.SetActive(false);
        }
    }

}
