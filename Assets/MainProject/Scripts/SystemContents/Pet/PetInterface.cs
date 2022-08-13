using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class PetInterface : MonoBehaviour
    {
        [HideInInspector] PetInventoryObject petinventory;
        public Dictionary<PetUIDisplay, PetInventorySlot> slotsOnInterface = new Dictionary<PetUIDisplay, PetInventorySlot>();
        List<PetUIDisplay> templist = new List<PetUIDisplay>();
        public PetUIDisplay inventorySlotObjectPrefab;

        [SerializeField] List<PetEquipUIslot> EquipedSlotImageList = new List<PetEquipUIslot>();

        [SerializeField] Button AllComposeBtn;
        [SerializeField] Button AllEnforceBtn;
        public void Init()
        {
            petinventory = InGameManager.Instance.petInventory;
            if (petinventory == null)
                return;

            Message.AddListener<UI.Event.PetEquiped>(EquipPetEvent);
           // Common.InGameManager.Instance.Localdata.LoadpetData(()=> BackendManager.Instance.LoadPetList(null));

            for (int i = 0; i < petinventory.GetSlots.Count; i++)
            {
                petinventory.GetSlots[i].parent = petinventory;
            }
            for (int i = 0; i < EquipedSlotImageList.Count; i++)
            {
                EquipedSlotImageList[i].Init(i);
            }
            CreateSlots();

            for (int i = 0; i < petinventory.EquipedPetSlotList.Count; i++)
            {
                EquipedSlotImageList[i].UpdateSlotData(petinventory.EquipedPetSlotList[i]);
            }

            AllComposeBtn.onClick.AddListener(AllCompose);
            AllEnforceBtn.onClick.AddListener(AllEnforce);
        }

        public void Release()
        {
            //BackendManager.Instance.SavePetList(petinventory);
            Message.RemoveListener<UI.Event.PetEquiped>(EquipPetEvent);
        }

        private void OnApplicationQuit()
        {
            Common.InGameManager.Instance.Localdata.SavepetData();
        }

        private void OnApplicationPause(bool pause)
        {
            if(pause)
                Common.InGameManager.Instance.Localdata.SavepetData();
        }
        public void CreateSlots()
        {
            slotsOnInterface = new Dictionary<PetUIDisplay, PetInventorySlot>();
            for (int i = 0; i < petinventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(inventorySlotObjectPrefab, Vector3.zero, Quaternion.identity, transform);

                slotsOnInterface.Add(obj, petinventory.GetSlots[i]);
                
            }

            foreach (KeyValuePair<PetUIDisplay, PetInventorySlot> data in slotsOnInterface)
            {
                data.Key.Init(data.Value);
                data.Value.StartSetting();
            }
        }

        private void Update()
        {
           
        }
        void EquipPetEvent(UI.Event.PetEquiped msg)
        {
            for(int i=0; i< petinventory.EquipedPetSlotList.Count; i++)
            {
                EquipedSlotImageList[i].UpdateSlotData(petinventory.EquipedPetSlotList[i]);
            }
        }

        void AllCompose()
        {
            foreach (KeyValuePair<PetUIDisplay, PetInventorySlot> data in slotsOnInterface)
            {
                while(data.Key.slot.pet.amount>=5)
                {
                    data.Key.PushCompose();
                }
            }
        }
        void AllEnforce()
        {
            foreach (KeyValuePair<PetUIDisplay, PetInventorySlot> data in slotsOnInterface)
            {
                while (data.Key.slot.pet.amount >= 1)
                {
                    data.Key.LevelUpButton();
                }
            }
        }
    }

}
