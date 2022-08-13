using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DLL_Common.Common;
using System.Linq;

namespace BlackTree
{
    public class OfflineReward : MonoBehaviour
    {
        EnemyReward offlineRewardValue;
        public Button closeBtn;

        [Header("타임,골드,포션")]
        public Text TimeTerm;
        public Text Goldamount;
        public Text Potionamount;

        [Header("박스 스크롤")]
        public OfflineRewardSlot slotPrefab;
        public Transform parent;
        public void Init()
        {
            offlineRewardValue = new EnemyReward(InGame.CharacterType.NormalMonster);
            CheckOfflineReward();
            closeBtn.onClick.AddListener(() => { this.gameObject.SetActive(false); });
        }

        public void Release()
        {

        }

        #region 오프보상 체크/출석보상 체크
        void CheckOfflineReward()
        {
            float elapsedtime = 0;
            bool IsGetReward =Common.InGameManager.Instance.GetPlayerData.CheckForOfflineReward(out elapsedtime);
            if (IsGetReward)
            {
#if UNITY_EDITOR
                Debug.Log("오프보상 획득가능");
#endif
                this.gameObject.SetActive(true);
                //오프보상 로직 ==>1회 보상 로직
                int chapter = Common.InGameManager.Instance.GetPlayerData.stage_Info.chapter;

                List<OfflineRewardData> offlineRewarddata = new List<OfflineRewardData>();
                for (int i = 0; i < InGameDataTableManager.OfflineRewardTableList.reward.Count; i++)
                {
                    if (InGameDataTableManager.OfflineRewardTableList.reward[i].chapter == chapter)
                    {
                        offlineRewarddata.Add(InGameDataTableManager.OfflineRewardTableList.reward[i]);
                    }
                }

                List<OfflineRewardData> sortedrewarddata = offlineRewarddata.OrderBy(x => x.rarity).ToList();
                //횟수 지정
                int count = (int)(elapsedtime / 60.0f);
                if (count > 20)
                    count = 20;
                int h = (int)(elapsedtime / 3600.0f );
                int m = (int)((elapsedtime %3600) / 60.0f);
                int s = (int)((elapsedtime % 3600) % 60.0f);
                TimeTerm.text = string.Format("{0}시간{1}분{2}초", (h), (m), s);
                //골드 및 포션 정보
                BigInteger gold =offlineRewardValue.CalculateGold();
                BigInteger potion = offlineRewardValue.CalculateMagicpotion();

                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gold, gold* count);
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, potion*count);

                Goldamount.text = string.Format("{0}골드", (gold * count).ToDisplay());
                Potionamount.text = string.Format("{0}포션", (potion* count).ToDisplay());
                //골드 및 포션 정보

                //아이템 박스 정보
                Dictionary<CurrencyType, int> currencylist = new Dictionary<CurrencyType, int>();
                List<int> boxidList = new List<int>();
                while (count > 0)
                {
                    float rate = Random.Range(0, 1100);
                    float rarity = 0;

                    for (int i = 0; i < sortedrewarddata.Count; i++)
                    {
                        rarity += sortedrewarddata[i].rarity;
                        if (rate <= rarity)
                        {
                            boxidList.Add(sortedrewarddata[i].box_idx);
                            break;
                        }
                    }
                    count--;
                }
                for (int i = 0; i < boxidList.Count; i++)
                {
                    CurrencyType _type = Common.InGameManager.Instance.GetPlayerData.Playercurrency.GetIdxToType(boxidList[i]);
                    Common.InGameManager.Instance.GetPlayerData.AddCurrency(_type, 1);
                    if (currencylist.ContainsKey(_type))
                        currencylist[_type] += 1;
                    else
                        currencylist.Add(_type, 1);

                }
                foreach(var _data in currencylist)
                {
                    var obj = Instantiate(slotPrefab);
                    obj.transform.SetParent(parent, false);
                    obj.Setting(_data.Key, _data.Value);
                }
                //아이템 박스 정보
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("오프보상 획득 불가능");
#endif
            }
        }

        #endregion
    }
}

