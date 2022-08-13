using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class CheatInven : MonoBehaviour
    {
        public GameObject CheatButtonPrefab;
        public void Awake()
        {
            for(int i=0; i < (int)CurrencyType.End; i++)
            {
                var obj = Instantiate(CheatButtonPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(this.transform, false);
                obj.GetComponent<CheatInvenSlot>().Init((CurrencyType)i);
            }
        }
    }

}
