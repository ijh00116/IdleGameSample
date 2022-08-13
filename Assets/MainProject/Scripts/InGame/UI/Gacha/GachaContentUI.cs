using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class GachaContentUI : MonoBehaviour
    {
        [SerializeField] ItemType GachaType;
        [SerializeField] List<GachaButtonType> ButtonList = new List<GachaButtonType>();

        [SerializeField] Slider GachaLvslider;
        [SerializeField] Text GachaLvText;

        [SerializeField] Text TicketCount;

        public void Init()
        {
            //뽑기 경치 레벨 등 초기화
            InGameManager.Instance.GetPlayerData.rewardinfo.GetExpInGachalv(GachaType, 0);

            for (int i=0; i< ButtonList.Count; i++)
            {
                ButtonList[i].Init();
            }

            Message.AddListener<UI.Event.GachaActivate>(GachaActivate);
            GachaActivate(null);

            switch (GachaType)
            {
                case ItemType.weapon:
                    TicketCount.text = InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_Gacha).value.ToDisplay();
                    break;
                case ItemType.wing:
                    TicketCount.text = InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_Gacha).value.ToDisplay();
                    break;
                case ItemType.pet:
                    TicketCount.text = InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_pet).value.ToDisplay();
                    break;
                case ItemType.s_relic:
                    TicketCount.text = InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_SRelic).value.ToDisplay();
                    break;
                default:
                    break;
            }
        }

        public void Release()
        {
            for (int i = 0; i < ButtonList.Count; i++)
            {
                ButtonList[i].Release();
            }
            Message.RemoveListener<UI.Event.GachaActivate>(GachaActivate);
        }

        void GachaActivate(UI.Event.GachaActivate msg)
        {
            int freeexp = 0;
            int freelv = 0;
            int exp = 0;
            int lv = 0;
            int freerewardIdx = 0;
            int _boxidx = 0;
            switch (GachaType)
            {
                case ItemType.weapon:
                    exp = InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponExp;
                    lv = InGameManager.Instance.GetPlayerData.GlobalUser.GachaWeaponLevel;
                    _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].weapon_box;
                    break;
                case ItemType.wing:
                    exp = InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingExp;
                    lv = InGameManager.Instance.GetPlayerData.GlobalUser.GachaWingLevel;
                    _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].wing_box;
                    break;
                case ItemType.pet:
                    exp = InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetExp;
                    lv = InGameManager.Instance.GetPlayerData.GlobalUser.GachaPetLevel;
                    _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].pet_box;
                    break;
                case ItemType.s_relic:
                    exp = InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicExp;
                    lv = InGameManager.Instance.GetPlayerData.GlobalUser.GachaSrelicLevel;
                    _boxidx = InGameDataTableManager.GachaTableList.point_reward[freerewardIdx].s_relic_box;
                    break;
                default:
                    break;
            }
            if(lv>1)
            {
                int startexp= InGameDataTableManager.GachaTableList.reward_box.Find(o => o.grade == (lv)).need_point;
                int endexp = InGameDataTableManager.GachaTableList.reward_box.Find(o => o.grade == (lv+1)).need_point;
                GachaLvslider.value = ((float)(exp - startexp) / (float)(endexp - startexp));
            }
            else
            {
                int startexp = 0;
                int endexp = InGameDataTableManager.GachaTableList.reward_box.Find(o => o.grade == (lv + 1)).need_point;
                GachaLvslider.value = ((float)(exp - startexp) / (float)(endexp - startexp));
            }

            if(freelv>1)
            {
                int endexp = InGameDataTableManager.GachaTableList.point_reward[freelv-1].point;
            }
            else
            {
                int endexp = InGameDataTableManager.GachaTableList.point_reward[0].point;
            }

            GachaLvText.text = string.Format("가챠레벨:{0}", lv);
        }
    }

}
