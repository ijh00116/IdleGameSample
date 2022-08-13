using BlackTree.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class DungeonWindowUI : MonoBehaviour
    {
        public GameObject DungeonUnit;
        public GameObject Content;

        public DungeonInfoUI dungeonInfo;

        public Text CowRoomName;

        [SerializeField] Text SoulText;
        [SerializeField] Text EnchantstoneText;
        [SerializeField] Text MagicStoneText;
        [SerializeField] Text DungeonTicketText;

        void Start()
        {
            dungeonInfo.Init();

            for (int i=0; i< InGameDataTableManager.DungeonTableList.Dungeon.Count; i++)
            {
                var obj = Instantiate(DungeonUnit, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(Content.transform, false);
                DungeonUnitUI unit = obj.GetComponent<DungeonUnitUI>();
                unit.Init(InGameDataTableManager.DungeonTableList.Dungeon[i].dg_stage);
            }

            LocalValue DungeonName = InGameDataTableManager.LocalizationList.info.Find(o => o.id == "info_050");

            CowRoomName.text = DungeonName.GetStringForLocal(true);

            Message.AddListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);

            SoulText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Soul).value.ToDisplay();
            EnchantstoneText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.EnchantStone).value.ToDisplay();
            MagicStoneText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicStone).value.ToDisplay();
            DungeonTicketText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_Dungeon).value.ToDisplay();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDestroy()
        {
            dungeonInfo.Release();
            Message.RemoveListener<UI.Event.CurrencyChange>(GetCurrencyUpdate);
        }

        void GetCurrencyUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.Type == CurrencyType.MagicStone ||
                msg.Type == CurrencyType.Soul ||
                msg.Type == CurrencyType.EnchantStone ||
                msg.Type == CurrencyType.Ticket_Dungeon)
            {
                SoulText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Soul).value.ToDisplay();
                EnchantstoneText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.EnchantStone).value.ToDisplay();
                MagicStoneText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.MagicStone).value.ToDisplay();
                DungeonTicketText.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_Dungeon).value.ToDisplay();
            }
        }
    }

}
