using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{


    public enum Attributes
    {
        AttackPower = 0,
        AttackSpeed,
        CriticalRate,
        CriticalDmg,
        MoveSpeed,
        GoldImprove,
    }

    [System.Serializable]
    public class GameBuff : IModifiers
    {
        public Attributes stat;
        public int value;

        public GameBuff() { }
        public GameBuff(int _data)
        {
            value = _data;
        }

        public void AddValue(ref int v)
        {
            v += GetValue;
        }

        int GetValue
        {
            get
            {
                return value;
            }
        }

    }


}
