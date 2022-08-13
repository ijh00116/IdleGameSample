using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public enum ItemType
    {
        Skill,
        Relic,
        weapon,
        wing,
        Costum,
        Quest,
        pet,
        s_relic,
        S_Skill,
        enchantstone,
        magicstone,
    }
    public enum GachaType
    {
        Advertise,
        Ticket_pet,
        Ticket_SRelic,
        Ticket_Gacha,
        MagicPotion,
        Gem,
        Free,

        End
    }
    public enum LevelUpType
    {
        LevelUp_5,
        LevelUp_100,
        LevelUp_200,
        LevelUp_500,
        LevelUp_1000,
        LevelUp_1500,
        LevelUp_5000,
        LevelUp_10000,
    }

    public enum LevelUpUIType
    {
        Levelup_1,
        Levelup_10,
        Levelup_100,

        None
    }

}