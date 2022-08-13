using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public class Relic
    {
        [Header("[유저 세이빙 데이터- 아이템 강화 레벨, 갯수 등등]")]
        public int Level = 0;
        public bool Unlocked = false;
        public int idx;

        public Relic()
        {
            idx = -1;
            Level = 0;
            Unlocked = false;
        }

        public Relic(int _idx)
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
