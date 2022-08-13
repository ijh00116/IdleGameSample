using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public struct StoryHistory
    {
        public int idx;
        public bool AlreadySeen;
    }
    [System.Serializable]
    public class DungeonInfo
    {
        public int KillCount;
        public int KingKillCount;
    }
    [System.Serializable]
    public class ChapterReward
    {
        public int ScenarioIndex;
        public int ChapterIndex;
        public bool Rewarded;
    }

    [System.Serializable]
    public class StageSubInfo
    {
        public List<StoryHistory> storyhistoryInfo = new List<StoryHistory>();

        public List<ChapterReward> RewardInfo = new List<ChapterReward>();
        public List<DungeonInfo> Dungeoninfo = new List<DungeonInfo>();
        public List<DungeonInfo> PetDungeoninfo = new List<DungeonInfo>();

        public StageMonster stageMonsterdata;
        public StageReward stageRewarddata;
        public DungeonPrime Dungeondata;
        public PetDungeonPrime CurrentPetDungeondata;

        public List<int> CurrentPetDungeonRewardItemList = new List<int>();
        public List<int> CurrentPetDungeonRewardPetList = new List<int>();

    }

    [System.Serializable]
    public class StageInfo
    {
        public int BestScenario=1;
        public int Bestchapter=1;
        public int BestStage=1;

        public int scenario = 1;// max scenario : 10
        public int chapter = 1; //max chapter :10
        public int Stage = 1;   //max stage=100
       
        public int currentWave = 0;

        public int CowWave = 0;//위치 변경 위한 웨이브 인덱스
        public int CowCurrentKillCount = 0;//진행중 소 죽인 카운트
        public int KingCowCurrentKillCount = 0;//진행중 소 죽인 카운트
        public int CowBestLevel = 0;//최고 던전 달성 스테이지
        public int CowCurrentLevel = 0;//현재 달성 혹은 달성중인 던전 스테이지

        //펫 관련
        public int Pet_CurrentChapter = 1;//현재 달성 혹은 달성중인 펫던전 챕터
        public int Pet_CurrentStage = 1;//현재 달성 혹은 달성중인 펫던전 스테이지

        public int Pet_BestChapter = 1; //달성 최고 챕터
        public int Pet_BestStage = 1; //달성 최고 스테이지 //맥스 50
        public int PetCurrentKillCount = 0;//진행중 펫 죽인 카운트

        public bool PetDungeonAutoRestart=false;
        public bool PetDungeonAutoNextStart = false;

        public int CurrentMaxwave;

        public StageSubInfo stagesubinfo { get {return InGameManager.Instance.GetPlayerData.stage_subinfo; } }
        //스토리 관련
        public void Init()
        {
            StageDataUpdate();

            //시나리오 챕터 둘다 10개까지만 적용되므로 여기서 10으로 상한선 둔다
            for (int i=0; i<DTConstraintsData.NormalMaxScenario; i++)
            {
                for (int j = 0; j < DTConstraintsData.NormalMaxChapter; j++)
                {
                    ChapterReward _chapterReward = new ChapterReward() { ChapterIndex = j, ScenarioIndex = i, Rewarded = false };
                    stagesubinfo.RewardInfo.Add(_chapterReward);
                }
            }
            //던전 데이터 임시 초기화
            for(int i=0; i< DTConstraintsData.DungeonMaxStage; i++)
            {
                DungeonInfo dunInfo = new DungeonInfo() { KillCount = 0 };
                stagesubinfo.Dungeoninfo.Add(dunInfo);
            }
            for (int i = 0; i < 15; i++)
            {
                DungeonInfo _petdunInfo = new DungeonInfo() { KillCount = 0 };
                stagesubinfo.PetDungeoninfo.Add(_petdunInfo);
            }
            for(int i=0; i<InGameDataTableManager.StoryTableList.main.Count; i++)
            {
                StoryHistory story;
                story.idx = InGameDataTableManager.StoryTableList.main[i].idx;
                story.AlreadySeen = false;
                stagesubinfo.storyhistoryInfo.Add(story);
            }
            BestScenario = scenario;
            Bestchapter = chapter;
        }

        public void Update()
        {
            StageDataUpdate();
            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.Scenario = BestScenario;
            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.Chapter= Bestchapter;
            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.Stage = BestStage;
            //Debug.Log(stageMonsterdata.monster_hp);

            CurrentMaxwave = -1;
        }

        void StageDataUpdate()
        {
            for (int i = 0; i < InGameDataTableManager.StageList.Monster.Count; i++)
            {
                if (scenario == InGameDataTableManager.StageList.Monster[i].scenario &&
                   chapter == InGameDataTableManager.StageList.Monster[i].chapter &&
                   Stage == InGameDataTableManager.StageList.Monster[i].stage)
                {
                    stagesubinfo.stageMonsterdata = InGameDataTableManager.StageList.Monster[i];
                    break;
                }
            }

            for (int i = 0; i < InGameDataTableManager.StageList.Reward.Count; i++)
            {
                if (scenario == InGameDataTableManager.StageList.Reward[i].scenario &&
                   chapter == InGameDataTableManager.StageList.Reward[i].chapter)
                {
                    stagesubinfo.stageRewarddata = InGameDataTableManager.StageList.Reward[i];
                    break;
                }
            }
        }

        public void CowStageDataUpdate()
        {
            //이걸 가지고 체력 등등 세팅
            int currentlevel = CowCurrentLevel;
            CowCurrentKillCount = 0;
            KingCowCurrentKillCount = 0;
            stagesubinfo.Dungeondata = InGameDataTableManager.DungeonTableList.Dungeon.Find(o => o.dg_stage == currentlevel + 1);
        }

        [System.NonSerialized]
        public List<PetDungeonItemReward> PetdungeonItemrewardList=new List<PetDungeonItemReward>();
        public void PetStageDataUpdate()
        {
            PetDungeonPrime petinfo = InGameDataTableManager.DungeonTableList.Dungeon_pet.Find(o => o.pet_chapter == Pet_CurrentChapter);
            stagesubinfo.CurrentPetDungeondata = petinfo;

            //아이템 보상 정보
            if (InGameManager.Instance.GetPlayerData.stage_Info.Pet_BestStage % 2 == 0)
            {
                int wingrewardidx = petinfo.even_stage_reward_item_group;
                PetdungeonItemrewardList = InGameDataTableManager.DungeonTableList.item_reward.FindAll(o => o.idx == wingrewardidx);
            }
            else//홀수 특수유물 획득
            {
                int relicrewardidx = petinfo.odd_stage_reward_item_group;
                PetdungeonItemrewardList = InGameDataTableManager.DungeonTableList.item_reward.FindAll(o => o.idx == relicrewardidx);
            }

            //현재획득 내용 리스트 전에 초기화
            stagesubinfo.CurrentPetDungeonRewardItemList.Clear();
            stagesubinfo.CurrentPetDungeonRewardPetList.Clear();
        }

        public int MaxWave()
        {
            MaxWaveUpdate();
            return CurrentMaxwave;
        }

        public void MaxWaveUpdate()
        {
            Data_Character _character = CharacterDataManager.Instance.PlayerCharacterdata;
            int wavecount = _character.ability.GetWaveMonsterCount();
            if(CurrentMaxwave!=wavecount)
                CurrentMaxwave = wavecount;
        }
    }

}
