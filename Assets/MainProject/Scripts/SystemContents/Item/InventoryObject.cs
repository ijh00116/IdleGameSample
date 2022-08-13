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
    public class InventoryObject
    {
        public ItemType type;
        [SerializeField]
        private Inventory Container = new Inventory();
        public List<InventorySlot> GetSlots => Container.Slots;

        public InventorySlot EquipedItem;

        public bool AddItem(Item item)
        {
            InventorySlot slot = FindItemOnInventory(item);
            if (slot == null)
                return false;
            
            slot.AddAmount(item.amount);
            return true;
        }

        public bool AddAmount(Item item,int value)
        {
            InventorySlot slot = FindItemOnInventory(item);
            if (slot == null)
                return false;
            
            //Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(string.Format("{0} 획득", item.slot.display.ItemName)));
            slot.AddAmount(value);
            return true;
        }

        public int EmptySlotCount
        {
            get
            {
                int counter = 0;
                for (int i = 0; i < GetSlots.Count; i++)
                {
                    if (GetSlots[i].item.idx <= -1)
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }

        public Item GetItemIninventory(int idx)
        {
            for (int i = 0; i < GetSlots.Count; i++)
            {
                if (GetSlots[i].item.idx==idx)
                {
                    return GetSlots[i].item;
                }
            }

            return null;
        }

        public InventorySlot FindItemOnInventory(Item item)
        {
            for (int i = 0; i < GetSlots.Count; i++)
            {
                if (GetSlots[i].item.idx == item.idx)
                {
                    return GetSlots[i];
                }
            }
            return null;
        }


        public InventorySlot GetEmptySlot()
        {
            for (int i = 0; i < GetSlots.Count; i++)
            {
                if (GetSlots[i].item.idx <= -1)
                {
                    return GetSlots[i];
                }
            }
            return null;
        }

        public void Init(ItemType _type)
        {
            type = _type;
            if (type == ItemType.weapon)
            {
                for (int i = 0; i < InGameDataTableManager.ItemTableList.weapon.Count; i++)
                {
                    InventorySlot slot = new InventorySlot();
                    slot.item= new Item(ItemType.weapon, InGameDataTableManager.ItemTableList.weapon[i].idx);
                    slot.itemData = new ItemData(_type, slot.item);
#if UNITY_EDITOR
                    if (Common.InGameManager.Instance.AllItemUnlock)
                    {
                        slot.item.Equiped = true;
                        slot.item.Unlocked = true;
                        slot.item.amount = 1;
                    }
#endif
                    if (i == 0)
                    {
                        slot.item.Equiped = true;
                        slot.item.Unlocked = true;
                        slot.item.amount = 1;
                    }
                    GetSlots.Add(slot);
                }
            }
            else if (type == ItemType.wing)
            {
                for (int i = 0; i < InGameDataTableManager.ItemTableList.wing.Count; i++)
                {
                    InventorySlot slot = new InventorySlot();
                    slot.item = new Item(ItemType.wing, InGameDataTableManager.ItemTableList.wing[i].idx);
                    slot.itemData = new ItemData(_type, slot.item);
#if UNITY_EDITOR
                    if (Common.InGameManager.Instance.AllItemUnlock)
                    {
                        slot.item.Equiped = true;
                        slot.item.Unlocked = true;
                        slot.item.amount = 1;
                    }
#endif
                    if (i == 0)
                    {
                        slot.item.Equiped = true;
                        slot.item.Unlocked = true;
                        slot.item.amount = 1;
                    }
                    GetSlots.Add(slot);
                }
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            Container.Clear();
        }
    }

    [System.Serializable]
    public class Inventory
    {
        public List<InventorySlot> Slots = new List<InventorySlot>();
        public Inventory()
        {
            for(int i=0; i< Slots.Count; i++)
            {
                Slots[i] = new InventorySlot();
            }
        }
        
        public void Clear()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].item = new Item();
            }
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        [System.NonSerialized] public InventoryObject parent;
        [System.NonSerialized] public Action onAfterUpdated;
        [System.NonSerialized] public Action onBeforeUpdated;
        [System.NonSerialized] public ItemUIDisplay display;

        public Item item;
        [System.NonSerialized] public ItemData itemData;
        public void AddAmount(int value) 
        {
            if (item.Unlocked == false)
                item.Unlocked = true;
            item.amount += value;
            UpdateSlot();
        }
         
        public void AddLevel(int value)
        {
            item.AddLevel(value);
            UpdateSlot();
        }

        public void AwakeItem()
        {
            item.AwakeLv++;
            UpdateSlot();
        }

        public void EquipItem()
        {
            onBeforeUpdated?.Invoke();

            if (parent.EquipedItem!=null)
            {
                if (parent.EquipedItem.item != item)
                {
                    if (parent.EquipedItem.itemData != null)
                    {
                        parent.EquipedItem.itemData.UnEquipItem();
                    }
                    parent.EquipedItem.onAfterUpdated?.Invoke();
                }
            }

            parent.EquipedItem = this;
            itemData.EquipItem();
            onAfterUpdated?.Invoke();
        }

        public void UpdateSlot()
        {
            //서버로드시에 이큅값이 트루면 여기로 들어올것이다.
            if(item.Equiped==true)
            {
                EquipItem();
                return;
            }

            onBeforeUpdated?.Invoke();

            itemData.UpdateData();
            onAfterUpdated?.Invoke();
        }

    }

}
