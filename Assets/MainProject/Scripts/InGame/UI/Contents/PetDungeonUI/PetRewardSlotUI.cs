using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class PetRewardSlotUI : MonoBehaviour
    {
        [SerializeField]public Image itemimage;
        [SerializeField] GameObject BlackImage;
        [SerializeField] Text EarnAmount;
        public PetInventorySlot slot;
        public void Setting(int amount, bool Earn=true)
        {
            BlackImage.SetActive(Earn);
            EarnAmount.text = string.Format("x{0}", amount);
        }
        public void Initialize()
        {
            itemimage.sprite = slot.slotDisplay.CurrentPetImagesprite;
            EarnAmount.text = null;
            BlackImage.SetActive(true);
            Amount = 0;
        }
        public int Amount = 0;
        public void AddAmount(int amount)
        {
            if(BlackImage.activeInHierarchy)
                BlackImage.SetActive(false);
            Amount+=amount;
            EarnAmount.text = string.Format("x{0}", Amount);
        }
    }

}
