using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlackTree
{
    public class RelicInventoryObject
    {
        public ItemType type;
        [SerializeField]
        private RelicInventory Container = new RelicInventory();
        public List< RelicInventorySlot> GetSlots => Container.Slots;

        public void Init()
        {
            type = ItemType.Relic;
            for (int i = 0; i < InGameDataTableManager.RelicList.relic.Count; i++)
            {
                RelicInventorySlot slot = new RelicInventorySlot();
                slot.item = new Relic(InGameDataTableManager.RelicList.relic[i].idx);
                slot.itemData = new RelicData(slot.item);
                GetSlots.Add(slot);
            }
        }

        [ContextMenu("Load")]
        public void Load()
        {
            //for (int i = 0; i < InGameDataTableManager.RelicList.relic.Count; i++)
            //{
            //    GetSlots[i].item = new Relic(InGameDataTableManager.RelicList.relic[i].idx);
            //    GetSlots[i].UpdateSlot();
            //}
        }
    }

    [System.Serializable]
    public class RelicInventory
    {
        public List<RelicInventorySlot> Slots = new List<RelicInventorySlot>();
    }

    //저장해야 할 데이터들...
    [System.Serializable]
    public class RelicInventorySlot
    {
        [System.NonSerialized] public RelicInventoryObject parent;
        [System.NonSerialized] public GameObject slotDisplay;

        [System.NonSerialized] public Action onAfterUpdated;
        [System.NonSerialized] public Action onBeforeUpdated;
        [System.NonSerialized] public ArtifactUIDisplay uiDisplay;

        public Relic item;
        [System.NonSerialized] public RelicData itemData;

        public void AddAmount(int value)
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
            onBeforeUpdated?.Invoke();

            itemData.UpdateData();

            onAfterUpdated?.Invoke();
        }

    }

}

