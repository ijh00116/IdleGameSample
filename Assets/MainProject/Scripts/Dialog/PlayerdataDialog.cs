using BlackTree.Common;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

namespace BlackTree.UI
{
    public class PlayerdataDialog : IDialog
    {
        [Header("골드,마법물약,보석")]
        [SerializeField] Text Gem;
        [SerializeField] Text Gold;
        [SerializeField] Text MagicPotion;

        [Header("유저 정보")]
        [SerializeField] Button UserImage;
        [SerializeField] Text UserInfoText;
        [SerializeField] Text UserNickName;
        [SerializeField] Slider UserEXPBar;

        [Header("옵션")]
        [SerializeField] Button OptionButton;
        [SerializeField] OptionWindow OptionWindow;

        [Header("보스클리어")]
        [SerializeField] SkeletonGraphic WinSpinAnim;
        protected override void OnEnter()
        {
            base.OnEnter();
            UserInfoToDisplay(null);

            Message.AddListener<UI.Event.CurrencyChange>(CurrencyToDisplay);
            Message.AddListener<UI.Event.UserInfoUIUpdate>(UserInfoToDisplay);
            StartSetting();

            OptionButton.onClick.AddListener(() => OptionWindow.gameObject.SetActive(true));

            OptionWindow.Init();

           // UserNickName.text = BackendManager.Instance.GetNickName();

            OptionWindow.gameObject.SetActive(false);

            Message.AddListener<UI.Event.DungeonStart>(StartDungeon);
            Message.AddListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);

            Message.AddListener<UI.Event.MainSceneBossEvent>(BossEvent);
            WinSpinAnim.gameObject.SetActive(false);
        }

        protected override void OnExit()
        {
            base.OnExit();
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyToDisplay);
            Message.RemoveListener<UI.Event.UserInfoUIUpdate>(UserInfoToDisplay);

            UserImage.onClick.RemoveAllListeners();

            Message.RemoveListener<UI.Event.DungeonStart>(StartDungeon);
            Message.RemoveListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);

            OptionWindow.Release();
            Message.RemoveListener<UI.Event.MainSceneBossEvent>(BossEvent);
        }

        void StartSetting()
        {
            Currency goldcurrency = InGameManager.Instance.GetPlayerData.Playercurrency.GetCurrency(CurrencyType.Gold);
            Gold.text = goldcurrency.value.ToDisplay();
            Currency mpCurrency = InGameManager.Instance.GetPlayerData.Playercurrency.GetCurrency(CurrencyType.MagicPotion);
            MagicPotion.text = mpCurrency.value.ToDisplay();
            Currency GemCurrency = InGameManager.Instance.GetPlayerData.Playercurrency.GetCurrency(CurrencyType.Gem);
            Gem.text = GemCurrency.value.ToString();

        }

        void CurrencyToDisplay(UI.Event.CurrencyChange msg)
        {
            if(msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.Gold && msg.Type != CurrencyType.Gem && msg.Type != CurrencyType.MagicPotion)
                    return;
            }
            

            switch (msg.Type)
            {
                case CurrencyType.Gold:
                    Gold.text = InGameManager.Instance.GetPlayerData.Playercurrency.GetCurrency(msg.Type).value.ToDisplay();
                    break;
                case CurrencyType.Gem:
                    Gem.text = InGameManager.Instance.GetPlayerData.Playercurrency.GetCurrency(msg.Type).value.ToString();
                    break;
                case CurrencyType.MagicPotion:
                    MagicPotion.text = InGameManager.Instance.GetPlayerData.Playercurrency.GetCurrency(msg.Type).value.ToDisplay();
                    break;
                default:
                    break;
            }
        }
        
        void UserInfoToDisplay(UI.Event.UserInfoUIUpdate msg)
        {
            int _currentexp = InGameManager.Instance.GetPlayerData.GlobalUser.CurrentExp;
            int _maxExp = InGameManager.Instance.GetPlayerData.GlobalUser.NeedExp;
            int _Userlevel= InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL;
            if (msg==null)
            {
                UserInfoText.text = string.Format("레벨:{0}", _Userlevel.ToString());
                UserEXPBar.value = (float)_currentexp / (float)_maxExp;
            }
            else
            {
                UserInfoText.text = string.Format("레벨:{0}", _Userlevel.ToString());
                UserEXPBar.value = (float)_currentexp / (float)_maxExp;
            }
        }
        void StartDungeon(UI.Event.DungeonStart msg)
        {
            DialogView.SetActive(false);
        }

        void ReturnToMainFromDungeon(UI.Event.DungeonEndStartMain msg)
        {
            DialogView.SetActive(true);
        }

        void BossEvent(UI.Event.MainSceneBossEvent msg)
        {
            if (WinSpinAnim.gameObject.activeInHierarchy == false)
                WinSpinAnim.gameObject.SetActive(true);

            WinSpinAnim.AnimationState.SetAnimation(0, "win_action", false);
        }
    }

}
