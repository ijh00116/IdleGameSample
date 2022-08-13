using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class RouletteTimer : MonoBehaviour
    {
        [SerializeField] Button AdButton;
        [SerializeField] Text LeftText;
        [SerializeField] GameObject AdButtonText;

        private void Awake()
        {
            AdButton.onClick.AddListener(PushAdRewardGemBtn);

            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeRouletAd == false)
            {
                LeftText.gameObject.SetActive(true);
                AdButtonText.gameObject.SetActive(false);
            }
            else
            {
                LeftText.gameObject.SetActive(false);
                AdButtonText.gameObject.SetActive(true);
            }
        }
        void Update()
        {
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeRouletAd == false)
            {
                Common.InGameManager.Instance.GetPlayerData.GlobalUser.RoulettAdLeftTime += Time.deltaTime;

                int lefttime = (DTConstraintsData.AD_ROULET_COOLTIME - (int)Common.InGameManager.Instance.GetPlayerData.GlobalUser.RoulettAdLeftTime);

                int m = lefttime / 60;
                int s = lefttime % 60;

                LeftText.text = string.Format("{0:D2}:{1:D2}", m, s);

                if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.RoulettAdLeftTime >= DTConstraintsData.AD_ROULET_COOLTIME)
                {
                    Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeRouletAd = true;
                    LeftText.gameObject.SetActive(false);
                    AdButtonText.SetActive(true);
                }
            }
        }

        void PushAdRewardGemBtn()
        {
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeRouletAd == false)
                return;
            Common.InGameManager.Instance.admob.ShowRewardAd(() => Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.RouletteCoupon, 2));
            Common.InGameManager.Instance.GetPlayerData.GlobalUser.CanSeeRouletAd = false;
            Common.InGameManager.Instance.GetPlayerData.GlobalUser.RoulettAdLeftTime = 0;
            LeftText.gameObject.SetActive(true);
            AdButtonText.gameObject.SetActive(false);
        }
    }

}
