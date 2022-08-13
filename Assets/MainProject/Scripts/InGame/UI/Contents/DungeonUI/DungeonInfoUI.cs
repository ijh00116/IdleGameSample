using DLL_Common.Common;
using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class DungeonInfoUI : MonoBehaviour
    {
        public Text DungeonName;
        public Text BestCowKillcount;
        public Text BestNormalCowKillcount;
        public Text BestCowKingKillcount;
        public Text KingRate;
        public Text DungeonTime;
        public Text Reward_Soul_forOne;
        public Text Reward_EnforceStone_forOne;
        public Text Reward_magicStone_forOne;
        public Button DungeonEnterButton;

        DungeonPrime CurrentClickedDungeonInfo;

        [SerializeField] DungeonEnterUI dungeonEnterUI;


        [Header("로컬라이제이션")]
        [SerializeField] Text EnterButtonText;
        [SerializeField] Text ClearButtonText;
        [SerializeField] Text Clear_10ButtonText;
        public void Init()
        {
            DungeonEnterButton.onClick.AddListener(TouchEnterDungeon);
            Message.AddListener<UI.Event.DungeonLevelTouch>(ChangeDungeonInfo);
            dungeonEnterUI.Init();

            LocalValue EnterLocal = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_097");
            LocalValue ClearLocal = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_098");

            EnterButtonText.text = EnterLocal.GetStringForLocal(true);
            ClearButtonText.text = ClearLocal.GetStringForLocal(true);
            Clear_10ButtonText.text =string.Format("{0}x10", ClearLocal.GetStringForLocal(true));

            Notentermsg = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_218");

            Message.AddListener<UI.Event.CharacterInfoUIUpdate>(DungeonInfoDataChanged);
        }

        public void Release()
        {
            DungeonEnterButton.onClick.RemoveAllListeners();
            Message.RemoveListener<UI.Event.DungeonLevelTouch>(ChangeDungeonInfo);
            Message.RemoveListener<UI.Event.CharacterInfoUIUpdate>(DungeonInfoDataChanged);
        }
        UI.Event.FlashPopup flashmsg = new UI.Event.FlashPopup(null);
        LocalValue Notentermsg;
        void TouchEnterDungeon()
        {
            if(InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_Dungeon).value>0)
            {
                InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Ticket_Dungeon, -1);
                InGameManager.Instance.StartCowDungeon(CurrentClickedDungeonInfo.dg_stage - 1);
                //dungeonEnterUI.SetDungeonEnter(EnterDungeonEvent);
                InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.DUNGEON_BATTLE, 1);
            }
            else
            {
                if (flashmsg.Eventmsg==null)
                {
                    flashmsg.Eventmsg= Notentermsg.GetStringForLocal(true);
                }
                Message.Send<UI.Event.FlashPopup>(flashmsg);
            }
            
        }

        void ChangeDungeonInfo(UI.Event.DungeonLevelTouch msg)
        {
            CurrentClickedDungeonInfo = msg.dungeondata;
            //Info.text = string.Format("던전 넘버:{0}\n카우 체력: {1}", CurrentClickedDungeonInfo.dg_stage, CurrentClickedDungeonInfo.monster_hp);
            int dungeonIndex = CurrentClickedDungeonInfo.dg_stage - 1;
            DungeonName.text = string.Format("유적지 {0}층", CurrentClickedDungeonInfo.dg_stage);
            BestCowKillcount.text = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeoninfo[dungeonIndex].KillCount.ToString();
            BestCowKingKillcount.text = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeoninfo[dungeonIndex].KingKillCount.ToString();
            int normalkill = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeoninfo[dungeonIndex].KillCount -
                InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeoninfo[dungeonIndex].KingKillCount;
            BestNormalCowKillcount.text = normalkill.ToString();

            KingRate.text =string.Format("{0:F0}%", (CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCowKingRate()).ToString());
            DungeonTime.text =  CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCowDungeonTime().ToString();

            BigInteger reward_soul= CharacterDataManager.Instance.PlayerCharacterdata.ability.GetDungeonMonsterKillSoulReward(CurrentClickedDungeonInfo.reward_soul);
            BigInteger reward_enforce = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetDungeonMonsterKillEnforceStoneReward(CurrentClickedDungeonInfo.reward_enchant_stone);
            Reward_Soul_forOne.text = reward_soul.ToString();
            Reward_EnforceStone_forOne.text= reward_enforce.ToString();
            Reward_magicStone_forOne.text = CurrentClickedDungeonInfo.reward_magic_stone.ToString();

            DungeonEnterButton.gameObject.SetActive(msg.CanEnter);
        }

        void DungeonInfoDataChanged(UI.Event.CharacterInfoUIUpdate msg)
        {
            BigInteger reward_soul = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetDungeonMonsterKillSoulReward(CurrentClickedDungeonInfo.reward_soul);
            BigInteger reward_enforce = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetDungeonMonsterKillEnforceStoneReward(CurrentClickedDungeonInfo.reward_enchant_stone);
            Reward_Soul_forOne.text = reward_soul.ToString();
            Reward_EnforceStone_forOne.text = reward_enforce.ToString();
            DungeonTime.text = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCowDungeonTime().ToString();
        }
    }

}
