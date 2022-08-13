using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DLL_Common.Common;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlackTree
{
    public class Data_CharacterInfo
    {
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                CharacterLevelUpdate();
            }
        }
        private int level;
        public BigInteger attack;
        public BigInteger hp;
        public float critical;
        public float critical_damage;
        public int attack_speed;
        public int move_speed;

        public BigInteger Need_Gold_1Level;
        public BigInteger Need_Gold_10Level;
        public BigInteger Need_Gold_100Level;

        string basicSavePath;

        UI.Event.CharacterInfoUIUpdate UISendMessager=new UI.Event.CharacterInfoUIUpdate();
        Data_Character _character;
        public void Init()
        {
#if UNITY_EDITOR
            basicSavePath = Application.dataPath + "/SaveData";
#else
            basicSavePath = Application.persistentDataPath;
#endif
            LoadCharacterInfo();

            _character = CharacterDataManager.Instance.PlayerCharacterdata;

            CharacterLevelUpdate();

            Message.AddListener<InGame.Event.CharacterAbilityUpdate>(LevelUpdatedOutSide);
        }

        public void CharacterLevelUpdate()
        {
            InGameManager.Instance.GetPlayerData.saveData.GlobalUser.CharacterLevel= this.level;
            //베이스스탯
            for (int i = 0; i < InGameDataTableManager.CharacterList.Status.Count; i++)
            {
                if (InGameDataTableManager.CharacterList.Status[i].level == _character.ability.GetCharacterLevel())
                {
                    BaseStatSetting(InGameDataTableManager.CharacterList.Status[i]);
                    break;
                }
            }
            //레벨업 필요 골드
            int index = 0;
            for (int i = 0; i < InGameDataTableManager.CharacterList.levelup.Count; i++)
            {
                if (InGameDataTableManager.CharacterList.levelup[i].level == _character.characterBaseData.level)
                {
                    index = i;
                    string needGold = InGameDataTableManager.CharacterList.levelup[i].need_gold;
                    this.Need_Gold_1Level = needGold;
                    string needGold_10 = InGameDataTableManager.CharacterList.levelup[index].need_gold_10;
                    Need_Gold_10Level = needGold_10;
                    string needGold_100 = InGameDataTableManager.CharacterList.levelup[index].need_gold_100;
                    Need_Gold_100Level = new BigInteger(needGold_100);
                    break;
                }
            }
            
            Message.Send<UI.Event.CharacterInfoUIUpdate>(UISendMessager);
        }

        void LevelUpdatedOutSide(InGame.Event.CharacterAbilityUpdate msg)
        {
            if(msg.abilType==AbilitiesType.CHA_LV_UP)
            {
                CharacterLevelUpdate();
            }
        }

        void BaseStatSetting(CharacterBasicStat status)
        {
            this.attack = status.attack;
            this.hp = status.hp;
            this.critical=status.critical;
            this.critical_damage=status.critical_damage;
            this.attack_speed=status.attack_speed;
            this.move_speed=status.move_speed;
         }

        public void Release()
        {
            SaveCharacterInfo();
            Message.RemoveListener<InGame.Event.CharacterAbilityUpdate>(LevelUpdatedOutSide);
        }

        void SaveCharacterInfo()
        {
           
        }

        void LoadCharacterInfo()
        {
            this.level = InGameManager.Instance.GetPlayerData.saveData.GlobalUser.CharacterLevel;
         
        }

    }

}
