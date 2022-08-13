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
    public class CharacterShape : CharacterAbility
    {
        [SerializeField]SkeletonDataAsset SkinAsset_0;
        [SerializeField] SkeletonDataAsset SkinAsset_1;
        public SkeletonAnimation skeletonanimation;

        float scale = 1.5f;
        Shader attachmentshader;
        [SpineSlot] public string WeaponSlot;
        [SpineSlot] public string WingSlot_Left;
        [SpineSlot] public string WingSlot_Right;

        Slot WeaponEquipSlot;
        Slot Wing_LEquipSlot;
        Slot Wing_REquipSlot;
        RegionAttachment currentWeaponattachment;
        RegionAttachment currentWing_Lattachment;
        RegionAttachment currentWing_Rattachment;
        Sprite CurrentWeaponsprite;

        Sprite CurrentWingSprite_L;
        Sprite CurrentWingSprite_R;

        bool IsWingParticle=false;

        float currentChagneTime=0;
        bool CanChangeCostumSkin=false;

        [SerializeField] List<GameObject> Wing=new List<GameObject>();
        Dictionary<string, GameObject> winglist = new Dictionary<string, GameObject>();
        protected override void OnDestroy()
        {
            Message.RemoveListener<UI.Event.ShapeChange>(CharacterShapeChange);
        }
        protected override void Start()
        {
            base.Start();
            Message.AddListener<UI.Event.ShapeChange>(CharacterShapeChange);
            Init();
            currentChagneTime = 0;
            CanChangeCostumSkin = true;

            for(int i=0; i< Wing.Count; i++)
            {
                winglist.Add("Wing" + (26 + i).ToString(), Wing[i]);
                Wing[i].SetActive(false);
            }
        }

       
        protected override void ProcessAbility()
        {
            base.ProcessAbility();
            if(CanChangeCostumSkin==false)
            {
                currentChagneTime += Time.deltaTime;
                if(currentChagneTime>2.0f)
                {
                    CanChangeCostumSkin = true;
                    currentChagneTime = 0;
                }
            }
            if (WeaponEquipSlot != null)
                WeaponEquipSlot.Attachment = currentWeaponattachment;
            if (Wing_LEquipSlot != null)
                Wing_LEquipSlot.Attachment = currentWing_Lattachment;
            if (Wing_REquipSlot != null)
                Wing_REquipSlot.Attachment = currentWing_Rattachment;
        }
         
        void Init()
        {
            attachmentshader = Shader.Find("Spine/Skeleton");
        }
   
        void CharacterShapeChange(UI.Event.ShapeChange msg)
        {
            if (msg.Equiped == false)
                return;
            if (this._character != msg._character)
                return;

            if(WeaponEquipSlot==null)
                WeaponEquipSlot = skeletonanimation.skeleton.FindSlot(WeaponSlot);
            if (Wing_LEquipSlot == null)
                Wing_LEquipSlot = skeletonanimation.skeleton.FindSlot(WingSlot_Left);
            if (Wing_REquipSlot == null)
                Wing_REquipSlot = skeletonanimation.skeleton.FindSlot(WingSlot_Right);
            

            if (msg.itemtype==ItemType.Costum)
            {
                if (CanChangeCostumSkin == false)
                    return;

                WeaponEquipSlot.Attachment = currentWeaponattachment;
                
                string currnetAnimName = skeletonanimation.AnimationName;
                if (currnetAnimName == null)
                    currnetAnimName = "run";

                //스킨 변경될때
                if(msg.SpineName== "knight02")
                {
                    if(skeletonanimation.skeletonDataAsset!=SkinAsset_1)
                    {
                        skeletonanimation.skeletonDataAsset = SkinAsset_1;
                        skeletonanimation.initialSkinName = msg.itemName;
                        skeletonanimation.Initialize(true);
                        if (_character.GetComponent<CharacterHandleWeapon>() != null)
                        {
                            _character.GetComponent<CharacterHandleWeapon>().SpineAnimationEventSetting();
                        }
                    }
                }
                else
                {
                    if (skeletonanimation.skeletonDataAsset != SkinAsset_0)
                    {
                        skeletonanimation.skeletonDataAsset = SkinAsset_0;
                        skeletonanimation.initialSkinName = msg.itemName;
                        skeletonanimation.Initialize(true);
                        if (_character.GetComponent<CharacterHandleWeapon>() != null)
                        {
                            _character.GetComponent<CharacterHandleWeapon>().SpineAnimationEventSetting();
                        }
                    }
                }
                //스킨 변경될때

                skeletonanimation.skeleton.SetSkin(msg.itemName);
                skeletonanimation.skeleton.SetSlotsToSetupPose();
                skeletonanimation.AnimationState.SetAnimation(0, currnetAnimName, true);

                if(CurrentWeaponsprite!=null)
                {
                    WeaponEquipSlot = skeletonanimation.skeleton.FindSlot(WeaponSlot);

                    currentWeaponattachment = CurrentWeaponsprite.ToRegionAttachmentPMAClone(attachmentshader);

                    currentWeaponattachment.SetScale(scale, scale);
                    currentWeaponattachment.UpdateOffset();

                    WeaponEquipSlot.Attachment = currentWeaponattachment;
                }
                //foreach (var _data in winglist)
                //{
                //    _data.Value.SetActive(false);
                //}
                //if (IsWingParticle==true)
                //{
                   

                //    winglist[msg.itemName].SetActive(true);
                //    currentWing_Rattachment = null;
                //    //currentWing_Rattachment.UpdateOffset();
                //    currentWing_Lattachment = null;
                //    //currentWing_Lattachment.UpdateOffset();
                //}
                if (IsWingParticle == false)
                {
                    
                    if (CurrentWingSprite_L != null)
                    {
                        Wing_LEquipSlot = skeletonanimation.skeleton.FindSlot(WingSlot_Left);

                        currentWing_Lattachment = CurrentWingSprite_L.ToRegionAttachmentPMAClone(attachmentshader);

                        currentWing_Lattachment.SetScale(scale, scale);
                        currentWing_Lattachment.UpdateOffset();

                        Wing_LEquipSlot.Attachment = currentWing_Lattachment;
                    }
                    if (CurrentWingSprite_R != null)
                    {
                        Wing_REquipSlot = skeletonanimation.skeleton.FindSlot(WingSlot_Right);

                        currentWing_Rattachment = CurrentWingSprite_R.ToRegionAttachmentPMAClone(attachmentshader);

                        currentWing_Rattachment.SetScale(scale, scale);
                        currentWing_Rattachment.UpdateOffset();

                        Wing_REquipSlot.Attachment = currentWing_Rattachment;
                    }
                }
            

                Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.SkinName = msg.itemName;
            }
            else if (msg.itemtype == ItemType.weapon)
            {
                foreach (var _data in Common.InGameManager.Instance.WeaponInventory.GetSlots)
                {
                    if (_data.itemData.itemInfo.sprite == msg.itemName)
                    {
                        CurrentWeaponsprite = _data.itemData.mySprite_R;
                    }
                }

                if(CurrentWeaponsprite != null)
                {
                    WeaponEquipSlot = skeletonanimation.skeleton.FindSlot(WeaponSlot);

                    currentWeaponattachment = CurrentWeaponsprite.ToRegionAttachmentPMAClone(attachmentshader);

                    currentWeaponattachment.SetScale(scale, scale);
                    currentWeaponattachment.UpdateOffset();

                    WeaponEquipSlot.Attachment = currentWeaponattachment;
                }
                Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.weaponName = msg.itemName;
            }
            else if (msg.itemtype == ItemType.wing)
            {
                foreach (var _data in Common.InGameManager.Instance.WingInventory.GetSlots)
                {
                    if (_data.itemData.itemInfo.sprite == msg.itemName)
                    {
                        CurrentWingSprite_L = _data.itemData.mySprite_L;
                        CurrentWingSprite_R = _data.itemData.mySprite_R;
                    }
                }
                foreach (var _data in winglist)
                {
                    _data.Value.SetActive(false);
                }

                if (msg.itemName == "Wing26" || msg.itemName == "Wing27" || msg.itemName == "Wing28" ||
                    msg.itemName == "Wing29" || msg.itemName == "Wing30")
                {
                    IsWingParticle = true;
                    
                    winglist[msg.itemName].SetActive(true);
                    currentWing_Rattachment = null;
                    currentWing_Lattachment = null;
                }
                else
                {
                    IsWingParticle = false;
                    if (CurrentWingSprite_L != null)
                    {
                        Wing_LEquipSlot = skeletonanimation.skeleton.FindSlot(WingSlot_Left);

                        currentWing_Lattachment = CurrentWingSprite_L.ToRegionAttachmentPMAClone(attachmentshader);

                        currentWing_Lattachment.SetScale(scale, scale);
                        currentWing_Lattachment.UpdateOffset();

                        Wing_LEquipSlot.Attachment = currentWing_Lattachment;
                    }
                    if (CurrentWingSprite_R != null)
                    {
                        Wing_REquipSlot = skeletonanimation.skeleton.FindSlot(WingSlot_Right);

                        currentWing_Rattachment = CurrentWingSprite_R.ToRegionAttachmentPMAClone(attachmentshader);

                        currentWing_Rattachment.SetScale(scale, scale);
                        currentWing_Rattachment.UpdateOffset();

                        Wing_REquipSlot.Attachment = currentWing_Rattachment;
                    }
                }
                
                Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp.wingName = msg.itemName;
            }
        }
    }

}
