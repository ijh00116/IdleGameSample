using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class MainDialogButtonText : MonoBehaviour
    {
        [SerializeField] Text Contents;
        [SerializeField] Text Hero;
        [SerializeField] Text Pet;
        [SerializeField] Text Equip;
        [SerializeField] Text Quest;
        [SerializeField] Text Relic;
        [SerializeField] Text Shop;

        public void Init()
        {
            LocalValue ContentLocal = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_001");
            LocalValue HeroLoacl= InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_002");
            LocalValue PetLocal= InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_003");
            LocalValue EquipLocal = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_004");
            LocalValue QuestLocal = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_005");
            LocalValue RelicLocal = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_006");
            LocalValue ShopLocal = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_007");

            Contents.text = ContentLocal.GetStringForLocal(true);
            Hero.text = HeroLoacl.GetStringForLocal(true);
            Pet.text = PetLocal.GetStringForLocal(true);
            Equip.text = EquipLocal.GetStringForLocal(true);
            Quest.text = QuestLocal.GetStringForLocal(true);
            Relic.text = RelicLocal.GetStringForLocal(true);
            Shop.text = ShopLocal.GetStringForLocal(true);
        }

        public void Release()
        {

        }
    }

}
