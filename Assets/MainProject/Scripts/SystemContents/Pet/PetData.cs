using DLL_Common.Common;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

namespace BlackTree
{
    #region 펫 부가 데이터(추후 보강(스킬, 코스튬 등))
    public class PetSkill
    {
        protected BigInteger value;
        protected BigInteger EnforceCurrency;
        //frame마다 발생 펫 이벤트(쿨타임 잴때 필요)
        public void FrameProcess()
        {

        }
        
        //일시적 이벤트
        public void Event()
        {

        }
    }

    public class EarnEnforceJewel_PetSkill:PetSkill
    {

    }

    public class PetCostume
    {
        protected BigInteger value;
        protected BigInteger EnforceCurrency;
        protected bool Equiped;
    }
    #endregion

    public class PetData
    {
        public PetInfo petInfo;//테이블에서 값 가져올것
        public ItemType itemType = ItemType.pet;

        public int MaxLevel;
        //머지 (합성하여 다음단계 펫으로 감)
        public int mix_count;
        public int mix_reward_Id;

        //분해
        public BigInteger PetFoodWhenSold;
        //합성
        public BigInteger PetLevelupCost_1;
        public BigInteger PetLevelupCost_10;
        public BigInteger PetLevelupCost_100;

        //능력치
        public BigInteger EquipPetAbillity=BigInteger.Zero;
        public BigInteger CollectPetAbillity = BigInteger.Zero;

        //능력치 타입
        AbilitiesType equipabilitytype;
        AbilitiesType collectabilitytype;
        public int Grade;
        public PetObject pet;

        public SkeletonDataAsset SpineData;

        public Sprite MyUIIcon;
        public PetData(PetObject _pet)
        {
            pet = _pet;
            Init();
        }
         
        void Init()
        {
            if (pet == null)
                return;
            petInfo = InGameDataTableManager.PetTableList.pet.Find(o => o.idx == pet.idx);
            if (petInfo == null)
                return;
            MaxLevel = petInfo.max_level;
            mix_count = petInfo.mix_count;
            mix_reward_Id = petInfo.mix_reward;
            PetFoodWhenSold = petInfo.pet_sell_get_food;
            Grade = petInfo.grade;

            string spineFullPath = "Spine/Monster/" + petInfo.SpineName.ToString();
            SpineData = Resources.Load<SkeletonDataAsset>(spineFullPath);

            AbilityInfo equipabInfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == petInfo.use_aidx);
            AbilityInfo collectabInfo = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == petInfo.collect_aidx);

            equipabilitytype = EnumExtention.ParseToEnum<AbilitiesType>(equipabInfo.abtype);
            collectabilitytype = EnumExtention.ParseToEnum<AbilitiesType>(collectabInfo.abtype);

            //스프라이트
            string path = string.Format("Images/GUI/Pet");
            string itemname = petInfo.icon;
            string fullPath = string.Format("{0}/{1}", path, itemname);

            MyUIIcon = Resources.Load<Sprite>(fullPath);
            //UpdateData();
            //안하는 이유는 업데이트 데이터는 백엔드매니저에서 해주기때문
        }

        public void UpdateData()
        {
            if (pet.Level > petInfo.max_level)
                return;
            #region 능력치
           
            //장착효과 능력치
            float DefaultValue_equip = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == petInfo.use_aidx).level_unit;
            float IncreaseRate_e = 1;
            switch (petInfo.use_aidx_gain)
            {
                case "aidx_gain_1":
                    IncreaseRate_e = InGameDataTableManager.PetTableList.gain[pet.Level-1].aidx_gain_1;
                    break;
                case "aidx_gain_2":
                    IncreaseRate_e = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_2;
                    break;
                case "aidx_gain_3":
                    IncreaseRate_e = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_3;
                    break;
                case "aidx_gain_4":
                    IncreaseRate_e = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_4;
                    break;
                case "aidx_gain_5":
                    IncreaseRate_e = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_5;
                    break;
                default:
                    break;
            }
            float gainvalue_e = DefaultValue_equip * IncreaseRate_e;
            EquipPetAbillity = gainvalue_e;
            //보유효과도 똑같이 세팅
            float DefaultValue_Poss = InGameDataTableManager.AbilityList.abilities.Find(o => o.idx == petInfo.collect_aidx).level_unit;
            float IncreaseRate_Poss = 1;
            switch (petInfo.collect_aidx_gain)
            {
                case "aidx_gain_1":
                    IncreaseRate_Poss = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_1;
                    break;
                case "aidx_gain_2":
                    IncreaseRate_Poss = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_2;
                    break;
                case "aidx_gain_3":
                    IncreaseRate_Poss = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_3;
                    break;
                case "aidx_gain_4":
                    IncreaseRate_Poss = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_4;
                    break;
                case "aidx_gain_5":
                    IncreaseRate_Poss = InGameDataTableManager.PetTableList.gain[pet.Level - 1].aidx_gain_5;
                    break;
                default:
                    break;
            }

            float gainvalue_p = DefaultValue_Poss * IncreaseRate_Poss;
            CollectPetAbillity = gainvalue_p;
            if (pet.Unlocked == true)
            {
                //어빌리티 세팅
                CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.HoldPet, collectabilitytype, petInfo.collect_aidx, CollectPetAbillity);
                if (pet.Equiped)
                {
                    CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.EquipPet, equipabilitytype, petInfo.use_aidx, EquipPetAbillity);
                }
                else
                {
                    CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.EquipPet, equipabilitytype, petInfo.use_aidx, 0);
                }
            }
            #endregion

            //펫 장착 정보 다른 곳에 알려주기(UI, 전투화면에 있는 펫)\

        }

        public void EquipPet()
        {
            if (pet.Equiped == false)
            {
                pet.Equiped = true;
                
            }
            if(pet.Equiped)
            {
                Message.Send<UI.Event.PetEquiped>(new UI.Event.PetEquiped(this));
                UpdateData();
            }
            
        }

        public void UnEquipItem()
        {
            if (pet.Equiped == true)
            {
                pet.Equiped = false;
                
            }
            if(pet.Equiped==false)
            {
                Message.Send<UI.Event.PetEquiped>(new UI.Event.PetEquiped(this));
                UpdateData();
            }
            
        }
    }

}

