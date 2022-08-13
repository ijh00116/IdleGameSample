using BlackTree.Common;
using BlackTree.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public partial class SceneFSM
    {

        void MainRegisterInit()
        {
            StateCallback _statecallback = new StateCallback() { OnEnter = MainGameStart_Enter, Update = MainGameStart_Update, OnExit = MainGameStart_Exit };
            _State.stateLookup.Add(ePlayScene.MainGameStart, _statecallback);
            _statecallback = new StateCallback() { OnEnter = MainInit_Enter, Update = MainInit_Update, OnExit = MainInit_Exit };
            _State.stateLookup.Add(ePlayScene.MainInit, _statecallback);
            _statecallback = new StateCallback() { OnEnter = MainUpdate_Enter, Update = MainUpdate_Update, OnExit = MainUpdate_Exit };
            _State.stateLookup.Add(ePlayScene.MainUpdate, _statecallback);

            _statecallback = new StateCallback() { OnEnter = null, Update = null, OnExit = null };
            _State.stateLookup.Add(ePlayScene.DoNothing, _statecallback);
        }
        #region 게임 최초 시작(전투 시작 전)
        void MainGameStart_Enter()
        {
            playerData.stage_Info.currentWave = 0;
            _State.ChangeState(ePlayScene.MainUpdate);
        }
        void MainGameStart_Update()
        {
        }
        void MainGameStart_Exit()
        {
        }
        #endregion

        #region 메인 시작(전투 시작 전)
        void MainInit_Enter()
        {
            mainCharacter._state.ChangeState(eActorState.Idle);
            playerData.CurrentWave = 0;
         
            _State.ChangeState(ePlayScene.MainUpdate);


        }
        void MainInit_Update()
        {
        }
        void MainInit_Exit()
        {
        }
        #endregion

        #region 메인전투 진행
        void MainUpdate_Enter()
        {
            InGameFader.Instance.FadeGame(WaveStart());
           // StartCoroutine(WaveStart());
        }
        /// <summary>
        /// 웨이브 진행 시작시 들어옴
        /// </summary>
        IEnumerator WaveStart()
        {
            playerData.stage_Info.MaxWaveUpdate();
            //BackendManager.Instance.RegisterRank();
            mainCharacter.GetComponent<InGame.CharacterController>().ResetPosition(true);
            InGameManager.Instance.CinemachineCamActive(true);
            StartCoroutine(StartMainCombat());
            yield break;
        }

        IEnumerator StartMainCombat()
        {
            //튜토리얼 검사
            //다음에 튜토 도입시 추가
            //bool TutorialFinished = false;
            //TutorialManager.Instance.StartTutorial(eTutorialDivision.BASIC_TUTORIAL, () => TutorialFinished = true);
            //yield return new WaitUntil(() => TutorialFinished == true);
            //튜토리얼 끝날때까지 기다리기
            playerData.stage_Info.Update();
            //적 리젠
            InGame.BTOPsetPosition.Instance.NormalCombatRegen();
            //플레이어 무브 활성화

            InGameManager.Instance.MainCharacterActivate(true);
            //playerData.CurrentWave = 1;
            yield break;
        }

        public void MainWaveUpdate()
        {
            if (InGameManager.Instance.InfiniteMode)
            {
                if (playerData.CurrentWave >= playerData.stage_Info.MaxWave())
                {
                    InGameManager.Instance.CinemachineCamActive(false);
                    _State.ChangeState(ePlayScene.MainInit);
                }
            }
            else
            {
                bool stageChanged = false;
                if (playerData.CurrentWave >= playerData.stage_Info.MaxWave())
                {
                    playerData.stage_Info.Stage += 1;
                    stageChanged = true;
                 
                }
                if (playerData.stage_Info.Stage > 100)
                {
                    playerData.stage_Info.Stage = 1;
                    playerData.stage_Info.chapter += 1;
                }
                if (playerData.stage_Info.chapter > 10)
                {
                    playerData.stage_Info.chapter = 1;
                    playerData.stage_Info.scenario += 1;
                }
                if (playerData.stage_Info.scenario >= 10)
                {
                    playerData.stage_Info.scenario = 10;
                }
                
                if (playerData.stage_Info.BestScenario < playerData.stage_Info.scenario)
                {
                    playerData.stage_Info.BestScenario = playerData.stage_Info.scenario;
                    playerData.stage_Info.Bestchapter = playerData.stage_Info.chapter;
                    playerData.stage_Info.BestStage = playerData.stage_Info.Stage;
                }
                if (playerData.stage_Info.chapter > playerData.stage_Info.Bestchapter)
                {
                    playerData.stage_Info.Bestchapter = playerData.stage_Info.chapter;
                    playerData.stage_Info.BestStage = playerData.stage_Info.Stage;
                }
                if (playerData.stage_Info.Stage > playerData.stage_Info.BestStage)
                {
                    playerData.stage_Info.BestStage = playerData.stage_Info.Stage;
                }
                if (stageChanged)
                {
                    InGameManager.Instance.CinemachineCamActive(false);
                    

                    //스테이지 미션데이터 추가
                    int TotalStageCount = (playerData.stage_Info.BestScenario-1) * 1000 + (playerData.stage_Info.Bestchapter-1) * 100 + playerData.stage_Info.Stage;
                    Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.SetMissionValue(MissionType.CHAPTER_1_STAGE, TotalStageCount,true);

                    Common.InGameManager.Instance.Localdata.SaveData(Common.InGameManager.Instance.GetPlayerData.saveData);
                    //BackendManager.Instance.SaveDBData(DTConstraintsData.UserData,Common.InGameManager.Instance.GetPlayerData.stage_Info);

                    playerData.saveData.userinfoPvp.RankScore= playerData.stage_Info.BestScenario * 1000 + playerData.stage_Info.Bestchapter * 100
                        + playerData.stage_Info.BestStage;

                    StartCoroutine(MainInitAfterEvent());
                }
            }
        }

        WaitForSeconds waitseconds = new WaitForSeconds(1.8f);
        IEnumerator MainInitAfterEvent()
        {
            mainCharacter._state.ChangeState(eActorState.EventAfterKillBoss);
            Message.Send<UI.Event.MainSceneBossEvent>(new UI.Event.MainSceneBossEvent());

            yield return waitseconds;

            if(_State.IsCurrentState(ePlayScene.MainUpdate))
            {
                _State.ChangeState(ePlayScene.MainInit);
            }
            else
            {
                Debug.LogError("던전 중");
            }
            
        }
        void MainUpdate_Update()
        {
           
        }
        void MainUpdate_Exit()
        {
        }

       
        #endregion
    }
}
