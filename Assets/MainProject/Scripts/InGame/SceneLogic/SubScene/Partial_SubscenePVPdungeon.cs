using BlackTree.Common;
using BlackTree.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public partial class SubSceneFSM
    {
        public PvpDungeonController pvpDungeonController;
        void pvpDungeonRegisterInit()
        {
            StateCallback _state = new StateCallback() { OnEnter = pvpDunGeonLoad_Enter, Update = pvpDunGeonLoad_Update, OnExit = pvpDunGeonLoad_Exit };
            _State.stateLookup.Add(ePlaySubScene.pvpDunGeonLoad, _state);
            _state = new StateCallback() { OnEnter = pvpDunGeonWait_Enter, Update = pvpDunGeonWait_Update, OnExit = pvpDunGeonWait_Exit };
            _State.stateLookup.Add(ePlaySubScene.pvpDunGeonWait, _state);



            _state = new StateCallback() { OnEnter = pvpDunGeonEnter_Enter, Update = pvpDunGeonEnter_Update, OnExit = pvpDunGeonEnter_Exit };
            _State.stateLookup.Add(ePlaySubScene.pvpDunGeonInit, _state);

            _state = new StateCallback() { OnEnter = pvpDunGeonUpdate_Enter, Update = pvpDunGeonUpdate_Update, OnExit = pvpDunGeonUpdate_Exit };
            _State.stateLookup.Add(ePlaySubScene.pvpDunGeonUpdate, _state);

            _state = new StateCallback() { OnEnter = pvpDunGeonRelease_Enter, Update = pvpDunGeonRelease_Update, OnExit = pvpDunGeonRelease_Exit };
            _State.stateLookup.Add(ePlaySubScene.pvpDunGeonRelease, _state);
        }

        #region 게임 첫 시작     
        [HideInInspector] public float pvpDungencurrentTime;
        void pvpDunGeonLoad_Enter()
        {
            StartCoroutine(pvpStartSetting());
        }
        //내 캐릭터 세팅
        IEnumerator pvpStartSetting()
        {
            pvpDungeonController.Init();

            _State.ChangeState(ePlaySubScene.pvpDunGeonWait);
            yield break;
        }

        void pvpDunGeonLoad_Update()
        {

        }
        void pvpDunGeonLoad_Exit()
        {

        }

        #endregion


        #region pvp던전 대기상태 시작       
        void pvpDunGeonWait_Enter()
        {

        }

        void pvpDunGeonWait_Update()
        {

        }
        void pvpDunGeonWait_Exit()
        {

        }
        #endregion

        #region 던전 시작       =>ui에서 호출함 적캐릭터 생성후 시간 지나면 싸움 시작
        void pvpDunGeonEnter_Enter()
        {
            pvpDungencurrentTime = 0;
            pvpDungeonController.PvpStart();
            UI.IDialog.RequestDialogExit<UI.PlayerdataDialog>();

            _State.ChangeState(ePlaySubScene.pvpDunGeonUpdate);
        }
        //시간 지나서 싸움 시작
        void pvpDunGeonEnter_Update()
        {
            
        }
        void pvpDunGeonEnter_Exit()
        {

        }
        #endregion

        #region 던전 진행
        void pvpDunGeonUpdate_Enter()
        {
            pvpDungencurrentTime = DTConstraintsData.PVPMaxTime;
        }

        void pvpDunGeonUpdate_Update()
        {
            pvpDungencurrentTime -= Time.deltaTime;
            if (pvpDungencurrentTime < 0)
            {
                _State.ChangeState(ePlaySubScene.pvpDunGeonRelease);
                pvpDungencurrentTime = 0;
            }
        }

        void pvpDunGeonUpdate_Exit()
        {

        }
        #endregion

        #region 던전 종료
        void pvpDunGeonRelease_Enter()
        {
            _State.ChangeState(ePlaySubScene.pvpDunGeonWait);
        }

        void pvpDunGeonRelease_Update()
        {

        }
        void pvpDunGeonRelease_Exit()
        {
            pvpDungeonController.Release();
            UI.IDialog.RequestDialogEnter<UI.PlayerdataDialog>();
        }
        #endregion
    }


}
