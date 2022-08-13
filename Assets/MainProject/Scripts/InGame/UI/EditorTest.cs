using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackTree.Common;

namespace BlackTree
{
    public class EditorTest : MonoBehaviour
    {
        [SerializeField]InputField scenario;
        [SerializeField] InputField chapter;
        [SerializeField] InputField stage;
        
        public void StageMove()
        {
            InGameManager.Instance.GetPlayerData.stage_Info.scenario = int.Parse(scenario.text);
            InGameManager.Instance.GetPlayerData.stage_Info.chapter = int.Parse(chapter.text);
            InGameManager.Instance.GetPlayerData.stage_Info.Stage = int.Parse(stage.text);
            InGameManager.Instance._sceneFsm._State.ChangeState(ePlayScene.MainInit);
        }

        public void MaxWeaponAmount()
        {
            for(int i=0; i < InGameManager.Instance.WeaponInventory.GetSlots.Count; i++)
            {
                InGameManager.Instance.WeaponInventory.GetSlots[i].AddAmount(100);
            }
        }
        public void MaxWeaponLevel()
        {
            for (int i = 0; i < InGameManager.Instance.WeaponInventory.GetSlots.Count; i++)
            {
                if(InGameManager.Instance.WeaponInventory.GetSlots[i].item.amount<=0)
                    InGameManager.Instance.WeaponInventory.GetSlots[i].AddAmount(1);
                int maxlevel = InGameManager.Instance.WeaponInventory.GetSlots[i].itemData.itemInfo.max_lv;
                switch (InGameManager.Instance.WeaponInventory.GetSlots[i].item.AwakeLv)
                {
                    case 0:
                        maxlevel = InGameManager.Instance.WeaponInventory.GetSlots[i].itemData.itemInfo.max_lv;
                        break;
                    case 1:
                        maxlevel = InGameManager.Instance.WeaponInventory.GetSlots[i].itemData.itemInfo.awake_max_lv;
                        break;
                    case 2:
                        maxlevel = InGameManager.Instance.WeaponInventory.GetSlots[i].itemData.itemInfo.awake2_max_lv;
                        break;
                    case 3:
                        maxlevel = InGameManager.Instance.WeaponInventory.GetSlots[i].itemData.itemInfo.awake3_max_lv;
                        break;
                }

                InGameManager.Instance.WeaponInventory.GetSlots[i].item.Level=maxlevel;
                InGameManager.Instance.WeaponInventory.GetSlots[i].UpdateSlot();
            }
        }

        public void MaxWingAmount()
        {
            for (int i = 0; i < InGameManager.Instance.WingInventory.GetSlots.Count; i++)
            {
                InGameManager.Instance.WingInventory.GetSlots[i].AddAmount(100);
            }
        }
        public void MaxWingLevel()
        {
            for (int i = 0; i < InGameManager.Instance.WingInventory.GetSlots.Count; i++)
            {
                if (InGameManager.Instance.WingInventory.GetSlots[i].item.amount <= 0)
                    InGameManager.Instance.WingInventory.GetSlots[i].AddAmount(1);
                int maxlevel = InGameManager.Instance.WingInventory.GetSlots[i].itemData.itemInfo.max_lv;
                InGameManager.Instance.WingInventory.GetSlots[i].item.Level = maxlevel;
                InGameManager.Instance.WingInventory.GetSlots[i].UpdateSlot();
            }
        }

        public void MaxPetAmount()
        {
            for (int i = 0; i < InGameManager.Instance.petInventory.GetSlots.Count; i++)
            {
                InGameManager.Instance.petInventory.GetSlots[i].AddAmount(100);
            }
        }
        public void MaxPetLevel()
        {
            for (int i = 0; i < InGameManager.Instance.petInventory.GetSlots.Count; i++)
            {
                if (InGameManager.Instance.petInventory.GetSlots[i].pet.amount <= 0)
                    InGameManager.Instance.petInventory.GetSlots[i].AddAmount(1);
                int maxlevel = InGameManager.Instance.petInventory.GetSlots[i].petData.petInfo.max_level;
                InGameManager.Instance.petInventory.GetSlots[i].pet.Level = maxlevel;
                InGameManager.Instance.petInventory.GetSlots[i].UpdateSlot();
            }
        }

        public void MaxrelicAmount()
        {
            for (int i = 0; i < InGameManager.Instance.RelicInventory.GetSlots.Count; i++)
            {
                InGameManager.Instance.RelicInventory.GetSlots[i].AddAmount(100);
            }
        }
        public void MaxrelicLevel()
        {
            for (int i = 0; i < InGameManager.Instance.RelicInventory.GetSlots.Count; i++)
            {
                if (InGameManager.Instance.RelicInventory.GetSlots[i].item.Unlocked == false)
                    InGameManager.Instance.RelicInventory.GetSlots[i].AddAmount(1);
                int maxlevel = InGameManager.Instance.RelicInventory.GetSlots[i].itemData.relicInfo.level_max;
                InGameManager.Instance.RelicInventory.GetSlots[i].item.Level = maxlevel;
                InGameManager.Instance.RelicInventory.GetSlots[i].UpdateSlot();
            }
        }

        public void MaxsrelicAmount()
        {
            for (int i = 0; i < InGameManager.Instance.SRelicInventory.GetSlots.Count; i++)
            {
                InGameManager.Instance.SRelicInventory.GetSlots[i].AddAmount(100);
            }
        }
        public void MaxsrelicLevel()
        {
            for (int i = 0; i < InGameManager.Instance.SRelicInventory.GetSlots.Count; i++)
            {
                if (InGameManager.Instance.SRelicInventory.GetSlots[i].srelic.amount <= 0)
                    InGameManager.Instance.SRelicInventory.GetSlots[i].AddAmount(1);
                int maxlevel = InGameManager.Instance.SRelicInventory.GetSlots[i].srelicData.srelicInfo.level_max;
                InGameManager.Instance.SRelicInventory.GetSlots[i].srelic.Level= maxlevel;
                InGameManager.Instance.SRelicInventory.GetSlots[i].UpdateSlot();
            }
        }

        public void MaxCostumLevel()
        {
            for (int i = 0; i < InGameManager.Instance.CostumInventory.GetSlots.Count; i++)
            {
                if (InGameManager.Instance.CostumInventory.GetSlots[i].item.Unlocked ==false)
                    InGameManager.Instance.CostumInventory.GetSlots[i].item.Unlocked=true;
                int maxlevel = InGameManager.Instance.CostumInventory.GetSlots[i].itemData.CostumInfo.max_lv;
                InGameManager.Instance.CostumInventory.GetSlots[i].item.Level = maxlevel;
                InGameManager.Instance.CostumInventory.GetSlots[i].UpdateSlot();
            }
        }
    }


}
