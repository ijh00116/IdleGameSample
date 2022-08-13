using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree.Items
{
    public abstract class ItemUserInterface : MonoBehaviour
    {
        [HideInInspector] public InventoryObject inventory;
        public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        public virtual void Init()
        {
            if (inventory == null)
                return;

            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                inventory.GetSlots[i].parent = inventory;
            }
            if (inventory.type == ItemType.weapon)
            {
                //BackendManager.Instance.LoadItemList(inventory.type, null);
                //Common.InGameManager.Instance.Localdata.LoadweaponData(() => BackendManager.Instance.LoadItemList(inventory.type, null));
            }
            else
            {
                //BackendManager.Instance.LoadItemList(inventory.type, null);
                //Common.InGameManager.Instance.Localdata.LoadwingData(() => BackendManager.Instance.LoadItemList(inventory.type, null));
            }
                

            CreateSlots();
        }
        private void Update()
        {
        }

        public virtual void SaveItemList()
        {
            if (inventory == null)
                return;
            if(inventory.type==ItemType.weapon)
            {
                Common.InGameManager.Instance.Localdata.SaveweaponData();
            }
            else
            {
                Common.InGameManager.Instance.Localdata.SavewingData();
            }
                
        }

        public abstract void CreateSlots();

        public void OnSlotUpdate(InventorySlot slot)
        {

        }
 
    }

}
