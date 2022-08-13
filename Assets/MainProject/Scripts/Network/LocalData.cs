using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree;
using BlackTree.Common;
using BlackTree.Model;
using System;

namespace BlackTree
{
    [System.Serializable]
    public class LocalSaveData_relic
    {
        public List<Relic> relicdata;
    }
    [System.Serializable]
    public class LocalSaveData_srelic
    {
        public List<SRelic> srelicdata;
    }
    [System.Serializable]
    public class LocalSaveData_weapon
    {
        public List<Item> Itemdata;
    }

    [System.Serializable]
    public class LocalSaveData_wing
    {
        public List<Item> Itemdata;
    }
    [System.Serializable]
    public class LocalSaveData_Costum
    {
        public List<Costum> Costumdata;
    }
    [System.Serializable]
    public class LocalSaveData_Pet
    {
        public List<PetObject> PetObjectdata;
    }
    [System.Serializable]
    public class LocalSaveData_skill
    {
        public List<SpecialSkill> skilldata;
    }
    public class LocalData
    {
        private readonly ISaveGameSerializer _serializer_user = new SaveGameBinarySerializer();
        private readonly ISaveGameSerializer _serializer_relic = new SaveGameBinarySerializer();
        private readonly ISaveGameSerializer _serializer_srelic = new SaveGameBinarySerializer();
        private readonly ISaveGameSerializer _serializer_weapon = new SaveGameBinarySerializer();
        private readonly ISaveGameSerializer _serializer_Wing = new SaveGameBinarySerializer();
        private readonly ISaveGameSerializer _serializer_Costum = new SaveGameBinarySerializer();
        private readonly ISaveGameSerializer _serializer_pet = new SaveGameBinarySerializer();
        private readonly ISaveGameSerializer _serializer_skill = new SaveGameBinarySerializer();

        LocalSaveData_relic saver_relic;
        LocalSaveData_srelic saver_srelic;
        LocalSaveData_weapon saver_weapon;
        LocalSaveData_wing saver_wing;
        LocalSaveData_Costum saver_costum;
        LocalSaveData_Pet saver_pet;
        LocalSaveData_skill saver_skill;
        public LocalData()
        {
            saver_relic = new LocalSaveData_relic();
            saver_relic.relicdata = new List<Relic>();
            saver_srelic = new LocalSaveData_srelic();
            saver_srelic.srelicdata = new List<SRelic>();
            saver_weapon = new LocalSaveData_weapon();
            saver_weapon.Itemdata = new List<Item>();
            saver_wing = new LocalSaveData_wing();
            saver_wing.Itemdata = new List<Item>();
            saver_costum = new LocalSaveData_Costum();
            saver_costum.Costumdata = new List<Costum>();
            saver_pet = new LocalSaveData_Pet();
            saver_pet.PetObjectdata = new List<PetObject>();
            saver_skill = new LocalSaveData_skill();
            saver_skill.skilldata = new List<SpecialSkill>();
        }

        #region 유저데이터
        //유저데이터
        public void SaveData(SaveData _userinfo)
        {
            //전처리
            for (int i = 0; i < _userinfo.Playercurrency.currencylist.Count; i++)
            {
                _userinfo.Playercurrency.currencylist[i].VALUE = _userinfo.Playercurrency.currencylist[i].value.ToString();
            }
#if SAVE_TEST
            SaveGame.Save("Login_Data", _userinfo, _serializer_user);
#endif
        }

