using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DLL_Common.Common;
using UnityEngine.UI;

namespace BlackTree
{
    public class Enforce_data
    {
        public CharacterEnchant myEnforceData;
        public AbilitiesType abilitytype;

        public float AttackImproveRate;
        public Enforce_data(int awakelv,int enchentlv)
        {
            abilitytype = AbilitiesType.ENFORCE_Gain_Rate;
            UpdateData(awakelv, enchentlv);
        }

        public void UpdateData(int awakelv, int enchentlv)
        {
            string Awakelevel = "D";
            switch (awakelv)
            {
                case 0:
                    Awakelevel = "D";
                    break;
                case 1:
                    Awakelevel = "C";
                    break;
                case 2:
                    Awakelevel = "B";
                    break;
                case 3:
                    Awakelevel = "A";
                    break;
                case 4:
                    Awakelevel = "S";
                    break;
                case 5:
                    Awakelevel = "SS";
                    break;
                case 6:
                    Awakelevel = "SSS";
                    break;
                case 7:
                    Awakelevel = "R";
                    break;
                case 8:
                    Awakelevel = "RR";
                    break;
                case 9:
                    Awakelevel = "RRR";
                    break;
                default:
                    break;
            }
            for (int i = 0; i < InGameDataTableManager.CharacterList.Enchant.Count; i++)
            {
                if (InGameDataTableManager.CharacterList.Enchant[i].awake == Awakelevel &&
                    InGameDataTableManager.CharacterList.Enchant[i].enchant == enchentlv)
                {
                    myEnforceData = InGameDataTableManager.CharacterList.Enchant[i];
                    break;
                }
            }
            AttackImproveRate = myEnforceData.enchant_gain_rate;

        }
    }

}
