using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ExitWindow : MonoBehaviour
    {
        [SerializeField] Button CancelExit;
        [SerializeField] Button exitGame;
        public void Init()
        {
            CancelExit.onClick.AddListener(() => this.gameObject.SetActive(false));
            exitGame.onClick.AddListener(Exitgame);
            Message.AddListener<UI.Event.PopupGameExit>(PopupWindow);
        }

        public void Release()
        {
            CancelExit.onClick.RemoveAllListeners();
            exitGame.onClick.RemoveAllListeners();
            Message.RemoveListener<UI.Event.PopupGameExit>(PopupWindow);
        }

        void PopupWindow(UI.Event.PopupGameExit msg)
        {
            this.gameObject.SetActive(true);
        }

        void Exitgame()
        {
            SoundManager.Instance.StopAllSound();
            Application.Quit();
        }
    }

}
