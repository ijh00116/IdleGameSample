using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using DLL_Common.Common;
using System;
using Spine.Unity;

namespace BlackTree
{
    public class CharacterPetControll : InGame.CharacterAbility
    {
        [HideInInspector] PetInventoryObject petinventory;
        [SerializeField] List<PetOfCharacter> petSlots = new List<PetOfCharacter>();
        [SerializeField] List<PetOfCharacter> SkypetSlots = new List<PetOfCharacter>();

        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < petSlots.Count; i++)
            {
                petSlots[i].petData = null;
                petSlots[i].DataSet();
            }

            for (int i = 0; i < petSlots.Count; i++)
            {
                petSlots[i].petData = null;
                petSlots[i].DataSet();
            }

            petinventory = InGameManager.Instance.petInventory;
            Message.AddListener<UI.Event.PetEquiped>(EquipPetEvent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Message.RemoveListener<UI.Event.PetEquiped>(EquipPetEvent);
        }

        void EquipPetEvent(UI.Event.PetEquiped msg)
        {
            if(petinventory==null)
                petinventory = InGameManager.Instance.petInventory;
            PetData data = msg.EquipedPet;
            if (data.pet.Equiped == false)
            {
                for (int i = 0; i < petSlots.Count; i++)
                {
                    if(petSlots[i].petData==data)
                    {
                        petSlots[i].petData = null;
                        petSlots[i].DataSet();
                        break;
                    }
                }
                for (int i = 0; i < SkypetSlots.Count; i++)
                {
                    if (SkypetSlots[i].petData == data)
                    {
                        SkypetSlots[i].petData = null;
                        SkypetSlots[i].DataSet();
                        break;
                    }
                }
                return;
            }
            else
            {
                bool Isequiped = false;
                if(data.petInfo.idx == 80009 || data.petInfo.idx == 80017)
                {
                    for (int i = 0; i < SkypetSlots.Count; i++)
                    {
                        if (SkypetSlots[i].petData == null)
                        {
                            SkypetSlots[i].petData = data;
                            SkypetSlots[i].DataSet();
                            break;
                        }
                    }
                   
                }
                else
                {
                    for (int i = 0; i < petSlots.Count; i++)
                    {
                        if (petSlots[i].petData == null)
                        {
                            petSlots[i].petData = data;
                            petSlots[i].DataSet();
                            Isequiped = true;
                            break;
                        }
                    }
                }
               
            }
        }

        public void ChangePetAnimation(string animName)
        {
            for (int i = 0; i < petSlots.Count; i++)
            {
                petSlots[i].SetAnimation(animName);
            }
            for (int i = 0; i < SkypetSlots.Count; i++)
            {
                SkypetSlots[i].SetAnimation(animName);
            }
        }
    }

}   
