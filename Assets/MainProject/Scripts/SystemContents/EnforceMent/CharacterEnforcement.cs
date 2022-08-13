using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace BlackTree
{
    [System.Serializable]
    public class CharacterEnforcement
    {
        //초월레벨
        public int AwakeLevel=0;
        //강화레벨
        public int EnchentLevel=0;
        public int TotalEnchentLevel = 0;

        [System.NonSerialized] public Enforce_data enForce_Data;
        [System.NonSerialized] public Action onAfterUpdated;

        public CharacterEnforcement()
        {
            
        }

        public void Init()
        {
            AwakeLevel = 0;
            EnchentLevel = 0;
            enForce_Data = new Enforce_data(AwakeLevel, EnchentLevel);
        }
        public void LoadSetting()
        {
            enForce_Data = new Enforce_data(AwakeLevel, EnchentLevel);
        }
        public void Update()
        {
            if(enForce_Data==null)
            {
                enForce_Data = new Enforce_data(AwakeLevel, EnchentLevel);
            }
            enForce_Data.UpdateData(AwakeLevel, EnchentLevel);
            onAfterUpdated?.Invoke();
        }
    }



   

}