        public void LoadData(System.Action serverLoad,System.Action firstSetting)
        {
#if UNITY_EDITOR
            if(SaveGame.Exists("Login_Data",SaveGamePath.DataPath))
#else
            if (SaveGame.Exists("Login_Data", SaveGamePath.PersistentDataPath))
#endif
            {
                var userinfo= SaveGame.Load("Login_Data", new SaveData(), _serializer_user);
                for (int i = 0; i < userinfo.Playercurrency.currencylist.Count; i++)
                {
                    userinfo.Playercurrency.currencylist[i].value = userinfo.Playercurrency.currencylist[i].VALUE;
                }
                for (int i = 0; i < InGameDataTableManager.shopTableList.battlepass.Count; i++)
                {
                    userinfo.battlepass.UserLevelBattlePassinfo[i].tableData = InGameDataTableManager.shopTableList.battlepass[i];
                }
                for (int i = 0; i < InGameDataTableManager.shopTableList.battlepass_fairy.Count; i++)
                {
                    userinfo.battlepass.FairyCountBattlePassinfo[i].tableData = InGameDataTableManager.shopTableList.battlepass_fairy[i];
                }
                for (int i = 0; i < InGameDataTableManager.shopTableList.battlepass_time.Count; i++)
                {
                    userinfo.battlepass.PlayingTimeBattlePassinfo[i].tableData = InGameDataTableManager.shopTableList.battlepass_time[i];
                }
                userinfo.missionInfo.LoadSetting();
                userinfo.Enforcement.LoadSetting();
                userinfo.GlobalUser.UpdateData();
                userinfo.userinfoPvp.Updatedata();

                DateTime parsedDate = InGameManager.Instance.GetPlayerData.GetServertime();
                if(userinfo.GlobalUser.CurrentPlayingDay.DayOfYear != parsedDate.DayOfYear)
                {
                    userinfo.GlobalUser.CurrentPlayingDay = parsedDate;
                    for (int i = 0; i < userinfo.missionInfo.dailyMission.Count; i++)
                    {
                        userinfo.missionInfo.dailyMission[i].Initialize();
                    }
                    for (int i = 0; i < userinfo.battlepass.PlayingTimeBattlePassinfo.Count; i++)
                    {
                        userinfo.battlepass.PlayingTimeBattlePassinfo[i].IsFreeTaken = false;
                        userinfo.battlepass.PlayingTimeBattlePassinfo[i].IsPaidTaken = false;
                    }
                    userinfo.GlobalUser.PlayingTime = 0;
                }


                InGameManager.Instance.GetPlayerData.saveData = userinfo;
            }
            else
            {
                serverLoad?.Invoke();
                firstSetting?.Invoke();
            }
            
        }
#endregion
#region 유물
        public void SaverelicData()
        {
            saver_relic.relicdata.Clear();
            //전처리
            for (int i=0; i< InGameManager.Instance.RelicInventory.GetSlots.Count; i++)
            {
                saver_relic.relicdata.Add(InGameManager.Instance.RelicInventory.GetSlots[i].item);
            }
#if SAVE_TEST
            SaveGame.Save("Login_relicData", saver_relic, _serializer_relic);
#endif
        }

        public void LoadRelicData(System.Action serverLoad)
        {
#if UNITY_EDITOR
            if (SaveGame.Exists("Login_relicData", SaveGamePath.DataPath))

#else
           if (SaveGame.Exists("Login_relicData", SaveGamePath.PersistentDataPath))
#endif
            {
                var relicinfo = SaveGame.Load("Login_relicData", new LocalSaveData_relic(), _serializer_relic);

                for (int i = 0; i < InGameManager.Instance.RelicInventory.GetSlots.Count; i++)
                {
                    InGameManager.Instance.RelicInventory.GetSlots[i].item = relicinfo.relicdata.Find(o=>o.idx== InGameManager.Instance.RelicInventory.GetSlots[i].itemData.relicInfo.idx);
                    InGameManager.Instance.RelicInventory.GetSlots[i].itemData =new RelicData(InGameManager.Instance.RelicInventory.GetSlots[i].item);
                    InGameManager.Instance.RelicInventory.GetSlots[i].UpdateSlot();
                }
                //InGameManager.Instance.GetPlayerData.saveData = userinfo;
            }
            else
            {
                serverLoad?.Invoke();
            }

        }
#endregion
#region 특수유물
        public void SavesrelicData()
        {
            saver_srelic.srelicdata.Clear();
            //전처리
            for (int i = 0; i < InGameManager.Instance.SRelicInventory.GetSlots.Count; i++)
            {
                saver_srelic.srelicdata.Add(InGameManager.Instance.SRelicInventory.GetSlots[i].srelic);
            }
#if SAVE_TEST
#endif
            SaveGame.Save("Login_srelicData", saver_srelic, _serializer_srelic);
        }

