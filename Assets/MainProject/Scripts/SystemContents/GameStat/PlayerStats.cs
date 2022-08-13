using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class PlayerStats : MonoBehaviour
    {

        private void Awake()
        {
          

        }

        public void AttributeModified(Attribute attribute)
        {

        }

        public void UpdateModifiedData()
        {
            //Common.InGameManager.Instance.GetPlayerData.ImprovedAttackPower = attributes[(int)Attributes.AttackPower].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedAttackSpeed = attributes[(int)Attributes.AttackSpeed].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedCriticalDmgRate = attributes[(int)Attributes.CriticalDmg].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedCriticalRate = attributes[(int)Attributes.CriticalRate].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedMoveSpeed = attributes[(int)Attributes.MoveSpeed].value.ModifiedValue;
        }

        public void CancelModifiedData()
        {
            //Common.InGameManager.Instance.GetPlayerData.ImprovedAttackPower -= attributes[(int)Attributes.AttackPower].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedAttackSpeed -= attributes[(int)Attributes.AttackSpeed].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedCriticalDmgRate -= attributes[(int)Attributes.CriticalDmg].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedCriticalRate -= attributes[(int)Attributes.CriticalRate].value.ModifiedValue;
            //Common.InGameManager.Instance.GetPlayerData.ImprovedMoveSpeed -= attributes[(int)Attributes.MoveSpeed].value.ModifiedValue;
        }
    }
}
