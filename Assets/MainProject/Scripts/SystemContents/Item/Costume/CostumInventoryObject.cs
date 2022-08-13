using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System;
using System.Linq;
using Spine;
using System.Collections.Generic;

namespace BlackTree
{
    public class CostumInventoryObject
    {
        public ItemType type;
        [SerializeField]
        private CostumInventory Container = new CostumInventory();
        public List<CostumInventorySlot> GetSlots => Container.Slots;

        public CostumInventorySlot EquipedItem;
        
        public void Init()
        {
            type = ItemType.Costum;

            for (int i = 0; i < InGameDataTableManager.ItemTableList.costum.Count; i++)
            {
                CostumInventorySlot slot = new CostumInventorySlot();

                slot.item = new Costum(InGameDataTableManager.ItemTableList.costum[i].idx);
                slot.itemData = new CostumData(slot.item);
                if (i == 0)
                {
                    slot.item.Equiped = true;
                }
                //slot.item.Unlocked = true;
                GetSlots.Add(slot);
            }

        }

   
    }

    [System.Serializable]
    public class CostumInventory
    {
        public List<CostumInventorySlot> Slots = new List<CostumInventorySlot>();
        public void Clear()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].item = new Costum();
            }
        }
    }
    [System.Serializable]
    public class CostumInventorySlot
    {
        [System.NonSerialized] public CostumInventoryObject parent;
        [System.NonSerialized] public GameObject slotDisplay;

        [System.NonSerialized] public Action<CostumInventorySlot> onAfterUpdated;
        [System.NonSerialized] public Action<CostumInventorySlot> onBeforeUpdated;

        public Costum item;
        [System.NonSerialized] public CostumData itemData;
        public void AddAmount()
        {
            if(item.Unlocked==false)
                item.Unlocked = true;
            UpdateSlot();
        }

        public void AddLevel(int value)
        {
            item.AddLevel(value);
            UpdateSlot();
        }

        public void UpdateSlot()
        {

            onBeforeUpdated?.Invoke(this);

            itemData.UpdateData();

            onAfterUpdated?.Invoke(this);
        }

        public void EquipItem()
        {
            onBeforeUpdated?.Invoke(this);

            if(parent.EquipedItem!=null)
            {
                if (parent.EquipedItem.item != item)
                {
                    if (parent.EquipedItem.itemData != null)
                    {
                        parent.EquipedItem.itemData.UnEquipItem();
                    }
                    parent.EquipedItem.onAfterUpdated?.Invoke(this);
                }
            }
            parent.EquipedItem = this;
            itemData.EquipItem();
            onAfterUpdated?.Invoke(this);
        }
    }
}
