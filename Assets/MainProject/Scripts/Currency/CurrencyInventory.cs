using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class CurrencyInventory : MonoBehaviour
    {
        GlobalCurrency currencyList;
        public GameObject CurrencyPrefab;
        public Dictionary<CurrencyType, CurrencyInvenSlot> CurrencySlotList = new Dictionary<CurrencyType, CurrencyInvenSlot>();

        [SerializeField] Button Allbtn;
        [SerializeField] Button Boxbtn;
        [SerializeField] Button Currencybtn;

        [SerializeField] GameObject AllbtnActive;
        [SerializeField] GameObject BoxbtnActive;
        [SerializeField] GameObject CurrencybtnActive;

        int CurrencyInvenType;
        [SerializeField] GachaActiveWindowInItem gachaactivewindow;
        public void Awake()
        {
            Message.AddListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
            currencyList = InGameManager.Instance.GetPlayerData.Playercurrency;
            UpdateCurrencyData();

            AllbtnActive.SetActive(false);
            BoxbtnActive.SetActive(false);
            CurrencybtnActive.SetActive(false);

            AllActive();

            Allbtn.onClick.AddListener(AllActive);
            Boxbtn.onClick.AddListener(BoxActive);
            Currencybtn.onClick.AddListener(CurrencyActive);

            CurrencyInvenType = 0;
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }

        void UpdateCurrencyData()
        {
            for(int i=0; i< currencyList.currencylist.Count; i++)
            {
                if(currencyList.currencylist[i].value>0)
                {
                    if(CurrencySlotList.ContainsKey(currencyList.currencylist[i].currencyType))
                    {
                        CurrencySlotList[currencyList.currencylist[i].currencyType].Updatedata();
                    }
                    else
                    {
                        var obj = Instantiate(CurrencyPrefab, Vector3.zero, Quaternion.identity);
                        obj.transform.SetParent(this.transform, false);
                        CurrencyInvenSlot invenslot = obj.GetComponent<CurrencyInvenSlot>();
                        invenslot.Init(currencyList.currencylist[i]);
                        invenslot.gachaactiveWindow = gachaactivewindow;
                        CurrencySlotList.Add(currencyList.currencylist[i].currencyType, obj.GetComponent<CurrencyInvenSlot>());
                    }
                }
            }
        }

        void GetCurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if(CurrencySlotList.ContainsKey(msg.Type))
            {
                CurrencySlotList[msg.Type].Updatedata();
            }
            else
            {
                var obj = Instantiate(CurrencyPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(this.transform, false);
                CurrencyInvenSlot invenslot = obj.GetComponent<CurrencyInvenSlot>();
                Currency _currency = currencyList.GetCurrency(msg.Type);
                invenslot.Init(_currency);
                invenslot.gachaactiveWindow = gachaactivewindow;
                CurrencySlotList.Add(_currency.currencyType, obj.GetComponent<CurrencyInvenSlot>());
            }

            if(CurrencyInvenType==1)
            {
                foreach (var _data in CurrencySlotList)
                {
                    _data.Value.gameObject.SetActive(false);
                }

                foreach (var _data in CurrencySlotList)
                {
                    if ((int)_data.Key >= (int)CurrencyType.WeaponBox_D && _data.Value.MyCurrency.value > 0)
                    {
                        _data.Value.gameObject.SetActive(true);
                    }
                }
            }
            else if(CurrencyInvenType==2)
            {
                foreach (var _data in CurrencySlotList)
                {
                    _data.Value.gameObject.SetActive(false);
                }

                foreach (var _data in CurrencySlotList)
                {
                    if ((int)_data.Key < (int)CurrencyType.WeaponBox_D && _data.Value.MyCurrency.value > 0)
                    {
                        _data.Value.gameObject.SetActive(true);
                    }
                }
            }
        }

        void AllActive()
        {
            AllbtnActive.SetActive(false);
            BoxbtnActive.SetActive(false);
            CurrencybtnActive.SetActive(false);

            foreach (var _data in CurrencySlotList)
            {
                if(_data.Value.MyCurrency.value > 0)
                {
                    _data.Value.gameObject.SetActive(true);
                }
            }

            AllbtnActive.SetActive(true);
            CurrencyInvenType = 0;
        }

        void BoxActive()
        {
            AllbtnActive.SetActive(false);
            BoxbtnActive.SetActive(false);
            CurrencybtnActive.SetActive(false);

            foreach (var _data in CurrencySlotList)
            {
                _data.Value.gameObject.SetActive(false);
            }

            foreach (var _data in CurrencySlotList)
            {
                if((int)_data.Key>= (int)CurrencyType.WeaponBox_D && _data.Value.MyCurrency.value>0)
                {
                    _data.Value.gameObject.SetActive(true);
                }
            }

            BoxbtnActive.SetActive(true);
            CurrencyInvenType = 1;
        }

        void CurrencyActive()
        {
            AllbtnActive.SetActive(false);
            BoxbtnActive.SetActive(false);
            CurrencybtnActive.SetActive(false);

            foreach (var _data in CurrencySlotList)
            {
                _data.Value.gameObject.SetActive(false);
            }

            foreach (var _data in CurrencySlotList)
            {
                if ((int)_data.Key < (int)CurrencyType.WeaponBox_D && _data.Value.MyCurrency.value > 0)
                {
                    _data.Value.gameObject.SetActive(true);
                }
            }

            CurrencybtnActive.SetActive(true);
            CurrencyInvenType = 2;
        }


    }

}
