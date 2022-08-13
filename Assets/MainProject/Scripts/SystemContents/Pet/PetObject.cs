using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [Serializable]
    public class PetObject
    {
  

        public int idx;
        public int amount;
        public bool Unlocked;
        public bool Equiped = false;
        public int Level = 1;

        public PetObject(int id)
        {
            idx = id;
            Level = 1;
            amount = 0;
            Unlocked = false;
            Equiped = false;
        
        }
    }

}
