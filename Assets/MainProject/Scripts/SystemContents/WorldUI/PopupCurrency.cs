using BlackTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DLL_Common.Common;
using DG.Tweening;

namespace BlackTree
{
    public class PopupCurrency : MonoBehaviour
    {
        public Image GoldImage;
        public Image PotionImage;
        public Text currencytext;
        public void Set(CurrencyType _type, BigInteger amount)
        {
            GoldImage.gameObject.SetActive(false);
            PotionImage.gameObject.SetActive(false);
            if (_type == CurrencyType.Gold)
            {
                GoldImage.gameObject.SetActive(true);
            }
            else if (_type == CurrencyType.MagicPotion)
            {
                PotionImage.gameObject.SetActive(true);
            }
            GoldImage.color = Color.white;
            currencytext.color = Color.white;
            PotionImage.color = Color.white;
            currencytext.text = string.Format("+{0}", amount.ToDisplay());

            StartCoroutine(TimeUpdate(_type));
        }
        WaitForSeconds tempwait = new WaitForSeconds(1.0f);
        IEnumerator TimeUpdate(CurrencyType _type)
        {
            yield return tempwait;
            if(_type==CurrencyType.Gold)
                GoldImage.DOFade(0, 1.0f);
            else if (_type == CurrencyType.MagicPotion)
                PotionImage.DOFade(0, 1.0f);

            currencytext.DOFade(0, 1.0f);
            yield return tempwait;

            this.gameObject.SetActive(false);
        }
    }
}

