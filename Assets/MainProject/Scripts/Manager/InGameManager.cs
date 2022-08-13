using BayatGames.SaveGameFree;
using BlackTree.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.Common
{
    public class InGameManager : MonoSingleton<InGameManager>
    {
        private  Model.PlayerDataModel PlayerData;
        public Model.PlayerDataModel GetPlayerData
        {
            get { return PlayerData; }
        }

        [Header("펫")]
        [HideInInspector]public PetInventoryObject petInventory;
        [Header("인벤토리")]
        public InventoryObject WeaponInventory;
        public InventoryObject WingInventory;
        public CostumInventoryObject CostumInventory;
        [Header("유물 스킬")]
        public RelicInventoryObject RelicInventory;
        [Header("특수 유물")]
        public SRelicInventoryObject SRelicInventory;
        [Header("특수스킬 인벤토리")]
        [HideInInspector]public SpecialSkillInventoryObject SpecialSkillInventory;
       
        public CinemachineSwitcher CameraSwither;

        [Header("캐릭터 프리팹")]
        public GameObject CharacterPrefab;
        public GameObject PVPCharacterPrefab;
        public GameObject SubCharacterPrefab;
        [HideInInspector] public GameObject mainPlayer;
        [SerializeField] public List<Transform> PlayerPositionList = new List<Transform>();
        [SerializeField] public List<Transform> DungeonPlayerPositionList = new List<Transform>();

        //펫던전용 플레이어 캐릭터
        [HideInInspector] public Character PetPlayerCharacter;
        [HideInInspector] public Character mainplayerCharacter;
        
        [Header("게임 모드")]
        public bool InfiniteMode;
        [HideInInspector]public bool ingamedataLoading;
        public bool IsMainGameStart;
        [HideInInspector]public SceneFSM _sceneFsm;
        public SubSceneFSM _PetsceneFsm;
        public SubSceneFSM _PvpsceneFsm;

        [HideInInspector]public UserInfoForPVP EnemyPvpInfo;
        [HideInInspector]public UI.BottomDialogType BottomDialogtype;

        public AdmobManager admob;
        [HideInInspector]public IapManager IAPManager;

        [HideInInspector] public Dictionary<RewardType, Sprite> UIIconImageList = new Dictionary<RewardType, Sprite>();
        public LocalData Localdata;
#if UNITY_EDITOR
        public bool AllItemUnlock = true;
#endif
        private void Start()
        {
            IsMainGameStart = false;

            if (_sceneFsm == null)
                _sceneFsm = this.GetComponent<SceneFSM>();

            BottomDialogtype = UI.BottomDialogType.Pet;

            //IAPManager = new IapManager();
            //IAPManager.InitUnityIAP();
            
            CameraInit();
        }

        private void OnApplicationPause(bool pause)
        {
           
        }
        protected virtual void OnApplicationQuit()
        {
            PlayerData?.Release();
        }
        private void Update()
        {
            //if(Input.GetKeyDown(KeyCode.O))
            //{
            //    Application.targetFrameRate = 10;
            //    UnityEngine.Rendering.OnDemandRendering.renderFrameInterval = 3;
            //}
            if(GetPlayerData!=null)
            {
                if (GetPlayerData.GlobalUser.GachaWeaponAdLeftTime >= 0)
                    GetPlayerData.GlobalUser.GachaWeaponAdLeftTime -= Time.deltaTime;
                if (GetPlayerData.GlobalUser.GachaWingAdLeftTime >= 0)
                    GetPlayerData.GlobalUser.GachaWingAdLeftTime -= Time.deltaTime;
                if (GetPlayerData.GlobalUser.GachaPetAdLeftTime >= 0)
                    GetPlayerData.GlobalUser.GachaPetAdLeftTime -= Time.deltaTime;
                if (GetPlayerData.GlobalUser.GachaSrelicAdLeftTime >= 0)
                    GetPlayerData.GlobalUser.GachaSrelicAdLeftTime -= Time.deltaTime;
            }
        

            if (GetPlayerData!=null)
            {
                GetPlayerData.GlobalUser.PlayingTime += Time.deltaTime;
                GetPlayerData.GlobalUser.TotalPlayingTime+= Time.deltaTime;
            }
                

            if(Application.internetReachability==NetworkReachability.NotReachable)
            {
                Debug.LogError("인터넷 문제로 게임을 종료합니다.");
                Message.Send<UI.Event.PopupNetworkCancel>(new UI.Event.PopupNetworkCancel());
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Message.Send<UI.Event.PopupGameExit>(new UI.Event.PopupGameExit());
            }

            if(Input.GetKeyDown(KeyCode.V))
            {
                //Localdata.SaveData(GetPlayerData.saveData);
                //Debug.LogError(Application.version+"현재버전");
                //Debug.LogError(BackendManager.Instance.GetLatestVersion()+"최신버전");
            }
        }
        float zoomvalue=0;
        float orthosizegap = 5.63f;
        float ScreenXgap = 0.02f;
        float ScreenYgap = 0.0382f;
        public void ZoomValueChange(float _data)
        {
            zoomvalue = _data;
        }
        public void CameraSwitch(bool Boss)
        {
            CameraSwither.ChangeCamera(Boss);
        }
        void CameraInit()
        {

        }

        public void CameraShake(float intensity, float time)
        {
            StartCoroutine(ShakeCameraCo(intensity, time));
        }
     
        IEnumerator ShakeCameraCo(float intensity, float time)
        {

            float ShakeTimer = time;
            while(ShakeTimer>0)
            {
                ShakeTimer -= Time.deltaTime;
                yield return null;
            }
        }

        public void CinemachineCamActive(bool on)
        {
            //카메라 설정
        }
        /// <summary>
        /// UI에서 던전 장면 전환 시작
        /// </summary>
        /// <param name="cowLevel">던전레벨</param>
        public void StartCowDungeon(int cowLevel)
        {
            //캐릭터 AI비활성
            //MainCharacterActivate(false);
            //캐릭터 AI비활성

            PlayerData.stage_Info.CowCurrentLevel = cowLevel;
            if (cowLevel>PlayerData.stage_Info.CowBestLevel)
            {
                PlayerData.stage_Info.CowBestLevel = cowLevel;
            }
            _sceneFsm._State.ChangeState(ePlayScene.DunGeonInit);
        }

        public void StartPetDungeon(int PetChapter,int PetStage)
        {
            PlayerData.stage_Info.Pet_CurrentChapter = PetChapter;
            PlayerData.stage_Info.Pet_CurrentStage = PetStage;

            if(GetPlayerData.stage_Info.Pet_BestChapter<PetChapter)
            {
                GetPlayerData.stage_Info.Pet_BestChapter = PetChapter;
                GetPlayerData.stage_Info.Pet_BestStage = PetStage;
            }
            if(GetPlayerData.stage_Info.Pet_BestStage<PetStage)
                GetPlayerData.stage_Info.Pet_BestStage = PetStage;

            //챕터,스테이지 값 세팅
            _PetsceneFsm._State.ChangeState(ePlaySubScene.PetDunGeonInit);
        }

        public void StartPVP(UserInfoForPVP enemyInfo)
        {
            EnemyPvpInfo = enemyInfo;

            _PvpsceneFsm._State.ChangeState(ePlaySubScene.pvpDunGeonInit);
        }

        //메인 전투게임 시작할지말지를 정해줌(bool변수)
        public void MainCharacterActivate(bool active)
        {
            if(active==false)
            {
                mainplayerCharacter._state.ChangeState(eActorState.Idle);
            }
            IsMainGameStart = active;
            mainplayerCharacter._state.ChangeState(eActorState.Move);
        }
        /// <summary>
        /// 카우 방에서 포지션 처음으로 가게 하는것
        /// </summary>
        public void CowWaveInfoUpdate()
        {
            _sceneFsm.DungeounWaveUpdate();
        }

        public void StartGame()
        {
            _sceneFsm._State.ChangeState(ePlayScene.MainGameStart);
        }

        public void ChangeModeAndPlay()
        {
            InfiniteMode = !InfiniteMode;
            _sceneFsm._State.ChangeState(ePlayScene.MainInit);
        }

        /// <summary>
        /// 웨이브 변경됨
        /// </summary>
        public void WaveInfoUpdate()
        {
            _sceneFsm.MainWaveUpdate();
        }
        [SerializeField]BG.BGParallax[] parallax;
        [SerializeField] BG.BGParallax[] Dungeonparallax;

        public IEnumerator LoadingIngameData()
        {
            IAPManager = new IapManager();
            IAPManager.InitUnityIAP();

            if (Localdata == null)
                Localdata = new LocalData();

            ingamedataLoading = false;
            PlayerData = new Model.PlayerDataModel();
            PlayerData.Init();

            mainPlayer = Instantiate(CharacterPrefab);
            mainplayerCharacter = mainPlayer.GetComponent<Character>();

            var subobj = Instantiate(SubCharacterPrefab);
            PetPlayerCharacter = subobj.GetComponent<Character>();

            InGame.CharacterPositionControll characterposcontrol = mainPlayer.GetComponent<BlackTree.InGame.CharacterPositionControll>();
            if (characterposcontrol != null)
            {
                characterposcontrol.PlayerPositionList = PlayerPositionList;
                characterposcontrol.DungeonPlayerPositionList = DungeonPlayerPositionList;
            }
                

            ingamedataLoading = true;

          

            mainplayerCharacter.GetComponent<InGame.CharacterController>().ResetPosition(true);

            //InventorySetting();

            //인벤토리 로드
            SpecialSkillInventory = new SpecialSkillInventoryObject();
            SpecialSkillInventory.Init(ItemType.S_Skill);

            //펫 로드
            petInventory = new PetInventoryObject();
            petInventory.Init();

            SRelicInventory = new SRelicInventoryObject();
            SRelicInventory.Init();

            RelicInventory = new RelicInventoryObject();
            RelicInventory.Init();

            //장비
            WeaponInventory = new InventoryObject();
            WeaponInventory.Init(ItemType.weapon);
            WingInventory = new InventoryObject();
            WingInventory.Init(ItemType.wing);

            CostumInventory = new CostumInventoryObject();
            CostumInventory.Init();

            string resourcePath = "Images/GUI/Icon";
            for(int i=0; i<(int)RewardType.End; i++)
            {
                RewardType _type = (RewardType)(i);
                string Fullpath = string.Format("{0}/{1}", resourcePath, _type.ToString());
                Sprite sp = Resources.Load<Sprite>(Fullpath);
                UIIconImageList.Add(_type, sp);
            }

            _sceneFsm.Initialize();
            _PetsceneFsm.Initialize();
            _PvpsceneFsm.Initialize();
            _PetsceneFsm._State.ChangeState(ePlaySubScene.PetDunGeonLoad);
            _PvpsceneFsm._State.ChangeState(ePlaySubScene.pvpDunGeonLoad);
            yield break;
        }

        public void BGPositionReset()
        {
            for (int i = 0; i < parallax.Length; i++)
            {
                parallax[i].Init();
            }
            for (int i = 0; i < Dungeonparallax.Length; i++)
            {
                Dungeonparallax[i].Init();
            }
        }
    }

}
