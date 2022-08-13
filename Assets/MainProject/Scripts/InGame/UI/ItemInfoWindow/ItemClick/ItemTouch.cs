using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree
{
    public class ItemTouch : MonoBehaviour,IDragHandler,IEndDragHandler,IPointerClickHandler,IBeginDragHandler
    {
        public Action TouchUpCallback;
        [HideInInspector]public ScrollRect scrollRect;
        bool moved = false;

        [SerializeField]EventTrigger eventTrigger;

        public void Init(Action callback,ScrollRect rect)
        {
            TouchUpCallback = callback;
            scrollRect = rect;
            AddEvent(EventTriggerType.PointerUp, OnClick);
        }

        void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            var _eventTrigger = new EventTrigger.Entry();
            _eventTrigger.eventID = type;
            _eventTrigger.callback.AddListener(action);
            eventTrigger.triggers.Add(_eventTrigger);
        }

        public void OnDrag(PointerEventData eventData)
        {
            scrollRect.OnDrag(eventData);
            moved = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            scrollRect.OnEndDrag(eventData);
            moved = false;
        }


        public void OnClick(BaseEventData eventdata)
        {
           if(moved==false)
            {
                TouchUpCallback?.Invoke();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            scrollRect.OnBeginDrag(eventData);
        }
    }


}
