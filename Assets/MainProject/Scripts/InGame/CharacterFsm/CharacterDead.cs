using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using System;
using Spine.Unity;
using DG.Tweening;

namespace BlackTree.InGame
{
    public class CharacterDead : CharacterAbility,IStateCallback
    {
        [SerializeField] eActorState Mystate;

        public EnemyReward RewardValue;

        public DieEffectControll DieparticlePrefab;
        public DieEffectControll dieAnimationObject;
        public Transform DieEffPos;
        public Action OnEnter => onEnter;
        public Action OnExit => onExit;
        public Action OnUpdate => onUpdate;

        InGame.Event.EnemyKilled enemyKilled;

        [SerializeField]public SpriteRenderer ShadowImage;

        [SerializeField] DropCurrency dropItemPrefab;
        List<DropCurrency> dropCurrencyList = new List<DropCurrency>();
        UI.Event.EarnMonsterGold earncurrency=new UI.Event.EarnMonsterGold();
        protected override void Start()
        {
            base.Start();
            _character._state.AddState(Mystate, this);

            enemyKilled = new Event.EnemyKilled(this.gameObject, _character.playertype);

            RewardValue = new EnemyReward(_character.playertype);

            dieAnimationObject = Instantiate(DieparticlePrefab);
            dieAnimationObject.transform.position = DieEffPos.position;
            //dieAnimationObject.transform.SetParent(this.transform);
            dieAnimationObject.gameObject.SetActive(false);

            if(dropItemPrefab!=null)
            {
                for (int i = 0; i < 8; i++)
                {
                    var obj = Instantiate(dropItemPrefab);
                    obj.transform.SetParent(this.transform, false);
                    obj.Init(i < 4);
                    dropCurrencyList.Add(obj);
                }
            }
            for (int i = 0; i < dropCurrencyList.Count; i++)
            {
                dropCurrencyList[i].gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < dropCurrencyList.Count; i++)
            {
                dropCurrencyList[i].gameObject.SetActive(false);
            }
        }
        protected override void OnDestroy()
        {

        }
        void onEnter()
        {
           if(_character.playertype!=CharacterType.PetEnemy)
                _character._Skeletonanimator.state.SetAnimation(0, "die", false);
            //보상 제공
            if (_character.playertype == CharacterType.NormalMonster || _character.playertype == CharacterType.Boss)
            {
                DLL_Common.Common.BigInteger gold = RewardValue.CalculateGold();
                DLL_Common.Common.BigInteger potion= RewardValue.CalculateMagicpotion();
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gold, gold);
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.MagicPotion, potion);
                Common.InGameManager.Instance.GetPlayerData.AddExp(RewardValue.CalculateExp());
                earncurrency.Type = CurrencyType.Gold;
                earncurrency.value = gold;
                Message.Send<UI.Event.EarnMonsterGold>(earncurrency);
                earncurrency.Type = CurrencyType.MagicPotion;
                earncurrency.value = potion;
                Message.Send<UI.Event.EarnMonsterGold>(earncurrency);
            }
            dieAnimationObject.gameObject.SetActive(true);
            //보상 제공
            dieAnimationObject.transform.position = DieEffPos.position;
            if (_character.playertype==CharacterType.PetEnemy)
                dieAnimationObject.AppearEffect("smoke");
            else
                dieAnimationObject.AppearEffect("idle");
            

            //각각의 타입별로 죽인 결과로 플레이어 데이터에 정보 제공
            //메세지로 보내서 인게임매니저에서 해결할것
            switch (_character.playertype)
            {
                case CharacterType.Player:
                    break;
                case CharacterType.PetEnemy:
                    Common.InGameManager.Instance.GetPlayerData.stage_Info.PetCurrentKillCount++;
                  
                    //펫 죽을때 얻는 장비 처리
                    GetPetRewardItem();
                    break;
                case CharacterType.NormalMonster:
                    //Common.InGameManager.Instance.GetPlayerData.CurrentWave += 1;
                    break;
                case CharacterType.Boss:
                    //Common.InGameManager.Instance.GetPlayerData.CurrentWave += 1;
                    break;
                case CharacterType.Mimic:
                    //Common.InGameManager.Instance.GetPlayerData.CurrentWave += 1;
                    break;
                case CharacterType.Cow:
                    Common.InGameManager.Instance.GetPlayerData.stage_Info.CowCurrentKillCount++;
                    break;
                case CharacterType.Cowking:
                    Common.InGameManager.Instance.GetPlayerData.stage_Info.KingCowCurrentKillCount++;
                    break;
                default:
                    break;
            }

