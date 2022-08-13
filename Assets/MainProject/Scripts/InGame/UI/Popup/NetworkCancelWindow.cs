using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class NetworkCancelWindow : MonoBehaviour
    {
        [SerializeField]Button exitGame;
        public void Init()
        {
            exitGame.onClick.AddListener(() => Application.Quit());
            Message.AddListener<UI.Event.PopupNetworkCancel>(PopupWindow);
        }

        public void Release()
        {
            exitGame.onClick.RemoveAllListeners();
            Message.RemoveListener<UI.Event.PopupNetworkCancel>(PopupWindow);
        }


        void PopupWindow(UI.Event.PopupNetworkCancel msg)
        {
            this.gameObject.SetActive(true);
        }
    }
}

