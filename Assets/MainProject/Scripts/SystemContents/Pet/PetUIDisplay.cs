using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlackTree
{
    public class PetUIDisplay : MonoBehaviour
    {
        [SerializeField] UI.BottomDialogType _type;

        [SerializeField] Text petName;
        [SerializeField] Image PetImage;
        [Header("버튼")]
        [SerializeField] Button LevelupButton;
        [SerializeField] GameObject PossibleUpArrow;
        [SerializeField] Button ComposeButton;
        [SerializeField] Button EquipButton;

        [SerializeField] Button CantLevelupButton;
        [SerializeField] Button CantComposeButton;
        [SerializeField] Button CantDecomposeButton;

        [SerializeField] Button UnlockedButton;

        [Header("능력치 정보")]
        [SerializeField] Text Level;
        [SerializeField] Text EquipAbillity;
        [SerializeField] Text CollectAbillity;
        [SerializeField] Text NeedLevelUpCost;

        [Header("수량")]
        [SerializeField] Slider amountBar;
        [SerializeField] Image AmountbarImage;
        [SerializeField] Text amountText;
        [SerializeField] Text amountText_0;

        [Header("수량 색깔 변화")]
        [SerializeField] Sprite GreenBar;
        [SerializeField] Sprite YellowBar;
        [Header("펫 이미지 넣기")]
        [SerializeField] Image PetImage_0;
        [SerializeField] Image PetImage_1;

        public PetInventorySlot slot;
        [HideInInspector]public Sprite CurrentPetImagesprite;
        [HideInInspector] public string LocalpetName;

        string useAbilitylocal;
        string CollectAbilitylocal;

        bool UpdateLvup;
        Coroutine LvBtnpushevent = null;
        WaitForSeconds btnpushdelay = new WaitForSeconds(0.5f);

        bool UpdataCompose;
        Coroutine ComposeBtnpushevent = null;
        public void Init(PetInventorySlot _slot)
        {
            //LevelupButton.onClick.AddListener(LevelUpButton);
            //ComposeButton.onClick.AddListener(PushCompose);
            EquipButton.onClick.AddListener(Equip);

            slot = _slot;
            slot.slotDisplay = this;
            slot.onAfterUpdated += SlotUIUpdate;

            string path ="Images/GUI/Pet/"+ slot.petData.petInfo.icon;
            CurrentPetImagesprite = Resources.Load<Sprite>(path);
            PetImage.sprite = CurrentPetImagesprite;
            //PetImage_0.sprite = CurrentPetImagesprite;
            PetImage_1.sprite = CurrentPetImagesprite;

            Message.AddListener<UI.Event.CurrencyChange>(CurrencyChangeUpdate);

            AbilityInfo info = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == slot.petData.petInfo.use_aidx);
            LocalValue lv = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == info.name);
            useAbilitylocal = lv.GetStringForLocal(true);

            AbilityInfo _info = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == slot.petData.petInfo.collect_aidx);
            LocalValue _lv = InGameDataTableManager.LocalizationList.ability.Find(o => o.id == _info.name);
            CollectAbilitylocal = _lv.GetStringForLocal(true);

            //이벤트 트리거 lv업
            EventTrigger trigger = LevelupButton.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => LvBtnDown());
            trigger.triggers.Add(pointerDown);

            var pointerDown_1 = new EventTrigger.Entry();
            pointerDown_1.eventID = EventTriggerType.PointerUp;
            pointerDown_1.callback.AddListener((e) => LvBtnUp());
            trigger.triggers.Add(pointerDown_1);
            UpdateLvup = false;

            //이벤트 트리거 합성
            EventTrigger _trigger = ComposeButton.gameObject.AddComponent<EventTrigger>();
            var _pointerDown = new EventTrigger.Entry();
            _pointerDown.eventID = EventTriggerType.PointerDown;
            _pointerDown.callback.AddListener((e) => ComposeBtnDown());
            _trigger.triggers.Add(_pointerDown);

            var _pointerDown_1 = new EventTrigger.Entry();
            _pointerDown_1.eventID = EventTriggerType.PointerUp;
            _pointerDown_1.callback.AddListener((e) => ComposeBtnUp());
            _trigger.triggers.Add(_pointerDown_1);
            UpdataCompose = false;
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyChangeUpdate);
        }

        private void Update()
        {
            
        }

        //펫 슬롯에 대한 UI 업데이트
        void SlotUIUpdate()
        {
            LocalValue petnamelocal = InGameDataTableManager.LocalizationList.pet.Find(o => o.id == slot.petData.petInfo.name);
            LocalpetName = petnamelocal.GetStringForLocal(true);
            petName.text = LocalpetName;
            PetData _data = slot.petData;

            if(slot.pet.Unlocked==true)
            {
                ComposeButton.gameObject.SetActive(true);
                LevelupButton.gameObject.SetActive(true);
                EquipButton.gameObject.SetActive(true);
                UnlockedButton.gameObject.SetActive(false);
            }
            else
            {
                ComposeButton.gameObject.SetActive(false);
                LevelupButton.gameObject.SetActive(false);
                EquipButton.gameObject.SetActive(false);
                UnlockedButton.gameObject.SetActive(true);
            }
            CantComposeButton.gameObject.SetActive(slot.pet.amount < 5);
            ComposeButton.enabled = (slot.pet.amount >= 5);

            
            EquipAbillity.text =string.Format("{0} {1}", useAbilitylocal, _data.EquipPetAbillity.ToFloat());
            CollectAbillity.text = string.Format("{0} {1}", CollectAbilitylocal, _data.CollectPetAbillity.ToFloat());
            Level.text ="Lv. "+ slot.pet.Level.ToString();

            if (slot.pet.Equiped)
            {
                EquipButton.transform.GetChild(0).gameObject.SetActive(true);
                EquipButton.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                EquipButton.transform.GetChild(0).gameObject.SetActive(false);
                EquipButton.transform.GetChild(1).gameObject.SetActive(true);
            }
                
            //레벨업 가능
            bool IsLevelup = (slot.pet.amount >= 1);

            LevelupButton.enabled=(IsLevelup);
            CantLevelupButton.gameObject.SetActive(!IsLevelup);

            CantDecomposeButton.gameObject.SetActive(slot.pet.amount <=0);

            amountBar.value = (float)slot.pet.amount / 5.0f;
            if (amountBar.value >= 1)
            {
                AmountbarImage.sprite = GreenBar;
                PossibleUpArrow.gameObject.SetActive(true);
            }
            else
            {
                AmountbarImage.sprite = YellowBar;
                PossibleUpArrow.gameObject.SetActive(false);
            }
                
            amountText.text = string.Format("{0}",slot.pet.amount);
            //amountText_0.text= string.Format("{0}", slot.pet.amount);

            if(slot.pet.Unlocked)
            {
                PetImage.color = Color.white;
            }
            else
            {
                PetImage.color = Color.black;
                EquipButton.transform.GetChild(0).gameObject.SetActive(false);
                EquipButton.transform.GetChild(1).gameObject.SetActive(false);
            }

            if(slot.pet.Level>=slot.petData.petInfo.max_level)
            {
                LevelupButton.gameObject.SetActive(false);
                CantLevelupButton.gameObject.SetActive(true);
            }
        }

        void LvBtnUp()
        {
            UpdateLvup = false;
            Common.InGameManager.Instance.Localdata.SavepetData();
        }
        void LvBtnDown()
        {
            UpdateLvup = true;
            if (LvBtnpushevent != null)
                StopCoroutine(LvBtnpushevent);
            LvBtnpushevent = null;
            LevelUpButton();
            LvBtnpushevent = StartCoroutine(LvBtnPush());
        }
        IEnumerator LvBtnPush()
        {
            yield return btnpushdelay;

            while (UpdateLvup)
            {
                LevelUpButton();
                yield return null;
            }
            yield break;
        }
        public void LevelUpButton()
        {
            //if(Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.Pet_Food).VALUE>slot.petData.PetLevelupCost_1)
            if (slot.pet.amount >= 1)
            {
                slot.AddAmount(-1);
                slot.AddLevel(1);
                Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.PET_LEVELUP, 1);
            }
            //레벨업 강화 요건 필요
        }

        void ComposeBtnUp()
        {
            UpdataCompose = false;
            Common.InGameManager.Instance.Localdata.SavepetData();
        }
        void ComposeBtnDown()
        {
            UpdataCompose = true;
            if (ComposeBtnpushevent != null)
                StopCoroutine(ComposeBtnpushevent);
            ComposeBtnpushevent = null;
            PushCompose();
            ComposeBtnpushevent = StartCoroutine(ComposeBtnPush());
        }
        IEnumerator ComposeBtnPush()
        {
            yield return btnpushdelay;

            while (UpdataCompose)
            {
                PushCompose();
                yield return null;
            }
            yield break;
        }

        public void PushCompose()
        {
            if(slot.pet.amount>=5)
            {
                slot.Compose();
                Common.InGameManager.Instance.GetPlayerData.saveData.missionInfo.IncMissionValue(MissionType.PET_MIX, 1);
                Common.InGameManager.Instance.Localdata.SavepetData();
            }
        }

      
        public void Equip()
        {
            if(slot.pet.Unlocked==true)
            {
                if(slot.pet.Equiped)
                {
                    slot.UnEquipPet();
                }
                else
                {
                    slot.EquipPet();
                }
                Common.InGameManager.Instance.Localdata.SavepetData();
            }
        }

        void CurrencyChangeUpdate(UI.Event.CurrencyChange msg)
        {
            if (slot.pet.Unlocked == false)
                return;
           
            // if (_type != Common.InGameManager.Instance.BottomDialogtype)
            //     return;

            bool IsLevelup = (slot.pet.amount >= 1);

            LevelupButton.enabled = (IsLevelup);
            CantLevelupButton.gameObject.SetActive(!IsLevelup);
          

            EquipButton.gameObject.SetActive(slot.pet.Unlocked);

            ComposeButton.enabled = (slot.pet.amount >= 5);
            CantComposeButton.gameObject.SetActive(slot.pet.amount < 5);

            CantDecomposeButton.gameObject.SetActive(slot.pet.amount <= 0);

            amountBar.value = (float)slot.pet.amount / 5.0f;
            if (amountBar.value >= 1)
            {
                AmountbarImage.sprite = GreenBar;
                PossibleUpArrow.gameObject.SetActive(true);
            }
            else
            {
                AmountbarImage.sprite = YellowBar;
                PossibleUpArrow.gameObject.SetActive(false);
            }
                
            amountText.text = string.Format("{0}/5", slot.pet.amount);
        }

    }

}
