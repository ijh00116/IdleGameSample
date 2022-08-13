using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BlackTree.UI;

namespace BlackTree.Contents
{
    public abstract class IContent : MonoBehaviour
    {
        protected string _name;

        public bool dontDestroy = false;
        public bool stackable = true;

        public delegate void OnComplete(GameObject obj);
        OnComplete _onLoadComplete;

        protected bool _contentLoadComplete = true;
        protected bool _uiLoadComplete = true;

        [Header("[Load UI]")]
        public List<string> _uiList = new List<string>();

        int _loadingCount = 0;
        Action _onUILoadComplete;

        //11. 각 컨텐트의 로드 호출
        public void Load(OnComplete complete)
        {
            _name = GetType().Name;
            _onLoadComplete = complete;

            Message.AddListener<Event.EnterContentMsg>(_name, Enter);
            Message.AddListener<Event.ExitContentMsg>(_name, Exit);

            StartCoroutine(LoadingProcess());
        }

        public void UiLoad(Action loadComplete)
        {
            _onUILoadComplete = loadComplete;
            _loadingCount = _uiList.Count;

            //14. uimanager에서 각 UI들 로드
            for(int i=0; i<_uiList.Count; i++)
            {
                UIManager.Instance.Load(_uiList[i], OnEachUILoadComplete);
            }

            StartCoroutine(MyUILoading());
        }

        public void UiUnLoad(bool GameExit)
        {
            if(GameExit==false)
            {
                for (int i = 0; i < _uiList.Count; i++)
                {
                    GameObject ui = UIManager.Instance.Get(_uiList[i]);
                    if (ui != null)
                        ui.GetComponent<IDialog>().Unload();

                    UIManager.Instance.Unload(_uiList[i]);
                }
            }
        }

        void OnEachUILoadComplete(GameObject ui)
        {
            //18. ui에 할당되었던 함수 호출
            ui.SetActive(true);
            var dialog = ui.GetComponent<IDialog>();
            dialog.Load();

            _loadingCount--;
        }

        //19. 모든 ui가 로드되고 코루틴 호출
        IEnumerator MyUILoading()
        {
            yield return new WaitWhile(() => _loadingCount > 0);

            if (_onUILoadComplete != null)
                _onUILoadComplete();
        }

        void LoadContentsUI()
        {
            if (_uiList.Count>0)
            {
                //ui로드가 끝나면 호출될 콜백함수 지정
                UiLoad(
                    () =>
                    {
                        //20. 코루틴에서 딜리게이트로 지정되었던 콜백함수 호출
                        _uiLoadComplete = true;
                        OnUILoadComplete();
                    });
            }
            else
            {
                _uiLoadComplete = true;
            }
        }

        IEnumerator LoadingProcess()
        {
            _uiLoadComplete = false;
            _contentLoadComplete = false;

            OnLoadStart();//12. 로딩프로세스 시작시 함수 호출
            LoadContentsUI();//13. 컨텐트에 할당된 UI들 Load

            do
            {
                yield return null;
            }
            while (!_uiLoadComplete || !_contentLoadComplete);

            OnLoadComplete();//22. ui 로드가 끝나고 호출되는곳

            if (_onLoadComplete != null)
                _onLoadComplete(gameObject);//23. 할당된 ContentLoader의 OnLoadComplete를 호출
        }

        protected void SetLoadComplete()
        {
            _contentLoadComplete = true;
        }

        /// <summary>
        /// 생성과 동시에 메시지 및 모델을 생성해야 할 경우 재정의 한 후 구현한다.
        /// 이 콜백을 재정의 하게 되면 적절한 타이밍에 SetLoadComplete() 를 호출해주어야 한다.
        /// </summary>
        protected virtual void OnLoadStart()
        {
            SetLoadComplete();
        }

        protected virtual void OnLoading(float progress)
        {
            /* BLANK */
        }

        protected virtual void OnLoadComplete()
        {
            /* BLANK */
        }


        //21. 모든 ui호출 끝난뒤 함수 호출되는곳
        protected virtual void OnUILoadComplete()
        {
            /* BLANK */
        }

        protected virtual void OnUnload()
        {
            /* BLANK */
        }

        public void Unload(bool GameExit)
        {
            Message.RemoveListener<Event.EnterContentMsg>(_name, Enter);
            Message.RemoveListener<Event.ExitContentMsg>(_name, Exit);

            OnExit();

            if (_uiList.Count > 0)
                UiUnLoad(GameExit);

            OnUnload();
        }

        void Enter(Event.EnterContentMsg msg)
        {

            //28. onenter 함수 호출
            OnEnter();
        }

        void Exit(Event.ExitContentMsg msg)
        {
            OnExit();

        }

        protected abstract void OnEnter();
        protected abstract void OnExit();

        public static void RequestContentEnter<T>() where T : IContent
        {
            Message.Send<Event.EnterContentMsg>(typeof(T).Name, new Event.EnterContentMsg());
        }

        public static void RequestContentExit<T>() where T : IContent
        {
            Message.Send<Event.ExitContentMsg>(typeof(T).Name, new Event.ExitContentMsg());
        }

        public static string GetMsgName<T>()
        {
            return typeof(T).Name;
        }
    }

}
