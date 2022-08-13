using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using BlackTree.Common;

namespace BlackTree
{
    public class RewardInfo
    {
        #region 박스 To 아이템 찾기
        /// <summary>
        /// 박스 열면 box_group데이터에서 찾아서 어떤 box_group데이터 주는지 알려줌
        /// 키값:reward_idx,밸류값: amount
        /// </summary>
        public Dictionary<Box_Group,int> FindboxinfoToBoxRewardIdx(int boxIdx)
        {
            Dictionary<Box_Group, int> BoxrewardList=new Dictionary<Box_Group, int>();

            BoxData _boxdata =InGameDataTableManager.BoxTableList.box.Find(o => o.idx == boxIdx);
            List<Box_Group> inBoxData = new List<Box_Group>();
            //박스 하나에 강화석이랑 같이 있어서 박스 구성물을 리스트로 뽑아냄
            inBoxData=InGameDataTableManager.BoxTableList.box_group.FindAll(o => o.idx == _boxdata.box_group_idx);

            //내용물의 id랑 갯수 정해서 넣어줌
            for(int i=0; i<inBoxData.Count; i++)
            {
                int randomcount = UnityEngine.Random.Range(inBoxData[i].count_min, inBoxData[i].count_max + 1);
                BoxrewardList.Add(inBoxData[i], randomcount);
            }

            return BoxrewardList;
        }

        //박스 열면 나오는 무기 idx 랜덤으로 1개 뱉어냄
        /// <summary>
        /// box_group의 reward_idx값을 아이템 타입에 따라 뒤져서 아이템을 가져옴
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="Boxreward_idx"></param>
        /// <returns></returns>
        public int FindItemIdx(int Boxreward_idx)
        {
            List<Reward_Item> rewardItemList = new List<Reward_Item>();

            ItemType _type = InGameDataTableManager.BoxTableList.box_group.Find(o => o.reward_idx == Boxreward_idx).reward_type;
            switch (_type)
            {
                case ItemType.weapon:
                case ItemType.wing:
                    rewardItemList = InGameDataTableManager.BoxTableList.reward_weapon.FindAll(o => o.idx == Boxreward_idx);
                    break;
                case ItemType.pet:
                    rewardItemList = InGameDataTableManager.BoxTableList.reward_pet.FindAll(o => o.idx == Boxreward_idx);
                    break;
                case ItemType.s_relic:
                    rewardItemList = InGameDataTableManager.BoxTableList.reward_s_relic.FindAll(o => o.idx == Boxreward_idx);
                    break;
                default:
                    break;
            }
           

            if(rewardItemList.Count<=0)
            {
                if(_type==ItemType.enchantstone)
                {
                    return -1;
                }
                else if(_type==ItemType.magicstone)
                {
                    return -2;
                }
                return 0;
            }
            else
            {
                int rate = 0;
                for(int i=0; i<rewardItemList.Count; i++)
                {
                    rate += rewardItemList[i].rarity;
                }
                //확률 정해서 돌려야함
                int lotto = UnityEngine.Random.Range(0, rate);
                int index = 0;
                rate = 0;
                for (int i = 0; i < rewardItemList.Count; i++)
                {
                    rate += rewardItemList[i].rarity;
                    if(lotto<=rate)
                    {
                        index = i;
                        break;
                    }
                }
                return rewardItemList[index].item_idx;
            }
        }
        #endregion

        #region 가챠
        /// <summary>
        /// 가챠 통해서 얻은 가차정보와 아이템타입을 토대로 boxID 리턴합니다.
        /// </summary>
        /// <param name="_itemtype"></param>
        /// <param name="cachainfo"></param>
        /// <returns></returns>
        public int GetBoxIDfromGachaItemtype(ItemType _itemtype,Gacha cachainfo)
        {
            int boxId = -1;
            int Level = 0;
            switch (_itemtype)
            {
                case ItemType.Skill:
                    break;
                case ItemType.Relic:
                    break;
                case ItemType.weapon:
                    Level = InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponLevel;
                    break;
                case ItemType.wing:
                    Level = InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingLevel;
                    break;
                case ItemType.Costum:
                    break;
                case ItemType.Quest:
                    break;
                case ItemType.pet:
                    Level = InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetLevel;
                    break;
                case ItemType.s_relic:
                    Level = InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicLevel;
                    break;
                default:
                    break;
            }
            GachaRewardBoxInfo boxinfo;
            boxinfo = InGameDataTableManager.GachaTableList.reward_box.Find(o => o.grade == Level);
            switch (cachainfo.reward_box_id)
            {
                case "reward_box_1":
                    boxId = boxinfo.reward_box_1;
                    break;
                case "reward_box_2":
                    boxId = boxinfo.reward_box_2;
                    break;
                case "reward_box_3":
                    boxId = boxinfo.reward_box_3;
                    break;
                case "reward_box_4":
                    boxId = boxinfo.reward_box_4;
                    break;
                default:
                    break;
            }

            return boxId;
        }

