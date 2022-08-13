using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ItemInformationUI : MonoBehaviour
    {
        [SerializeField] Text ItemName;
        [SerializeField] Button PopupEnforce;
        [SerializeField] Button PopupCompose;
        [SerializeField] Button PopupAwake;

        [SerializeField] GameObject PopupEnforceSelected;
        [SerializeField] GameObject PopupComposeSelected;
        [SerializeField] GameObject PopupAwakeSelected;

        [SerializeField] ItemEnforceWindow   EnforceWindow;
        [SerializeField] ItemComposeWindow   ComposeWindow;
        [SerializeField] ItemAwakeWindow AwakeWindow;

        ItemUIDisplay currentInvenSlot;
        public void Init()
        {
            ItemName.text = "오래된 검";

            EnforceWindow.Init();
            ComposeWindow.Init();
            AwakeWindow.Init();

            PopupEnforce.onClick.AddListener(PopupEnforceWinodw);
            PopupCompose.onClick.AddListener(PopupComposeWinodw);
            PopupAwake.onClick.AddListener(PopupAwakeWinodw);
        }

        public void Release()
        {
            EnforceWindow.Release();
            AwakeWindow.Release();
        }

        public void PopupItemInfoWindow(ItemUIDisplay _invenslotui)
        {
            currentInvenSlot = _invenslotui;
       
            ItemName.text = currentInvenSlot.ItemName;

            if (EnforceWindow.gameObject.activeInHierarchy)
            {
                PopupEnforceWinodw();
            }
            else if (ComposeWindow.gameObject.activeInHierarchy)
            {
                PopupComposeWinodw();
            }
            else if (AwakeWindow.gameObject.activeInHierarchy)
            {
                PopupAwakeWinodw();
            }
            else
            {
                PopupEnforceWinodw();
            }
        }

        void PopupEnforceWinodw()
        {
            InActiveWindows();

            PopupEnforceSelected.SetActive(true);
          
            EnforceWindow.gameObject.SetActive(true);

            EnforceWindow.PopupSetting(currentInvenSlot);
        }

        void PopupComposeWinodw()
        {
            InActiveWindows();

            PopupComposeSelected.SetActive(true);
            ComposeWindow.gameObject.SetActive(true);
            ComposeWindow.PopupSetting(currentInvenSlot);
        }

        void PopupAwakeWinodw()
        {
            InActiveWindows();

            PopupAwakeSelected.SetActive(true);
            AwakeWindow.gameObject.SetActive(true);
            AwakeWindow.PopupSetting(currentInvenSlot);
        }

        void InActiveWindows()
        {
            EnforceWindow.gameObject.SetActive(false);
            ComposeWindow.gameObject.SetActive(false);
            AwakeWindow.gameObject.SetActive(false);

            PopupEnforceSelected.SetActive(false);
            PopupComposeSelected.SetActive(false);
            PopupAwakeSelected.SetActive(false);
        }
    }

}
