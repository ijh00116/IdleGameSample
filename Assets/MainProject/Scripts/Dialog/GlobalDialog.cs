using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackTree.Common;

public class GlobalDialog : MonoBehaviour
{
    [Header("골드")]
    public Text Gold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Gold.text = InGameManager.Instance.PlayerData.Gold.ToString();
    }
}
