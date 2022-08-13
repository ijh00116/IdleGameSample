using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class DungeonUnitUI : MonoBehaviour
    {
        DungeonPrime dungeonData;
        [SerializeField] Text DungeonName;
        [SerializeField] Image SelectImage;

        [SerializeField] GameObject CantEnterObject;
        [SerializeField] Text CantEnterText;

        Button button;
        int dgLevel;

        bool CanPlayDungeonLv;
        public void Init(int DG_stageLevel)
        {
            Message.AddListener<UI.Event.DungeonLevelTouch>(dungeonLevelUpdate);

            dgLevel = DG_stageLevel;
            dungeonData = InGameDataTableManager.DungeonTableList.Dungeon.Find(o => o.dg_stage == DG_stageLevel);
            button = this.GetComponent<Button>();
            button.onClick.AddListener(TouchButton);
            
            LocalValue dungeontitle = InGameDataTableManager.LocalizationList.info.Find(o => o.id == dungeonData.dg_stage_name);
            DungeonName.text = string.Format("{0}", dungeontitle.GetStringForLocal(true));

            SelectImage.gameObject.SetActive(false);
            if (DG_stageLevel==1)
            {
                Message.Send<UI.Event.DungeonLevelTouch>(new UI.Event.DungeonLevelTouch(dungeonData,true));
            }
            StageInfo stageinfo = Common.InGameManager.Instance.GetPlayerData.stage_Info;
            int stagescore = dungeonData.open_scenario * 1000 + dungeonData.open_chapter * 100 + dungeonData.open_stage;
            int currentscore = stageinfo.BestScenario * 1000 + stageinfo.Bestchapter * 100 + stageinfo.BestStage;
            if (stagescore<=currentscore)
            {
                CantEnterObject.SetActive(false);
                //button.enabled = true;
                CanPlayDungeonLv = true;
            }
            else
            {
                //button.enabled = false;
                CantEnterObject.SetActive(true);
                LocalValue dungeoncantenter = InGameDataTableManager.LocalizationList.info.Find(o => o.id == dungeonData.open_stage_script_id);
                CantEnterText.text = string.Format("<color=white>{0}</color>\n<color=red>{1}</color>", 
                    dungeontitle.GetStringForLocal(true), dungeoncantenter.GetStringForLocal(true));
                CanPlayDungeonLv = false;
            }
            
            
        }

        private void OnDestroy()
        {
            Message.RemoveListener<UI.Event.DungeonLevelTouch>(dungeonLevelUpdate);
        }

        public void Release()
        {
            
        }

        void TouchButton()
        {
            Message.Send<UI.Event.DungeonLevelTouch>(new UI.Event.DungeonLevelTouch(dungeonData, CanPlayDungeonLv));
        }

        void dungeonLevelUpdate(UI.Event.DungeonLevelTouch msg)
        {
            if(dgLevel==msg.dungeondata.dg_stage)
            {
                SelectImage.gameObject.SetActive(true);
            }
            else
            {
                SelectImage.gameObject.SetActive(false);
            }
        }
    }

}
