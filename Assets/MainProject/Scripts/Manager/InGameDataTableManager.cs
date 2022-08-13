using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;
using BlackTree.Model;

namespace BlackTree
{
    //public class Quest_DataTable
    //{
    //    public List<QuestInfo> quest;
    //    public List<QuestGainInfo> gain;
    //}
    public class Character_DataTable
    {
        public List<CharacterEnchant> Enchant;
        public List<CharacterBasicStat> Status;
        public List<LevelUpInfo> levelup;
        public List<ETCUserInfo> user;
    }
    public class Stage_DataTable
    {
        public List<StageMonster> Monster;
        public List<StageGroup> group;
        public List<StageReward> Reward;
        public List<StageBoxReward> box_reward;
    }

    public class Relic_DataTable
    {
        public List<RelicInfo> relic;
        public List<SRelicInfo> s_relic;
        public List<RelicNeedCurrency> gain;
        public List<ActiveSkillInfo> active_skill;
        public List<FeverReward> fever_reward;
    }

    public class Ability_DataTable
    {
        public List<AbilityInfo> abilities;
    }

    public class Localization_DataTable
    {
        public List<LocalValue> info;
        public List<LocalValue> pvp;
        public List<LocalValue> currency;
        public List<LocalValue> ability;
        public List<LocalValue> relic;
        public List<LocalValue> pet;
        public List<LocalValue> shop;
        public List<LocalValue> skill;
        public List<LocalValue> system;
        public List<LocalValue> game;
        public List<LocalValue> fairy;
        public List<LocalValue> weapon;
        public List<LocalValue> title;
        public List<LocalValue> box;
        public List<LocalValue> mission;
        public List<LocalValue> mail;
        public List<LocalValue> help;
        public List<LocalValue> item;
        public List<LocalValue> story;
        
    }

    public class Skill_DataTable
    {
        public List<PassiveSkillPrime> passive;
        public List<PassiveSkillLevelupNeedSoul> gain;
    }

    public class Item_DataTable
    {
        public List<ItemInformation> weapon;
        public List<ItemInformation> wing;
        public List<CostumInformation> costum;
        public List<ItemGain> gain;
    }

    public class Dungeon_DataTable
    {
        public List<DungeonPrime> Dungeon;
        public List<PetDungeonPrime> Dungeon_pet;
        public List<PetReward> pet_reward;
        public List<PetDungeonItemReward> item_reward;
        public List<PetDungeonGainInfo> gain;
    }

    public class Offline_DataTable
    {
        public List<OfflineRewardData> reward;
    }
    public class DailyReward_DataTable
    {
        public List<DailyRewardData> attendance;
    }
    public class Box_DataTable
    {
        public List<BoxData> box;
        public List<Box_Group> box_group;
        public List<Reward_Item> reward_weapon;
        public List<Reward_Item> reward_pet;
        public List<Reward_Item> reward_s_relic;
    }

    public class Pet_DataTable
    {
        public List<PetInfo> pet;
        public List<PetGain> gain;
    }

    public class Gacha_DataTable
    {
        public List<Gacha> gacha;
        public List<GachapotionGainRate> gain;
        public List<GachaRewardBoxInfo> reward_box;
        public List<GachaPointReward> point_reward;
    }

    public class Tutorial_DataTable
    {
        public List<TutorialTouch> tutorial_touch;
    }

    public class Test_LocalizationTable
    {
        public List<LocalValue> dialog;
        public List<LocalValue> tutorial;
    }

    public class Mission_DataTable
    {
        public List<DailyMissionDesc> daily_mission;
        public List<GuideMissionDesc> guide_mission;
        public List<RepeatMissionDesc> repeat_mission;
    }

    public class PVP_DataTable
    {
        public List<PVP_BattleReward> battle_reward;
        public List<PVP_DailyReward> daily_reward;
        public List<PVP_WeekReward> week_reward;
        public List<PVP_Rankreset> week_rank_reset;
        public List<PVP_BattleAbility> battle_ability;
    }

    public class ShopDataTable
    {
        public List<ShopGoodstable> limited;
        public List<ShopGoodstable> package;
        public List<ShopGoodstable> mileage;
        public List<ShopGoodstable> gem;
        public List<ShopGoodstable> goods;
        public List<ShopGoodstable> buff;

        public List<Battlepasstable> battlepass;
        public List<Battlepasstable> battlepass_fairy;
        public List<Battlepasstable> battlepass_time;
        public List<ShopGoodstable> pvp;
        public List<ShopRewardInfotable> reward;
        public List<Iapinformationtable> iap;
    }

    public class RouletteTable
    {
        public List<RouletteTabledata> roulette;
    }

    public class StoryTable
    {
        public List<StoryBookMain> main;
        public List<StoryBookScript> script;
    }

    public class NewbiepackageTable
    {
        public List<Newbieinfo> newbie;
        public List<Iapinfo> iap;
    }

