using DLL_Common.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public class BattlePassDay
    {
        public bool IsFreeTaken { get; set; }
        public bool IsPaidTaken { get; set; }

        [System.NonSerialized] public Battlepasstable tableData;
    }

    [System.Serializable]
    public class IAPInfo
    {
        public int ShopItemIdx;
        public int BoughtCount;//구매 횟수
        public bool IsBought=false;
    }

    [System.Serializable]
    public class PaymentInfo
    {
        public bool BattlePassPayed { get; set; }
        public List<BattlePassDay> UserLevelBattlePassinfo = new List<BattlePassDay>();
        public List<BattlePassDay> FairyCountBattlePassinfo = new List<BattlePassDay>();
        public List<BattlePassDay> PlayingTimeBattlePassinfo = new List<BattlePassDay>();

        [System.NonSerialized]public List<IAPInfo> IAPInfo=new List<IAPInfo>();
        public void Init()
        {
            for(int i=0; i<InGameDataTableManager.shopTableList.battlepass.Count; i++)
            {
                BattlePassDay bp = new BattlePassDay();
                bp.tableData = InGameDataTableManager.shopTableList.battlepass[i];
                bp.IsFreeTaken = false;
                bp.IsPaidTaken = false;
                UserLevelBattlePassinfo.Add(bp);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.battlepass_fairy.Count; i++)
            {
                BattlePassDay bp = new BattlePassDay();
                bp.tableData = InGameDataTableManager.shopTableList.battlepass_fairy[i];
                bp.IsFreeTaken = false;
                bp.IsPaidTaken = false;
                FairyCountBattlePassinfo.Add(bp);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.battlepass_time.Count; i++)
            {
                BattlePassDay bp = new BattlePassDay();
                bp.tableData = InGameDataTableManager.shopTableList.battlepass_time[i];
                bp.IsFreeTaken = false;
                bp.IsPaidTaken = false;
                PlayingTimeBattlePassinfo.Add(bp);
            }

            for (int i=0; i<InGameDataTableManager.shopTableList.goods.Count; i++)
            {
                IAPInfo data = new IAPInfo();
                data.ShopItemIdx = InGameDataTableManager.shopTableList.goods[i].idx;
                data.BoughtCount = 0;
                data.IsBought = false;
                IAPInfo.Add(data);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.gem.Count; i++)
            {
                IAPInfo data = new IAPInfo();
                data.ShopItemIdx = InGameDataTableManager.shopTableList.gem[i].idx;
                data.BoughtCount = 0;
                data.IsBought = false;
                IAPInfo.Add(data);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.pvp.Count; i++)
            {
                IAPInfo data = new IAPInfo();
                data.ShopItemIdx = InGameDataTableManager.shopTableList.pvp[i].idx;
                data.BoughtCount = 0;
                data.IsBought = false;
                IAPInfo.Add(data);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.mileage.Count; i++)
            {
                IAPInfo data = new IAPInfo();
                data.ShopItemIdx = InGameDataTableManager.shopTableList.mileage[i].idx;
                data.BoughtCount = 0;
                data.IsBought = false;
                IAPInfo.Add(data);
            }
            for (int i = 0; i < InGameDataTableManager.shopTableList.limited.Count; i++)
            {
                IAPInfo data = new IAPInfo();
                data.ShopItemIdx = InGameDataTableManager.shopTableList.limited[i].idx;
                data.BoughtCount = 0;
                IAPInfo.Add(data);
            }
        }
    }

}
