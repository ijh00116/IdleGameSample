using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class AdmobManager : MonoBehaviour
    {
        public bool isTestMode=true;
        //public Text LogText;
        public Button FrontAdsBtn, RewardAdsBtn;

        System.Action RewardCallback;
        void Start()
        {
            LoadBannerAd();
            LoadFrontAd();
            LoadRewardAd();
        }

        void Update()
        {
            //FrontAdsBtn.interactable = frontAd.IsLoaded();
           // RewardAdsBtn.interactable = rewardAd.IsLoaded();
        }

   



        #region 배너 광고
        const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
        const string bannerID = "";


        void LoadBannerAd()
        {
         
            ToggleBannerAd(false);
        }

        public void ToggleBannerAd(bool b)
        {
        }
        #endregion



        #region 전면 광고
        const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
        const string frontID = "";


        void LoadFrontAd()
        {
            
        }

        public void ShowFrontAd()
        {
            LoadFrontAd();
        }
        #endregion

        #region 리워드 광고
        //const string rewardTestID = "ca-app-pub-3940256099942544/1033173712";
        const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
        const string rewardID = "";


        void LoadRewardAd()
        {
           
        }

        public void ShowRewardAd(System.Action action)
        {
            LoadRewardAd();
            RewardCallback = action;

            StartCoroutine(ShowScreenAd());
        }

        IEnumerator ShowScreenAd()
        {
            yield break;
        }
        #endregion
    }
}
