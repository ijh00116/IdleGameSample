using BlackTree.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class EquimentUI : MonoBehaviour
    {
        public Button CloseItemInfoButton;
        public Button WeaponBtn;
        public Button ShieldBtn;

        public GameObject WeaponBtnOn;
        public GameObject ShieldBtnOn;

        public GameObject WeaponWindow;
        public GameObject ShieldWindow;

        public Items.ItemDynamicInterface Weaponinterface;
        public Items.ItemDynamicInterface Winginterface;
        public CostumUserInterface costuminterface;

        public ItemInformationUI ItemInfoUI;

        [SerializeField] Text EnchantstoneText;

        [Header("코스튬")]
        public GameObject CostumWindow;
        public Button CostumButton;
        public GameObject CostumBtnOn;

        public GameObject WeaponTopMenu;
        public GameObject WingTopMenu;
        public GameObject CostumTopMenu;
        public void Awake()
        {
            WeaponWindow.SetActive(true);
            ShieldWindow.SetActive(true);
            WeaponBtn.onClick.AddListener(()=> {
                ItemInfoUI.gameObject.SetActive(false);
                activeWindow(WeaponWindow);
            });
            ShieldBtn.onClick.AddListener(() => {
                ItemInfoUI.gameObject.SetActive(false);
                activeWindow(ShieldWindow);
            });

            WeaponBtnOn.SetActive(false);
            ShieldBtnOn.SetActive(false);

            CloseItemInfoButton.onClick.AddListener(() => ItemInfoUI.gameObject.SetActive(false));

            ItemInfoUI.Init();
            ItemInfoUI.gameObject.SetActive(false);

            Message.AddListener<UI.Event.PopupItemInformationUI>(PopupItemInfoWindow);
            Message.AddListener<UI.Event.OtherUIPopup>(OtherUIPopuped);

            WeaponWindow.SetActive(true); 
            ShieldWindow.SetActive(false);
            WeaponBtnOn.SetActive(true); 
            ShieldBtnOn.SetActive(false);
            CostumWindow.SetActive(false);
            CostumBtnOn.SetActive(false);

            Weaponinterface.Init();
            Winginterface.Init();
            costuminterface.Init();
            Message.AddListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);

            CostumButton.onClick.AddListener(() => {
                ItemInfoUI.gameObject.SetActive(false);
                activeWindow(CostumWindow); 
            });

            CostumBtnOn.SetActive(false);

            EnchantstoneText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.EnchantStone).value.ToDisplay();
        }
        private void OnApplicationQuit()
        {
            Weaponinterface.SaveItemList();
            Winginterface.SaveItemList();
            
        }
    

        private void OnDestroy()
        {
            ItemInfoUI.Release();

            Message.RemoveListener<UI.Event.PopupItemInformationUI>(PopupItemInfoWindow);
            Message.RemoveListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
            Message.RemoveListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }

        void PopupItemInfoWindow(UI.Event.PopupItemInformationUI msg)
        {
            ItemInfoUI.gameObject.SetActive(true);
            ItemInfoUI.PopupItemInfoWindow(msg.InvenSlotui);
        }

        void OtherUIPopuped(UI.Event.OtherUIPopup msg)
        {
            ItemInfoUI.gameObject.SetActive(false);
        }

        void GetCurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if(msg.Type == CurrencyType.EnchantStone)
            {
                EnchantstoneText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.EnchantStone).value.ToDisplay();
            }
        }

        void activeWindow(GameObject obj)
        {
            WeaponWindow.SetActive(false); 
            WeaponBtnOn.SetActive(false);
            ShieldWindow.SetActive(false);
            ShieldBtnOn.SetActive(false);
            CostumWindow.SetActive(false);
            CostumBtnOn.SetActive(false);

            WeaponTopMenu.SetActive(false);
            WingTopMenu.SetActive(false);
            CostumTopMenu.SetActive(false);

            if (obj==WeaponWindow)
            {
                WeaponWindow.SetActive(true);
                WeaponBtnOn.SetActive(true);
                WeaponTopMenu.SetActive(true);
            }
            else if (obj == ShieldWindow)
            {
                ShieldWindow.SetActive(true);
                ShieldBtnOn.SetActive(true);
                WingTopMenu.SetActive(true);
            }
            else if (obj == CostumWindow)
            {
                CostumWindow.SetActive(true);
                CostumBtnOn.SetActive(true);
                CostumTopMenu.SetActive(true);
            }



        }
    }

}
