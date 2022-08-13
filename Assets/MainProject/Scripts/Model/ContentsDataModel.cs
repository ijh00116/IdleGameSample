using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlackTree.Model
{
    [CreateAssetMenu(fileName = "ContentsData", menuName = "GameData/ContentsData")]
    public class ContentsDataModel : ScriptableObject
    {
        public float DecreaseLevelUpRate;       //레벨업 비용 감소
        public float IncreaseQuestReward;
        public float Decrease_CostofUpgradeQuest;
        public float Decrease_QuestTime;
        public float Increase_GoldOfKillMonster;
        public float Increase_DryCellGain;
        public float Increase_GainGoldOfKillBoss;
        public float Increase_GainDryCellOfKillBoss;
        public float Increase_MimicGenerationRate;
        public float Increase_GoldGain_KillMimic;
        public float Increase_DryCellGainGain_KillMimic;
        public float Increase_CowRoomTime;
        public float Increase_CowKingGenRate;
        public float Increase_GainMilkInCowRoom;
        public float Increase_GainDarkBallRate;
        public float TenTimesOfGoldRate_KillEnemy;
        public float TenTimesOfDryCellRate_KillEnemy;
    }

}