            if(_character.playertype!=CharacterType.Player && _character.playertype != CharacterType.PvpPlayer)
            {
               InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.MONSTER_KILL, 1);
            }

            float randomFairy = UnityEngine.Random.Range(0, 1000);
            if(randomFairy<500)
            {
                InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.FAIRY_GETTING_COUNT, 1);
            }

            if(ShadowImage!=null)
                ShadowImage.DOColor(new Color(1, 1, 1, 0), 0.7f);
            for (int i = 0; i < dropCurrencyList.Count; i++)
            {
                dropCurrencyList[i].gameObject.SetActive(true);
            }
            for (int i=0; i< dropCurrencyList.Count; i++)
            {
                dropCurrencyList[i].Test();
            }

            //30마리마다 무기상자
            if(InGameManager.Instance.GetPlayerData.saveData.missionInfo._playingRecord.GetMissionValue(MissionType.MONSTER_KILL)
                %DTConstraintsData.BATTLE_STAGE_ITEM_MON_KILL == 0)
            {
                GetBoxForKillMonster();
            }

            //50마리마다 요정 선물 활성화
            if (InGameManager.Instance.GetPlayerData.saveData.missionInfo._playingRecord.GetMissionValue(MissionType.MONSTER_KILL)
                % DTConstraintsData.GIFT_FAIRY_GEN_COOLTIME == 0)
            {
                Message.Send<InGame.Event.FairyPresentActive>(new InGame.Event.FairyPresentActive());
            }

            Message.Send<InGame.Event.EnemyKilled>(enemyKilled);
        }

        void onExit()
        {

        }

        void onUpdate()
        {
           
        }

        void GetBoxForKillMonster()
        {
            int scenario = InGameManager.Instance.GetPlayerData.stage_Info.scenario;
            int chapter= InGameManager.Instance.GetPlayerData.stage_Info.chapter;

            int rewardidx = InGameDataTableManager.StageList.Reward.Find(o => o.scenario == scenario && o.chapter == chapter).reward_idx;
            var boxlist = InGameDataTableManager.StageList.box_reward.FindAll(o => o.idx == rewardidx);

            CurrencyType _type= InGameManager.Instance.GetPlayerData.Playercurrency.GetIdxToType(boxlist[0].box_idx);

            InGameManager.Instance.GetPlayerData.AddCurrency(_type, 1);
        }
        /// <summary>
        /// 아이템 얻는정보 여기서 출력
        /// 펫보상 정보는 펫던전컨트롤러에서 쌓아줌_펫 이동연출 해주기 위함
        /// </summary>
        void GetPetRewardItem()
        {
            PetDungeonPrime petinfo = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeondata;
            float rate = petinfo.reward_item_rate + (petinfo.reward_item_rate_stage* (InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage - 1));

            float random = UnityEngine.Random.Range(0, 100);
            //장비 획득 가능
            if(random <=rate)
            {
                //짝수 날개 획득
                if(InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage % 2==0)
                {
                    List<PetDungeonItemReward> rewardList = InGameManager.Instance.GetPlayerData.stage_Info.PetdungeonItemrewardList;
                    int randomindex = UnityEngine.Random.Range(0, rewardList.Count);

                    int idx = rewardList[randomindex].reward_idx;

                    InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeonRewardItemList.Add(idx);
                    InventorySlot slot = InGameManager.Instance.WingInventory.GetSlots.Find(o => o.item.idx == idx);
                }
                else//홀수 특수유물 획득
                {
                    List<PetDungeonItemReward> rewardList = InGameManager.Instance.GetPlayerData.stage_Info.PetdungeonItemrewardList;
                    int randomindex = UnityEngine.Random.Range(0, rewardList.Count);

                    int idx = rewardList[randomindex].reward_idx;

                    InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeonRewardItemList.Add(idx);
                    SRelicInventorySlot slot = InGameManager.Instance.SRelicInventory.GetSlots.Find(o => o.srelic.idx== idx);
                }
            }
            //Message.Send<InGame.Event.EnemyKilled>(enemyKilled);
            StartCoroutine(FadeCharactercolor());
        }

        IEnumerator FadeCharactercolor()
        {
            float alphavalue = 1;
            while (alphavalue>0.2f)
            {
                alphavalue -= Time.deltaTime;
                _character._Skeletonanimator.skeleton.A = alphavalue;
                yield return null;
            }

            yield break;
        }
    
    }

}
