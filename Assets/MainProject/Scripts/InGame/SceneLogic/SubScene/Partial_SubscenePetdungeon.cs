using BlackTree.Common;
using BlackTree.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public partial class SubSceneFSM
    {
        public PetDungeonController petdungeoncontroller;
        void PetDungeonRegisterInit()
        {
            StateCallback _state = new StateCallback() { OnEnter = PetDunGeonLoad_Enter, Update = PetDunGeonLoad_Update, OnExit = PetDunGeonLoad_Exit};
            _State.stateLookup.Add(ePlaySubScene.PetDunGeonLoad, _state);
            _state = new StateCallback() { OnEnter = PetDunGeonWait_Enter, Update = PetDunGeonWait_Update, OnExit = PetDunGeonWait_Exit};
            _State.stateLookup.Add(ePlaySubScene.PetDunGeonWait, _state);



            _state = new StateCallback() { OnEnter = PetDunGeonEnter_Enter, Update = PetDunGeonEnter_Update, OnExit = PetDunGeonEnter_Exit };
            _State.stateLookup.Add(ePlaySubScene.PetDunGeonInit, _state);

            _state = new StateCallback() { OnEnter = PetDunGeonUpdate_Enter, Update = PetDunGeonUpdate_Update, OnExit = PetDunGeonUpdate_Exit };
            _State.stateLookup.Add(ePlaySubScene.PetDunGeonUpdate, _state);

            _state = new StateCallback() { OnEnter = PetDunGeonRelease_Enter, Update = PetDunGeonRelease_Update, OnExit = PetDunGeonRelease_Exit };
            _State.stateLookup.Add(ePlaySubScene.PetDunGeonRelease, _state);
        }

        #region 게임 첫 시작     
        [HideInInspector] public float PetDungencurrentTime;
        void PetDunGeonLoad_Enter()
        {
            StartCoroutine(StartSetting());
        }

        IEnumerator StartSetting()
        {
            petdungeoncontroller.Init();

            _State.ChangeState(ePlaySubScene.PetDunGeonWait);
            yield break;
        }

        void PetDunGeonLoad_Update()
        {
           
        }
        void PetDunGeonLoad_Exit()
        {
          
        }
        #endregion

        
        #region 펫던전 대기상태 시작       
        void PetDunGeonWait_Enter()
        {
            petdungeoncontroller.PetDungeonExit();
        }

        void PetDunGeonWait_Update()
        {
        
        }
        void PetDunGeonWait_Exit()
        {
          
        }
        #endregion

        #region 던전 시작       
        void PetDunGeonEnter_Enter()
        {
            PetDungencurrentTime = 0;
            playerData.stage_Info.PetStageDataUpdate();
            Common.InGameManager.Instance.GetPlayerData.stage_Info.PetCurrentKillCount = 0;
        }

        void PetDunGeonEnter_Update()
        {
            _State.ChangeState(ePlaySubScene.PetDunGeonUpdate);
        }
        void PetDunGeonEnter_Exit()
        {
           
        }
        #endregion

        #region 던전 진행
        void PetDunGeonUpdate_Enter()
        {
            petdungeoncontroller.PetDungeonStart();
        }

        void PetDunGeonUpdate_Update()
        {
            PetDungencurrentTime += Time.deltaTime;
            if (PetDungencurrentTime > 10)
            {
                _State.ChangeState(ePlaySubScene.PetDunGeonRelease);
                PetDungencurrentTime = 0;
            }
        }

        void PetDunGeonUpdate_Exit()
        {

        }
        #endregion

        #region 던전 종료
        void PetDunGeonRelease_Enter()
        {
            Message.Send<UI.Event.PetDungeonEnd>(new UI.Event.PetDungeonEnd());
            _State.ChangeState(ePlaySubScene.PetDunGeonWait);
       
        }

        void PetDunGeonRelease_Update()
        {

        }
        void PetDunGeonRelease_Exit()
        {
            
        }
        #endregion
    }


}
