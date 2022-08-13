using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class RankingInterface : MonoBehaviour
    {
        List<UserInfoForPVP> UserPvpInfoList = new List<UserInfoForPVP>();
        public RankingUISlot rankslotprefab;
        [Header("pvp랭킹")]
        public Transform PVPRankingScrollparent;
        public Transform MyPVPRankingparent;
        [Header("스테이지랭킹")]
        public Transform STAGERankingScrollparent;
        public Transform MySTAGERankingparent;

        [SerializeField] Button StageRankingButton;
        [SerializeField] Button PVPRankingButton;

        [SerializeField] GameObject StageRankingButtonSelected;
        [SerializeField] GameObject PVPRankingButtonSelected;

        [SerializeField] GameObject StageRankingWindow;
        [SerializeField] GameObject PVPRankingWindow;

        [SerializeField] Text NotRankedTextInPvp;
        [SerializeField] Text NotRankedTextInStage;

        bool ismyRankExist;
        public void Init()
        {
            RankingInterfaceInit();
        }
        public void Release()
        {
            //BackendManager.Instance.RegisterRank(null);
        }
        void RankingInterfaceInit()
        {
            #region pvp랭킹 세팅
            ////pvp유저 세팅
            //UserPvpInfoList = BackendManager.Instance.GetRankList(DTConstraintsData.PVPRTRankTableUUID);
            //if (UserPvpInfoList == null)
            //    return;
            //List<UserInfoForPVP> sortedlist = UserPvpInfoList.OrderBy(o => o.RankScore).ToList();
            //for (int i = 0; i < sortedlist.Count; i++)
            //{
            //    var obj = Instantiate(rankslotprefab);
            //    obj.transform.SetParent(PVPRankingScrollparent, false);
            //    obj.Init(sortedlist[i], true);
            //}
            ////pvp 내꺼 세팅
            //if (BackendManager.Instance.IsMyRankExist(DTConstraintsData.PVPRTRankTableUUID))
            //{
            //    NotRankedTextInPvp.gameObject.SetActive(false);
            //    UserInfoForPVP my = Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp;
            //    if (my != null)
            //    {
            //        var _obj = Instantiate(rankslotprefab);
            //        _obj.transform.SetParent(MyPVPRankingparent, false);
            //        _obj.Init(my, true);
            //    }
            //}
            //else
            //{
            //    NotRankedTextInPvp.gameObject.SetActive(true);
            //    NotRankedTextInPvp.text = string.Format("랭킹정보에 등록되지 않았습니다. 게임을 더 플레이 하시면 랭킹정보가 등록됩니다.");
            //}
            #endregion

            //스테이지랭킹유저 세팅
            ismyRankExist = false;
            
            List<UserInfoForPVP> _sortedlist = UserPvpInfoList.OrderByDescending(o => o.RankScore).ToList();
            for (int i = 0; i < _sortedlist.Count; i++)
            {
                var obj = Instantiate(rankslotprefab);
                obj.transform.SetParent(STAGERankingScrollparent, false);
                obj.Init(_sortedlist[i],i, false);
                //if(_sortedlist[i].NickName== BackendManager.Instance.GetNickName())
                //{
                //    ismyRankExist = true;
                //    var __obj = Instantiate(rankslotprefab);
                //    __obj.transform.SetParent(MySTAGERankingparent, false);
                //    __obj.Init(_sortedlist[i],i, false);
                //}
            }
            ////스테이지랭킹 내꺼 세팅
            //if (BackendManager.Instance.IsMyRankExist(DTConstraintsData.STAGERTRankTableUUID))
            //{
            //    NotRankedTextInStage.gameObject.SetActive(false);
            //    UserInfoForPVP _my = BackendManager.Instance.GetMyRTRankInfo(DTConstraintsData.STAGERTRankTableUUID);
            //    Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp = _my;
            //    var __obj = Instantiate(rankslotprefab);
            //    __obj.transform.SetParent(MySTAGERankingparent, false);
            //    __obj.Init(_my, _my.RankingNumber, false);
            //}
            //else
            {
                NotRankedTextInStage.gameObject.SetActive(true);
                NotRankedTextInStage.text = string.Format("랭킹정보에 등록되지 않았습니다. 게임을 더 플레이 하시면 랭킹정보가 등록됩니다.");
            }

            StageRankingButton.onClick.AddListener(() =>
            {
                StageRankingWindow.SetActive(true);
                PVPRankingWindow.SetActive(false);
                StageRankingButtonSelected.SetActive(true);
                PVPRankingButtonSelected.SetActive(false);
            });

            //pvp랭킹은 나중 구현
            //PVPRankingButton.onClick.AddListener(() =>
            //{
            //    StageRankingWindow.SetActive(false);
            //    PVPRankingWindow.SetActive(true);
            //    StageRankingButtonSelected.SetActive(false);
            //    PVPRankingButtonSelected.SetActive(true);
            //});

            StageRankingWindow.SetActive(true);
            PVPRankingWindow.SetActive(false);
            StageRankingButtonSelected.SetActive(true);
            PVPRankingButtonSelected.SetActive(false);
        }
    }

}
