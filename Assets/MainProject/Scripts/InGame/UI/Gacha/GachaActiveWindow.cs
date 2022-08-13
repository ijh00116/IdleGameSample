using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using BlackTree.Common;
using Spine;
using DG.Tweening;
using System.Linq;
using DLL_Common.Common;

namespace BlackTree
{
    public class GachaActiveWindow : MonoBehaviour
    {
        [SerializeField] GameObject GachaAnimationParent;
        public SkeletonGraphic GachaEvent;
        public Image WhiteImage;

        [SerializeField] GameObject GachaScrollview;
        [SerializeField] GameObject GachaWeaponScrollview;

        [SerializeField] Transform GachaRewardParent;
        [SerializeField] Transform GachaWeaponRewardParent;
        [SerializeField] GameObject ButtonsParent;
        [SerializeField] Button GachaInfoWindowOffbtn;
        [SerializeField] Button ReGachaButton;
        [SerializeField] Button ReGachaButton_33;

        [SerializeField] GachaRewardUISlot GachaGetItemSlotPrefab;
        [SerializeField] GachaRewardUISlot GachaGetWeaponSlotprefab;
        List<GachaRewardUISlot> RewardList = new List<GachaRewardUISlot>();

        public void Init()
        {
            GachaInfoWindowOffbtn.onClick.AddListener(() => this.gameObject.SetActive(false));
            this.gameObject.SetActive(false);
            Message.AddListener<UI.Event.GachaActivate>(GachaOn);

            //GachaEvent.AnimationState.Complete += OnSpineAnimationEnd;
            //GachaEvent.AnimationState.Event += FadeEvent;
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.GachaActivate>(GachaOn);
        }

        WaitForSeconds waitforanim = new WaitForSeconds(2.367f);

        void GachaOn(UI.Event.GachaActivate msg)
        {
            ButtonsParent.SetActive(false);
            ReGachaButton.onClick.RemoveAllListeners();
            ReGachaButton_33.onClick.RemoveAllListeners();

            WhiteImage.color = new Color(1, 1, 1, 0);
            foreach (var _data in RewardList)
            {
                Destroy(_data.gameObject);
            }
            RewardList.Clear();

            this.gameObject.SetActive(true);
            if(msg.IsEvent==true)
            {
                GachaAnimationParent.SetActive(true);
                GachaEvent.AnimationState.SetAnimation(0, "action_1", false);
            }
            

            StartCoroutine(GachaEventStart(msg));
        }

   
        IEnumerator GachaEventStart(UI.Event.GachaActivate msg)
        {
            if (msg.IsEvent == true)
            {
                yield return waitforanim;
                GachaAnimationParent.SetActive(false);
            }

            foreach (KeyValuePair<int, int> data in msg.Itemvalue)
            {
                GachaRewardUISlot obj = null;

                switch (msg._itemtype)
                {
                    case ItemType.weapon:
                        Item weapon = InGameManager.Instance.WeaponInventory.GetItemIninventory(data.Key);
                        InventorySlot weaponslot= InGameManager.Instance.WeaponInventory.FindItemOnInventory(weapon);
                        InGameManager.Instance.WeaponInventory.AddAmount(weapon, data.Value);
                        for(int i=0; i< data.Value; i++)
                        {
                            obj = Instantiate(GachaGetWeaponSlotprefab);
                            obj.PopupSetting<InventorySlot>(weaponslot, 1);
                            obj.transform.SetParent(GachaWeaponRewardParent, false);
                            RewardList.Add(obj);
                        }
                        break;
                    case ItemType.wing:
                        Item wing = InGameManager.Instance.WingInventory.GetItemIninventory(data.Key);
                        InventorySlot wingslot = InGameManager.Instance.WingInventory.FindItemOnInventory(wing);
                        InGameManager.Instance.WingInventory.AddAmount(wing, data.Value);
                        for (int i = 0; i < data.Value; i++)
                        {
                            obj = Instantiate(GachaGetWeaponSlotprefab);
                            obj.PopupSetting<InventorySlot>(wingslot, 1);
                            obj.transform.SetParent(GachaWeaponRewardParent, false);
                            RewardList.Add(obj);
                        }
                        break;
                    case ItemType.pet:
                        PetObject pet = InGameManager.Instance.petInventory.GetItemIninventory(data.Key);
                        PetInventorySlot petslot = InGameManager.Instance.petInventory.FindItemOnInventory(pet);
                        InGameManager.Instance.petInventory.AddAmount(pet, data.Value);
                        for (int i = 0; i < data.Value; i++)
                        {
                            obj = Instantiate(GachaGetItemSlotPrefab);
                            obj.PopupSetting<PetInventorySlot>(petslot, 1);
                            obj.transform.SetParent(GachaRewardParent, false);
                            RewardList.Add(obj);
                        }
                       
                        break;
                    case ItemType.s_relic:
                        SRelic srelic = InGameManager.Instance.SRelicInventory.GetItemIninventory(data.Key);
                        SRelicInventorySlot Srslot = InGameManager.Instance.SRelicInventory.FindItemOnInventory(srelic);
                        InGameManager.Instance.SRelicInventory.AddAmount(srelic, data.Value);
                        for (int i = 0; i < data.Value; i++)
                        {
                            obj = Instantiate(GachaGetItemSlotPrefab);
                            obj.PopupSetting<SRelicInventorySlot>(Srslot, 1);
                            obj.transform.SetParent(GachaRewardParent, false);
                            RewardList.Add(obj);
                        }
                        break;
                    default:
                        break;
                }
            }
            if (msg._itemtype == ItemType.weapon || msg._itemtype == ItemType.wing)
            {
                if (msg.EnforceJewelAmount > 0)
                {
                    var _obj = Instantiate(GachaGetWeaponSlotprefab);
                    _obj.PopupSetting<int>(0, msg.EnforceJewelAmount);
                    _obj.transform.SetParent(GachaWeaponRewardParent, false);
                    RewardList.Add(_obj);
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.EnchantStone, msg.EnforceJewelAmount);
                }
                GachaScrollview.SetActive(false);
                GachaWeaponScrollview.SetActive(true);
            }
            else
            {
                GachaScrollview.SetActive(true);
                GachaWeaponScrollview.SetActive(false);
            }

