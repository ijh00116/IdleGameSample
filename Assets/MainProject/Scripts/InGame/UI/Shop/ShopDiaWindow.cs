using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ShopDiaWindow : MonoBehaviour
    {
        [SerializeField] private Button BuyDiabtn;
        // Start is called before the first frame update
        void Start()
        {
            BuyDiabtn.onClick.AddListener(Buydia);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Buydia()
        {
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem,10);
        }
    }

}
