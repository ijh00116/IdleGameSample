using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlackTree.UI
{
    public class EventMessagePopup : MonoBehaviour
    {
        [BTReadOnly] public float CurrentTime;
        [SerializeField] Image BG;
        [SerializeField] Text popuptext;

        public void SettingPopupInfo(string _textdata)
        {
            CurrentTime = 0.0f;
            transform.localPosition = Vector3.zero;
            BG.color = Color.black;
            popuptext.color = Color.white;
            popuptext.text = _textdata;


            transform.DOLocalMove(new Vector3(0, 100, 0), 2.0f);
            BG.DOFade(0.0f, 2.0f);
            popuptext.DOFade(0.0f, 2.0f);
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;
            if(CurrentTime>=2.0f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

}
