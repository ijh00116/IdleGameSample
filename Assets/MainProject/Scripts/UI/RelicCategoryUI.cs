using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class RelicCategoryUI : MonoBehaviour
    {
        [SerializeField] Text PotionText;
        public void Init()
        {
            Message.AddListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
            Message.AddListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }
        public void Release()
        {
            Message.RemoveListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
            Message.RemoveListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }
      
    
        void OtherUIPopuped(UI.Event.OtherUIPopup msg)
        {
            //if(msg.PopupUI!=this.gameObject)
            //{
            //    gachaui.SetActive(false);
            //}
            //else
            //{
            //    gachaui.SetActive(true);
            //}
            
        }

        void GetCurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.Type == CurrencyType.MagicPotion)
            {
                PotionText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value.ToDisplay();
            }
        }
    }

}
