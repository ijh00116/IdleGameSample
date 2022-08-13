using BlackTree;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatInvenSlot : MonoBehaviour
{
    public CurrencyType currencyType;
    public Text text;
    public void Init(CurrencyType _type)
    {
        currencyType = _type;
        this.GetComponent<Button>().onClick.AddListener(PushButton);
        text.text = currencyType.ToString();
    }
    void PushButton()
    {
        BlackTree.Common.InGameManager.Instance.GetPlayerData.AddCurrency(currencyType, 20000000000000);

    }
}
