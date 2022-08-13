using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class EarnCurrencyText : MonoBehaviour
    {
        public CurrencyType currencytype;

        [HideInInspector] public BigInteger Currentvalue = BigInteger.Zero;
        [SerializeField] Text CurrencyText;
        string CurrencyLocal=null;
        public void Initialize()
        {
            Currentvalue = 0;
            if(CurrencyLocal==null)
            {
                LocalValue lv = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == currencytype.ToString());
                if(lv!=null)
                {
                    CurrencyLocal = lv.GetStringForLocal(true);
                }
                CurrencyText.text = string.Format("{0} x{1}", CurrencyLocal, 0);

            }
        }
        public void CurrencyTextSet(CurrencyType _type,BigInteger data)
        {
            if (this.gameObject.activeInHierarchy == false)
                return;

            if(_type==currencytype)
            {
                Currentvalue += data;
                CurrencyText.text = string.Format("{0} x{1}", CurrencyLocal, Currentvalue.ToDisplay());
            }
        }
    }

}