        //아래는 가차 레벨포인트 획득
        public void GetExpInGachalv(ItemType GaChaItemType, int rewardCount)
        {
            int index = 0;
            GachaRewardBoxInfo info = null;
            switch (GaChaItemType)
            {
                case ItemType.weapon:
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponExp += rewardCount;
                    index = 0;
                    for (int i = 0; i < InGameDataTableManager.GachaTableList.reward_box.Count; i++)
                    {
                        if (InGameDataTableManager.GachaTableList.reward_box[i].need_point < InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponExp)
                            index = i;
                    }
                    info = InGameDataTableManager.GachaTableList.reward_box[index];
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponLevel = info.grade;
                    break;
                case ItemType.wing:
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingExp += rewardCount;
                    index = 0;
                    for (int i = 0; i < InGameDataTableManager.GachaTableList.reward_box.Count; i++)
                    {
                        if (InGameDataTableManager.GachaTableList.reward_box[i].need_point < InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingExp)
                            index = i;
                    }
                    info = InGameDataTableManager.GachaTableList.reward_box[index];
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingLevel = info.grade;
                    break;
                case ItemType.pet:
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetExp += rewardCount;
                    index = 0;
                    for (int i = 0; i < InGameDataTableManager.GachaTableList.reward_box.Count; i++)
                    {
                        if (InGameDataTableManager.GachaTableList.reward_box[i].need_point < InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetExp)
                            index = i;
                    }
                    info = InGameDataTableManager.GachaTableList.reward_box[index];
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetLevel = info.grade;
                    break;
                case ItemType.s_relic:
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicExp += rewardCount;
                    index = 0;
                    for (int i = 0; i < InGameDataTableManager.GachaTableList.reward_box.Count; i++)
                    {
                        if (InGameDataTableManager.GachaTableList.reward_box[i].need_point < InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicExp)
                            index = i;
                    }
                    info = InGameDataTableManager.GachaTableList.reward_box[index];
                    InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicLevel = info.grade;
                    break;
                default:
                    break;
            }
        }

        #endregion

        public CurrencyType RewardtypeToCurrencyType(RewardType _type,int boxid=0)
        {
            CurrencyType currency = CurrencyType.Gem;
            switch (_type)
            {
                case RewardType.REWARD_GEM:
                    currency = CurrencyType.Gem;
                    break;
                case RewardType.REWARD_ESSENCE:
                    currency = CurrencyType.SuperMagicPotion;
                    break;
                case RewardType.REWARD_POTION:
                    currency = CurrencyType.MagicPotion;
                    break;
                case RewardType.REWARD_SOUL:
                    currency = CurrencyType.Soul;
                    break;
                case RewardType.REWARD_ENCHANT_STONE:
                    currency = CurrencyType.EnchantStone;
                    break;
                case RewardType.REWARD_MAGIC_STONE:
                    currency = CurrencyType.MagicStone;
                    break;
                case RewardType.REWARD_DG_TICKET:
                    currency = CurrencyType.Ticket_Dungeon;
                    break;
                case RewardType.REWARD_PVP_TICKET:
                    currency = CurrencyType.Ticket_PVPDungeon;
                    break;
                case RewardType.REWARD_PET_TICKET:
                    currency = CurrencyType.Ticket_PetDungeon;
                    break;
                case RewardType.REWARD_MILEAGE:
                    currency = CurrencyType.Mileage;
                    break;
                case RewardType.REWARD_BOX:
                    currency = Common.InGameManager.Instance.GetPlayerData.Playercurrency.GetIdxToType(boxid);
                    break;
                case RewardType.REWARD_GACHA_TICKET:
                    currency = CurrencyType.Ticket_Gacha;
                    break;
                case RewardType.REWARD_GOLD:
                    currency = CurrencyType.Gold;
                    break;
                case RewardType.REWARD_ROULETTE_TICKET:
                    currency = CurrencyType.RouletteCoupon;
                    break;
                default:
                    break;
            }
            return currency;
        }
    }
}

public enum RewardType
{
    REWARD_GEM,//보석
    REWARD_ESSENCE,//슈퍼건전지
    REWARD_POTION, //건전지
    REWARD_SOUL,//우유
    REWARD_ENCHANT_STONE,//강화석
    REWARD_MAGIC_STONE,//암흑구슬
    REWARD_DG_TICKET,//던전입장 티켓
    REWARD_PVP_TICKET,//pvp던전 입장티켓
    REWARD_PET_TICKET,//펫던전 입장티켓
    REWARD_MILEAGE,//마일리지
    REWARD_BOX,//박스
    REWARD_GACHA_TICKET,//뽑기 티켓
    REWARD_GOLD,//골드
    REWARD_ROULETTE_TICKET,//룰렛티켓
    REWARD_GACHA_PET,//펫 뽑기티켓
    REWARD_GACHA_S_RELIC,//특수유물 뽑기 티켓

    End,

    REWARD_BUFF_ATK,//버프(atk)15분
    REWARD_BUFF_GOLD,//버프(골드획득)15분
    REWARD_BUFF_SPEED,//버프(이속,공속)15분
    REWARD_BUFF_POTION,//버프(물약)15분
    REWARD_BUFF_AUTOTOUCH,//버프(자동터치)15분
    REWARD_AD_REMOVE,//광고 제거

    REWARD_BUFF_ATK_UNLIMITED,//버프(atk)영구
    REWARD_BUFF_SPEED_UNLIMITED,//버프(이속공속)영구
    REWARD_BUFF_GOLD_UNLIMITED,//버프(골드획득)영구
    REWARD_BUFF_POTION_UNLIMITED,//버프(물약)영구
    REWARD_BUFF_AUTOTOUCH_UNLIMITED,//버프(자동터치)영구

    None,
}

[Serializable]public struct Reward
{
    public RewardType Type;
    public int Amount;
}
