using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public enum AbilityValueType
    {
        Skill,
        Costume,
        HoldWeapon,
        HoldWing,
        EquipWeapon,
        EquipWing,
        Enforce,
        Relic,
        EquipPet,
        HoldPet,
        SRelic,
        ActiveSkillStat,
        AdBuff,
        Newbie,

        End
    }
    public class AbilityValueCache
    {
        public class Value
        {
            public BigInteger value;
            public int AbilityIdx;
            public Value(BigInteger value)
            {
                this.value = value;
            }
        }

        public Dictionary<AbilitiesType,Dictionary<int,Value>> values { get; private set; }

        public AbilityValueCache()
        {
            values = new Dictionary<AbilitiesType, Dictionary<int, Value>>();
        }


        public void Add(int abilIdx,AbilitiesType abilityType)
        {
            if (!values.ContainsKey(abilityType))
            {
                Value _value = new Value(BigInteger.Zero);
                Dictionary<int, Value> valueList = new Dictionary<int, Value>();
                valueList.Add(abilIdx, _value);
                values.Add(abilityType, valueList);
            }
            else
            {
                Dictionary<int, Value> valueList = values[abilityType];
                if (!valueList.ContainsKey(abilIdx))
                {
                    values[abilityType].Add(abilIdx, new Value(BigInteger.Zero));
                }
            }
        }

        public bool HasValue(AbilitiesType abilityType)
        {
            return values.ContainsKey(abilityType);
        }

        public void SetValue(AbilitiesType abilityType,int abilIndex ,BigInteger value)
        {
            if (values.ContainsKey(abilityType))
            {
                Dictionary<int, Value> valueList = values[abilityType];
                foreach (KeyValuePair<int, Value> data in valueList)
                {
                    if(data.Key==abilIndex)
                        data.Value.value = value;
                }
            }
        }

        public BigInteger GetValue(AbilitiesType abilityType,int abilIndex)
        {
            BigInteger data = BigInteger.Zero;
            if (values.ContainsKey(abilityType))
            {
                Dictionary<int, Value> valueList = values[abilityType];
                if(valueList.ContainsKey(abilIndex))
                    data = valueList[abilIndex].value;
            }
            return data;
        }

        public BigInteger GetValue(AbilitiesType abilityType)
        {
            BigInteger data = BigInteger.Zero;
            if (values.ContainsKey(abilityType))
            {
                Dictionary<int, Value> valueList = values[abilityType];
                foreach (KeyValuePair<int,Value> _data in valueList)
                {
                    data += _data.Value.value;
                }
            }

            return data;
        }

    }

}
