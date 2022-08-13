using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class SRelicCategoryUI : MonoBehaviour
    {
        [SerializeField] GameObject infoui;
        [SerializeField] Text SrelicInfoSubscribe;
        public void Init()
        {
            Message.AddListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
            LocalValue lv = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_217");
            SrelicInfoSubscribe.text = string.Format("{0}",lv.GetStringForLocal(true));
        }
        public void Release()
        {
            Message.RemoveListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
        }
     

        private void OnApplicationQuit()
        {
            Common.InGameManager.Instance.Localdata.SavesrelicData();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Common.InGameManager.Instance.Localdata.SavesrelicData();
            }
        }

        void OtherUIPopuped(UI.Event.OtherUIPopup msg)
        {
            if (msg.PopupUI != this.gameObject)
            {
                infoui.SetActive(false);
            }
            else
            {
                infoui.SetActive(true);
            }
        }
    }
}

