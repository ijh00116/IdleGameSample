using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using BlackTree.InGame;

namespace BlackTree
{
    public class CharacterShapeInPet : MonoBehaviour
    {
        public SkeletonAnimation skeletonanimation;
        Shader attachmentshader;

        [SpineSlot] public string WeaponSlot;
        [SpineSlot] public string Wing_lSlot;
        [SpineSlot] public string Wing_rSlot;
        Slot weapon_EquipSlot;
        Slot wing_lEquipSlot;
        Slot wing_rEquipSlot;
        RegionAttachment currentattachment;

        List<string> CharacterSkinName = new List<string>();
        float scale = 1.5f;
        private void Awake()
        {
            for (int i = 0; i < InGameDataTableManager.ItemTableList.costum.Count; i++)
            {
                CharacterSkinName.Add(InGameDataTableManager.ItemTableList.costum[i].SkinName);
            }

            attachmentshader = Shader.Find("Spine/Skeleton");
        }
        public void ShapeChange(UserInfoForPVP info)
        {
            if (weapon_EquipSlot == null)
            {
                weapon_EquipSlot = skeletonanimation.skeleton.FindSlot(WeaponSlot);
            }
            if (weapon_EquipSlot != null)
                weapon_EquipSlot.Attachment = currentattachment;

            if (wing_lEquipSlot == null)
            {
                wing_lEquipSlot = skeletonanimation.skeleton.FindSlot(Wing_lSlot);
            }
            if (wing_lEquipSlot != null)
                wing_lEquipSlot.Attachment = currentattachment;

            if (wing_rEquipSlot == null)
            {
                wing_rEquipSlot = skeletonanimation.skeleton.FindSlot(Wing_rSlot);
            }
            if (wing_rEquipSlot != null)
                wing_rEquipSlot.Attachment = currentattachment;

            string currnetAnimName = skeletonanimation.AnimationName;
            if (currnetAnimName == null)
                currnetAnimName = "run";

            skeletonanimation.skeleton.SetSkin(info.SkinName);
            skeletonanimation.skeleton.SetSlotsToSetupPose();
            skeletonanimation.AnimationState.SetAnimation(0, currnetAnimName, true);

            this.GetComponent<CharacterHandleWeaponInPVP>().SpineAnimationEventSetting();

            Sprite _sprite = null;
            foreach (var _data in Common.InGameManager.Instance.WeaponInventory.GetSlots)
            {
                if (_data.itemData.itemInfo.sprite == info.weaponName)
                {
                    _sprite = _data.itemData.mySprite_R;
                }
            }
            if (_sprite != null)
            {
                currentattachment = _sprite.ToRegionAttachmentPMAClone(attachmentshader);

                currentattachment.SetScale(scale, scale);
                currentattachment.UpdateOffset();

                weapon_EquipSlot.Attachment = currentattachment;
            }

            Sprite _sprite_r = null;
            Sprite _sprite_l = null;
            foreach (var _data in Common.InGameManager.Instance.WingInventory.GetSlots)
            {
                if (_data.itemData.itemInfo.sprite == info.wingName)
                {
                    _sprite_r = _data.itemData.mySprite_R;
                    _sprite_l = _data.itemData.mySprite_L;
                }
            }
            if (_sprite_r != null)
            {
                currentattachment = _sprite_r.ToRegionAttachmentPMAClone(attachmentshader);

                currentattachment.SetScale(scale, scale);
                currentattachment.UpdateOffset();

                wing_rEquipSlot.Attachment = currentattachment;
            }

            if (_sprite_l != null)
            {
                currentattachment = _sprite_l.ToRegionAttachmentPMAClone(attachmentshader);

                currentattachment.SetScale(scale, scale);
                currentattachment.UpdateOffset();

                wing_lEquipSlot.Attachment = currentattachment;
            }
        }
    }
}

