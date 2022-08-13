using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    [System.Serializable]
    public class UserInfoForPVP 
    {
        public string NickName { get; set; }
        public int RankScore { get; set; }
        public string HealthPoint { get; set; }
        public string SkinName { get; set; }
        public string weaponName { get; set; }
        public string wingName{ get; set; }
        public int RankingNumber { get; set; }
        public int Scenario { get; set; }
        public int Chapter { get; set; }
        public int Stage { get; set; }


        [System.NonSerialized]
        public Dictionary<PVPAbilityType, string> AbilityList = new Dictionary<PVPAbilityType, string>();
    
        public void Init()
        {
            RankScore = 1000;

            Scenario = 1;
            Chapter = 1;
            Stage = 1;

            for (int i=0; i<InGameDataTableManager.PVPTableList.battle_ability.Count; i++)
            {
                if(InGameDataTableManager.PVPTableList.battle_ability[i].abtype!=PVPAbilityType.End)
                    AbilityList.Add(InGameDataTableManager.PVPTableList.battle_ability[i].abtype, "0");
                if (InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype != PVPAbilityType.End)
                    AbilityList.Add(InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype, "0");
                if (InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype_2 != PVPAbilityType.End)
                    AbilityList.Add(InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype_2, "0");
            }
        }
        public void Updatedata()
        {
            RankScore = Scenario * 1000 + Chapter * 100 + Stage;
            if(AbilityList==null)
            {
                AbilityList = new Dictionary<PVPAbilityType, string>();
                for (int i = 0; i < InGameDataTableManager.PVPTableList.battle_ability.Count; i++)
                {
                    if (InGameDataTableManager.PVPTableList.battle_ability[i].abtype != PVPAbilityType.End)
                        AbilityList.Add(InGameDataTableManager.PVPTableList.battle_ability[i].abtype, "0");
                    if (InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype != PVPAbilityType.End)
                        AbilityList.Add(InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype, "0");
                    if (InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype_2 != PVPAbilityType.End)
                        AbilityList.Add(InGameDataTableManager.PVPTableList.battle_ability[i].SkillAbtype_2, "0");
                }
            }
                

           
        }
    }
}
