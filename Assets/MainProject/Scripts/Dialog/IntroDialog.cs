using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.UI;
using UnityEngine.UI;
using BlackTree.Common;
using BlackTree.Scene;
using BayatGames.SaveGameFree;

namespace BlackTree.UI
{
    public class IntroDialog : MonoBehaviour
    {
        //게임 스타트 버튼
        [Header("게임스타트")]
        [SerializeField] Button GameStart;

        [Header("권한 동의")]
        [SerializeField] Button agreeTermsButton;
        [SerializeField] GameObject AgreeTermsWindow;

        [Header("로그인창")]
        [SerializeField] GameObject LoginWindow;
        [SerializeField] Button GuestLoginButton;
        [SerializeField] Button GoogleLoginButton;

        [Header("닉네임 입력")]
        [SerializeField] GameObject NickNameWindow;
        [SerializeField] InputField NickNameInput;
        [SerializeField] Button NickNameConfirm;

        [Header("닉네임 사용중")]
        [SerializeField] GameObject alreadyUseWindow;
        [SerializeField] Button AlreadyWindowConfirm;

        [Header("전체")]
        public GameObject View;

        public GameObject LoadingImage;

        [Header("업데이트")]
        public GameObject UpdateWindow;
        public Button ExitBtn;
        public Button GoUpdate;

        string StoreUrl = "https://onesto.re/0000756385";

        bool HasAgreed
        {
            get { return Data_local.Instance.agreeTerms.enable; }
            set { Data_local.Instance.agreeTerms.enable = value; }
        }
        protected void Awake()
        {
            LoadingImage.SetActive(false);
            View.SetActive(true);
#if UNITY_EDITOR
            SaveGame.SavePath = SaveGamePath.DataPath;
#else
            SaveGame.SavePath = SaveGamePath.PersistentDataPath;
#endif

            GameStart.onClick.AddListener(TouchStartBtn);
            agreeTermsButton.onClick.AddListener(TouchAgreeTerms);
            GuestLoginButton.onClick.AddListener(TouchGuestLoginButton);
            GoogleLoginButton.onClick.AddListener(TouchGoogleLoginButton);
            NickNameConfirm.onClick.AddListener(MakeNickName);
            AlreadyWindowConfirm.onClick.AddListener(() => alreadyUseWindow.SetActive(false));

            UpdateWindow.gameObject.SetActive(false);
            ExitBtn.onClick.AddListener(()=>Application.Quit());
            GoUpdate.onClick.AddListener(GoStore);

            AgreeTermsWindow.SetActive(false);
            LoginWindow.gameObject.SetActive(false);
            GameStart.gameObject.SetActive(false);
            NickNameWindow.SetActive(false);
            alreadyUseWindow.SetActive(false);
            StartCoroutine(StartLogin());
        }

        protected  void OnDestroy()
        {
            //GameStart.onClick.RemoveAllListeners();

            agreeTermsButton.onClick.RemoveAllListeners();
        }

        IEnumerator StartLogin()
        {
           // Debug.Log("구글해시:" + Backend.Utils.GetGoogleHash());
            if (HasAgreed==false)
                AgreeTermsWindow.SetActive(true);
            yield return new WaitUntil( ()=>AgreeTermsWindow.activeInHierarchy == false);
            yield return Authorization.TryAutoLogin();
            //if (BackendManager.Instance.authorization.status==Authorization.Status.NotAuthorized)
            //{
            //    yield return StartCoroutine(WaitforLogin());
            //}
        
    
#if UNITY_EDITOR

#else
          
            string currentversion = Application.version;
            string latestversion = BackendManager.Instance.GetLatestVersion();
            float current = float.Parse(currentversion);
            float latest = float.Parse(latestversion);
            if(current<latest)
            {
                Debug.LogError("버전 업글 요구");
                Debug.LogError(currentversion);
                Debug.LogError(latestversion);
                UpdateWindow.gameObject.SetActive(true);
                yield break;
            }
#endif
            //닉네임
            string nick = "Localname";

            if(string.IsNullOrEmpty(nick))
            {
                NickNameWindow.SetActive(true);
            }
            
            yield return new WaitUntil(() => NickNameWindow.activeInHierarchy == false);
            yield return null;

            yield return StartCoroutine(SceneManager.Instance.LoadInGameData());
            SceneManager.Instance.LoadStartScene();

            LoadingImage.SetActive(true);
            yield return new WaitUntil(()=>SceneManager.Instance.AllSceneLoaded==true&& SceneManager.Instance.BeforeGameDataLoading == true);
            LoadingImage.SetActive(false);
            GameStart.gameObject.SetActive(true);
            yield break;
        }

        IEnumerator WaitforLogin()
        {
            LoginWindow.gameObject.SetActive(true);
            //yield return new WaitUntil(() => BackendManager.Instance.authorization.status == Authorization.Status.Done);
            LoginWindow.gameObject.SetActive(false);
            yield break;
        }

        void GoStore()
        {
            Application.OpenURL(StoreUrl);
            Application.Quit();
        }
        void MakeNickName()
        {
        }

        void NicknameAlreadyUse()
        {
            alreadyUseWindow.SetActive(true);
            NickNameInput.text = "";
        }

        void TouchGuestLoginButton()
        {
        }

        void TouchGoogleLoginButton()
        {
        }

        void TouchAgreeTerms()
        {
            HasAgreed = true;
            AgreeTermsWindow.SetActive(false);
        }

        void TouchStartBtn()
        {
            InGameFader.Instance.FadeGame(InGameStart());
        }

        IEnumerator InGameStart()
        {
            yield return new WaitUntil(()=>SceneManager.Instance.AllSceneLoaded);
            View.SetActive(false);


            InGameManager.Instance.StartGame();

            yield return new WaitUntil(() => InGameManager.Instance.IsMainGameStart == true);
            yield break;
        }
    }

}
