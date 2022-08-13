using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;
using BlackTree.InGame;

namespace BlackTree
{
    public class EnemyReward
    {
        CharacterType charactertype;
        public EnemyReward(CharacterType _type)
        {
            charactertype = _type;
        }
        public BigInteger CalculateGold()
        {
            BigInteger reward = InGameManager.Instance.GetPlayerData.GlobalUser.RewardGold;

            CharacterDataAbility ability = CharacterDataManager.Instance.PlayerCharacterdata.ability;

            float random = Random.Range(0, 100);
            int timesValue = 1;
            if(random< ability.GetTenTimesGoldRate())
            {
                timesValue = 10;
            }

            if(charactertype== CharacterType.NormalMonster)
            {
               return ability.GetMonsterKillGoldReward(reward)*timesValue;
            }
            else if(charactertype == CharacterType.Boss)
            {
                return ability.GetBossKillGoldReward(reward) * timesValue;
            }
            else if (charactertype == CharacterType.Mimic)
            {
                return ability.GetMimicKillGoldReward(reward) * timesValue;
            }
            else
            {
                return ability.GetMonsterKillGoldReward(reward) * timesValue;
            }
        }

        public BigInteger CalculateMagicpotion()
        {
            int stage = InGameManager.Instance.GetPlayerData.stage_Info.Stage;
            int basemagic = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageRewarddata.base_stage_magic_pottion;
            int gainrate = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageRewarddata.stage_gain_magic_potion;

            BigInteger reward = basemagic + stage * gainrate;
            CharacterDataAbility ability = CharacterDataManager.Instance.PlayerCharacterdata.ability;
            float random = Random.Range(0, 100);
            int timesValue = 1;

            if (random < ability.GetTenTimesPotionRate())
            {
                timesValue = 10;
            }
            if (charactertype == CharacterType.NormalMonster)
            {
                return ability.GetMonsterKillPotionReward(reward) * timesValue;
            }
            else if (charactertype == CharacterType.Boss)
            {
                return ability.GetBossKillPotionReward(reward) * timesValue;
            }
            else if (charactertype == CharacterType.Mimic)
            {
                return ability.GetMimicKillPotionReward(reward) * timesValue;
            }
            else
            {
                return ability.GetMonsterKillPotionReward(reward) * timesValue;
            }
        }


        public int CalculateExp()
        {
            int exp = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageRewarddata.user_exp;

            return exp;
        }
    }

}