        public void LoadsRelicData(System.Action serverLoad)
        {
#if UNITY_EDITOR
            if (SaveGame.Exists("Login_srelicData", SaveGamePath.DataPath))

#else
           if (SaveGame.Exists("Login_srelicData", SaveGamePath.PersistentDataPath))
#endif
            {
                var srelicinfo = SaveGame.Load("Login_srelicData", new LocalSaveData_srelic(), _serializer_srelic);

                for (int i = 0; i < InGameManager.Instance.SRelicInventory.GetSlots.Count; i++)
                {
                    InGameManager.Instance.SRelicInventory.GetSlots[i].srelic = srelicinfo.srelicdata.Find(o => o.idx == InGameManager.Instance.SRelicInventory.GetSlots[i].srelicData.srelicInfo.idx);
                    InGameManager.Instance.SRelicInventory.GetSlots[i].srelicData =new SRelicData(InGameManager.Instance.SRelicInventory.GetSlots[i].srelic);
                    InGameManager.Instance.SRelicInventory.GetSlots[i].UpdateSlot();
                }
            }
            else
            {
                serverLoad?.Invoke();
            }

        }
#endregion
#region 무기
        public void SaveweaponData()
        {
            saver_weapon.Itemdata.Clear();
            //전처리
            for (int i = 0; i < InGameManager.Instance.WeaponInventory.GetSlots.Count; i++)
            {
                saver_weapon.Itemdata.Add(InGameManager.Instance.WeaponInventory.GetSlots[i].item);
            }
#if SAVE_TEST
  SaveGame.Save("Login_weaponData", saver_weapon, _serializer_weapon);
#endif

        }

        public void LoadweaponData(System.Action serverLoad)
        {
#if UNITY_EDITOR
            if (SaveGame.Exists("Login_weaponData", SaveGamePath.DataPath))

#else
           if (SaveGame.Exists("Login_weaponData", SaveGamePath.PersistentDataPath))
#endif
            {
                var weaponinfo = SaveGame.Load("Login_weaponData", new LocalSaveData_weapon(), _serializer_weapon);

                for (int i = 0; i < InGameManager.Instance.WeaponInventory.GetSlots.Count; i++)
                {
                    InGameManager.Instance.WeaponInventory.GetSlots[i].item = weaponinfo.Itemdata.Find(o => o.idx == InGameManager.Instance.WeaponInventory.GetSlots[i].itemData.itemInfo.idx);
                    InGameManager.Instance.WeaponInventory.GetSlots[i].itemData =new ItemData(ItemType.weapon, InGameManager.Instance.WeaponInventory.GetSlots[i].item);
                    InGameManager.Instance.WeaponInventory.GetSlots[i].UpdateSlot();
                }
            }
            else
            {
                serverLoad?.Invoke();
            }

        }
#endregion
#region 날개
        public void SavewingData()
        {
            saver_wing.Itemdata.Clear();
            //전처리
            for (int i = 0; i < InGameManager.Instance.WingInventory.GetSlots.Count; i++)
            {
                saver_wing.Itemdata.Add(InGameManager.Instance.WingInventory.GetSlots[i].item);
            }
//#if SAVE_TEST
    SaveGame.Save("Login_wingData", saver_wing, _serializer_Wing);
//#endif

        }

        public void LoadwingData(System.Action serverLoad)
        {
#if UNITY_EDITOR
            if (SaveGame.Exists("Login_wingData", SaveGamePath.DataPath))

#else
           if (SaveGame.Exists("Login_wingData", SaveGamePath.PersistentDataPath))
#endif
            {
                var winginfo = SaveGame.Load("Login_wingData", new LocalSaveData_wing(), _serializer_Wing);

                for (int i = 0; i < InGameManager.Instance.WingInventory.GetSlots.Count; i++)
                {
                    InGameManager.Instance.WingInventory.GetSlots[i].item = winginfo.Itemdata.Find(o => o.idx == InGameManager.Instance.WingInventory.GetSlots[i].itemData.itemInfo.idx);
                    InGameManager.Instance.WingInventory.GetSlots[i].itemData =new ItemData(ItemType.wing, InGameManager.Instance.WingInventory.GetSlots[i].item);
                    InGameManager.Instance.WingInventory.GetSlots[i].UpdateSlot();
                }
            }
            else
            {
                serverLoad?.Invoke();
            }

        }
#endregion

#region 코스튬
        public void SaveCostumData()
        {
            saver_costum.Costumdata.Clear();
            //전처리
            for (int i = 0; i < InGameManager.Instance.CostumInventory.GetSlots.Count; i++)
            {
                saver_costum.Costumdata.Add(InGameManager.Instance.CostumInventory.GetSlots[i].item);
            }
#if SAVE_TEST
    SaveGame.Save("Login_costumData", saver_costum, _serializer_Costum);
#endif

        }

