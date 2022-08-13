using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using DLL_Common.Common;
using BlackTree.Common;
using DG.Tweening;

namespace BlackTree
{
    public class FairyController : MonoBehaviour
    {
        [Header("캐릭터 정보")]
        [SerializeField] public SkeletonAnimation _Skeletonanimator;

        [SerializeField] Transform departure;
        [SerializeField] Transform arrive;

        bool HasPresent=false;
        int maxRarity = 0;

        UI.Event.FlashPopup flashpopup;
        private void Start()
        {
            HasPresent = false;
            _Skeletonanimator.state.Complete += OnSpineAnimationEndInAttack;
            _Skeletonanimator.state.SetAnimation(0, "run", true);

            maxRarity = 0;
            for (int i=0; i< InGameDataTableManager.fairyTableList.fairy.Count; i++)
            {
                maxRarity += InGameDataTableManager.fairyTableList.fairy[i].rarity;
            }

            flashpopup = new UI.Event.FlashPopup(null);
            Message.AddListener<InGame.Event.FairyPresentActive>(ActivatePresent);

            _Skeletonanimator.transform.localPosition = departure.localPosition;
        }

        private void Update()
        {
            if(HasPresent==false)
                return;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
                if (hitInfo)
                {
                    Debug.Log(hitInfo.collider.transform.gameObject.name);
                    if(hitInfo.collider.transform.gameObject== this.gameObject)
                    {
                        PushPresent();
                    }
                }
            }
#endif
            for (var i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                    if (hitInfo)
                    {
                        if (hitInfo.collider.transform.gameObject == this.gameObject)
                        {
                            PushPresent();
                        }
                    }
                }
            }
        }
        private void OnDestroy()
        {
            Message.RemoveListener<InGame.Event.FairyPresentActive>(ActivatePresent);
        }

        //선물 클릭
        void PushPresent()
        {
            HasPresent = false;

            int randomrate = Random.Range(0, maxRarity);
            int raritydecider = 0;
            int index = 0;
            for(int i=0; i< InGameDataTableManager.fairyTableList.fairy.Count; i++)
            {
                raritydecider += InGameDataTableManager.fairyTableList.fairy[i].rarity;
                if(randomrate<=raritydecider)
                {
                    index = i;
                    break;
                }
            }

            FairyInfo fi = InGameDataTableManager.fairyTableList.fairy[index];

            CurrencyType _currencytype= InGameManager.Instance.GetPlayerData.rewardinfo.RewardtypeToCurrencyType(fi.reward_type, fi.reward_idx);
            string RewardCountvalue=null;
            if(_currencytype==CurrencyType.Gold)
            {
                BigInteger gold = InGameManager.Instance.GetPlayerData.GlobalUser.RewardGold;
                BigInteger _plusgold = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetMonsterKillGoldReward(gold);
                InGameManager.Instance.GetPlayerData.AddCurrency(_currencytype, fi.reward_count * _plusgold);
                RewardCountvalue = (fi.reward_count * _plusgold).ToDisplay();
            }
            else if (_currencytype == CurrencyType.MagicPotion)
            {
                int stage = InGameManager.Instance.GetPlayerData.stage_Info.Stage;
                int basemagic = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageRewarddata.base_stage_magic_pottion;
                int gainrate = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageRewarddata.stage_gain_magic_potion;

                BigInteger reward = basemagic + stage * gainrate;
                BigInteger pluspotion= CharacterDataManager.Instance.PlayerCharacterdata.ability.GetMonsterKillPotionReward(reward);
                InGameManager.Instance.GetPlayerData.AddCurrency(_currencytype, fi.reward_count * pluspotion);
                RewardCountvalue = (fi.reward_count * pluspotion).ToDisplay();
            }
            else if (_currencytype == CurrencyType.Soul)
            {
                DungeonPrime dungeonData=null;
                int beststage = InGameManager.Instance.GetPlayerData.stage_Info.CowBestLevel;
                dungeonData = InGameDataTableManager.DungeonTableList.Dungeon.Find(o => o.dg_stage == beststage+1);

                InGameManager.Instance.GetPlayerData.AddCurrency(_currencytype, fi.reward_count * dungeonData.reward_soul);
                RewardCountvalue = (fi.reward_count * dungeonData.reward_soul).ToString();
            }
            else if (_currencytype == CurrencyType.MagicStone)
            {
                DungeonPrime dungeonData = null;
                int beststage = InGameManager.Instance.GetPlayerData.stage_Info.CowBestLevel;
                dungeonData = InGameDataTableManager.DungeonTableList.Dungeon.Find(o => o.dg_stage == beststage+1);

                InGameManager.Instance.GetPlayerData.AddCurrency(_currencytype, fi.reward_count * dungeonData.reward_magic_stone);
                RewardCountvalue = (fi.reward_count * dungeonData.reward_magic_stone).ToString();
            }
            else
            {
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(_currencytype, fi.reward_count);
                RewardCountvalue = fi.reward_count.ToString();
            }
            

            LocalValue lv = InGameDataTableManager.LocalizationList.fairy.Find(o => o.id == fi.message_id);
            string local = lv.GetStringForLocal(true);
            string msgvalue = null;
            if (fi.reward_type==RewardType.REWARD_BOX)
            {
                msgvalue = string.Format(local);
            }
            else
            {
                msgvalue = string.Format(local, RewardCountvalue);
            }

            flashpopup.Eventmsg = msgvalue;
            Message.Send<UI.Event.FlashPopup>(flashpopup);

            _Skeletonanimator.state.SetAnimation(0, "action_out", false);

            _Skeletonanimator.transform.DOLocalMove(arrive.localPosition, 2.0f);
        }

        

        //선물 들기
        void ActivatePresent(InGame.Event.FairyPresentActive msg)
        {
            if (HasPresent == true)
                return;

            HasPresent = true;
            _Skeletonanimator.state.SetAnimation(0, "action_in", false);

            _Skeletonanimator.transform.localPosition = departure.localPosition;
            _Skeletonanimator.transform.DOLocalMove(Vector3.zero,2.0f);
        }

        private void OnSpineAnimationEndInAttack(TrackEntry trackentry)
        {
            if (trackentry.Animation.Name == "action_in")
            {
                _Skeletonanimator.state.SetAnimation(0, "action_idle", true);
            }
            if (trackentry.Animation.Name == "action_out")
            {
                _Skeletonanimator.state.SetAnimation(0, "run", true);
            }
        }
    }

}
