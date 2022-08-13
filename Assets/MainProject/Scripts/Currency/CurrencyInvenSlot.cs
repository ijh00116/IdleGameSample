using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DLL_Common.Common;

namespace BlackTree
{
    public class CurrencyInvenSlot : MonoBehaviour
    {
        public Button MyButton;
        public Text CurrencyNameDisplay;
        public Text CurrencyDisplay;
        public Image CurrencyIcon;

        [HideInInspector]public Currency MyCurrency;

        [HideInInspector]public GachaActiveWindowInItem gachaactiveWindow;
        public void Init(Currency _currency)
        {
            this.gameObject.SetActive(true);
            MyButton.onClick.AddListener(CurrencyItemTouch);
            MyCurrency = _currency;

            //if(MyCurrency.BoxIdx>0)
            //{
            //    BoxData boxdata= InGameDataTableManager.BoxTableList.box.Find(o => o.idx == MyCurrency.BoxIdx);
            //    LocalValue currencylocal = InGameDataTableManager.LocalizationList.box.Find(o => o.id == boxdata.name);
            //    CurrencyNameDisplay.text = currencylocal.GetStringForLocal(true);
            //}
            //else
            //{
            //    LocalValue currencylocal = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == MyCurrency.currencyType.ToString());
            //    CurrencyNameDisplay.text = currencylocal.GetStringForLocal(true);
            //}
            Updatedata();
        }
  
        public void Updatedata()
        {
            if(MyCurrency.value>0)
            {
                if(this.gameObject.activeInHierarchy==false)
                {
                    this.gameObject.SetActive(true);
                }
                if (MyCurrency.BoxIdx > 0)
                {
                    BoxData boxdata = InGameDataTableManager.BoxTableList.box.Find(o => o.idx == MyCurrency.BoxIdx);
                    LocalValue currencylocal = InGameDataTableManager.LocalizationList.box.Find(o => o.id == boxdata.name);
                    CurrencyNameDisplay.text = currencylocal.GetStringForLocal(true);
                }
                else
                {
                    LocalValue currencylocal = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == MyCurrency.currencyType.ToString());
                    CurrencyNameDisplay.text = currencylocal.GetStringForLocal(true);
                }
            }
            CurrencyDisplay.text = MyCurrency.value.ToDisplay();
            //CurrencyNameDisplay.text = MyCurrency.currencyType.ToString();
            if (CurrencyIcon.sprite==null)
            {
                switch (MyCurrency.currencyType)
                {
                    case CurrencyType.Gold:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/Icon_Gold");
                        break;
                    case CurrencyType.MagicPotion:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_POTION");
                        break;
                    case CurrencyType.SuperMagicPotion:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_ESSENCE");
                        break;
                    case CurrencyType.Gem:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_GEM");
                        break;
                    case CurrencyType.EnchantStone:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_ENCHANT_STONE");
                        break;
                    case CurrencyType.Soul:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_SOUL");
                        break;
                    case CurrencyType.MagicStone:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_MAGIC_STONE");
                        break;
                   
                    case CurrencyType.Mileage:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_MILEAGE");
                        break;
                    case CurrencyType.Ticket_Dungeon:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_PET_TICKET");
                        break;
                    case CurrencyType.Ticket_PetDungeon:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_PET_TICKET");
                        break;
                    case CurrencyType.Ticket_PVPDungeon:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_PET_TICKET");
                        break;
                    case CurrencyType.RouletteCoupon:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_PET_TICKET");
                        break;
                    case CurrencyType.Ticket_pet:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_PET_TICKET");
                        break;
                    case CurrencyType.Ticket_SRelic:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_PET_TICKET");
                        break;
                    case CurrencyType.Ticket_Gacha:
                        CurrencyIcon.sprite = Resources.Load<Sprite>("Images/GUI/Icon/REWARD_PET_TICKET");
                        break;
                    default:
                        break;
                }
                if ((int)MyCurrency.currencyType >= (int)CurrencyType.WeaponBox_D)
                {
                    BoxData boxdata = InGameDataTableManager.BoxTableList.box.Find(o => o.boxType == MyCurrency.currencyType);
                    string icon = boxdata.icon;
                    CurrencyIcon.sprite = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Inventory/{0}", icon));
                }
            }
           
        }

