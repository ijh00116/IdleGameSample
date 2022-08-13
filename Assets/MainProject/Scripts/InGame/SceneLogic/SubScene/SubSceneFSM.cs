using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Scene;
using BlackTree.Model;
using BlackTree.InGame;
using BlackTree.Common;

namespace BlackTree
{
    public enum ePlaySubScene
    {
        DoNothing,

        //펫던전
        PetDunGeonLoad = -1,
        
        PetDunGeonWait,
        PetDunGeonInit,
        PetDunGeonUpdate,
        PetDunGeonRelease,

        pvpDunGeonLoad,

        pvpDunGeonWait,
        pvpDunGeonInit,
        pvpDunGeonUpdate,
        pvpDunGeonRelease,
    }

    public partial class SubSceneFSM : MonoBehaviour,IRegistStateCallback
    {
        public SceneStateMachine<ePlaySubScene> _State = null;

        PlayerDataModel playerData;
        Character subCharacter;

        public void Initialize()
        {
            if (_State == null)
                _State = SceneStateMachine<ePlaySubScene>.Initialize(this);
            else
                _State.ChangeState(ePlaySubScene.DoNothing);

            playerData = InGameManager.Instance.GetPlayerData;
            subCharacter = InGameManager.Instance.PetPlayerCharacter;

            RegistCallback();
            //_State.ChangeState(ePlaySubScene.PetDunGeonLoad);
        }
        public void RegistCallback()
        {
            _State.stateLookup = new Dictionary<ePlaySubScene, StateCallback>();

            PetDungeonRegisterInit();

            pvpDungeonRegisterInit();
        }

        
    }

}
