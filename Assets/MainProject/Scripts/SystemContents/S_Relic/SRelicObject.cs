using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public class SRelic
    {
        
        [Header("[유저 세이빙 데이터- 아이템 강화 레벨, 갯수 등등]")]
        public int Level = 1;
        public int amount = 0;
        public bool Unlocked = false;
        public int idx;

       

        public SRelic()
        {
            idx = -1;
            Level = 0;
            amount = 0;
            Unlocked = false;
        }

        public SRelic(int _idx)
        {
            idx = _idx;
            Unlocked = false;
           
        }

        public void AddLevel(int value)
        {
            Level += value;
        }
    }
}
