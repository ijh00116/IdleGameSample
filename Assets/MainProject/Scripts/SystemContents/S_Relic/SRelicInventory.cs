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
    public class SRelicInventoryObject
    {
        public ItemType type;
        [SerializeField]
        private SRelicInventory Container = new SRelicInventory();
        public List<SRelicInventorySlot> GetSlots => Container.Slots;

        [System.NonSerialized] public Dictionary<SkillType, List<SRelicInventorySlot>> GetslotList=new Dictionary<SkillType, List<SRelicInventorySlot>>();


        public void Init()
        {
            for (int i = 0; i < InGameDataTableManager.RelicList.s_relic.Count; i++)
            {
                SRelicInventorySlot slot = new SRelicInventorySlot();
               
                slot.srelic = new SRelic(InGameDataTableManager.RelicList.s_relic[i].idx);
                slot.srelicData = new SRelicData(slot.srelic);
                GetSlots.Add(slot);
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            Container.Clear();
        }

        public bool AddAmount(SRelic item, int value)
        {
            SRelicInventorySlot slot = FindItemOnInventory(item);
            if (slot == null)
                return false;

            
           // Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(string.Format("{0}획득", item.srelicData.srelicName)));
            slot.AddAmount(value);
            return true;
        }

        public SRelicInventorySlot FindItemOnInventory(SRelic item)
        {
            for (int i = 0; i < GetSlots.Count; i++)
            {
                if (GetSlots[i].srelic.idx == item.idx)
                {
                    return GetSlots[i];
                }
            }
            return null;
        }


        public SRelic GetItemIninventory(int idx)
        {
            for (int i = 0; i < GetSlots.Count; i++)
            {
                if (GetSlots[i].srelic.idx == idx)
                {
                    return GetSlots[i].srelic;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class SRelicInventory
    {
        public List<SRelicInventorySlot> Slots = new List<SRelicInventorySlot>();
        public void Clear()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].srelic = new SRelic();
            }
        }
    
    }

    [System.Serializable]
    public class SRelicInventorySlot
    {
        [System.NonSerialized] public SRelicInventoryObject parent;
        [System.NonSerialized] public SRelicUIDisplay slotDisplay;

        [System.NonSerialized] public Action onAfterUpdated;
        [System.NonSerialized] public Action onBeforeUpdated;

        public SRelic srelic;
        [System.NonSerialized] public SRelicData srelicData;
        public void AddAmount(int value)
        {
            srelic.amount += value;
            if (srelic.Unlocked == false)
                srelic.Unlocked = true;
            UpdateSlot();

        }

        public void AddLevel(int value)
        {
            srelic.AddLevel(value);
            UpdateSlot();

        }

        public void UpdateSlot()
        {
            onBeforeUpdated?.Invoke();

            srelicData.UpdateData();

            onAfterUpdated?.Invoke();
        }
    }
}