        public void LoadCostumData(System.Action serverLoad)
        {
#if UNITY_EDITOR
            if (SaveGame.Exists("Login_costumData", SaveGamePath.DataPath))
#else
            if (SaveGame.Exists("Login_costumData", SaveGamePath.PersistentDataPath))
#endif
            {
                var costuminfo = SaveGame.Load("Login_costumData", new LocalSaveData_Costum(), _serializer_Costum);

                for (int i = 0; i < InGameManager.Instance.CostumInventory.GetSlots.Count; i++)
                {
                    InGameManager.Instance.CostumInventory.GetSlots[i].item = costuminfo.Costumdata.Find(o => o.idx == InGameManager.Instance.CostumInventory.GetSlots[i].itemData.CostumInfo.idx);
                    InGameManager.Instance.CostumInventory.GetSlots[i].itemData = new CostumData(InGameManager.Instance.CostumInventory.GetSlots[i].item);
                    InGameManager.Instance.CostumInventory.GetSlots[i].UpdateSlot();
                }
            }
            else
            {
                serverLoad?.Invoke();
            }

        }
#endregion

#region 펫
        public void SavepetData()
        {
            saver_pet.PetObjectdata.Clear();
            //전처리
            for (int i = 0; i < InGameManager.Instance.petInventory.GetSlots.Count; i++)
            {
                saver_pet.PetObjectdata.Add(InGameManager.Instance.petInventory.GetSlots[i].pet);
            }
#if SAVE_TEST
   SaveGame.Save("Login_petData", saver_pet, _serializer_pet);
#endif

        }

        public void LoadpetData(System.Action serverLoad)
        {
#if UNITY_EDITOR
            if (SaveGame.Exists("Login_petData", SaveGamePath.DataPath))

#else
           if (SaveGame.Exists("Login_petData", SaveGamePath.PersistentDataPath))
#endif
            {
                var petinfo = SaveGame.Load("Login_petData", new LocalSaveData_Pet(), _serializer_pet);

                for (int i = 0; i < InGameManager.Instance.petInventory.GetSlots.Count; i++)
                {
                    InGameManager.Instance.petInventory.GetSlots[i].pet = petinfo.PetObjectdata.Find(o => o.idx == InGameManager.Instance.petInventory.GetSlots[i].petData.petInfo.idx);
                    InGameManager.Instance.petInventory.GetSlots[i].petData =new PetData(InGameManager.Instance.petInventory.GetSlots[i].pet);
                    InGameManager.Instance.petInventory.GetSlots[i].UpdateSlot();
                }
            }
            else
            {
                serverLoad?.Invoke();
            }

        }
#endregion

#region 스킬정보
        public void SaveskillData()
        {
            saver_skill.skilldata.Clear();
            //전처리
            for (int i = 0; i < InGameManager.Instance.SpecialSkillInventory.GetSlots.Count; i++)
            {
                saver_skill.skilldata.Add(InGameManager.Instance.SpecialSkillInventory.GetSlots[i].skill);
            }
#if SAVE_TEST
    SaveGame.Save("Login_skillData", saver_skill, _serializer_skill);
#endif

        }

        public void LoadskillData(System.Action serverLoad)
        {
#if UNITY_EDITOR
            if (SaveGame.Exists("Login_skillData", SaveGamePath.DataPath))

#else
           if (SaveGame.Exists("Login_skillData", SaveGamePath.PersistentDataPath))
#endif
            {
                var skillinfo = SaveGame.Load("Login_skillData", new LocalSaveData_skill(), _serializer_skill);

                for (int i = 0; i < InGameManager.Instance.SpecialSkillInventory.GetSlots.Count; i++)
                {
                    InGameManager.Instance.SpecialSkillInventory.GetSlots[i].skill = skillinfo.skilldata.Find(o => o.idx == InGameManager.Instance.SpecialSkillInventory.GetSlots[i].specialskilldata.skillInfo.idx);
                    InGameManager.Instance.SpecialSkillInventory.GetSlots[i].specialskilldata.specialskill = InGameManager.Instance.SpecialSkillInventory.GetSlots[i].skill;
                    InGameManager.Instance.SpecialSkillInventory.GetSlots[i].UpdateSlot();
                }
            }
            else
            {
                serverLoad?.Invoke();
            }

        }
#endregion
    }

}