        public void CurrencyItemTouch()
        {
            if (MyCurrency.BoxIdx<0)
                return;

            gachaactiveWindow.Clear();

            int enchantstone = 0;
            int magicstone = 0;

            BigInteger value = MyCurrency.value;
            while (value > 0)
            {
                Dictionary<Box_Group, int> rewardList = InGameManager.Instance.GetPlayerData.rewardinfo.FindboxinfoToBoxRewardIdx(MyCurrency.BoxIdx);
              
                foreach (var data in rewardList)
                {
                    if(data.Key.reward_type==ItemType.enchantstone || data.Key.reward_type == ItemType.magicstone)
                    {
                        if(data.Key.reward_type==ItemType.enchantstone)
                        {
                            enchantstone += data.Value;
                        }
                        else
                        {
                            magicstone += data.Value;
                        }

                    }
                    else
                    {
                        int itemIdx = InGameManager.Instance.GetPlayerData.rewardinfo.FindItemIdx(data.Key.reward_idx);

                        if (data.Key.reward_type==ItemType.weapon)
                        {
                            Item item = InGameManager.Instance.WeaponInventory.GetItemIninventory(itemIdx);
                            InventorySlot slot = InGameManager.Instance.WeaponInventory.FindItemOnInventory(item);
                            InGameManager.Instance.WeaponInventory.AddAmount(item,data.Value);
                            var obj = Instantiate(gachaactiveWindow.GachaGetWeaponSlotPrefab);
                            obj.PopupSetting<InventorySlot>(slot, 1);
                            obj.transform.SetParent(gachaactiveWindow.parent, false);
                            gachaactiveWindow.CurrenctGachalist.Add(obj);
                        }
                        else if (data.Key.reward_type == ItemType.wing)
                        {
                            Item item = InGameManager.Instance.WingInventory.GetItemIninventory(itemIdx);
                            InventorySlot slot = InGameManager.Instance.WingInventory.FindItemOnInventory(item);
                            InGameManager.Instance.WingInventory.AddAmount(item,data.Value);
                            var obj = Instantiate(gachaactiveWindow.GachaGetWeaponSlotPrefab);
                            obj.PopupSetting<InventorySlot>(slot, 1);
                            obj.transform.SetParent(gachaactiveWindow.parent, false);
                            gachaactiveWindow.CurrenctGachalist.Add(obj);
                        }
                        else if (data.Key.reward_type == ItemType.s_relic)
                        {
                            SRelic item = InGameManager.Instance.SRelicInventory.GetItemIninventory(itemIdx);
                            SRelicInventorySlot slot = InGameManager.Instance.SRelicInventory.FindItemOnInventory(item);
                            InGameManager.Instance.SRelicInventory.AddAmount(item,data.Value);
                            var obj = Instantiate(gachaactiveWindow.GachaGetItemSlotPrefab);
                            obj.PopupSetting<SRelicInventorySlot>(slot, 1);
                            obj.transform.SetParent(gachaactiveWindow.parent, false);
                            gachaactiveWindow.CurrenctGachalist.Add(obj);
                        }
                        else if (data.Key.reward_type == ItemType.pet)
                        {
                            PetObject item = InGameManager.Instance.petInventory.GetItemIninventory(itemIdx);
                            PetInventorySlot slot = InGameManager.Instance.petInventory.FindItemOnInventory(item);
                            InGameManager.Instance.petInventory.AddAmount(item, data.Value);
                            var obj = Instantiate(gachaactiveWindow.GachaGetItemSlotPrefab);
                            obj.PopupSetting<PetInventorySlot>(slot, 1);
                            obj.transform.SetParent(gachaactiveWindow.parent, false);
                            gachaactiveWindow.CurrenctGachalist.Add(obj);
                        }
                    }
                }
                value--;
            }

            if (enchantstone > 0)
            {
                InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.EnchantStone, enchantstone);
                var obj = Instantiate(gachaactiveWindow.GachaGetWeaponSlotPrefab);
                obj.PopupSetting<int>(0, enchantstone);
                obj.transform.SetParent(gachaactiveWindow.parent, false);
                gachaactiveWindow.CurrenctGachalist.Add(obj);
            }
            if (magicstone > 0)
            {
                InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicStone, magicstone);
                var obj = Instantiate(gachaactiveWindow.GachaGetWeaponSlotPrefab);
                obj.PopupSetting<int>(0, magicstone);
                obj.transform.SetParent(gachaactiveWindow.parent, false);
                gachaactiveWindow.CurrenctGachalist.Add(obj);
            }
            InGameManager.Instance.GetPlayerData.AddCurrency(MyCurrency.currencyType, -MyCurrency.value);
            if (MyCurrency.value <= 0)
            {
                this.gameObject.SetActive(false);
            }
            gachaactiveWindow.Popup();

        }
    }

}
