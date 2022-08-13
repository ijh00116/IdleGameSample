using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Scene;
using BlackTree.Model;
using BlackTree.InGame;
using BlackTree.Common;

namespace BlackTree
{
    public enum ePlayScene
    {
        DoNothing=-1,
        /// <summary>
        /// 메인
        /// </summary>
        MainGameStart = 0,
        MainInit,
        MainUpdate,
        MainRelease,
        //던전
        DunGeonInit,
        DunGeonUpdate,
        DunGeonRelease,
        //펫던전
        PVPDunGeonInit,
        PVPDunGeonUpdate,
        PVPDunGeonRelease,
    }

    public partial class SceneFSM : MonoBehaviour, IRegistStateCallback
    {
        public SceneStateMachine<ePlayScene> _State = null;

        PlayerDataModel playerData;
        Character mainCharacter;

        public void Initialize()
        {
            if (_State == null)
                _State = SceneStateMachine<ePlayScene>.Initialize(this);
            else
                _State.ChangeState(ePlayScene.DoNothing);

            playerData = InGameManager.Instance.GetPlayerData;
            mainCharacter = InGameManager.Instance.mainplayerCharacter;

            RegistCallback();
            _State.ChangeState(ePlayScene.DoNothing);
        }

        public void RegistCallback()
        {
            _State.stateLookup = new Dictionary<ePlayScene, StateCallback>();

            DungeonRegisterInit();
            MainRegisterInit();
        }

    }

}
