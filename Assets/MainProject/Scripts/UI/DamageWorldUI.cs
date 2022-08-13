using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DLL_Common.Common;
using DG.Tweening;

namespace BlackTree
{
    public class DamageWorldUI : MonoBehaviour
    {
        public Text Damage;
        public int punchscale=1;
        public float elecscale=0.3f;
        public float duration = 0.5f;
        public void Setting(bool isCri,BigInteger damage)
        {
            Damage.text = damage.ToDisplay();
            this.transform.localPosition = Vector3.zero;
            Vector3 endpos = new Vector3(0, 7, 0);

            if (isCri)
            {
                Damage.transform.localScale = new Vector3(2, 2, 2);
                Damage.color = Color.red;
                Damage.transform.DOPunchScale(new Vector3(5, 5, 5), duration, punchscale,elecscale);
            }
            else
            {
                Damage.transform.localScale = Vector3.one;
                Damage.color = Color.white;
                Damage.transform.DOPunchScale(new Vector3(4, 4, 4), duration, punchscale, elecscale);
            }
        
            this.transform.DOLocalMove(endpos, 2f);
            this.transform.DOLocalMove(endpos, 2f);
            Damage.DOFade(0, 3f);

            
        }
        private void Update()
        {
            //this.transform.Translate(Vector3.up * Time.deltaTime*DTConstraintsData.UI_DAMAGETEXT_SPEED);
        }
    }

}
