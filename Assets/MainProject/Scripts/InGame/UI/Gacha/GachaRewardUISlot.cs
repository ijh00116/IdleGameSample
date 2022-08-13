using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class GachaRewardUISlot : MonoBehaviour
    {
        [SerializeField] Image Icon;
        [SerializeField] Image OutLine;
        [SerializeField] Text Amount;
        [SerializeField] Text ItemName;

        int amountvalue = 0;
        public void PopupSetting<T>(T item,int amount)
        {
            amountvalue = amount;
            Amount.gameObject.SetActive(false);
            if (item.GetType() == typeof(int))//강화석
            {
                Amount.gameObject.SetActive(true);
                Amount.text = string.Format("x{0}", amount.ToString()); 
                ItemName.text = "강화석";
            }
            if(item.GetType()==typeof(InventorySlot))
            {
                InventorySlot _data = item as InventorySlot;
                OutLine.sprite = _data.display.currentOutlineSpriteImage;
                Icon.sprite = _data.display.currentSpriteImage;
                LocalValue _itemname = InGameDataTableManager.LocalizationList.weapon.Find(o => o.id == _data.itemData.itemInfo.name);
                ItemName.text = _itemname.kr;
                //Amount.text = amount.ToString();
            }

            if (item.GetType() == typeof(SRelicInventorySlot))
            {
                SRelicInventorySlot _data = item as SRelicInventorySlot;
                
                Icon.sprite = _data.slotDisplay.currentSpriteImage;
                LocalValue _itemname = InGameDataTableManager.LocalizationList.relic.Find(o => o.id == _data.srelicData.srelicInfo.name);
                ItemName.text = _itemname.kr;
                //Amount.text = amount.ToString();
            }

            if (item.GetType() == typeof(PetInventorySlot))
            {
                PetInventorySlot _data = item as PetInventorySlot;
                Icon.sprite = _data.slotDisplay.CurrentPetImagesprite;
                LocalValue _itemname = InGameDataTableManager.LocalizationList.pet.Find(o => o.id == _data.petData.petInfo.name);
                ItemName.text = _itemname.kr;
                //Amount.text = amount.ToString();
            }
        }

    }

}
