using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class DungeonEnterUI : MonoBehaviour
    {
        public System.Action YesBtnEvent;

        [SerializeField] Button YesBtn;
        [SerializeField] Button NoBtn;

        public void Init()
        {
            YesBtn.onClick.AddListener(YesTouchEvent);
            NoBtn.onClick.AddListener(NoTouchEvent);
            this.gameObject.SetActive(false);
        }

        public void SetDungeonEnter(Action yesbtnevent)
        {
            this.gameObject.SetActive(true);
            YesBtnEvent = null;
            YesBtnEvent = yesbtnevent;
        }

        void YesTouchEvent()
        {
            YesBtnEvent?.Invoke();
            this.gameObject.SetActive(false);
        }

        void NoTouchEvent()
        {
            this.gameObject.SetActive(false);
        }
    }

}
