using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace BlackTree
{
    public class TutorialWindow : MonoBehaviour
    {
        TutorialTouch currentTutorialtouch;
        Action touchCallback;

        [SerializeField]GameObject _mask;
        [SerializeField] Text _NameText;
        [SerializeField] Text _ContentText;

        public void Init()
        {
            var trigger = _mask.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };

            entry.callback.AddListener((data) => OnPointerClick((PointerEventData)data));
            trigger.triggers.Add(entry);

            this.gameObject.SetActive(false);
        }
        public void ShowUI(TutorialTouch tutotouch,Action _callback)
        {
            currentTutorialtouch = tutotouch;
            touchCallback = _callback;

            gameObject.SetActive(true);
            //테이블 데이터에서 튜토터치의 다이얼로그 name이랑 desc가지고 스트링값 세팅
            string name=null;
            string Content = null;

            LocalValue currentnameLocalvalue = InGameDataTableManager.TestLocalTableList.tutorial.Find(o => o.id == currentTutorialtouch.name_id);
            LocalValue currentcontentLocalvalue = InGameDataTableManager.TestLocalTableList.tutorial.Find(o => o.id == currentTutorialtouch.desc_id);

            //국가 설정
            name = currentnameLocalvalue.kr.ToString();
            Content = currentcontentLocalvalue.kr.ToString();

            _NameText.text = name;
            _ContentText.text = Content;
        }

        public void OnPointerClick(PointerEventData data)
        {
            touchCallback?.Invoke();
            touchCallback = null;
            gameObject.SetActive(false);

        }
    }

}
