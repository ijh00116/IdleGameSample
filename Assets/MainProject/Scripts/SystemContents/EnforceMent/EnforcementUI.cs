using DLL_Common.Common;
using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class EnforcementUI : MonoBehaviour
    {
        CharacterEnforcement Enforcement;

        [SerializeField] Button enforceBtn;//강화(숫자 올리는거)
        [SerializeField] Image CantenforceBtn;
        [SerializeField] Button AwakeBtn;//진화(알파벳 올리는거)
        [SerializeField] Image CantAwakeBtn;

        [SerializeField] Text EnforceNeedCurrency;
        [SerializeField] Text AwakeNeedSoulCurrency;
        [SerializeField] Text AwakeNeedGemCurrency;

        [SerializeField] Text AwakeInfoText;
        [SerializeField] Text AwakelvText;
        [SerializeField] Text EnforceLvInfo;
        [SerializeField] Text AttackInfo;

        [SerializeField] Image[] EnforceImage;
        [SerializeField] Image[] AwakeImage;

        public void Init()
        {
            Enforcement = InGameManager.Instance.GetPlayerData.Enforcement;


            Enforcement.onAfterUpdated += ChangeEnforceInfo;
            AwakeBtn.onClick.AddListener(TouchevolutionUP);
            enforceBtn.onClick.AddListener(TouchenforceUP);

            Enforcement.Update();

            if (Enforcement.EnchentLevel >= 10)
            {
                enforceBtn.gameObject.SetActive(false);
                AwakeBtn.gameObject.SetActive(true);
            }
            else
            {
                enforceBtn.gameObject.SetActive(true);
                AwakeBtn.gameObject.SetActive(false);
            }

            Message.AddListener<UI.Event.CurrencyChange>(CurrencyUpdate);
            Message.AddListener<UI.Event.CharacterInfoUIUpdate>(StatUpdate);
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyUpdate);
            Message.RemoveListener<UI.Event.CharacterInfoUIUpdate>(StatUpdate);
        }
   
        void TouchevolutionUP()
        {
            BigInteger needsoul = Enforcement.enForce_Data.myEnforceData.need_soul;
            if (needsoul > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Soul).value)
                return;
            BigInteger needgem = Enforcement.enForce_Data.myEnforceData.need_;
            if (needgem > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value)
                return;

            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Soul, -needsoul);
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Gem, -needgem);
            Enforcement.AwakeLevel++;
            Enforcement.EnchentLevel = 0;
            Enforcement.Update();

            InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.CHA_EVOLUTION, 1);
        }

        void TouchenforceUP()
        {
            if (Enforcement.EnchentLevel >= 10)
                return;
            BigInteger needsoul = Enforcement.enForce_Data.myEnforceData.need_soul;
            if (needsoul <= Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Soul).value)
            {
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.Soul, -needsoul);

                Enforcement.EnchentLevel++;
                Enforcement.TotalEnchentLevel++;
                Enforcement.Update();

                InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.CHA_ENCHANT, 1);
            }
            
        }

        void ChangeEnforceInfo()
        {
            for (int i = 0; i < EnforceImage.Length; i++)
            {
                if (i < Enforcement.EnchentLevel)
                    EnforceImage[i].color = new Color(1, 1, 1, 1);
                else
                    EnforceImage[i].color = new Color(0, 0, 0, 1);
            }

            for (int i = 0; i < AwakeImage.Length; i++)
            {
                if (i < Enforcement.AwakeLevel)
                    AwakeImage[i].color = new Color(1, 1, 1, 1);
                else
                    AwakeImage[i].color = new Color(0, 0, 0, 1);
            }

          

            BigInteger needsoul = Enforcement.enForce_Data.myEnforceData.need_soul;
            bool acrivebtn = needsoul > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Soul).value;
            CantenforceBtn.gameObject.SetActive(acrivebtn);
            CantAwakeBtn.gameObject.SetActive(acrivebtn);
            
            BigInteger needgem = Enforcement.enForce_Data.myEnforceData.need_;
            if (needgem > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value)
            {
                //CantenforceBtn.gameObject.SetActive(true);
                CantAwakeBtn.gameObject.SetActive(true);
            }
            if (Enforcement.EnchentLevel >= 10)
            {
                enforceBtn.gameObject.SetActive(true);
                CantenforceBtn.gameObject.SetActive(true);
                AwakeBtn.gameObject.SetActive(true);
                AwakeBtn.gameObject.SetActive(true);
            }
            else
            {
                enforceBtn.gameObject.SetActive(true);
                AwakeBtn.gameObject.SetActive(true);
                CantAwakeBtn.gameObject.SetActive(true);
                AwakeBtn.gameObject.SetActive(false);
            }
           
            Enforce_data data = Enforcement.enForce_Data;

            CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.Enforce,data.abilitytype,8300001,data.AttackImproveRate);

            EnforceNeedCurrency.text = string.Format("{0}", data.myEnforceData.need_soul);
            AwakeNeedSoulCurrency.text= string.Format("{0}", data.myEnforceData.need_soul);
            AwakeNeedGemCurrency.text = string.Format("{0}", data.myEnforceData.need_);

            AwakeInfoText.text= string.Format("공격력 강화 LV{0}달성시 가능", (Enforcement.AwakeLevel+1)*10);
            EnforceLvInfo.text = string.Format("강화 LV.{0}", (Enforcement.AwakeLevel) * 10 + Enforcement.EnchentLevel);
            AwakelvText.text = string.Format("초월 등급 {0}", Enforcement.enForce_Data.myEnforceData.awake);
            BigInteger currentatk= CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage();
            int enchant_totallv = 0;
            CharacterEnchant nextEnchant;
            if (Enforcement.enForce_Data.myEnforceData.enchant_ui<100)
            {
                if (Enforcement.enForce_Data.myEnforceData.enchant<10)
                {
                    nextEnchant = InGameDataTableManager.CharacterList.Enchant.Find(o => o.awake == Enforcement.enForce_Data.myEnforceData.awake &&
                     o.enchant == Enforcement.enForce_Data.myEnforceData.enchant+1);
                }
                else
                {
                    nextEnchant = InGameDataTableManager.CharacterList.Enchant.Find(o => o.enchant_ui == Enforcement.enForce_Data.myEnforceData.enchant_ui &&
                    o.enchant==0);
                }
                
            }
            else
            {
                nextEnchant = null;
            }
            if(nextEnchant!=null)
            {
                BigInteger nextatk = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetNormalAtkDamage()*nextEnchant.enchant_gain_rate;
                AttackInfo.text = string.Format("{0}>>{1}", currentatk.ToDisplay(),nextatk.ToDisplay());
            }
            else
            {
                AttackInfo.text = null;
            }
            
            //Message.Send<UI.Event.CharacterInfoUIUpdate>(new UI.Event.CharacterInfoUIUpdate());
        }

        void StatUpdate(UI.Event.CharacterInfoUIUpdate msg)
        {
            BigInteger currentatk = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage();
            CharacterEnchant nextEnchant;
            if (Enforcement.enForce_Data.myEnforceData.enchant_ui < 100)
            {
                if (Enforcement.enForce_Data.myEnforceData.enchant < 10)
                {
                    nextEnchant = InGameDataTableManager.CharacterList.Enchant.Find(o => o.awake == Enforcement.enForce_Data.myEnforceData.awake &&
                     o.enchant == Enforcement.enForce_Data.myEnforceData.enchant + 1);
                }
                else
                {
                    nextEnchant = InGameDataTableManager.CharacterList.Enchant.Find(o => o.enchant_ui == Enforcement.enForce_Data.myEnforceData.enchant_ui &&
                    o.enchant == 0);
                }

            }
            else
            {
                nextEnchant = null;
            }
            if (nextEnchant != null)
            {
                BigInteger nextatk = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetNormalAtkDamage() * nextEnchant.enchant_gain_rate;
                AttackInfo.text = string.Format("{0}>>{1}", currentatk.ToDisplay(), nextatk.ToDisplay());
            }
            else
            {
                AttackInfo.text = null;
            }
        }
        void CurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.Soul && msg.Type != CurrencyType.Gem)
                    return;
            }
            BigInteger needsoul = Enforcement.enForce_Data.myEnforceData.need_soul;
            bool acrivebtn = needsoul > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Soul).value;
            CantenforceBtn.gameObject.SetActive(acrivebtn);
            CantAwakeBtn.gameObject.SetActive(acrivebtn);

            BigInteger needgem = Enforcement.enForce_Data.myEnforceData.need_;
            if (needgem > Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Gem).value)
            {
                //CantenforceBtn.gameObject.SetActive(true);
                CantAwakeBtn.gameObject.SetActive(true);
            }

            if (Enforcement.EnchentLevel >= 10)
            {
                CantenforceBtn.gameObject.SetActive(true);
                AwakeBtn.gameObject.SetActive(true);
            }
            else
            {
                AwakeBtn.gameObject.SetActive(false);
                CantAwakeBtn.gameObject.SetActive(true);
            }
            
        }
    }
}