            for(int i=0; i< RewardList.Count; i++)
            {
                RewardList[i].gameObject.SetActive(false);
            }
            var rnd = new System.Random();
            var randomized = RewardList.OrderBy(o => rnd.Next());

            foreach(var _data in randomized)
            {
                _data.gameObject.SetActive(true);
                _data.gameObject.transform.SetAsLastSibling();
                yield return null;
                yield return null;
                yield return null;
            }
            //for (int i=0; i< RewardList.Count; i++)
            //{
            //    RewardList[i].gameObject.SetActive(true);
            //    yield return null;
            //    yield return null;
            //}
            ButtonsActivate(msg);
        }

        void ButtonsActivate(UI.Event.GachaActivate msg)
        {
            ButtonsParent.SetActive(true);
            if(msg._buttonType.gachaType!=GachaType.Advertise)
            {
                ReGachaButton.gameObject.SetActive(true);
                ReGachaButton_33.gameObject.SetActive(true);
                ReGachaButton.onClick.AddListener(() => { msg._buttonType.PushButton(msg._buttonType.RewardCount); });
                ReGachaButton_33.onClick.AddListener(() => { msg._buttonType.PushButton(-1); });
            }
            else
            {
                ReGachaButton.gameObject.SetActive(false);
                ReGachaButton_33.gameObject.SetActive(false);
            }
          
        }

        CurrencyType GetCurrencyType(GachaType _gachatype)
        {
            string _type = _gachatype.ToString();
            CurrencyType _itemtype = EnumExtention.ParseToEnum<CurrencyType>(_type);
            return _itemtype;
        }

        public void OnSpineAnimationEnd(TrackEntry trackentry)
        {
            GachaAnimationParent.SetActive(false);
        }
        public void FadeEvent(TrackEntry trackentry, Spine.Event _event)
        {
            if (_event.Data.Name == "open eyes")
                WhiteImage.DOFade(1, 0.3f);
        }
    }

}
