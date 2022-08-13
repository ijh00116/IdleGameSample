using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ShopUI : MonoBehaviour
    {
        [Header("상점 윈도우")]
        public GameObject limitedWindow;
        public GameObject limitedWindowParent;
        public GameObject PackageWindow;
        public GameObject PackageWindowParent;
        public GameObject MileageWindow;
        public GameObject MileageWindowNormalParent;
        public GameObject MileageWindowExParent;
        public GameObject GemWindow;
        public GameObject GemWindowParent;
        public GameObject GoodsWindow;
        public GameObject GoodsWindowParent;

        [Header("상점 버튼")]
        public Button LimitedButton;
        public GameObject LimitedButtonSelected;
        public Button PackageButton;
        public GameObject PackageButtonSelected;
        public Button MileageButton;
        public GameObject MileageButtonSelected;
        public Button GemButton;
        public GameObject GemButtonSelected;
        public Button GoodsButton;
        public GameObject GoodsButtonSelected;

        public ShopItemWindow UISlotDefaultPrefab;
        public ShopItemWindow UISlotNormalPrefab;
        public ShopItemWindow UISlotExPrefab;

        List<ShopItemWindow> UISlotList = new List<ShopItemWindow>();
        public void Init()
        {
            LimitedButton.onClick.AddListener(()=> { ButtonClick(LimitedButton); });
            PackageButton.onClick.AddListener(() => { ButtonClick(PackageButton); });
            MileageButton.onClick.AddListener(() => { ButtonClick(MileageButton); });
            GemButton.onClick.AddListener(() => { ButtonClick(GemButton); });
            GoodsButton.onClick.AddListener(() => { ButtonClick(GoodsButton); });

            ShopItemWindow prefabForUISlot =null;


            for (int i=0; i<InGameDataTableManager.shopTableList.limited.Count; i++)
            {
                CreateUISlot(InGameDataTableManager.shopTableList.limited[i], limitedWindowParent.transform);
            }

            for (int i = 0; i < InGameDataTableManager.shopTableList.package.Count; i++)
            {
                CreateUISlot(InGameDataTableManager.shopTableList.package[i], PackageWindowParent.transform);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.mileage.Count; i++)
            {
                if(InGameDataTableManager.shopTableList.mileage[i].LayoutType==LayoutType.ex)
                    CreateUISlot(InGameDataTableManager.shopTableList.mileage[i], MileageWindowExParent.transform);
                else
                    CreateUISlot(InGameDataTableManager.shopTableList.mileage[i], MileageWindowNormalParent.transform);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.gem.Count; i++)
            {
                CreateUISlot(InGameDataTableManager.shopTableList.gem[i], GemWindowParent.transform);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.goods.Count; i++)
            {
                CreateUISlot(InGameDataTableManager.shopTableList.goods[i], GoodsWindowParent.transform);
            }
            ButtonClick(LimitedButton);
        }

        public void CreateUISlot(ShopGoodstable _GoodsList,Transform parent)
        {
            ShopItemWindow prefabForUISlot = null;

            if (_GoodsList.LayoutType == LayoutType.none)
                prefabForUISlot = UISlotDefaultPrefab;
            else if (_GoodsList.LayoutType == LayoutType.normal)
                prefabForUISlot = UISlotNormalPrefab;
            else if (_GoodsList.LayoutType == LayoutType.ex)
                prefabForUISlot = UISlotExPrefab;

            var obj = Instantiate(prefabForUISlot);
            obj.transform.SetParent(parent, false);
            obj.Init(_GoodsList);
            UISlotList.Add(obj);
        }

        public void Release()
        {
            foreach(var _data in UISlotList)
            {
                _data.Release();
            }
        }

        void ButtonClick(Button ShopBtn)
        {
            LimitedButtonSelected.SetActive(false);
            PackageButtonSelected.SetActive(false);
            MileageButtonSelected.SetActive(false);
            GemButtonSelected.SetActive(false);
            GoodsButtonSelected.SetActive(false);

            if (ShopBtn== LimitedButton)
            {
                LimitedButtonSelected.SetActive(true);
                limitedWindow.transform.SetAsLastSibling();
            }
            else if (ShopBtn == PackageButton)
            {
                PackageButtonSelected.SetActive(true);
                PackageWindow.transform.SetAsLastSibling();
            }
            else if (ShopBtn == MileageButton)
            {
                MileageButtonSelected.SetActive(true);
                MileageWindow.transform.SetAsLastSibling();
            }
            else if (ShopBtn == GemButton)
            {
                GemButtonSelected.SetActive(true);
                GemWindow.transform.SetAsLastSibling();
            }
            else if (ShopBtn == GoodsButton)
            {
                GoodsButtonSelected.SetActive(true);
                GoodsWindow.transform.SetAsLastSibling();
            }
        }
    }

}
