using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class ItemUIDisplay : MonoBehaviour
    {
        public InventorySlot slot;

        [SerializeField] ItemTouch InfoButton;

        [SerializeField] Text isEquiped;
        [SerializeField] Text AmountText;
        [SerializeField] Slider ItemAmount;

        [SerializeField] Text GradeText;

        [SerializeField] Image UIImage;
        [SerializeField] Image LockImage;
        [SerializeField] Image OutUIImage;

        [SerializeField] Text LevelText;

        [Header("초월 별 갯수")]
        [SerializeField] Image[] StarImages;
        
        [HideInInspector]public Sprite currentSpriteImage;
        [HideInInspector] public Sprite currentOutlineSpriteImage;
        [HideInInspector] public string _GradeText;
        [HideInInspector] public string ItemName;


        [SerializeField] Image amountFillImage;
        [SerializeField] GameObject PossibleLvUpArrow;
        [SerializeField] Sprite FillYellowImage;
        [SerializeField] Sprite FillGreenImage;
        public void Init(InventorySlot _slot,ScrollRect rect)
        {
            slot = _slot;

            InfoButton.Init(PopupItemInfo, rect);

            slot.onAfterUpdated += SlotUIUpdate;
            slot.onBeforeUpdated += SlotUIBeforUpdate;

            LocalValue _itemname = InGameDataTableManager.LocalizationList.weapon.Find(o => o.id == slot.itemData.itemInfo.grade_name);
            ItemName = _itemname.GetStringForLocal(true);
        }

        private void Awake()
        {
        }

        private void OnDestroy()
        {
        }

        void PopupItemInfo()
        {
            Message.Send<UI.Event.PopupItemInformationUI>(new UI.Event.PopupItemInformationUI(this));
        }

        public void SlotUIUpdate()
        {
            if(gameObject.activeInHierarchy==false)
                this.gameObject.SetActive(true);

            if (currentSpriteImage == null)
            {
                if(slot.itemData.itemtype==ItemType.weapon)
                    currentSpriteImage = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Weapon/{0}", slot.itemData.itemInfo.icon));
                else if (slot.itemData.itemtype == ItemType.wing)
                    currentSpriteImage = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Wing/{0}", slot.itemData.itemInfo.icon));
            }
            if(currentOutlineSpriteImage==null)
            {
                if (slot.itemData.itemtype == ItemType.weapon)
                    currentOutlineSpriteImage = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Weapon/{0}", slot.itemData.itemInfo.outline_sprite));
                else if (slot.itemData.itemtype == ItemType.wing)
                    currentOutlineSpriteImage = Resources.Load<Sprite>(string.Format("Images/GUI/Item/Weapon/{0}", slot.itemData.itemInfo.outline_sprite));
            }
            if (slot.item.Unlocked == false)
            {
                UIImage.color = new Color(0, 0, 0, 1);
                LockImage.gameObject.SetActive(true);
            }
            else
            {
                UIImage.color = new Color(1, 1, 1, 1);
                LockImage.gameObject.SetActive(false);
            }
                
            UIImage.sprite = currentSpriteImage;
            OutUIImage.sprite = currentOutlineSpriteImage;


            if (isEquiped!=null)
                isEquiped.text = (slot.item.Equiped) ? "장착" : null;

            ItemAmount.value = slot.item.amount / 10.0f;
            AmountText.text = string.Format("{0}/5", slot.item.amount);

            switch(slot.itemData.itemInfo.grade)
            {
                case "d":
                    _GradeText = string.Format("저급({0}등급)", slot.item.AwakeLv);
                    break;
                case "b":
                    _GradeText = string.Format("일반({0}등급)", slot.item.AwakeLv);
                    break;
                case "c":
                    _GradeText = string.Format("고급({0}등급)", slot.item.AwakeLv);
                    break;
                case "a":
                    _GradeText = string.Format("영웅({0}등급)", slot.item.AwakeLv);
                    break;
                case "s":
                    _GradeText = string.Format("전설({0}등급)", slot.item.AwakeLv);
                    break;
                case "ss":
                    _GradeText = string.Format("신화({0}등급)", slot.item.AwakeLv);
                    break;
            }
            GradeText.text = _GradeText;

            LevelText.text = string.Format("Lv.{0}", slot.item.Level); 

            for(int i=0; i< StarImages.Length; i++)
            {
                StarImages[i].gameObject.SetActive(i < slot.item.AwakeLv);
            }

            //레벨업 가능할때 이미지 교체
            if(ItemAmount.value>=1)
            {
                amountFillImage.sprite = FillGreenImage;
                PossibleLvUpArrow.SetActive(true);
            }
            else
            {
                amountFillImage.sprite = FillYellowImage;
                PossibleLvUpArrow.SetActive(false);
            }
        }

        public void SlotUIBeforUpdate()
        {
            if (isEquiped != null)
                isEquiped.text = (slot.item.Equiped) ? "장착" : null;
        }

     
    }

}
