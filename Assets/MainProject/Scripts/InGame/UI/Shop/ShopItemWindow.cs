using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ShopItemWindow : MonoBehaviour
    {
        [Header("상단")]
        [SerializeField] Text ItemTitle;
        [SerializeField] Image IconImage;
        [SerializeField] Text ItemCountText;
        [SerializeField] ShopSubItem ShopSubItemPrefab;

        [Header("중단")]
        [SerializeField] GameObject eventIcon;
        [SerializeField] Text ItemDetailContentText;
        [SerializeField] Transform SubgoodsParent;

        [Header("하단버튼")]
        [SerializeField] Image ButtonImage;
        [SerializeField] Button BuyButton;
        [SerializeField] Text ButtonPriceTxt;

        [Header("재화 이미지")]
        [SerializeField] ShopSubItem Subitem;

        public ShopGoodstable myShopData;
        ShopRewardInfotable myRewardInfo;
        float NeedPrice=-1;
        string AddItemFeedbackmsg;

        UI.Event.FlashPopup Flashpopup = new UI.Event.FlashPopup(null);
        public void Init(ShopGoodstable tabledata)
        {
            myShopData = tabledata;
            BuyButton.onClick.AddListener(ButtonPush);

            if (myShopData.price_type != PriceType.payment)
                NeedPrice = myShopData.price_count;
            else
                NeedPrice = Common.InGameManager.Instance.IAPManager.GetPaymentPrice(myShopData.iap_idx.ToString());

            //아이템 정보 업데이트
            LocalValue _itemtitle = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == myShopData.name);
            ItemTitle.text = _itemtitle.GetStringForLocal(true);
            Sprite _iconImage = Resources.Load<Sprite>(string.Format("Images/GUI/PayShopIcon/{0}", myShopData.icon));
            if (_iconImage != null)
                IconImage.sprite = _iconImage;

            ShopRewardInfotable itemrewardInfo= InGameDataTableManager.shopTableList.reward.Find(o => o.idx == myShopData.reward_idx);
            if(itemrewardInfo!=null)
            {
                string IconCount = string.Format("x {0}", itemrewardInfo.reward_count);
                ItemCountText.text = IconCount;
            }

            eventIcon.SetActive(myShopData.event_icon == 1);

            LocalValue _itemdetailedtitle = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == myShopData.desc_1);
            ItemDetailContentText.text = _itemdetailedtitle.GetStringForLocal(true);

            if (myShopData.price_type != PriceType.payment)
                ButtonPriceTxt.text = string.Format("W {0:N2}", NeedPrice);
            else
                ButtonPriceTxt.text = string.Format("{0} {1:N2}", myShopData.price_type.ToString(), NeedPrice);

            //아이템 정보 업데이트
            LocalValue feedbackmsg = InGameDataTableManager.LocalizationList.shop.Find(o => o.id == myShopData.message);
            if(feedbackmsg!=null)
                AddItemFeedbackmsg = feedbackmsg.GetStringForLocal(true);

            //서브아이템 리스트 업뎃
            //if(myShopData.subinfoIcon_0!=RewardType.None)
            //{
            //    var subitem = Instantiate(Subitem);
            //    subitem.transform.SetParent(SubgoodsParent);


            //}
        }

        private void OnEnable()
        {
            if (myShopData == null)
                return;
            if (NeedPrice < 0)
            {
                if (myShopData.price_type != PriceType.payment)
                {
                    NeedPrice = myShopData.price_count;
                    ButtonPriceTxt.text = string.Format("W {0:N2}", NeedPrice);
                }
                else
                {
                    NeedPrice = Common.InGameManager.Instance.IAPManager.GetPaymentPrice(myShopData.iap_idx.ToString());
                    ButtonPriceTxt.text = string.Format("{0} {1:N2}", myShopData.price_type.ToString(), NeedPrice);
                }
                    
            }
        }
        public void Release()
        {
            BuyButton.onClick.RemoveAllListeners();
        }

        void ButtonPush()
        {
          
            if(myShopData.price_type == PriceType.Gem_Count)
            {
                if (NeedPrice > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value)
                    return;
                AddReward(myRewardInfo);
            }
            if (myShopData.price_type == PriceType.Gem)
            {
                if (myShopData.price_count > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value)
                    return;
                AddReward(myRewardInfo);
            }
            if (myShopData.price_type== PriceType.Ad)
            {
                Common.InGameManager.Instance.admob.ShowRewardAd(()=> { AddReward(myRewardInfo); } );
            }
            if (myShopData.price_type == PriceType.payment)
            {
                //구글 올리고 상점 본격적 구현시 업데이트
                Common.InGameManager.Instance.IAPManager.Purchase(myShopData.iap_idx.ToString() ,()=> { AddReward(myRewardInfo);});
            }
            if (myShopData.price_type ==PriceType.pvp_point)
            {
                //AddReward(myRewardInfo);
            }
        }

        void AddReward(ShopRewardInfotable RewardIdx)
        {
            ShopRewardInfotable rewardinfo = RewardIdx;
            RewardType type = rewardinfo.reward_type;
            int rewardcount = rewardinfo.reward_count;
            CurrencyType currencytype=CurrencyType.NotCurrency;
            switch (type)
            {
                case RewardType.REWARD_GEM:
                    currencytype = CurrencyType.Gem;
                    break;
                case RewardType.REWARD_ESSENCE:
                    currencytype = CurrencyType.SuperMagicPotion;
                    break;
                case RewardType.REWARD_POTION:
                    currencytype = CurrencyType.MagicPotion;
                    break;
                case RewardType.REWARD_SOUL:
                    currencytype = CurrencyType.Soul;
                    break;
                case RewardType.REWARD_ENCHANT_STONE:
                    currencytype = CurrencyType.EnchantStone;
                    break;
                case RewardType.REWARD_MAGIC_STONE:
                    currencytype = CurrencyType.MagicStone;
                    break;
                case RewardType.REWARD_DG_TICKET:
                    currencytype = CurrencyType.Ticket_Dungeon;
                    break;
                case RewardType.REWARD_PVP_TICKET:
                    currencytype = CurrencyType.Ticket_PVPDungeon;
                    break;
                case RewardType.REWARD_PET_TICKET:
                    currencytype = CurrencyType.Ticket_PetDungeon;
                    break;
                case RewardType.REWARD_MILEAGE:
                    currencytype = CurrencyType.Mileage;
                    break;
                case RewardType.REWARD_BOX:
                    currencytype =Common.InGameManager.Instance.GetPlayerData.Playercurrency.GetIdxToType(int.Parse(rewardinfo.reward_box_idx));
                    break;
                case RewardType.REWARD_BUFF_ATK:
                    break;
                default:
                    break;
            }

            if(currencytype!=CurrencyType.NotCurrency)
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(currencytype, rewardcount);
            else//재화가 아닐 경우
            {
                if (type == RewardType.REWARD_BUFF_ATK)
                {
                    Message.Send<InGame.Event.BuffActivate>(new InGame.Event.BuffActivate(InGame.BuffType.AttackPower, DTConstraintsData.BuffTime));
                }
            }
#if UNITY_EDITOR
            Debug.Log("광고보상 : " + type.ToString()+":"+ currencytype + ":"+ rewardcount);
#endif
            Flashpopup.Eventmsg = AddItemFeedbackmsg;
            Message.Send(Flashpopup);
        }
    }

}
