using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BlackTree.Common;
using BlackTree.InGame;

//로직 시작
namespace BlackTree.Scene
{
    public class SceneManager : MonoSingleton<SceneManager>
    {
        public Constants.SceneName startScene;
        public Constants.SceneName nowScene;

        Dictionary<Constants.SceneName, GameObject> _scenes;
        LinkedList<Constants.SceneName> _showList;

        Transform _root;

        public bool AllSceneLoaded;

        public bool BeforeGameDataLoading;
        protected override void Init()
        {
            _scenes = new Dictionary<Constants.SceneName, GameObject>();
            _showList = new LinkedList<Constants.SceneName>();

            _root = this.transform;

            //StartCoroutine(Load_Singleton());
            BeforeGameDataLoading = false;
            StartCoroutine(BeforStartGameData());
        }

        IEnumerator BeforStartGameData()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //yield return new WaitUntil(() => BackendManager.Instance.IsLogin == true);
            //테이블부터 몽땅 로드 해야함//
            Debug.Log(1);
            //테이블 로드
            yield return StartCoroutine(TableManager.Instance.Load());
            Debug.Log(2);
            //사운드 로드
            yield return StartCoroutine(SoundManager.Instance.Setup());
            Debug.Log(3);
            //게임 정보 테이블 로드
            yield return StartCoroutine(InGameDataTableManager.Instance.Load());
            Debug.Log(4);
            //테이블부터 몽땅 로드 해야함//
            yield return StartCoroutine(InGameManager.Instance.LoadingIngameData());//재화 스테이지 강화 유저정보 등 로드함, 캐릭터 생성
            Debug.Log(5);
            //캐릭터 관련 로드
            yield return StartCoroutine(CharacterDataManager.Instance.LoadCharacterData());//메인캐릭터 로드해서 레벨 테이블 토대로 업데이트 함
                                                                                           //인게임 데이터 로드
                                                                                           //인게임 데이터 세팅 //서버 로드 포함
            Debug.Log(6);
            //적 캐릭터 관련 로드
            yield return StartCoroutine(BTOPsetPosition.Instance.CreateEnemyObject());//적 세팅(여기선 DB참조 없음(생성시마다 체력을 테이블에서 참조함))
            Debug.Log(7);

            BeforeGameDataLoading = true;
        }

        public IEnumerator LoadInGameData()
        {
            yield return new WaitUntil(() => BeforeGameDataLoading == true);

            SoundManager.Instance.PlaySound((int)SoundType.BGM);
            //게임 로드 다 된뒤에 랭크 등록 한번
            //BackendManager.Instance.RegisterRank();

            yield break;
        }

        public void LoadStartScene()
        {
            if (startScene == Constants.SceneName.None)
                return;
            AllSceneLoaded = false;

            var sceneName = startScene;
            var scene = GetRoot(startScene);
            if (scene == null)
            {
                var fullpath = string.Format("Scenes/{0}", sceneName);
                StartCoroutine(ResourceLoader.Instance.Load<GameObject>(fullpath, o => OnPostLoadProcess(o)));
            }
        }

        protected override void Release()
        {
            UnloadAll(true);
        }

        public GameObject GetRoot(Constants.SceneName sceneName)
        {
            GameObject scene;
            _scenes.TryGetValue(sceneName, out scene);
            return scene;
        }

        public void Load(Constants.SceneName sceneName)
        {
            LoadRoot(sceneName);
        }

        void LoadRoot(Constants.SceneName sceneName)
        {
            AllSceneLoaded = false;

            UnloadAll();

            nowScene = sceneName;
            var scene = GetRoot(nowScene);
            if(scene==null)
            {
                var fullPath= string.Format("Scenes/{0}", sceneName);
                StartCoroutine(ResourceLoader.Instance.Load<GameObject>(fullPath, o => OnPostLoadProcess(o)));
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0} is arleady exist", sceneName));
#endif
            }
        }

        void OnPostLoadProcess(Object o)
        {
            var scene = Instantiate(o) as GameObject;

            var sceneScript = scene.GetComponent<IScene>();
            scene.name = sceneScript.sceneName.ToString();
            scene.transform.SetParent(_root);

            _scenes.Add(sceneScript.sceneName, scene);
            _showList.AddLast(sceneScript.sceneName);
            SetupScene(scene);
        }

        void SetupScene(GameObject scene)
        {
            var scenescript = scene.GetComponent<IScene>();
            //1. 로드에셋으로  IScene인터페이스클래스에서 작업 시작
            scenescript.LoadAssets(
                ()=> 
                {
                    AllSceneLoaded = true;
                    //30. 모든 컨텐트와 ui 호출된후 호출
                    //InGameManager.Instance.StartGame();
                });
        }

        public void Unload(Constants.SceneName sceneName,bool ExitGame)
        {
            var scene = GetRoot(sceneName);
            if(scene!=null)
            {
                scene.GetComponent<IScene>().Unload(ExitGame);

                _scenes.Remove(sceneName);
                _showList.Remove(sceneName);

                var fullpath = string.Format("Scenes/{0}", scene.name);
                //if (ResourceLoader.Instance!=null)
                //    ResourceLoader.Instance.Unload(fullpath);
            }
        }

        public void UnloadAll(bool ExitGame=false)
        {
            LinkedListNode<Constants.SceneName> node;

            while(true)
            {
                node = _showList.First;
                if (node == null)
                    break;
                Unload(node.Value, ExitGame);
            }
        }
    }
}

