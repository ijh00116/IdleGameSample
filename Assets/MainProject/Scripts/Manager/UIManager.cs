using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;

namespace BlackTree.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public GameObject InGameDialogViewer;
        public GameObject GlobalDialogViewer;

        Dictionary<string, GameObject> Uis;

        public delegate void OnComplete(GameObject ui);
        Dictionary<string, System.Delegate> OnCompleteEvents;

        string ASSET_PATH = "Dialogs";

        protected override void Init()
        {
            Uis = new Dictionary<string, GameObject>();
            OnCompleteEvents = new Dictionary<string, System.Delegate>();
            //var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.Instance
            //    | System.Reflection.BindingFlags.Public 
            //    | System.Reflection.BindingFlags.NonPublic
            //     | System.Reflection.BindingFlags.DeclaredOnly);

            //int a = methods.Length;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        protected override void Release()
        {
            UnloadAll();
        }

        public GameObject Get(string uiname)
        {
            GameObject ui;
            Uis.TryGetValue(uiname, out ui);
            return ui;
        }

        //15. ui리소스 로드하고 리소스 로드후 호출되는 콜백함수 지정(AddEvent에 IContent의OnEachUILoadComplete를 Add)
        public void Load(string uiName, OnComplete oncomplete)
        {
            AddEvent(uiName, oncomplete);

            GameObject ui = Get(uiName);
            if(ui==null)
            {
                var path = string.Format("{0}/{1}", ASSET_PATH, uiName);
                StartCoroutine(ResourceLoader.Instance.Load<GameObject>(path, OnPostLoadProcess));
            }
            else
            {
                ui.SetActive(true);

                AttachtoCanvas(ui);
                RaiseEvent(ui);
            }
        }

        //16. IContent에서 지정한 OnEachUILoadComplete를 RaiseEvent함수를 통해 콜백함수 호출
        void OnPostLoadProcess(Object o)
        {
            var ui = Instantiate(o) as GameObject;

            ui.SetActive(true);
            ui.name = o.name;

            AttachtoCanvas(ui);
            Uis.Add(ui.name, ui);
            RaiseEvent(ui);
        }

        void AttachtoCanvas(GameObject ui)
        {
            int UienumIdx = EnumExtention.ParseToInt<UISibling>(ui.name);
            var ParentObj = GetUIParentBySiblingIndex(UienumIdx);
            ui.SetLayerInChildren(ParentObj.layer);
            ui.transform.SetParent(ParentObj.gameObject.transform,false);
            ui.transform.SetAsLastSibling();
        }

        void AddEvent(string uiName, OnComplete oncomplete)
        {
            if (oncomplete == null)
                return;

            System.Delegate _event;
            if(OnCompleteEvents.TryGetValue(uiName,out _event))
            {
                OnCompleteEvents[uiName] = (OnComplete)OnCompleteEvents[uiName] + oncomplete;
            }
            else
            {
                OnCompleteEvents.Add(uiName, oncomplete);
            }
        }

        void RaiseEvent(GameObject ui)
        {
            System.Delegate _event;
            if(OnCompleteEvents.TryGetValue(ui.name,out _event))
            {
                var oncom=(OnComplete)_event;
                oncom(ui);//17. 함수 호출

                OnCompleteEvents.Remove(ui.name);
            }
          
        }


        public void Unload(string uiName)
        {
            GameObject ui = Get(uiName);
            if (ui != null)
            {
                Destroy(ui);
                Uis.Remove(uiName);

                var fullpath = string.Format("{0}{1}", ASSET_PATH, uiName);
            }
        }

        public void UnloadAll()
        {
            OnCompleteEvents?.Clear();

            foreach (var ui in Uis)
            {
                ui.Value.GetComponent<IDialog>().Unload();
                Destroy(ui.Value);
            }
            Uis.Clear();
        }

        public void SetSibling(RectTransform _rt,string name)
        {
            int UienumIdx = EnumExtention.ParseToInt<UISibling>(name);
            var ParentObj = GetUIParentBySiblingIndex(UienumIdx);
            _rt.gameObject.layer = ParentObj.gameObject.layer;
            _rt.gameObject.transform.SetParent(ParentObj.gameObject.transform);
            _rt.gameObject.transform.SetAsLastSibling();
        }

        public void SetasLastSibling(string uiName)
        {
            GameObject uiobj = null;
            if(Uis.TryGetValue(uiName,out uiobj))
            {
                uiobj.transform.SetAsLastSibling();
            }
        }

        GameObject GetUIParentBySiblingIndex(int index)
        {
            GameObject obj = null;
            if(index<500)
            {
                obj = InGameDialogViewer;
            }
            else if(index<1000)
            {
                obj = GlobalDialogViewer;
            }
            return obj;
        }
    }
}

