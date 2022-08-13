using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class BuffInterface : MonoBehaviour
    {
        [SerializeField]List<BuffUISlot> UIBuffList = new List<BuffUISlot>();
        public Dictionary<InGame.BuffType, BuffUISlot> BuffSlotList = new Dictionary<InGame.BuffType, BuffUISlot>();

        public Button CloseBtn;

        public void Init()
        {
            UpdateBuffData();
            CloseBtn.onClick.AddListener(() => this.gameObject.SetActive(false));
        }

        public void Release()
        {
            for (int i = 0; i < UIBuffList.Count; i++)
            {
                UIBuffList[i].Release();
            }
        }
        
        void UpdateBuffData()
        {
            for(int i=0; i< UIBuffList .Count; i++)
            {
                BuffSlotList.Add(UIBuffList[i].BuffType, UIBuffList[i]);
                UIBuffList[i].Init();
            }
        }
    }

}
