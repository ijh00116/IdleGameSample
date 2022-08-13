using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;
using DLL_Common.Common;

namespace BlackTree.InGame
{
    public enum BuffType
    {
        AttackPower,
        AttackSpeed,
        MonsterGold,
        MoveSpeed,
        AutoSkill,
        MonsterPotion,

        End
    }
    public class Buff
    {
        public BuffType BuffType;
        public int idx;
        public float CurrentTime;
        public float maxTime;
        public bool IsActive=false;
        public float value;
    }
    public class CharactersBuff : CharacterAbility
    {
        List<Buff> BuffAbilities = new List<Buff>();

        protected override void Start()
        {
            base.Start();
            Buff newbuff = new Buff();
            newbuff.BuffType = BuffType.AttackPower;
            newbuff.idx = 100031;
            newbuff.CurrentTime =0;
            newbuff.maxTime = DTConstraintsData.BuffTime;
            newbuff.value = 2;
            BuffAbilities.Add(newbuff);

            Buff newbuff_1 = new Buff();
            newbuff_1.BuffType = BuffType.AttackSpeed;
            newbuff_1.idx = 100235;
            newbuff_1.CurrentTime = 0; 
            newbuff_1.maxTime = DTConstraintsData.BuffTime;
            newbuff_1.value = 1.5f;
            BuffAbilities.Add(newbuff_1);

            Buff newbuff_2 = new Buff();
            newbuff_2.BuffType = BuffType.MonsterGold;
            newbuff_2.idx = 100613;
            newbuff_2.CurrentTime = 0;
            newbuff_2.maxTime = DTConstraintsData.BuffTime;
            newbuff_2.value = 2;
            BuffAbilities.Add(newbuff_2);

            Buff newbuff_3 = new Buff();
            newbuff_3.BuffType = BuffType.MoveSpeed;
            newbuff_3.idx = 101715;
            newbuff_3.CurrentTime = 0;
            newbuff_3.maxTime = DTConstraintsData.BuffTime;
            newbuff_3.value = 1.5f;
            BuffAbilities.Add(newbuff_3);

            Buff newbuff_4 = new Buff();
            newbuff_4.BuffType = BuffType.MonsterPotion;
            newbuff_4.idx = 101716;
            newbuff_4.CurrentTime = 0;
            newbuff_4.maxTime = DTConstraintsData.BuffTime;
            newbuff_4.value = 2.0f;
            BuffAbilities.Add(newbuff_4);

            for (int i=0; i<BuffAbilities.Count;i++)
            {
                timer.Bufftype = BuffAbilities[i].BuffType;
                timer.ElapsedTime = BuffAbilities[i].CurrentTime;
                Message.Send<InGame.Event.BuffTimer>(timer);
            }
            Message.AddListener<InGame.Event.BuffActivate>(BuffActive);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Message.RemoveListener<InGame.Event.BuffActivate>(BuffActive);
        }

        InGame.Event.BuffTimer timer = new Event.BuffTimer();
        protected override void ProcessAbility()
        {
            base.ProcessAbility();
            foreach(var _data in BuffAbilities)
            {
                if (_data.IsActive == false)
                    continue;
                AbilitiesType _type = AbilitiesType.CHA_ATTACK_UP;
                _data.CurrentTime -= Time.deltaTime;
                //버프 종료
                if (_data.CurrentTime <= 0)
                {
                    _data.IsActive = false;
                    switch (_data.BuffType)
                    {
                        case BuffType.AttackPower:
                            _type = AbilitiesType.CHA_AD_ATTACK_BUFF;
                            break;
                        case BuffType.AttackSpeed:
                            _type = AbilitiesType.CHA_ATTACK_SPEED_UP;
                            break;
                        case BuffType.MonsterGold:
                            _type = AbilitiesType.CHA_AD_REWARDGOLD_BUFF;
                            break;
                        case BuffType.MoveSpeed:
                            _type = AbilitiesType.CHA_AD_MOVESPEED_BUFF;
                            break;
                        case BuffType.MonsterPotion:
                            _type = AbilitiesType.CHA_AD_REWARDPOTION_BUFF;
                            break;
                        case BuffType.End:
                            break;
                        default:
                            break;
                    }
                    if(_type!=AbilitiesType.End)
                    {
                        CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.AdBuff,
                        _type, _data.idx, 0);
                    }
                    else
                    {

                    }
                    Common.InGameManager.Instance.GetPlayerData.GlobalUser.BuffActive(_data.BuffType, false);
                }
                timer.Bufftype = _data.BuffType;
                timer.ElapsedTime = _data.CurrentTime;
                Common.InGameManager.Instance.GetPlayerData.GlobalUser.UpdateBuffTime(_data.BuffType, (int)timer.ElapsedTime);
                Message.Send<InGame.Event.BuffTimer>(timer);
            }
        }

        void BuffActive(InGame.Event.BuffActivate msg)
        {
            Buff _buff = BuffAbilities.Find(o => o.BuffType == msg.buffType);

            if(_buff!=null)
            {
                AbilitiesType _type = AbilitiesType.CHA_ATTACK_UP;
                switch (_buff.BuffType)
                {
                    case BuffType.AttackPower:
                        _type = AbilitiesType.CHA_AD_ATTACK_BUFF;
                        break;
                    case BuffType.AttackSpeed:
                        _type = AbilitiesType.CHA_ATTACK_SPEED_UP;
                        break;
                    case BuffType.MonsterGold:
                        _type = AbilitiesType.CHA_AD_REWARDGOLD_BUFF;
                        break;
                    case BuffType.MoveSpeed:
                        _type = AbilitiesType.CHA_AD_MOVESPEED_BUFF;
                        break;
                    case BuffType.MonsterPotion:
                        _type = AbilitiesType.CHA_AD_REWARDPOTION_BUFF;
                        break;
                    case BuffType.End:
                        break;
                    default:
                        break;
                }
                if(_type!=AbilitiesType.End)
                {
                    CharacterDataManager.Instance.PlayerCharacterdata.SetAbilityValue(AbilityValueType.AdBuff,
                    _type, _buff.idx, _buff.value);
                }

                Common.InGameManager.Instance.GetPlayerData.GlobalUser.BuffActive(_buff.BuffType, true);

                _buff.IsActive = true;
                _buff.CurrentTime = msg.StartTime;
            }
        }
    }
}

