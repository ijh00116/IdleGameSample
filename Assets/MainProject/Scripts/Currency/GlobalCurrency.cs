using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlackTree
{
    public enum CurrencyType
    {
        //소모형 아이템 타입
        Gold=0,
        MagicPotion,//건전지
        SuperMagicPotion,//슈퍼건전지
        Gem,//보석

        EnchantStone,//강화석
        Soul,           //영혼(우유)
        MagicStone,     //암흑ㄱ구슬
        Mileage,

        //던전티켓
        Ticket_Dungeon,
        Ticket_PetDungeon,
        Ticket_PVPDungeon,

        //티켓종류
        RouletteCoupon,
   
        Ticket_pet,
        Ticket_SRelic,
        Ticket_Gacha,
        NotCurrency,

        //박스형 아이템 타입
        /// <summary>
        /// 박스 시작점
        /// </summary>
        WeaponBox_D,
        WeaponBox_C,
        WeaponBox_B,
        WeaponBox_A,
        WeaponBox_S,
        WeaponBox_SS,

        WingBox_D,
        WingBox_C,
        WingBox_B,
        WingBox_A,
        WingBox_S,
        WingBox_SS,

        EnforceJewelBox_small,
        EnforceJewelBox_mid,
        EnforceJewelBox_Big,

        EnforceJewelBox_JackPot,

        MagicStoneBox_small,
        MagicStoneBox_mid,
        MagicStoneBox_Big,
        MagicStoneBox_JackPot,

        AllClassWeaponBox_1,
        AllClassWeaponBox_2 ,
        AllClassWeaponBox_3 ,
        AllClassWeaponBox_4 ,
        AllClassWeaponBox_5 ,
        AllClassWeaponBox_6 ,
        AllClassWeaponBox_7 ,
        AllClassWeaponBox_8 ,
        AllClassWeaponBox_9 ,
        AllClassWeaponBox_10,
        AllClassWeaponBox_11,
        AllClassWingBox_1   ,
        AllClassWingBox_2   ,
        AllClassWingBox_3   ,
        AllClassWingBox_4   ,
        AllClassWingBox_5   ,
        AllClassWingBox_6   ,
        AllClassWingBox_7   ,
        AllClassWingBox_8   ,
        AllClassWingBox_9   ,
        AllClassWingBox_10  ,
        AllClassWingBox_11  ,
        AllClassPetBox_1    ,
        AllClassPetBox_2    ,
        AllClassPetBox_3    ,
        AllClassPetBox_4    ,
        AllClassPetBox_5    ,
        AllClassPetBox_6    ,
        AllClassPetBox_7    ,
        AllClassPetBox_8    ,
        AllClassPetBox_9    ,
        AllClassPetBox_10   ,
        AllClassPetBox_11,
        AllClassSRelicBox,
        PVPWeaponBox,
        PVPWingBox,
        GoldWeaponBox,
        GoldWingBox,

    

        End
    }

    [System.Serializable]
    public class Currency
    {
        public CurrencyType currencyType;

        [System.NonSerialized]
        public BigInteger value=BigInteger.Zero;

        public int BoxIdx = -1;
        public string VALUE;
    }

    [System.Serializable]
    public class CurrencySaveInfo
    {
        public Dictionary<CurrencyType, string> currencylist = new Dictionary<CurrencyType, string>();
    }
    [System.Serializable]
    public class GlobalCurrency
    {
        public List<Currency> currencylist = new List<Currency>();
        public Currency GetCurrency(CurrencyType _CurrenyType)
        {
            Currency _currency = currencylist.Find(curruncy => curruncy.currencyType == _CurrenyType);
            int boxid = -1;
            if ((int)_CurrenyType >=(int)CurrencyType.WeaponBox_D)
                boxid = GetIdxFromType(_CurrenyType);
            if (_currency == null)
            {
                _currency = new Currency() { currencyType = _CurrenyType, value = BigInteger.Zero ,BoxIdx= boxid };
                currencylist.Add(_currency);
            }

            return _currency;
        }

        public void UpdateCurrency(CurrencyType _CurrenyType, BigInteger _value)
        {
            var updateCurreny = GetCurrency(_CurrenyType);

            int boxid = -1;
            if ((int)_CurrenyType >= (int)CurrencyType.WeaponBox_D)
                boxid = GetIdxFromType(_CurrenyType);
            if (null == updateCurreny)
            {
                if(boxid<0)
                    currencylist.Add(new Currency() { currencyType = _CurrenyType, value = _value});
                else
                    currencylist.Add(new Currency() { currencyType = _CurrenyType, value = _value,BoxIdx= boxid });
            }
            else
            {
                updateCurreny.value = _value;
            }
        }

        public CurrencyType GetIdxToType(int boxidx)
        {
            CurrencyType _type = CurrencyType.End;

            BoxData boxdata = InGameDataTableManager.BoxTableList.box.Find(o => o.idx == boxidx);
            if(boxdata==null)
            {
                _type = CurrencyType.Gem;
#if UNITY_EDITOR
                Debug.LogError("박스인덱스 해당 아이템 존재 안함!!");
#endif
            }
            else
            {
                _type = boxdata.boxType;
            }

            return _type;
        }

        public int GetIdxFromType(CurrencyType boxtype)
        {
            int _IDX = -1;
            BoxData boxdata = InGameDataTableManager.BoxTableList.box.Find(o => o.boxType == boxtype);
            if (boxdata == null)
            {
                _IDX = -1;
#if UNITY_EDITOR
                Debug.LogError("박스인덱스 해당 아이템 존재 안함!!");
#endif
            }
            else
            {
                _IDX = boxdata.idx;
            }


            return _IDX;
        }

    }

}
