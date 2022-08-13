using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree.Items
{
    public class ItemDynamicInterface : ItemUserInterface
    {
        public GameObject inventorySlotObjectPrefab;
        public ItemType inventype;
        public ScrollRect scrollRect;
        public override void Init()
        {
            switch (inventype)
            {
                case ItemType.weapon:
                    inventory = InGameManager.Instance.WeaponInventory;
                    break;
                case ItemType.wing:
                    inventory = InGameManager.Instance.WingInventory;
                    break;
                default:
                    inventory = null;
                    break;
            }

            base.Init();
        }
        private void Update()
        {

        }
        public override void SaveItemList()
        {
            base.SaveItemList();
        }

        public override void CreateSlots()
        {
            slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(inventorySlotObjectPrefab, Vector3.zero, Quaternion.identity, transform);
          
                slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            }

            foreach (KeyValuePair<GameObject, InventorySlot> data in slotsOnInterface)
            {
                data.Key.GetComponent<ItemUIDisplay>().Init(data.Value,scrollRect);
                data.Value.display = data.Key.GetComponent<ItemUIDisplay>();
                data.Value.UpdateSlot();
            }

        }
    }

}
