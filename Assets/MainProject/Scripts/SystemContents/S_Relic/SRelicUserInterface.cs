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
    public class SRelicUserInterface : MonoBehaviour
    {
        [HideInInspector] public SRelicInventoryObject inventory;
        
        public Dictionary<GameObject, SRelicInventorySlot> slotsOnInterface = new Dictionary<GameObject, SRelicInventorySlot>();
        public GameObject inventorySlotObjectPrefab;

        Dictionary<SkillType, List<SRelicUIDisplay>> SRelicUIList;
        public Button lvUp_1;
        public Button lvUp_10;
        public Button lvUp_100;

        public GameObject lvUp_1Selected;
        public GameObject lvUp_10Selected;
        public GameObject lvUp_100Selected;

        public SpecialSkillInterface specialSkillInterface;

        public SkillInformationUI skillInformationUI;

        [SerializeField] Text PotionText;
        protected virtual void Awake()
        {
            if (inventory == null)
                inventory = InGameManager.Instance.SRelicInventory;

            //BackendManager.Instance.LoadSRelicList(null);
            //Common.InGameManager.Instance.Localdata.LoadsRelicData(() => BackendManager.Instance.LoadSRelicList(null)) ;

            SRelicUIList = new Dictionary<SkillType, List<SRelicUIDisplay>>();

            lvUp_1.onClick.AddListener(() => RelicUIButtonSet(LevelUpUIType.Levelup_1));
            lvUp_10.onClick.AddListener(() => RelicUIButtonSet(LevelUpUIType.Levelup_10));
            lvUp_100.onClick.AddListener(() => RelicUIButtonSet(LevelUpUIType.Levelup_100));

            CreateSlots();

            specialSkillInterface.Init(this);
            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                inventory.GetSlots[i].parent = inventory;
            }

            skillInformationUI.Init();

            foreach (var data in SRelicUIList)
            {
                if (data.Key == SkillType.SKILL_USE_LIGHTNING_SLASH)
                {
                    foreach (var _data in data.Value)
                    {
                        if (_data.slot.srelicData.srelicInfo.skill_idx == (int)data.Key)
                            _data.gameObject.SetActive(true);
                        else
                            _data.gameObject.SetActive(false);
                    }
                }
            }

            RelicUIButtonSet(LevelUpUIType.Levelup_1);

            Message.AddListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }

        public void CreateSlots()
        {
            slotsOnInterface = new Dictionary<GameObject, SRelicInventorySlot>();
            for (int i = 0; i < inventory.GetSlots.Count; i++)
            {
                var obj = Instantiate(inventorySlotObjectPrefab);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.SetParent(transform, false);
                slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            }

            foreach (KeyValuePair<GameObject, SRelicInventorySlot> data in slotsOnInterface)
            {
                SRelicUIDisplay uidisplay = data.Key.GetComponent<SRelicUIDisplay>();
                data.Value.slotDisplay = uidisplay;
                uidisplay.Init(data.Value);
                int skillIndex = data.Value.srelicData.srelicInfo.skill_idx;
                SkillType stype = (SkillType)skillIndex;

                if (SRelicUIList.ContainsKey(stype))
                {
                    SRelicUIList[stype].Add(uidisplay);
                    inventory.GetslotList[stype].Add(data.Value);
                }
                else
                {
                    List<SRelicUIDisplay> newlist = new List<SRelicUIDisplay>();
                    newlist.Add(uidisplay);
                    SRelicUIList.Add(stype, newlist);

                    List<SRelicInventorySlot> _newslotlist = new List<SRelicInventorySlot>();
                    _newslotlist.Add(data.Value);
                    inventory.GetslotList.Add(stype, _newslotlist);
                }

                data.Value.UpdateSlot();
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

            foreach (var data in SRelicUIList)
            {
                foreach (var _data in data.Value)
                {
                    _data.levelupUIType = type;
                    _data.slot.UpdateSlot();
                }
            }
        }

        public void SkillUIButtonSet(SpecialSkillSlotUI slotui)
        {
            SRelicUIDisplay srelicui=null;
            foreach (var data in SRelicUIList)
            {
                foreach (var _data in data.Value)
                {
                    int skillindex = (int)slotui.slot.specialskilldata.skillType;

                    if (_data.slot.srelicData.srelicInfo.skill_idx == skillindex)
                    {
                        _data.gameObject.SetActive(true);
                        srelicui = _data;
                    }
                    else
                        _data.gameObject.SetActive(false);
                }
            }
            if(srelicui!=null)
            {

            }

            skillInformationUI.PopupActiveSkillUI(slotui);
        }

        protected virtual void OnDestroy()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }
        protected virtual void OnApplicationQuit()
        {
        
        }

        void GetCurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.Type == CurrencyType.MagicPotion)
            {
                PotionText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicPotion).value.ToDisplay();
            }
        }
    }

}
