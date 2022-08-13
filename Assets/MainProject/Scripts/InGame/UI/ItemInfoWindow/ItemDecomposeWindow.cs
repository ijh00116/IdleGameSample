using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ItemDecomposeWindow : MonoBehaviour
    {
        [SerializeField] Image CurrentItemIcon;
        [SerializeField] Image AwakeCurrencyIcon;

        [SerializeField] Button PlusDeComposeCount;
        [SerializeField] Button MinusDeComposeCount;

        [SerializeField] Button MinimumDeComposeCount;
        [SerializeField] Button MaximumDeComposeCount;

        [SerializeField] Text ItemDeComposeCount;
        [SerializeField] Text AwakePlusCountUI;

        [SerializeField] Button DeComposeBtn;

        int DeComposeCount;
        int AwakePlusCount;
        private InventorySlot MyInventoryslot;

        public void Init()
        {
            DeComposeBtn.onClick.AddListener(PushDeComposeButton);
        }
        public void PopupSetting(InventorySlot slot)
        {
            MyInventoryslot = slot;
            ItemData _itemdata = MyInventoryslot.itemData;

            if (_itemdata.itemtype == ItemType.weapon)
                CurrentItemIcon.sprite = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Weapon/{0}", _itemdata.itemInfo.icon));
            else if (_itemdata.itemtype == ItemType.wing)
                CurrentItemIcon.sprite = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Wing/{0}", _itemdata.itemInfo.icon));

            AwakeCurrencyIcon.sprite = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Wing/{0}", _itemdata.itemInfo.icon));

            DeComposeCount = (int)(MyInventoryslot.item.amount -1);
            ItemDeComposeCount.text = DeComposeCount.ToString();

            AwakePlusCount = DeComposeCount * _itemdata.Recycle_StoneCount;
            AwakePlusCountUI.text = AwakePlusCount.ToString();
        }

        void PushDeComposeButton()
        {
            if (MyInventoryslot.item.amount > 1)
            {
                MyInventoryslot.item.amount -= DeComposeCount;
                //초월석 추가 여기따가 넣으면 됨(재화 인벤 추가된 후에)
                //MyNextInventorySlot.AddAmount(ComposeCount);
               
                //Common.InGameManager.Instance.GetPlayerData.AddCurrency(MyInventoryslot.item.itemData.awakeItem_Type, AwakePlusCount);

                DeComposeCount = (int)(MyInventoryslot.item.amount-1);
                ItemDeComposeCount.text = DeComposeCount.ToString();
            }
        }
    }

}
