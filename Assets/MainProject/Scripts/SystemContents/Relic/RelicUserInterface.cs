using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree.Items
{
    public class RelicUserInterface : MonoBehaviour
    {
        [HideInInspector] public RelicInventoryObject inventory;
        public Dictionary<GameObject, RelicInventorySlot> slotsOnInterface = new Dictionary<GameObject, RelicInventorySlot>();
        public GameObject inventorySlotObjectPrefab;
        List<ArtifactUIDisplay> RelicUIList;
        public Button lvUp_1;
        public Button lvUp_10;
        public Button lvUp_100;

        public GameObject lvUp_1Selected;
        public GameObject lvUp_10Selected;
        public GameObject lvUp_100Selected;
        //public ArtifactUIETC artifactUIEtc;

        protected virtual void Awake()
        {
            if (inventory == null)
                inventory = InGameManager.Instance.RelicInventory;

            RelicUIList = new List<ArtifactUIDisplay>();
            lvUp_1.onClick.AddListener(() => RelicUIButtonSet(LevelUpUIType.Levelup_1));
            lvUp_10.onClick.AddListener(() => RelicUIButtonSet(LevelUpUIType.Levelup_10));
            lvUp_100.onClick.AddListener(() => RelicUIButtonSet(LevelUpUIType.Levelup_100));

            //InGameManager.Instance.Localdata.LoadRelicData(() => { BackendManager.Instance.LoadRelicList(null); });
           
            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                inventory.GetSlots[i].parent = inventory;
            }
            CreateSlots();

           // artifactUIEtc.Init();

            RelicUIButtonSet(LevelUpUIType.Levelup_1);
        }
        public void CreateSlots()
        {
            slotsOnInterface = new Dictionary<GameObject, RelicInventorySlot>();
            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(inventorySlotObjectPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(transform, false);
                inventory.GetSlots[i].slotDisplay = obj;
                slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            }

            bool IsbuybtnActivate = false;
            foreach (KeyValuePair<GameObject, RelicInventorySlot> data in slotsOnInterface)
            {
                ArtifactUIDisplay uidisplay = data.Key.GetComponent<ArtifactUIDisplay>();
                uidisplay.Init(data.Value);
                RelicUIList.Add(uidisplay);
                data.Value.UpdateSlot();
                if (data.Value.item.Unlocked)
                    IsbuybtnActivate = false;
                else
                {
                    if(IsbuybtnActivate==false)
                    {
                        IsbuybtnActivate = true;
                        uidisplay.ActivateBuyButton();
                    }
                }
            }

        }

        void RelicUIButtonSet(LevelUpUIType type)
        {
            lvUp_1Selected.SetActive(false);
            lvUp_10Selected.SetActive(false);
            lvUp_100Selected.SetActive(false);

            switch (type)
            {
                case LevelUpUIType.Levelup_1:
                    lvUp_1Selected.SetActive(true);
                    break;
                case LevelUpUIType.Levelup_10:
                    lvUp_10Selected.SetActive(true);
                    break;
                case LevelUpUIType.Levelup_100:
                    lvUp_100Selected.SetActive(true);
                    break;
                default:
                    break;
            }

            foreach (var data in RelicUIList)
            {
                data.levelupUIType = type;
                data.slot.onAfterUpdated?.Invoke();
            }
        }
        protected void OnDestroy()
        {
            if (inventory == null)
                return;

           // artifactUIEtc.Release();
        }
        private void OnApplicationQuit()
        {
            InGameManager.Instance.Localdata.SaverelicData();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                InGameManager.Instance.Localdata.SaverelicData();
            }
        }
    }
}
