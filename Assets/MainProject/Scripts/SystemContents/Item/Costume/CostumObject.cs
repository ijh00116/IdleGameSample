using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public class Costum
    {
        [Header("[유저 세이빙 데이터- 아이템 강화 레벨, 갯수 등등]")]
        public int Level = 1;
        public bool Unlocked = false;
        public bool Equiped;
        public int idx;

        public Costum()
        {
            idx = -1;
            Level = 1;
            Equiped = false;
            Unlocked = false;
        }

        public Costum(int _idx)
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
