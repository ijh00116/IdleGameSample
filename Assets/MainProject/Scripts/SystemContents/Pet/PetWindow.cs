using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class PetWindow : MonoBehaviour
    {
        [SerializeField]Mask mask;
        [SerializeField] Image image;
        // Start is called before the first frame update
        void Awake()
        {
            Message.AddListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
        }

        private void OnDestroy()
        {
            Message.RemoveListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
        }

        void OtherUIPopuped(UI.Event.OtherUIPopup msg)
        {
            if (msg.PopupUI != this.gameObject)
            {
                mask.enabled = true;
            }
            else
            {
                mask.enabled = false;
            }

        }
    }

}