    public class FairyInfo_DataTable
    {
        public List<FairyInfo> fairy;
    }
    public class InGameDataTableManager : MonoSingleton<InGameDataTableManager>
    {
        public static Character_DataTable CharacterList;
        public static Stage_DataTable StageList;
        public static Relic_DataTable RelicList;
        public static Ability_DataTable AbilityList;
        public static Localization_DataTable LocalizationList;
        public static Item_DataTable ItemTableList;
        public static Dungeon_DataTable DungeonTableList;
        public static Offline_DataTable OfflineRewardTableList;
        public static DailyReward_DataTable DailyRewardTableList;
        public static Box_DataTable BoxTableList;
        public static Pet_DataTable PetTableList;
        public static Gacha_DataTable GachaTableList;
        public static Tutorial_DataTable TutorialTableList;
        public static Mission_DataTable MissionTableList;
        public static PVP_DataTable PVPTableList;
        public static Test_LocalizationTable TestLocalTableList;
        public static ShopDataTable shopTableList;
        public static RouletteTable RouletteEventTableList;
        public static StoryTable StoryTableList;
        public static NewbiepackageTable NewbiePackage;
        public static FairyInfo_DataTable fairyTableList;
        protected string _path
        {
            get
            {
                return Application.dataPath + "/Resources/Tables/Json";
            }
        }

        protected override void Init()
        {
            base.Init();
            CharacterList = new Character_DataTable();
            StageList = new Stage_DataTable();
            RelicList = new Relic_DataTable();
            AbilityList=new Ability_DataTable();
            LocalizationList = new Localization_DataTable();
            ItemTableList = new Item_DataTable();
            DungeonTableList = new Dungeon_DataTable();
            OfflineRewardTableList = new Offline_DataTable();
            DailyRewardTableList = new DailyReward_DataTable();
            BoxTableList = new Box_DataTable();
            PetTableList = new Pet_DataTable();
            GachaTableList = new Gacha_DataTable();
            TutorialTableList = new Tutorial_DataTable();
            TestLocalTableList = new Test_LocalizationTable();
            MissionTableList = new Mission_DataTable();
            PVPTableList = new PVP_DataTable();
            RouletteEventTableList = new RouletteTable();
            shopTableList = new ShopDataTable();
            StoryTableList = new StoryTable();
            NewbiePackage = new NewbiepackageTable();
            fairyTableList = new FairyInfo_DataTable();
        }
  

        private void Update()
        {
        }
         
        public IEnumerator Load()
        {
            CharacterList= ReadData<Character_DataTable>("DT_KR_Characters.xlsx");
            StageList = ReadData<Stage_DataTable>("DT_KR_Stage.xlsx");
            RelicList= ReadData<Relic_DataTable>("DT_KR_Relic.xlsx");
            AbilityList = ReadData<Ability_DataTable>("DT_KR_Abillities.xlsx");
            LocalizationList = ReadData<Localization_DataTable>("DT_KR_Localization.xlsx");
            ItemTableList = ReadData<Item_DataTable>("DT_KR_Weapons.xlsx");
            DungeonTableList = ReadData<Dungeon_DataTable>("DT_KR_Dungeon.xlsx");
            OfflineRewardTableList = ReadData<Offline_DataTable>("DT_KR_Offline.xlsx");
            DailyRewardTableList = ReadData<DailyReward_DataTable>("DT_KR_Attendance.xlsx");
            BoxTableList = ReadData<Box_DataTable>("DT_KR_Box.xlsx");
            PetTableList = ReadData<Pet_DataTable>("DT_KR_Pet.xlsx");
            GachaTableList = ReadData<Gacha_DataTable>("DT_KR_Gacha.xlsx");
            MissionTableList = ReadData<Mission_DataTable>("DT_KR_Mission.xlsx");
            PVPTableList= ReadData<PVP_DataTable>("DT_KR_Pvp.xlsx");
            RouletteEventTableList = ReadData<RouletteTable>("DT_KR_Event.xlsx");
            shopTableList = ReadData<ShopDataTable>("DT_KR_Shop.xlsx");
            StoryTableList = ReadData<StoryTable>("DT_KR_Story.xlsx");
            NewbiePackage = ReadData<NewbiepackageTable>("DT_KR_Newbie.xlsx");
            fairyTableList = ReadData<FairyInfo_DataTable>("DT_KR_Fairy.xlsx");

            //임시 튜토 테이블
            TutorialTableList = ReadData<Tutorial_DataTable>("DT_SR_Dialog.xlsx");
            TestLocalTableList = ReadData<Test_LocalizationTable>("DT_SR_Localization.xlsx");

            yield break;
        }
         
        T ReadData<T>(string fileName,BundleType bundleType=BundleType.None)
        {
            bundleType = BundleType.None;

            var path = new System.Text.StringBuilder();
            path.Append(bundleType != BundleType.None ? "" : "Tables/");
            path.Append(fileName);
            if (bundleType != BundleType.None)
            {
                 path.Append(".json");
            }

            TextAsset jsonString = Resources.Load<TextAsset>(path.ToString());

            if (jsonString != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString.text);
            }
            return default;
        }
    }

}
