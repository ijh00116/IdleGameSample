using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DLL_Common.Common;
using BlackTree.Common;
using BlackTree.UI.Event;

namespace BlackTree
{
    //일단은 버튼에 설정된 값대로 설정해줌
    public class GachaButtonType : MonoBehaviour
    {
        public ItemType GaChaItemType;
        public GachaType gachaType;
        public int Price_Value;
        public int RewardCount;
        int Currentrewardcount;
        Button GachaButton;
        Gacha myGachaInfo;

        GachaActivate gachaActive=new GachaActivate();

        int _boxidx = 0;
        int currentFreePoint;
        int freerewardIdx = 0;

        [SerializeField] Text AdLeftTime;
        [SerializeField] GameObject SeeAd;
        bool CanSeeAd;
        public void Init()
        {
            Gacha _gachainfo=
                InGameDataTableManager.GachaTableList.gacha.Find(o => o.category == GaChaItemType && o.price_type == gachaType.ToString() && o.reward_count == RewardCount);
            Price_Value = _gachainfo.price_value;
            if (gachaType==GachaType.Free)
            {
                switch (GaChaItemType)
                {
                    case ItemType.weapon:
                        currentFreePoint = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].point;
                        _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].weapon_box;
                        break;
                    case ItemType.wing:
                        currentFreePoint = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].point;
                        _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].wing_box;
                        break;
                    case ItemType.pet:
                        currentFreePoint = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].point;
                        _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].pet_box;
                        break;
                    case ItemType.s_relic:
                        currentFreePoint = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].point;
                        _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].s_relic_box;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                myGachaInfo = InGameDataTableManager.GachaTableList.gacha.Find(o => o.price_type == gachaType.ToString() &&
                     o.price_value == Price_Value &&
                    o.category == GaChaItemType);
            }

            GachaButton = this.GetComponent<Button>();
            GachaButton.onClick.AddListener(()=> { PushButton(RewardCount,true); });
        }

        public void Release()
        {
            GachaButton.onClick.RemoveAllListeners();
        }

        float currentAdLeftTime=0;
        private void Update()
        {
            if(gachaType==GachaType.Advertise)
            {
                switch (GaChaItemType)
                {
                    case ItemType.weapon:
                        currentAdLeftTime = Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponAdLeftTime;
                        break;
                    case ItemType.wing:
                        currentAdLeftTime = Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingAdLeftTime;
                        break;
                    case ItemType.pet:
                        currentAdLeftTime = Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetAdLeftTime;
                        break;
                    case ItemType.s_relic:
                        currentAdLeftTime = Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicAdLeftTime;
                        break;
                    default:
                        break;
                }
                if (currentAdLeftTime <= 0)
                {
                    CanSeeAd = true;
                    return;
                }
                else
                {
                    CanSeeAd = false;
                }
                int m = (int)(currentAdLeftTime / 60.0f);
                int s = (int)(currentAdLeftTime % 60.0f);

                AdLeftTime.text = string.Format("{0:D2}:{1:D2}", m, s);

                SeeAd.SetActive(currentAdLeftTime <= 0);
                AdLeftTime.gameObject.SetActive(currentAdLeftTime > 0);
            }
            
        }

        bool SawAd = false;
        public void PushButton(int rewardc,bool isEvent=false)
        {
            //박스의 경우 재화를 아이디를 통해서 할지 타입으로 해서 추가하거나 업뎃할지 정해서 코드 수정 할것.
            if(gachaType==GachaType.Advertise)
            {
                if(CanSeeAd)
                {
                    SawAd = false;
                    StartCoroutine(AddItemAfterAdvertise(rewardc, isEvent));
                    Common.InGameManager.Instance.admob.ShowRewardAd(() => { SawAd = true; });
                }
                return;
            }
            else
            {
                if (rewardc < 0)
                    Currentrewardcount = 33;
                else
                    Currentrewardcount = RewardCount;

                Gacha _gachainfo =
              InGameDataTableManager.GachaTableList.gacha.Find(o => o.category == GaChaItemType && o.price_type == gachaType.ToString() && o.reward_count == Currentrewardcount);
                Price_Value = _gachainfo.price_value;
                BigInteger value = InGameManager.Instance.GetPlayerData.GetCurrency(GetCurrencyType(gachaType)).value;
                if (value > Price_Value)
                {
                    InGameManager.Instance.GetPlayerData.AddCurrency(GetCurrencyType(gachaType),-Price_Value);
                    int _Boxid = InGameManager.Instance.GetPlayerData.rewardinfo.GetBoxIDfromGachaItemtype(GaChaItemType, myGachaInfo);
                    GetItemFromBoxIdAndResultEvent(GaChaItemType,_Boxid,isEvent);
                }
                if(gachaType==GachaType.Gem)
                {
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_COUNT, Currentrewardcount);
                }
                if(GaChaItemType==ItemType.weapon)
                {
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_WEAPON, Currentrewardcount);
                    InGameManager.Instance.Localdata.SaveweaponData();
                }
                if (GaChaItemType == ItemType.wing)
                {
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_WING, Currentrewardcount);
                    InGameManager.Instance.Localdata.SavewingData();
                }
                if (GaChaItemType == ItemType.s_relic)
                {
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_S_RELIC, Currentrewardcount);
                    InGameManager.Instance.Localdata.SavesrelicData();
                }
                if (GaChaItemType == ItemType.pet)
                {
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_PET, Currentrewardcount);
                    InGameManager.Instance.Localdata.SavepetData();
                }
            }
            //BackendManager.Instance.SaveDBData(DTConstraintsData.UserData, InGameManager.Instance.GetPlayerData.GlobalUser);
            InGameManager.Instance.Localdata.SaveData(InGameManager.Instance.GetPlayerData.saveData);
        }

        IEnumerator AddItemAfterAdvertise(int rewardc, bool isEvent = false)
        {
            while(SawAd==false)
            {
                yield return null;
            }

            AddItemCallback(rewardc, isEvent);
        }

        void AddItemCallback(int rewardc, bool isEvent = false)
        {
            switch (GaChaItemType)
            {
                case ItemType.weapon:
                    Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponAdLeftTime=DTConstraintsData.GachaAdCoolTime;
                    break;
                case ItemType.wing:
                    Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingAdLeftTime = DTConstraintsData.GachaAdCoolTime;
                    break;
                case ItemType.pet:
                    Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetAdLeftTime = DTConstraintsData.GachaAdCoolTime;
                    break;
                case ItemType.s_relic:
                    Common.InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicAdLeftTime = DTConstraintsData.GachaAdCoolTime;
                    break;
                default:
                    break;
            }
            if (rewardc < 0)
                    Currentrewardcount = 33;
                else
                    Currentrewardcount = RewardCount;
                int _Boxid = InGameManager.Instance.GetPlayerData.rewardinfo.GetBoxIDfromGachaItemtype(GaChaItemType, myGachaInfo);
                GetItemFromBoxIdAndResultEvent(GaChaItemType, _Boxid, isEvent);
                if (gachaType == GachaType.Gem)
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_COUNT, Currentrewardcount);
                if (GaChaItemType == ItemType.weapon)
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_WEAPON, Currentrewardcount);
                if (GaChaItemType == ItemType.wing)
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_WING, Currentrewardcount);
                if (GaChaItemType == ItemType.s_relic)
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_S_RELIC, Currentrewardcount);
                if (GaChaItemType == ItemType.pet)
                    InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.GACHA_PET, Currentrewardcount);
            InGameManager.Instance.Localdata.SaveData(InGameManager.Instance.GetPlayerData.saveData);
            InGameManager.Instance.Localdata.SaveData(InGameManager.Instance.GetPlayerData.saveData);
            //BackendManager.Instance.SaveDBData(DTConstraintsData.UserData, InGameManager.Instance.GetPlayerData.GlobalUser);
        }

        CurrencyType GetCurrencyType(GachaType _gachatype)
        {
            string _type = _gachatype.ToString();
            CurrencyType _itemtype = EnumExtention.ParseToEnum<CurrencyType>(_type);
            return _itemtype;
        }
        
        void GetItemFromBoxIdAndResultEvent(ItemType _itemtype,int boxId,bool isevent)
        {
            int _value = Currentrewardcount;

            //최종적으로 들어갈 아이템값
            //키값:아이템idx,밸류값:아이템amount
            Dictionary<int, int> Itemvalue = new Dictionary<int, int>();
            int EnforceJewelAmount = 0;
            int MagicstoneAmount=0;

            while (_value > 0)
            {
                Dictionary<Box_Group, int> boxgroupList = InGameManager.Instance.GetPlayerData.rewardinfo.FindboxinfoToBoxRewardIdx(boxId);
                Dictionary<int, int> ItemIdxList = new Dictionary<int, int>();

                foreach (var data in boxgroupList)
                {
                    int itemIdx = InGameManager.Instance.GetPlayerData.rewardinfo.FindItemIdx(data.Key.reward_idx);
                    if (data.Key.reward_type == ItemType.enchantstone || data.Key.reward_type == ItemType.magicstone)
                    {
                        if (data.Key.reward_type == ItemType.enchantstone)
                            ItemIdxList.Add(-1, data.Value);
                        else
                            ItemIdxList.Add(-2, data.Value);
                    }
                    else
                    {
                        ItemIdxList.Add(itemIdx, data.Value);
                    }
                        
                }
                foreach (KeyValuePair<int, int> data in ItemIdxList)
                {
                    if (data.Key <0)//키값 0이면 id가 0이라 강화석 또는 마법석
                    {
                        if(data.Key==-1)
                        {
                            EnforceJewelAmount += data.Value;
                        }
                        else if(data.Key==-2)
                        {
                            MagicstoneAmount += data.Value;
                        }
                    }
                    else
                    {
                        if (Itemvalue.ContainsKey(data.Key))
                        {
                            Itemvalue[data.Key] += data.Value;
                        }
                        else
                        {
                            Itemvalue.Add(data.Key, data.Value);
                        }
                    }
                }
                _value--;
            }
            //뽑기 레벨 경험치
            InGameManager.Instance.GetPlayerData.rewardinfo.GetExpInGachalv(GaChaItemType, Currentrewardcount);
            //뽑기 레벨 경험치

            gachaActive.Clear();
            gachaActive.Itemvalue = Itemvalue;
            gachaActive.EnforceJewelAmount = EnforceJewelAmount;
            gachaActive.magicstoneamount = MagicstoneAmount;
            gachaActive._itemtype = _itemtype;
            gachaActive.IsEvent = isevent;
            gachaActive._buttonType = this;

            Message.Send<GachaActivate>(gachaActive);
        }
    }
}
