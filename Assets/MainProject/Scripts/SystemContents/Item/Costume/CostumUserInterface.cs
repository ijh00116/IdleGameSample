using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree.Items
{
    public class CostumUserInterface : MonoBehaviour
    {
        [HideInInspector] public CostumInventoryObject inventory;
        public Dictionary<GameObject, CostumInventorySlot> slotsOnInterface = new Dictionary<GameObject, CostumInventorySlot>();

        public GameObject inventorySlotObjectPrefab;
        public ItemType inventype;

        List<CostumUIDisplay> costumUIDisplay = new List<CostumUIDisplay>();
        public Button lvUp_1;
        public Button lvUp_10;
        public Button lvUp_100;

        public GameObject lvUp_1Selected;
        public GameObject lvUp_10Selected;
        public GameObject lvUp_100Selected;

        //[SerializeField] Text PotionText;
        public void Init()
        {
            if (inventory == null)
            {
                inventory = Common.InGameManager.Instance.CostumInventory;
            }

            lvUp_1.onClick.AddListener(() => SkillUIButtonSet(LevelUpUIType.Levelup_1));
            lvUp_10.onClick.AddListener(() => SkillUIButtonSet(LevelUpUIType.Levelup_10));
            lvUp_100.onClick.AddListener(() => SkillUIButtonSet(LevelUpUIType.Levelup_100));

            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                inventory.GetSlots[i].parent = inventory;
            }
            //Common.InGameManager.Instance.Localdata.LoadCostumData(()=> BackendManager.Instance.LoadCostumList(null));
            //BackendManager.Instance.LoadCostumList(null);

            CreateSlots();

            SkillUIButtonSet(LevelUpUIType.Levelup_1);

            Message.AddListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }
      
        protected virtual void OnApplicationQuit()
        {
            if (inventory == null)
                return;
            Common.InGameManager.Instance.Localdata.SaveCostumData();
            Message.RemoveListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }

        protected void OnApplicationPause(bool pause)
        {
            if(pause)
            {
                Common.InGameManager.Instance.Localdata.SaveCostumData();
            }
        }
        public void CreateSlots()
        {
            slotsOnInterface = new Dictionary<GameObject, CostumInventorySlot>();
            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(inventorySlotObjectPrefab, Vector3.zero, Quaternion.identity, transform);

                inventory.GetSlots[i].slotDisplay = obj;
                slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            }

            foreach (KeyValuePair<GameObject, CostumInventorySlot> data in slotsOnInterface)
            {
                CostumUIDisplay uidisplay = data.Key.GetComponent<CostumUIDisplay>();
                costumUIDisplay.Add(uidisplay);
                uidisplay.Init(data.Value);

                if(data.Value.item.Equiped)
                    data.Value.EquipItem();
                else
                    data.Value.UpdateSlot();

            }
        }

        void SkillUIButtonSet(LevelUpUIType type)
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
            foreach (var data in costumUIDisplay)
            {
                data.LevelUpUISetting(type);
            }
        }

        void GetCurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.Type == CurrencyType.MagicPotion)
            {
               // PotionText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value.ToDisplay();
            }
        }
    }

}
