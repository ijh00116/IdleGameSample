using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    //서버 저장정보 데이터
    [System.Serializable]
    public class Item
    {
        [Header("[유저 세이빙 데이터- 아이템 강화 레벨, 갯수 등등]")]
        public int Level = 1;
        public int amount = 0;
        public bool Equiped=false;
        public bool Unlocked = false;
        public int idx;
        public int AwakeLv;

        public Item()
        {
            idx= -1;
            Level = 1;
            Equiped = false;
            Unlocked = false;
        }

        public Item(ItemType _type,int _idx)
        {
            idx = _idx;
            Equiped = false;
            Unlocked = false;
            Level = 1;
            amount = 0;
            AwakeLv = 0;
          
        }

        public void AddLevel(int value)
        {
          
                Level += value;
        }

    }

}
