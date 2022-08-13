using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ContentTitle : MonoBehaviour
    {
        public Button DungeonButton;
        public Button PetDungeonButton;
        public Button PVPDungeonButton;

        public GameObject DungeonWindow;
        public GameObject PetDungeonWindow;
        public GameObject PVPDungeonWindow;

        public Button BackButton_dungeon;
        public Button BackButton_Pet;
        public Button BackButton_PVP;

        public PetDungeonWindowUI petDungeonWindow;
        [SerializeField] PVPWindowUI pvpWindow;

        public Text DungeonTicketNum;
        public Text PetDungeonTicketNum;

        public void Init()
        {
            petDungeonWindow.Init();
            //pvpWindow.Init();

            DungeonButton.onClick.AddListener(OnDungeonWindow);
            PetDungeonButton.onClick.AddListener(OnPetDungeonWindow);
            PVPDungeonButton.onClick.AddListener(OnPVPWindow);
            BackButton_dungeon.onClick.AddListener(AllInActive);
            BackButton_Pet.onClick.AddListener(AllInActive);
            BackButton_PVP.onClick.AddListener(AllInActive);

            AllInActive();

            DungeonTicketNum.text = string.Format("x{0}" ,Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_Dungeon).value.ToDisplay());
            PetDungeonTicketNum.text = string.Format("x{0}", Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_PetDungeon).value.ToDisplay());

            Message.AddListener<UI.Event.SideWindowPopup>(OpenPetDungeon);
            Message.AddListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
            Message.AddListener<UI.Event.CurrencyChange>(CurrencyChanged);
        }

        public void Release()
        {
            petDungeonWindow.Release();
            //pvpWindow.Release();

            Message.RemoveListener<UI.Event.SideWindowPopup>(OpenPetDungeon);
            Message.RemoveListener<UI.Event.OtherUIPopup>(OtherUIPopuped);
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyChanged);

        }
        void OnDungeonWindow()
        {
            DungeonWindow.SetActive(true);
        }

        void OnPetDungeonWindow()
        {
            PetDungeonWindow.SetActive(true);
            petDungeonWindow.PopupMyWindow();
        }
        void OnPVPWindow()
        {
            PVPDungeonWindow.SetActive(true);
            PVPDungeonWindow.GetComponent<PVPWindowUI>().PvpUIPopup();
        }

        void AllInActive()
        {
            DungeonWindow.SetActive(false);
            PetDungeonWindow.SetActive(false);
            PVPDungeonWindow.SetActive(false);
        }

        void OpenPetDungeon(UI.Event.SideWindowPopup msg)
        {
            if (msg.type ==UI.SideButtonType.QuickPetButton)
                PetDungeonWindow.SetActive(true);
        }

        void OtherUIPopuped(UI.Event.OtherUIPopup msg)
        {
            if (msg.PopupUI != this.gameObject)
            {
                AllInActive();
            }
            else
            {
                
            }
        }

        void CurrencyChanged(UI.Event.CurrencyChange msg)
        {
            if(msg.Type==CurrencyType.Ticket_Dungeon ||
                msg.Type == CurrencyType.Ticket_PetDungeon)
            {
                DungeonTicketNum.text = string.Format("x{0}", Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_Dungeon).value.ToDisplay());
                PetDungeonTicketNum.text = string.Format("x{0}", Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Ticket_PetDungeon).value.ToDisplay());
            }
        }
    }

}
