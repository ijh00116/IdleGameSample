using BlackTree.Common;
using BlackTree.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class DungeonDialog : IDialog
    {
        [Header("던전 타이틀,시간")]
        [SerializeField] Text DungeonName;
        [SerializeField] Text BossRate;
        [SerializeField] Text GoblinKillCountLocal;
        [SerializeField] Text GoblinKingKillCountLocal;
        [SerializeField] Text TotalKillCountLocal;
        [SerializeField] Text Time;
        //던전 킬 카운트
        [SerializeField] Text GoblinKillCount;
        [SerializeField] Text OgreKillCount;
        [SerializeField] Text TotalKillCount;
        //던전 보상
        [SerializeField] Text RewardSoul;
        [SerializeField] Text RewardEnchantStone;
        [SerializeField] Text RewardMagicStone;

        //던전 종료 윈도우
        [SerializeField] GameObject DungeonEndWindow;
        [SerializeField] Button DungeonEndButton;
        [SerializeField] Text DungeonNameInEnd;
        //던전 킬 카운트
        [SerializeField] Text GoblinKillCountInEnd;
        [SerializeField] Text OgreKillCountInEnd;
        [SerializeField] Text TotalKillCountInEnd;
        //던전 보상
        [SerializeField] Text RewardSoulInEnd;
        [SerializeField] Text RewardEnchantStoneInEnd;
        [SerializeField] Text RewardMagicStoneInEnd;



        DungeonPrime CurrentDungeonPrime;
        protected override void OnLoad()
        {
            base.OnLoad();
            Message.AddListener<UI.Event.DungeonLevelTouch>(SetDungeonPrime);
            Message.AddListener<UI.Event.DungeonEnd>(dungeonEnd);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Message.RemoveListener<UI.Event.DungeonLevelTouch>(SetDungeonPrime);
            Message.RemoveListener<UI.Event.DungeonEnd>(dungeonEnd);
        }
        protected override void OnEnter()
        {
            DungeonEndWindow.SetActive(false);
            DungeonEndButton.onClick.AddListener(GoToMain);
        }

        protected override void OnExit()
        {
            DungeonEndButton.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (this.DialogView.gameObject.activeInHierarchy == false)
                return;
            Time.text = string.Format("{0:D2}:{1:D2}", FloatToMin(InGameManager.Instance._sceneFsm.CurrentDungeonTime - InGameManager.Instance._sceneFsm.DungencurrentTime),
                 FloatToSec(InGameManager.Instance._sceneFsm.CurrentDungeonTime - InGameManager.Instance._sceneFsm.DungencurrentTime));
            GoblinKillCount.text = string.Format("{0} KILL", InGameManager.Instance.GetPlayerData.stage_Info.CowCurrentKillCount);
            OgreKillCount.text = string.Format("{0} KILL", InGameManager.Instance.GetPlayerData.stage_Info.KingCowCurrentKillCount);
            TotalKillCount.text = string.Format("{0} KILL", InGameManager.Instance.GetPlayerData.stage_Info.CowCurrentKillCount+
                InGameManager.Instance.GetPlayerData.stage_Info.KingCowCurrentKillCount);
            RewardTextSet();
        }

        int FloatToMin(float time)
        {
            int lefttime = (int)time;

            int m = lefttime / 60;

            return m;
        }
        int FloatToSec(float time)
        {
            int lefttime = (int)time;

            int s = lefttime % 60;

            return s;
        }

        void RewardTextSet()
        {
            int killcount = InGameManager.Instance.GetPlayerData.stage_Info.CowCurrentKillCount;
            int kingkillcount = InGameManager.Instance.GetPlayerData.stage_Info.KingCowCurrentKillCount;
            RewardSoul.text = string.Format("x{0}", CurrentDungeonPrime.reward_soul * (killcount+ kingkillcount));
            RewardEnchantStone.text = string.Format("x{0}",CurrentDungeonPrime.reward_enchant_stone * (killcount+ kingkillcount));
            RewardMagicStone.text = string.Format("x{0}",CurrentDungeonPrime.reward_magic_stone * kingkillcount);
        }

        void SetDungeonPrime(UI.Event.DungeonLevelTouch msg)
        {
            CurrentDungeonPrime = msg.dungeondata;
            DungeonName.text = string.Format("유적지 {0}층", CurrentDungeonPrime.dg_stage);
            DungeonNameInEnd.text = string.Format("유적지 {0}층 결과", CurrentDungeonPrime.dg_stage);
            float kingrate = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCowKingRate();

            LocalValue TotalKillcountInfo= InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_090");
            LocalValue CowKillcountInfo = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_091");
            LocalValue KingKillcountInfo = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_092");
            LocalValue bossrateInfo = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_093");
            GoblinKillCountLocal.text = string.Format(CowKillcountInfo.GetStringForLocal(true));
            GoblinKingKillCountLocal.text = string.Format(KingKillcountInfo.GetStringForLocal(true));
            TotalKillCountLocal.text = string.Format(TotalKillcountInfo.GetStringForLocal(true));
            BossRate.text = string.Format("{0} <color=green>{1:N2}%</color>", bossrateInfo.GetStringForLocal(true), kingrate);
        }
        void dungeonEnd(UI.Event.DungeonEnd msg)
        {
            DungeonEndWindow.SetActive(true);
            int killcount = InGameManager.Instance.GetPlayerData.stage_Info.CowCurrentKillCount;
            GoblinKillCountInEnd.text = string.Format("{0} KILL", InGameManager.Instance.GetPlayerData.stage_Info.CowCurrentKillCount);
            OgreKillCountInEnd.text = string.Format("{0} KILL", InGameManager.Instance.GetPlayerData.stage_Info.KingCowCurrentKillCount);
            TotalKillCountInEnd.text = string.Format("{0} KILL", InGameManager.Instance.GetPlayerData.stage_Info.CowCurrentKillCount +
                InGameManager.Instance.GetPlayerData.stage_Info.KingCowCurrentKillCount);
            RewardSoulInEnd.text = string.Format("x{0}", CurrentDungeonPrime.reward_soul * killcount);
            RewardEnchantStoneInEnd.text = string.Format("x{0}", CurrentDungeonPrime.reward_enchant_stone * killcount);
            RewardMagicStoneInEnd.text = string.Format("x{0}", CurrentDungeonPrime.reward_magic_stone * killcount);
        }

        void GoToMain()
        {
            InGameManager.Instance._sceneFsm._State.ChangeState(ePlayScene.MainInit);
        }
    }

}
