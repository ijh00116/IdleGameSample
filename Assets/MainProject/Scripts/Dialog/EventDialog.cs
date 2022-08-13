using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

namespace BlackTree.UI
{
    public class EventDialog : IDialog
    {
        [Header("이벤트 팝업")]
        public EventMessagePopup NoticeMessage;
        List<EventMessagePopup> eventMessageList=new List<EventMessagePopup>();
       // public GridLayoutGroup MessageGrider;
        public Transform PopupParent;

        [Header("튜토 관련")]
        public TutorialWindow TutoWindow;
        TutorialTouch currentTutorialTouch;

        public ExitWindow Exitwindow;
        public NetworkCancelWindow networkWindow;
        protected override void OnEnter()
        {
            base.OnEnter();
       
            Vector2 gridCellsize = new Vector2(Screen.width, 100);
           // MessageGrider.cellSize = gridCellsize;

            Message.AddListener<UI.Event.FlashPopup>(PopupEventMessage);
            Message.AddListener<UI.Event.TutorialUIpopup>(PopupTutorialEvent);

            TutoWindow.Init();
            Message.AddListener<UI.Event.DungeonStart>(StartDungeon);
            Message.AddListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);

            Exitwindow.Init();
            networkWindow.Init();


        }

        protected override void OnExit()
        {
            base.OnExit();
            Message.RemoveListener<UI.Event.FlashPopup>(PopupEventMessage);
            Message.RemoveListener<UI.Event.TutorialUIpopup>(PopupTutorialEvent);

            Message.RemoveListener<UI.Event.DungeonStart>(StartDungeon);
            Message.RemoveListener<UI.Event.DungeonEndStartMain>(ReturnToMainFromDungeon);

            Exitwindow.Release();
            networkWindow.Release();
        }

        private void Update()
        {
          
        }

        void PopupEventMessage(UI.Event.FlashPopup msg)
        {
            EventMessagePopup _eventobj=null;
            for(int i=0; i< eventMessageList.Count;i++)
            {
                if(eventMessageList[i].gameObject.activeInHierarchy==false)
                {
                    _eventobj = eventMessageList[i];
                    break;
                }
            }
            if(_eventobj==null)
            {
                _eventobj= Instantiate(NoticeMessage);
                eventMessageList.Add(_eventobj);
            }
            _eventobj.transform.SetParent(PopupParent.transform,false);
            _eventobj.gameObject.SetActive(true);
            _eventobj.SettingPopupInfo(msg.Eventmsg);
        }
        

        void PopupTutorialEvent(UI.Event.TutorialUIpopup msg)
        {
            currentTutorialTouch = msg.tutorialTouch;
            TutoWindow.ShowUI(currentTutorialTouch,msg.callbackNext);
        }

        void StartDungeon(UI.Event.DungeonStart msg)
        {
            DialogView.SetActive(false);
        }

        void ReturnToMainFromDungeon(UI.Event.DungeonEndStartMain msg)
        {
            DialogView.SetActive(true);
        }

     



    }

}
