using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class OfflineRewardSlot : MonoBehaviour
    {
        public Image BoxImage;
        public Text Name;
        public Text Amount;
        
        public void Setting(CurrencyType _type,int count)
        {
            BoxData boxdata = InGameDataTableManager.BoxTableList.box.Find(o => o.boxType == _type);
            string icon = boxdata.icon;
            BoxImage.sprite = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Inventory/{0}", icon));
            LocalValue _data = InGameDataTableManager.LocalizationList.box.Find(o => o.id == boxdata.name);
            Name.text = string.Format("{0}", _data.GetStringForLocal(true));
            Amount.text = string.Format("x{0}", count);
        }
    }
}

