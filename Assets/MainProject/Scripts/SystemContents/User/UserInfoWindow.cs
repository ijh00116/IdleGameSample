using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class UserInfoWindow : MonoBehaviour
    {
        [SerializeField] Button CloseButton;
        public void Init()
        {
            CloseButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        }

        public void Release()
        {
            CloseButton.onClick.RemoveAllListeners();
        }
    }

}
