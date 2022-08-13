using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ReviewWindow : MonoBehaviour
    {
        public Button LaterReview;
        public Button Confirm;
        public void Init()
        {
            LaterReview.onClick.AddListener(PushConfirm);
            Confirm.onClick.AddListener(PushConfirm);
        }

        public void Release()
        {
            LaterReview.onClick.RemoveAllListeners();
            Confirm.onClick.RemoveAllListeners();
        }
        
        void PushConfirm()
        {
            this.gameObject.SetActive(false);
        }
    }
}

