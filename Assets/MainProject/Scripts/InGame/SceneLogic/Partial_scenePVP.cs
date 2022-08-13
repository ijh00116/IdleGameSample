using BlackTree.Common;
using BlackTree.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public partial class SceneFSM
    {
        void PVPDungeonRegisterInit()
        {
            StateCallback _state = new StateCallback() { OnEnter = PVPDungeon_Enter, Update = PVPDunGeon_Update, OnExit = PVPDunGeon_Exit };
            _State.stateLookup.Add(ePlayScene.PVPDunGeonInit, _state);
            _state = new StateCallback() { OnEnter = PVPDunGeonUpdate_Enter, Update = PVPDunGeonUpdate_Update, OnExit = PVPDunGeonUpdate_Exit };
            _State.stateLookup.Add(ePlayScene.PVPDunGeonUpdate, _state);
            _state = new StateCallback() { OnEnter = PVPDunGeonRelease_Enter, Update = PVPDunGeonRelease_Update, OnExit = PVPDunGeonRelease_Exit };
            _State.stateLookup.Add(ePlayScene.PVPDunGeonRelease, _state);
        }

        #region 던전 시작       
        [HideInInspector] public float PVPDungencurrentTime;
        void PVPDungeon_Enter()
        {
            UI.IDialog.RequestDialogExit<PlayerdataDialog>();
            UI.IDialog.RequestDialogExit<SideDialog>();
            UI.IDialog.RequestDialogExit<SideButtonDialog>();

            UI.IDialog.RequestDialogEnter<DungeonDialog>();
        }

        IEnumerator ResetPVPDungeon()
        {
            //Common.InGameManager.Instance.GetPlayerData.CowWave = 0;
            //InGame.BTOPsetPosition.Instance.CowCombatRegen();
            //mainCharacter._controller.ResetPositionAndMove(false);

            //InGameManager.Instance.MainCharacterActivate(true);
            yield break;
        }

        void PVPDunGeon_Update()
        {
            //DungencurrentTime += Time.deltaTime;
            //if (DungencurrentTime > DTConstraintsData.DG_BATTLE_TIME_SEC)
            //{
            //    InGameManager.Instance.MainCharacterActivate(false);
            //    _State.ChangeState(ePlayScene.DunGeonRelease);
            //    DungencurrentTime = 0;
            //}
        }
        void PVPDunGeon_Exit()
        {
           
        }
        
        #endregion

        #region 던전 진행
        void PVPDunGeonUpdate_Enter()
        {

        }

        void PVPDunGeonUpdate_Update()
        {

        }



        void PVPDunGeonUpdate_Exit()
        {

        }
        #endregion

        #region 던전 종료
        void PVPDunGeonRelease_Enter()
        {

        }

        void PVPDunGeonRelease_Update()
        {

        }
        void PVPDunGeonRelease_Exit()
        {
            UI.IDialog.RequestDialogEnter<PlayerdataDialog>();
            UI.IDialog.RequestDialogEnter<SideDialog>();
            UI.IDialog.RequestDialogEnter<SideButtonDialog>();

            UI.IDialog.RequestDialogExit<DungeonDialog>();
        }
        #endregion

    }
}