using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.UI
{
    public class IDialog : MonoBehaviour
    {
        protected RectTransform Rt;
        protected string TypeName;
        public GameObject DialogView;

        protected bool IsEnter = false;

        protected virtual void Awake()
        {
            if (DialogView == null)
                throw new System.NullReferenceException(string.Format("{0} dialogview Null", TypeName));
            TypeName = GetType().Name;
            Rt = GetComponent<RectTransform>();
        }
        protected virtual void OnDestroy()
        {

        }
        public void Load()
        {
            TypeName = GetType().Name;
            Rt = GetComponent<RectTransform>();

            Message.AddListener<Event.ShowDialogMsg>(TypeName, Enter);
            Message.AddListener<Event.HideDialogMsg>(TypeName, Exit);
            
            //UIManager.Instance.SetSibling(Rt, TypeName);

            OnLoad();
            DialogView.SetActive(false);
        }

        protected virtual void OnLoad()
        {

        }

        public void Unload()
        {
            Message.RemoveListener<Event.ShowDialogMsg>(TypeName, Enter);
            Message.RemoveListener<Event.HideDialogMsg>(TypeName, Exit);

            OnExit();
            OnUnload();
        }

        protected virtual void OnUnload()
        {

        }

        private void Enter(Event.ShowDialogMsg msg)
        {
            if(DialogView==null)
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0}'s DialogView is Null",TypeName));
#endif
                return;
            }
            DialogView.SetActive(true);
            OnEnter();
        }

        

        private void Exit(Event.HideDialogMsg msg)
        {
            if (DialogView == null)
            {
#if UNITY_EDITOR
                Debug.LogError(string.Format("{0}'s DialogView is Null", TypeName));
#endif
                return;
            }
            DialogView.SetActive(false);
            OnExit();
        }

        protected virtual void OnEnter()
        {

        }

        protected virtual void OnExit()
        {

        }

        public static void RequestDialogEnter<T>()where T:IDialog
        {
            Message.Send<Event.ShowDialogMsg>(typeof(T).Name,new Event.ShowDialogMsg());
        }

        public static void RequestDialogExit<T>()where T:IDialog
        {
            Message.Send<Event.HideDialogMsg>(typeof(T).Name, new Event.HideDialogMsg());
        }
    }
}

