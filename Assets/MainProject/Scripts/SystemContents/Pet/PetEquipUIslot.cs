using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class PetEquipUIslot : MonoBehaviour
    {
        [SerializeField] Button UnequipPetbtn;
        [SerializeField] GameObject petimage;
        Image PetImagesprite;
        [SerializeField] GameObject petLocked;
        PetInventorySlot slotData;
        [SerializeField] Text UnlockInfo;
        int PossiblePetCount;
        int UnlockLv = 100;
        int currentEquipedSlotIndex = 0;
        public void Init(int i)
        {
            PossiblePetCount = 1;
            UnlockLv = 100;
            currentEquipedSlotIndex = i;
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 100)
            {
                PossiblePetCount = 2;
                UnlockLv = 100;
            }
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 150)
            {
                PossiblePetCount = 3;
                UnlockLv = 150;
            }
                
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 200)
            {
                PossiblePetCount = 4;
                UnlockLv = 200;
            }
                

            if(currentEquipedSlotIndex > PossiblePetCount)
            {
                UnlockInfo.gameObject.SetActive(true);
                UnlockInfo.text = string.Format("Lv{0} 달성시\n해금", UnlockLv);
            }
            else
            {
                UnlockInfo.gameObject.SetActive(false);
            }
            UnequipPetbtn.onClick.AddListener(Unequip);
            PetImagesprite = petimage.GetComponent<Image>();

            Message.AddListener<UI.Event.GlobalLvUp>(GlobalLvChanged);
        }

        public void UpdateSlotData(PetInventorySlot _slot)
        {
            if(_slot!=null)
            {
                slotData = _slot;
                petLocked.SetActive(false);
                petimage.SetActive(true);
                PetImagesprite.sprite = _slot.petData.MyUIIcon;
            }
            else
            {
                slotData = null;
                petLocked.SetActive(true);
                petimage.SetActive(false);
            }
 
        }

        void Unequip()
        {
            if (slotData == null)
                return;
            slotData.UnEquipPet();
        }

        void GlobalLvChanged(UI.Event.GlobalLvUp msg)
        {
            PossiblePetCount = 1;
            UnlockLv = 100;
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 100)
            {
                PossiblePetCount = 2;
                UnlockLv = 100;
            }
            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 150)
            {
                PossiblePetCount = 3;
                UnlockLv = 150;
            }

            if (Common.InGameManager.Instance.GetPlayerData.GlobalUser.LEVEL >= 200)
            {
                PossiblePetCount = 4;
                UnlockLv = 200;
            }


            if (currentEquipedSlotIndex > PossiblePetCount)
            {
                UnlockInfo.gameObject.SetActive(true);
                UnlockInfo.text = string.Format("Lv{0} 달성시\n해금", UnlockLv);
            }
            else
            {
                UnlockInfo.gameObject.SetActive(false);
            }
        }
    }
}

