using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ShopDrycellWindow : MonoBehaviour
    {
        [SerializeField] private Button buydrycell;
        // Start is called before the first frame update
        void Start()
        {
            buydrycell.onClick.AddListener(Buydia);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Buydia()
        {
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion,-20);
            Common.InGameManager.Instance.GetPlayerData.AddCurrency( CurrencyType.Gem,10);
        }
    }

}
