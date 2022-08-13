using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree
{
    public class SpecialSkillInterface : MonoBehaviour
    {
        [HideInInspector] public SpecialSkillInventoryObject inventory;
        public Dictionary<GameObject, SpecialSkillInventorySlot> slotsOnInterface = new Dictionary<GameObject, SpecialSkillInventorySlot>();

        public GameObject inventorySlotObjectPrefab;

        [HideInInspector]public List<SpecialSkillSlotUI> specialSkillUIList=new List<SpecialSkillSlotUI>();
        Items.SRelicUserInterface srelicInterface;
        public void Init(Items.SRelicUserInterface srelicinterface)
        {
            srelicInterface = srelicinterface;
            if (inventory == null)
                inventory = InGameManager.Instance.SpecialSkillInventory;

            //Common.InGameManager.Instance.Localdata.LoadskillData(()=> BackendManager.Instance.LoadSpecialSkillList(null));
            CreateSlots();
            
        }

        public void CreateSlots()
        {
            slotsOnInterface = new Dictionary<GameObject, SpecialSkillInventorySlot>();
            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(inventorySlotObjectPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(transform, false);
                //inventory.GetSlots[i] = obj;
                slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            }

            foreach (var data in slotsOnInterface)
            {
                SpecialSkillSlotUI uidisplay = data.Key.GetComponent<SpecialSkillSlotUI>();
                uidisplay.Init(data.Value);
                uidisplay.SrelicUI = srelicInterface;
                uidisplay.skillinterface = this;
                data.Value.UpdateSlot();
                specialSkillUIList.Add(uidisplay);
            }

            specialSkillUIList[0].PushButton();
        }

        private void OnApplicationQuit()
        {
            if (inventory == null)
                return;
            Common.InGameManager.Instance.Localdata.SaveskillData();
        }

        private void OnApplicationPause(bool pause)
        {
            if (inventory == null)
                return;
            Common.InGameManager.Instance.Localdata.SaveskillData();
        }

    }
}
