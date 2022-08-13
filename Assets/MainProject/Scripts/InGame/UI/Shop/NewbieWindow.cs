using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class NewbieWindow : MonoBehaviour
    {
        [SerializeField] Text packagetext;
        [SerializeField] Button PackageBuy;

        Newbieinfo currentPackage;
        Newbieinfo NextPackage;
        public void Init()
        {
            currentPackage = Common.InGameManager.Instance.GetPlayerData.GlobalUser.currentnewbieinfo;
            if (currentPackage==null)
            {
                NextPackage = InGameDataTableManager.NewbiePackage.newbie.Find(o => o.idx == 290001);
            }
            else
            {
                NextPackage = InGameDataTableManager.NewbiePackage.newbie.Find(o => o.idx == currentPackage.next_idx);
            }

            if (NextPackage == null)
            {
                packagetext.text = "패키지 구매 완료";
                PackageBuy.gameObject.SetActive(false);
            }
            else
            {
                AbilityInfo abilinfo_1 = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == NextPackage.a_aidx_1);
                LocalValue abil_1 = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo_1.name);
                AbilityInfo abilinfo_2 = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == NextPackage.a_aidx_2);
                LocalValue abil_2 = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo_2.name);
                AbilityInfo abilinfo_3 = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == NextPackage.a_aidx_3);
                LocalValue abil_3 = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo_3.name);

                LocalValue gemlocal = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == CurrencyType.Gem.ToString());
                LocalValue mileagelocal = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == CurrencyType.Mileage.ToString());

                packagetext.text = string.Format("<color=green>{0}</color> :<color=red>{1}%</color>\n" +
                    "<color=green>{2}</color> :<color=red>{3}%</color>\n" +
                    "<color=green>{4}</color> :<color=red>{5}%</color>\n" +
                    "<color=green>{6}</color> :<color=red>{7}</color>\n" +
                    "<color=green>{8}</color> :<color=red>{9}</color>\n", abil_1.GetStringForLocal(true), (int)abilinfo_1.level_unit,
                     abil_2.GetStringForLocal(true), (int)abilinfo_2.level_unit, abil_3.GetStringForLocal(true), (int)abilinfo_3.level_unit,
                      gemlocal.GetStringForLocal(true), NextPackage.reward_gem, mileagelocal.GetStringForLocal(true), NextPackage.reward_mileage);

                PackageBuy.gameObject.SetActive(true);
            }

            PackageBuy.onClick.AddListener(BuyCurrentPackage);
        }

        public void Release()
        {
            PackageBuy.onClick.RemoveAllListeners();
        }

        void BuyCurrentPackage()
        {
            //유니티 구매

            //유니티 구매

            //구매 후 처리
            if(Common.InGameManager.Instance.GetPlayerData.GlobalUser.currentnewbieinfo == null)
            {
                Common.InGameManager.Instance.GetPlayerData.GlobalUser.currentnewbieinfo = InGameDataTableManager.NewbiePackage.newbie.Find(o=>o.idx==290001);
                currentPackage = Common.InGameManager.Instance.GetPlayerData.GlobalUser.currentnewbieinfo;
                NextPackage= InGameDataTableManager.NewbiePackage.newbie.Find(o => o.idx == currentPackage.next_idx);
                Common.InGameManager.Instance.GetPlayerData.GlobalUser.CurrentNewbiePackageIdx = currentPackage.idx;
            }
            else
            {
                Common.InGameManager.Instance.GetPlayerData.GlobalUser.currentnewbieinfo = InGameDataTableManager.NewbiePackage.newbie.Find
                    (o => o.idx == Common.InGameManager.Instance.GetPlayerData.GlobalUser.currentnewbieinfo.next_idx);
                currentPackage = Common.InGameManager.Instance.GetPlayerData.GlobalUser.currentnewbieinfo;
                NextPackage = InGameDataTableManager.NewbiePackage.newbie.Find(o => o.idx == currentPackage.next_idx);
                Common.InGameManager.Instance.GetPlayerData.GlobalUser.CurrentNewbiePackageIdx = currentPackage.idx;
            }
            if(NextPackage == null)
            {
                packagetext.text = "패키지 구매 완료";
                PackageBuy.gameObject.SetActive(false);
            }
            else
            {
                AbilityInfo abilinfo_1 = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx ==NextPackage.a_aidx_1);
                LocalValue abil_1 = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo_1.name);
                AbilityInfo abilinfo_2 = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == NextPackage.a_aidx_2);
                LocalValue abil_2 = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo_2.name);
                AbilityInfo abilinfo_3 = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == NextPackage.a_aidx_3);
                LocalValue abil_3 = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == abilinfo_3.name);

                LocalValue gemlocal = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == CurrencyType.Gem.ToString());
                LocalValue mileagelocal = InGameDataTableManager.LocalizationList.currency.Find(o => o.id == CurrencyType.Mileage.ToString());

                packagetext.text = string.Format("<color=green>{0}</color> :<color=red>{1}%</color>\n" +
                    "<color=green>{2}</color> :<color=red>{3}%</color>\n"+
                    "<color=green>{4}</color> :<color=red>{5}%</color>\n"+
                    "<color=green>{6}</color> :<color=red>{7}</color>\n"+
                    "<color=green>{8}</color> :<color=red>{9}</color>\n", abil_1.GetStringForLocal(true),(int)abilinfo_1.level_unit,
                     abil_2.GetStringForLocal(true), (int)abilinfo_2.level_unit, abil_3.GetStringForLocal(true), (int)abilinfo_3.level_unit,
                      gemlocal.GetStringForLocal(true), NextPackage.reward_gem, mileagelocal.GetStringForLocal(true), NextPackage.reward_mileage);

                PackageBuy.gameObject.SetActive(true);
            }
        }
    }

}
