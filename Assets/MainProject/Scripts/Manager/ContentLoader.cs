using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BlackTree;
using BlackTree.Common;

namespace BlackTree.Contents
{

    public class ContentLoader : MonoSingleton<ContentLoader>
    {
        Dictionary<string, IContent> ContentMap = new Dictionary<string, IContent>();
        List<string> InLoadProgressing = new List<string>();

        public delegate void OnComplete(GameObject ui);
        Dictionary<string, System.Delegate> _completeEventMap = new Dictionary<string, System.Delegate>();

        private bool _loadComplete = false;

        //6. 컨텐트 로드 시작
        public IEnumerator Load(string contentName,OnComplete loadComplete)
        {
            //7. 해당 컨텐트가 이미 있다면 콜백함수만 호출
            if (ContentMap.ContainsKey(contentName))
            {
                if (loadComplete != null)
                    loadComplete(ContentMap[contentName].gameObject);

                yield break;
            }
            //if (_inLoadProgressing.Contains(contentName))
            //{
            //    AddEvent(contentName, loadComplete);
            //    yield break;
            //}
            else//8. 없다면 컨텐트 생성 및 스크립트를 컴포넌트로 할당해주고 작업해줌
            {
                InLoadProgressing.Add(contentName);
                _loadComplete = false;

                var fullpath = string.Format("Contents/{0}", contentName);
                yield return StartCoroutine(ResourceLoader.Instance.Load<GameObject>(fullpath, o => SetupContent(o, contentName, loadComplete)));

                while (!_loadComplete)
                    yield return null;

                InLoadProgressing.Remove(contentName);
            }
        }

        //9. 셋업 컨텐트 컨텐트 오브젝트 생성 및 컨텐트 Dictionary에 추가 후 컨텐트 스크립트 Load 호출
        void SetupContent(Object o, string contentName, OnComplete loadComplete)
        {
            if (!ContentMap.ContainsKey(contentName))
            {
                var content = Instantiate(o) as GameObject;
                content.name = contentName;
                content.transform.SetParent(gameObject.transform);

                var contentScript = content.GetComponent<IContent>();
                AddEvent(contentName, loadComplete);//10.1. 컨텐트의 loadComplete 함수 콜백 추가
                contentScript.Load(OnLoadComplete);//10. 로드 호출

                ContentMap.Add(contentName, contentScript);//10.2 컨텐트 맵에 컨텐트 컴포넌트 추가
            }
        }

        void OnLoadComplete(GameObject content)
        {
            _loadComplete = true;
            RaiseEvent(content);//24. addevent로 넣어준 이벤트 실행
        }

        void AddEvent(string contentName, OnComplete oncomplete)
        {
            if (oncomplete == null)
                return;
            System.Delegate oncom;

            if(_completeEventMap.TryGetValue(contentName,out oncom))
            {
                _completeEventMap[contentName] = (OnComplete)_completeEventMap[contentName] + oncomplete;
            }
            else
            {
                _completeEventMap.Add(contentName, oncomplete);
            }
        }

        void RaiseEvent(GameObject content)
        {
            System.Delegate events;
            _completeEventMap.TryGetValue(content.name, out events);
            if (events == null)
                return;
            var oncomplete = (OnComplete)events;
            oncomplete(content);

            _completeEventMap.Remove(content.name);
        }

        public void Unload(string contentName,bool GameExit=false)
        {
            IContent contents;
            ContentMap.TryGetValue(contentName, out contents);
            if (contents != null && !contents.dontDestroy)
            {
                contents.Unload(GameExit);
                Destroy(contents.gameObject);

                ContentMap.Remove(contentName);

                var fullpath = string.Format("Contents/{0}", contentName);
                if(ResourceLoader.Instance!=null)
                {
                    ResourceLoader.Instance.Unload(fullpath);
                }
            }
        }

        public void UnloadAll(bool GameExit=false)
        {
            foreach (var contents in ContentMap)
            {
                contents.Value.Unload(GameExit);
                Destroy(contents.Value.gameObject);
            }

            ContentMap.Clear();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void Release()
        {
            UnloadAll(true);
        }
    }
}

