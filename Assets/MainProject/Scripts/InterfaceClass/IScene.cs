using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using BlackTree.Contents;

namespace BlackTree.Scene
{
    public class IScene : MonoBehaviour
    {
        public Constants.SceneName sceneName;

        public List<string> contentsList = new List<string>();
        public List<string> enterContentList = new List<string>();

        Action _onLoadComplete = null;
        bool _resourceLoadComplete = false;
        int _loadingContentsCount = 0;

        //2. 로드에셋으로 작업 시작
        public void LoadAssets(Action onComplete)
        {
            _onLoadComplete = onComplete;
            StartCoroutine(LoadContents());
        }

        //3. 컨텐트 로드
        IEnumerator LoadContents()
        {
            OnLoadStart();//4. 로드컨텐트 시작시에 함수 호출

            Application.targetFrameRate = -1;

            while (_resourceLoadComplete == false)
                yield return null;

            _loadingContentsCount = contentsList.Count;

            //5. 할당된 컨텐트들을 컨텐트로더(싱글톤)를 통해 로드 시작 Load함수에서 내부로직 다시 돌아감
            for (int i = 0; i < contentsList.Count; ++i)
            {
                yield return StartCoroutine(ContentLoader.Instance.Load(contentsList[i],
                    c =>
                    {
                        _loadingContentsCount--;
                        OnContentLoadComplete(c); //컨텐트가 각각 로드 될떄마다 호출되는 콜백함수
                        //25. 콜백함수 호출됨
                    }));
            }

        

            EnterContents();//27. 엔터콘텐트 함수 호출
            OnLoadComplete();//29. 로드 끝나고 함수 호출

            Application.targetFrameRate = 50;

            if (_onLoadComplete != null)
                _onLoadComplete();
        }

        protected void SetResourceLoadComplete()
        {
            _resourceLoadComplete = true;
        }
        protected virtual void OnLoadStart()
        {
            SetResourceLoadComplete();
        }

        void EnterContents()
        {
            //var firstContentList = (0 < enterContentList.Count) ? enterContentList : contentsList;
            for (int i = 0; i < enterContentList.Count; i++)
                Message.Send<Contents.Event.EnterContentMsg>(enterContentList[i], new Contents.Event.EnterContentMsg());
        }

        protected virtual void OnLoadComplete()
        {
            /* BLANK */
        }

        //26. 컨텐트 로드 끝나면 호출됨(call back)
        protected virtual void OnContentLoadComplete(GameObject content)
        {
            /* BLANK */
        }

        public void Unload(bool ExitGame)
        {
            OnUnload();

            if(ExitGame==false)
            {
                if (ContentLoader.Instance != null)
                {
                    for (int i = 0; i < contentsList.Count; ++i)
                        ContentLoader.Instance.Unload(contentsList[i]);
                }
            }
           // Destroy(ContentLoader.Instance.gameObject);

            Destroy(gameObject);
        }

        protected virtual void OnUnload()
        {
            /* BLANK */
        }
    }
}

