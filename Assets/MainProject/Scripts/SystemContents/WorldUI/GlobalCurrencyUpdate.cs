using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class GlobalCurrencyUpdate : MonoBehaviour
    {
        [SerializeField]List<PopupCurrency> CurrencyPopup = new List<PopupCurrency>();
        private void Start()
        {
            for(int i=0; i< CurrencyPopup.Count; i++)
            {
                CurrencyPopup[i].gameObject.SetActive(false);
            }
            Message.AddListener<UI.Event.EarnMonsterGold>(CurrencyToDisplay);
        }

        private void OnDestroy()
        {
            Message.RemoveListener<UI.Event.EarnMonsterGold>(CurrencyToDisplay);
        }

    
        void CurrencyToDisplay(UI.Event.EarnMonsterGold msg)
        {
            if (msg.Type != CurrencyType.Gold && msg.Type != CurrencyType.Gem && msg.Type != CurrencyType.MagicPotion)
                return;

            PopupCurrency pc=null;
            for(int i=0; i< CurrencyPopup.Count; i++)
            {
                if (CurrencyPopup[i].gameObject.activeInHierarchy == false)
                    pc = CurrencyPopup[i];
            }
            if(pc==null)
            {
                pc = CurrencyPopup[0];
            }
            pc.transform.SetAsFirstSibling();
            pc.gameObject.SetActive(true);

            pc.Set(msg.Type, msg.value);
        }
    
    }

}
