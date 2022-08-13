using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackTree.Common;
using DLL_Common.Common;
using UnityEngine.EventSystems;

namespace BlackTree
{
    public class CharacterStatUI : MonoBehaviour
    {
        public Text Level;
        public Text StatusInfoName;
        public Text StatusInfoDesc;

        public Button Level_1Up;
        public Button Level_10Up;
        public Button Level_100Up;
        public Text Need_LevelUp_gold;

        public Button LevelUpButton;
        public GameObject CantLevelUpImage;

        int currentLevelCount;

        Data_Character character;

        BigInteger Level_1UpCost=BigInteger.Zero;
        BigInteger Level_10UpCost = BigInteger.Zero;
        BigInteger Level_100UpCost = BigInteger.Zero;

        public GameObject lvUp_1Selected;
        public GameObject lvUp_10Selected;
        public GameObject lvUp_100Selected;

        [SerializeField] Text GoldText;

        bool LvBtnPushed = false;
        public void Init()
        {
            LevelUpButton.gameObject.SetActive(true);

            Message.AddListener<UI.Event.CharacterInfoUIUpdate>(InfoUpdate);
            Message.AddListener<UI.Event.CurrencyChange>(AcceptCurrencyUpdated);
            currentLevelCount = 1;
            InfoUpdate(null);

            Level_1Up.onClick.AddListener(() => { LevelCountButton(1); });
            Level_10Up.onClick.AddListener(() => { LevelCountButton(10); });
            Level_100Up.onClick.AddListener(() => { LevelCountButton(100); });

            EventTrigger trigger = LevelUpButton.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => LvBtnDown());
            trigger.triggers.Add(pointerDown);

            var pointerDown_1 = new EventTrigger.Entry();
            pointerDown_1.eventID = EventTriggerType.PointerUp;
            pointerDown_1.callback.AddListener((e) => LvBtnUp());
            trigger.triggers.Add(pointerDown_1);
            LvBtnPushed = false;
        }
        
        public void Release()
        {
            Message.RemoveListener<UI.Event.CharacterInfoUIUpdate>(InfoUpdate);
            Message.RemoveListener<UI.Event.CurrencyChange>(AcceptCurrencyUpdated);
        }

        WaitForSeconds btnpushdelay = new WaitForSeconds(0.5f);
        Coroutine Btnpushevent=null;
        IEnumerator LvBtnPush()
        {
            yield return btnpushdelay;

            while(LvBtnPushed)
            {
                Levelup();
                yield return null;
            }
            yield break;
        }

        void LvBtnUp()
        {
            LvBtnPushed = false;
        }
        void LvBtnDown()
        {
            LvBtnPushed = true;
            if(Btnpushevent!=null)
                StopCoroutine(Btnpushevent);
            Btnpushevent = null;
            Levelup();
            Btnpushevent=StartCoroutine(LvBtnPush());
        }

        void Levelup()
        {
            BigInteger costLevelUp = character.ability.GetLevelUpCost(character.characterBaseData.Need_Gold_1Level);
            if (currentLevelCount == 10)
                costLevelUp = character.ability.GetLevelUpCost(character.characterBaseData.Need_Gold_10Level);
            else if (currentLevelCount == 100)
                costLevelUp = character.ability.GetLevelUpCost(character.characterBaseData.Need_Gold_100Level);

            if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value>= costLevelUp)
            {
            
                CharacterDataManager.Instance.PlayerCharacterdata.characterBaseData.Level += currentLevelCount;
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gold, -costLevelUp);

                InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.CHA_LEVELUP, currentLevelCount);
            }
        }

        void InfoUpdate(UI.Event.CharacterInfoUIUpdate msg)
        {
            character = CharacterDataManager.Instance.PlayerCharacterdata;
            Level.text =string.Format( character.characterBaseData.Level.ToString() + "(" + (int)character.GetAbilityValue(AbilitiesType.CHA_LV_UP).ToFloat() + ")");
            
            LevelCountButton(currentLevelCount);
            StatUpdate();
        }

        void LevelCountButton(int levelCount)
        {
            lvUp_1Selected.SetActive(false);
            lvUp_10Selected.SetActive(false);
            lvUp_100Selected.SetActive(false);

            switch (levelCount)
            {
                case 1:
                    lvUp_1Selected.SetActive(true);
                    break;
                case 10:
                    lvUp_10Selected.SetActive(true);
                    break;
                case 100:
                    lvUp_100Selected.SetActive(true);
                    break;
                default:
                    break;
            }

            if (currentLevelCount != levelCount)
                currentLevelCount = levelCount;

            LevelUpButton.gameObject.SetActive(true);
            //LevelUpButton.onClick.RemoveAllListeners();
           // LevelUpButton.onClick.AddListener(() => Levelup());
            

            Level_1UpCost = character.ability.GetLevelUpCost(character.characterBaseData.Need_Gold_1Level);
            Level_10UpCost = character.ability.GetLevelUpCost(character.characterBaseData.Need_Gold_10Level);
            Level_100UpCost = character.ability.GetLevelUpCost(character.characterBaseData.Need_Gold_100Level);

            if(levelCount==1)
            {
                Need_LevelUp_gold.text = "LV+1\n" + Level_1UpCost.ToDisplay();
                if (character.characterBaseData.Level < DTConstraintsData.PLAYER_MAX_LEVEL)
                {
                    if(Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value >= Level_1UpCost)
                    {
                        LevelUpButton.enabled = true;
                        CantLevelUpImage.SetActive(false);
                    }
                    else
                    {
                        LevelUpButton.enabled = false;
                        CantLevelUpImage.SetActive(true);
                    }
                        
                }
                else
                {
                    LevelUpButton.enabled = false;
                    CantLevelUpImage.SetActive(true);
                }
            }
            else if (levelCount == 10)
            {
                Need_LevelUp_gold.text = "LV+10\n" + Level_10UpCost.ToDisplay();
                if (character.characterBaseData.Level < DTConstraintsData.PLAYER_MAX_LEVEL - 10)
                {
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value >= Level_10UpCost)
                    {
                        CantLevelUpImage.SetActive(false);
                        LevelUpButton.enabled = true;
                    }
                    else
                    {
                        CantLevelUpImage.SetActive(true);
                        LevelUpButton.enabled = false;
                    }
                        
                }
                else
                {
                    CantLevelUpImage.SetActive(true);
                    LevelUpButton.enabled = false;
                }
            }
            else if (levelCount == 100)
            {
                Need_LevelUp_gold.text = "LV+100\n" + Level_100UpCost.ToDisplay();
                if (character.characterBaseData.Level < DTConstraintsData.PLAYER_MAX_LEVEL - 100)
                {
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value >= Level_100UpCost)
                    {
                        CantLevelUpImage.SetActive(false);
                        LevelUpButton.enabled = true;
                    }
                    else
                    {
                        CantLevelUpImage.SetActive(true);
                        LevelUpButton.enabled = false;
                    }
                }
                else
                {
                    CantLevelUpImage.SetActive(true);
                    LevelUpButton.enabled = false;
                }
            }
        }
        void AcceptCurrencyUpdated(UI.Event.CurrencyChange msg)
        {
            if (msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.Gold)
                    return;
            }

            if (msg.Type == CurrencyType.Gold)
            {
                GoldText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value.ToDisplay();
            }

            if (currentLevelCount==1)
            {
                if (character.characterBaseData.Level < DTConstraintsData.PLAYER_MAX_LEVEL)
                {
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value >= Level_1UpCost)
                    {
                        CantLevelUpImage.SetActive(false);
                        LevelUpButton.enabled = true;
                    }
                    else
                    {
                        CantLevelUpImage.SetActive(true);
                        LevelUpButton.enabled = false;
                    }
                }
            }
            else if( currentLevelCount==10)
            {
                if (character.characterBaseData.Level < DTConstraintsData.PLAYER_MAX_LEVEL - 10)
                {
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value >= Level_10UpCost)
                    {
                        CantLevelUpImage.SetActive(false);
                        LevelUpButton.enabled = true;
                    }
                    else
                    {
                        CantLevelUpImage.SetActive(true);
                        LevelUpButton.enabled = false;
                    }
                }
            }
            else if (currentLevelCount == 100)
            {
                if (character.characterBaseData.Level < DTConstraintsData.PLAYER_MAX_LEVEL - 100)
                {
                    if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gold).value >= Level_100UpCost)
                    {
                        CantLevelUpImage.SetActive(false);
                        LevelUpButton.enabled = true;
                    }
                    else
                    {
                        CantLevelUpImage.SetActive(true);
                        LevelUpButton.enabled = false;
                    }
                }
            }
        }     
    

        void StatUpdate()
        {
            CharacterDataAbility ability= CharacterDataManager.Instance.PlayerCharacterdata.ability;
            StatusInfoName.text = string.Format(
                "공격력\n"+
                "체력\n" +
                "공격속도\n" +
                "크리확률\n" +
                "크리데미지\n" +
                "이동속도\n");

            StatusInfoDesc.text = string.Format(
                "{0}\n" +
                "{1}\n" +
                "{2}\n" +
                "+{3:N2}%\n" +
                "+{4:N1}%\n" +
                "{5}\n"
                , ability.GetAtkDamage().ToDisplay(), ability.GetCharacterHp().ToDisplay(), (int)ability.GetAttackSpeed()*1000.0f, (float)ability.GetCriticalRate(),
                (float)ability.GetCriticalDamageRate()*100, (int)ability.GetMoveSpeed() * 1000.0f);


            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_LV] = ability.GetCharacterLevel().ToString();

            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_ATTACK] =
               (ability.Characterdata.characterBaseData.attack*ability.Characterdata.GetAbilityValue(AbilitiesType.ENFORCE_Gain_Rate)).ToString();
            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_ATTACK_UP] = ability.Characterdata.GetAbilityValue(AbilitiesType.CHA_ATTACK_UP).ToString();
            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_BAGIC_ATTACK_UP] = ability.Characterdata.GetAbilityValue(AbilitiesType.CHA_BAGIC_ATTACK_UP).ToString();

            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_ATTACK_SPEED_UP] = (ability.GetAttackSpeed().ToString());

            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.HealthPoint = ability.GetCharacterHp().ToString();
           
            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_CRITICAL_PER] = ability.GetCriticalRate().ToString();
            InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.AbilityList[PVPAbilityType.CHA_CRITICAL_DAMAGE_UP] = ability.GetCriticalDamageRate().ToString();

        }

    
    }

}
