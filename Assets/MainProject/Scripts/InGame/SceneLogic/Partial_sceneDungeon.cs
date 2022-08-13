using BlackTree.Common;
using BlackTree.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public partial class SceneFSM
    {
        public float CurrentDungeonTime=0;
        void DungeonRegisterInit()
        {
            StateCallback _state = new StateCallback() { OnEnter = DunGeon_Enter, Update = DunGeon_Update, OnExit = DunGeon_Exit };
            _State.stateLookup.Add(ePlayScene.DunGeonInit, _state);
            _state = new StateCallback() { OnEnter = DunGeonUpdate_Enter, Update = DunGeonUpdate_Update, OnExit = DunGeonUpdate_Exit };
            _State.stateLookup.Add(ePlayScene.DunGeonUpdate, _state);
            _state = new StateCallback() { OnEnter = DunGeonRelease_Enter, Update = DunGeonRelease_Update, OnExit = DunGeonRelease_Exit };
            _State.stateLookup.Add(ePlayScene.DunGeonRelease, _state);
        }

        #region 던전 시작       
        [HideInInspector]public float DungencurrentTime;
        void DunGeon_Enter()
        {
            playerData.stage_Info.CowStageDataUpdate();
            
            InGameFader.Instance.FadeGame(ResetDungeon());

            Message.Send<UI.Event.DungeonStart>(new UI.Event.DungeonStart());
            UI.IDialog.RequestDialogEnter<DungeonDialog>();

            CurrentDungeonTime = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCowDungeonTime();
        }

        IEnumerator ResetDungeon()
        {
            Common.InGameManager.Instance.GetPlayerData.CowWave = 0;
            InGame.BTOPsetPosition.Instance.CowCombatRegen();
            mainCharacter.GetComponent<InGame.CharacterController>().ResetPosition(false);
            InGameManager.Instance.MainCharacterActivate(true);
            InGameManager.Instance.CinemachineCamActive(true);
            yield break;
        }
        
        void DunGeon_Update()
        {
            DungencurrentTime += Time.deltaTime;
            if (DungencurrentTime > CurrentDungeonTime)
            {
                _State.ChangeState(ePlayScene.DunGeonRelease);
                DungencurrentTime = 0;
            }
        }
        void DunGeon_Exit()
        {
            int cowLevel = playerData.stage_Info.CowCurrentLevel;
            int killcount = playerData.stage_Info.CowCurrentKillCount;
            int kingKillCount= playerData.stage_Info.KingCowCurrentKillCount;
            if (playerData.stage_Info.stagesubinfo.Dungeoninfo[cowLevel].KillCount < killcount)
            {
                playerData.stage_Info.stagesubinfo.Dungeoninfo[cowLevel].KillCount = killcount;
            }
            if (playerData.stage_Info.stagesubinfo.Dungeoninfo[cowLevel].KingKillCount < kingKillCount)
            {
                playerData.stage_Info.stagesubinfo.Dungeoninfo[cowLevel].KingKillCount = kingKillCount;
            }
            //재화 추가
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Soul, InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeondata.reward_soul * (killcount+kingKillCount));
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.EnchantStone, InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeondata.reward_soul * (killcount + kingKillCount));
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicStone, InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeondata.reward_soul * killcount);
            //재화 추가
            InGameManager.Instance.Localdata.SaveData(InGameManager.Instance.GetPlayerData.saveData);
            //BackendManager.Instance.SaveListDBData(DTConstraintsData.UserData, InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeoninfo);
        }
        public void DungeounWaveUpdate()
        {
            if (playerData.CowWave >= playerData.stage_Info.MaxWave())
            {
                InGameFader.Instance.FadeGame(ResetDungeon());
            }
        }
        #endregion

        #region 던전 진행
        void DunGeonUpdate_Enter()
        {

        }

        void DunGeonUpdate_Update()
        {

        }

      

        void DunGeonUpdate_Exit()
        {

        }
        #endregion

        #region 던전 종료
        void DunGeonRelease_Enter()
        {
            mainCharacter._state.ChangeState(eActorState.Idle);
            Message.Send<UI.Event.DungeonEnd>(new UI.Event.DungeonEnd());
        }

        void DunGeonRelease_Update()
        {
           
        }
        void DunGeonRelease_Exit()
        {
            Message.Send<UI.Event.DungeonEndStartMain>(new UI.Event.DungeonEndStartMain());
            UI.IDialog.RequestDialogExit<DungeonDialog>();
        }
        #endregion
    }
}
