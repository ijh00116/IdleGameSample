using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackTree.InGame;

namespace BlackTree
{
    public class BuffTimeIcon : MonoBehaviour
    {
        [SerializeField] BuffType MyType;
        [SerializeField] Text TimeText;
        public void Init()
        {
            TimeText.color = Color.white;
            TimeText.text = string.Format("OFF");
            Message.AddListener<InGame.Event.BuffTimer>(BuffTimer);
        }

        public void Release()
        {
            Message.RemoveListener<InGame.Event.BuffTimer>(BuffTimer);
        }

        void BuffTimer(InGame.Event.BuffTimer msg)
        {
            if (msg.Bufftype != MyType)
                return;
            if (this.gameObject.activeInHierarchy == false)
                return;

            if (msg.ElapsedTime <= 0)
            {
                TimeText.color = Color.white;
                TimeText.text = string.Format("OFF");
            }
            else
            {
                int lefttime = (int)msg.ElapsedTime;

                int m = lefttime / 60;
                int s = lefttime % 60;
                TimeText.color = Color.yellow;
                TimeText.text = string.Format("{0:D2}:{1:D2}", m, s);
            }
        }
    }
}
