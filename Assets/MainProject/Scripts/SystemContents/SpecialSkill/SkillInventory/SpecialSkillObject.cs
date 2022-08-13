using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public class SpecialSkill
    {
        public int idx;
        public int Level = 1;
        public bool UnLocked;

        public float LeftCoolTime;
        public bool IsAuto=true;

        public SpecialSkill(int _idx)
        {
            idx = _idx;
            Level = 0;
            UnLocked = false;
            LeftCoolTime = 0;

         
        }
    }

}
