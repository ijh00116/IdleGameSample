using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class PetInventoryObject
    {
        public ItemType type;
        [SerializeField] private PetInventory Container = new PetInventory();
        public List<PetInventorySlot> GetSlots => Container.Slots;

        [System.NonSerialized]public PetInventorySlot EquipedPet;

        [System.NonSerialized] public List<PetInventorySlot> EquipedPetSlotList = new List<PetInventorySlot>();

        public void Init()
        {
            type = ItemType.pet;

            for(int i=0; i< DTConstraintsData.MaxEquiptPetCount; i++)
            {
                //5개 미리 할당;
                EquipedPetSlotList.Add(null);
            }

            for (int i = 0; i < InGameDataTableManager.PetTableList.pet.Count; i++)
            {
                PetInventorySlot slot = new PetInventorySlot();
                slot.pet= new PetObject(InGameDataTableManager.PetTableList.pet[i].idx);
                slot.petData = new PetData(slot.pet);
                GetSlots.Add(slot);
            }
        }

        public bool AddAmount(PetObject item, int value)
        {
            PetInventorySlot slot = FindItemOnInventory(item);
            if (slot == null)
                return false;
  
            //Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup(string.Format("{0} 획득", item._slot.slotDisplay.LocalpetName)));
            slot.AddAmount(value);
            Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.PET_GET, value);
            return true;
        }

        public PetInventorySlot FindItemOnInventory(PetObject item)
        {
            for (int i = 0; i < GetSlots.Count; i++)
            {
                if (GetSlots[i].pet.idx == item.idx)
                {
                    return GetSlots[i];
                }
            }
            return null;
        }


        public PetObject GetItemIninventory(int idx)
        {
            for (int i = 0; i < GetSlots.Count; i++)
            {
                if (GetSlots[i].pet.idx == idx)
                {
                    return GetSlots[i].pet;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class PetInventory
    {
        public List<PetInventorySlot> Slots = new List<PetInventorySlot>();
    }
    [Serializable]
    public class PetInventorySlot
    {
        [System.NonSerialized] public PetInventoryObject parent;
        [System.NonSerialized] public PetUIDisplay slotDisplay;
        [System.NonSerialized] public Action onAfterUpdated;
        [System.NonSerialized] public Action onBeforeUpdated;

        public PetObject pet;
        [System.NonSerialized] public PetData petData;

        UI.Event.PetAmountAdded petadded;
        public PetInventorySlot()
        {
            petadded = new UI.Event.PetAmountAdded();
            petadded.slot = this;
        }
        public void AddAmount(int value)
        {
            pet.amount += value;
            if (pet.Unlocked == false)
                pet.Unlocked = true;
            UpdateSlot();

            petadded.AddCount = value;

            if (value>0)
            {
                Message.Send<UI.Event.PetAmountAdded>(petadded);
            }
        }
        
        public void EquipPet()
        {
            onBeforeUpdated?.Invoke();

            int PossiblePetCount = 2;
            if(Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL>=100)
                PossiblePetCount = 3;
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 150)
                PossiblePetCount = 4;
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 200)
                PossiblePetCount = 5;
            if (parent.EquipedPetSlotList.Contains(this)==false)
            {
                int count = 0;
                for (int i = 0; i < parent.EquipedPetSlotList.Count; i++)
                {
                    if (parent.EquipedPetSlotList[i] != null)
                        count++;
                }
                if(count>= PossiblePetCount)
                {
#if UNITY_EDITOR
                    Debug.LogError("펫슬록 꽉참 해제해주세요");
#endif
                    Message.Send<UI.Event.FlashPopup>(new UI.Event.FlashPopup("펫슬롯이 꽉찼습니다."));
                    return;
                }
                else
                {
                    for (int i = 0; i < parent.EquipedPetSlotList.Count; i++)
                    {
                        if (parent.EquipedPetSlotList[i]==null)
                        {
                            parent.EquipedPetSlotList[i] = this;
                            petData.EquipPet(); //장비 장착
                            break;
                        }
                    }
                }
            }
            onAfterUpdated?.Invoke();//장착 업데이트
        }

        public void UnEquipPet()
        {
            PetInventorySlot slot = parent.EquipedPetSlotList.Find(o => o == this);
            for(int i=0;i< parent.EquipedPetSlotList.Count; i++)
            {
                if(this== parent.EquipedPetSlotList[i])
                {
                    //parent.EquipedPetSlotList[i].pet.amount++;
                    parent.EquipedPetSlotList[i] = null;
                    slot.petData.UnEquipItem();
                    break;
                }
            }
            //parent.EquipedPetSlotList.Remove(slot);
            slot.onAfterUpdated?.Invoke();
        }

        public void AddLevel(int value)
        {
            pet.Level += value;
            UpdateSlot();
        }

        public void Compose()
        {
            PetInventorySlot nextslot = parent.GetSlots.Find(o => o.petData.petInfo.idx == petData.petInfo.idx + 1);
            if(nextslot==null)
            {
#if UNITY_EDITOR
                Debug.LogError("펫 합성 에러 현재 펫 idx: " + petData.petInfo.idx);
#endif
                return;
            }
            AddAmount(-5);
            nextslot.AddAmount(1);
        }

       

        public void UpdateSlot()
        {
            onBeforeUpdated?.Invoke();
            petData.UpdateData();
            onAfterUpdated?.Invoke();
        }

        public void StartSetting()
        {
            if (pet.Equiped)
            {
                EquipPet();
                return;
            }
            petData.UpdateData();
            onAfterUpdated?.Invoke();
        }
    }
}
