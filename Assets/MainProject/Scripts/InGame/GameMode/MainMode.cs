using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using DLL_Common.Common;
using BlackTree.Common;

namespace BlackTree
{
    public class MainMode : CombatMode
    {
        public override void EnemyRegen(int poolcount)
        {
            //오브젝트 active 활성화 하고 체력 세팅 해주기
            for (int i = InGameManager.Instance.GetPlayerData.CurrentWave; i < InGameManager.Instance.GetPlayerData.stage_Info.MaxWave(); i++)
            {
                Data_Character _Datacharacter = CharacterDataManager.Instance.PlayerCharacterdata;
                if (InGameManager.Instance.InfiniteMode)
                {
                    float random = Random.Range(0, 100.0f);
                    if (random <= _Datacharacter.ability.GetMimicAppearRate())
                    {
                        GetEnemySetting(i, CharacterType.Mimic);
                    }
                    else
                    {
                        GetEnemySetting(i, CharacterType.NormalMonster);
                    }
                }
                else
                {
                    if (i == InGameManager.Instance.GetPlayerData.stage_Info.MaxWave() - 1)
                    {
                        GetEnemySetting(i, CharacterType.Boss);
                    }
                    else
                    {
                        float random = Random.Range(0, 100.0f);
                        if (random <= _Datacharacter.ability.GetMimicAppearRate())
                        {
                            GetEnemySetting(i, CharacterType.Mimic);
                        }
                        else
                        {
                            GetEnemySetting(i, CharacterType.NormalMonster);
                        }
                    }
                }
            }
        }

        void GetEnemySetting(int locationIndex, CharacterType monstertype)
        {
            string hpstring = null;
            BigInteger hp = BigInteger.Zero;
            Data_Character _Datacharacter = CharacterDataManager.Instance.PlayerCharacterdata;
            GameObject obj = GetEnemyPooledObject(monstertype);
            Character _character = obj.GetComponent<Character>();
            Health _health = _character._health;
            _character.playertype = monstertype;

            switch (monstertype)
            {
                case CharacterType.NormalMonster:
                    hpstring = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageMonsterdata.monster_hp;
                    if (hpstring.Contains(","))
                        hpstring = hpstring.Replace(",", "");
                    hp = new BigInteger(hpstring);
                    _Datacharacter.ability.GetMonsterHp(ref hp);
                    break;
                case CharacterType.Mimic:
                    hpstring = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageMonsterdata.monster_hp;
                    if (hpstring.Contains(","))
                        hpstring = hpstring.Replace(",", "");
                    hp = new BigInteger(hpstring);
                    _Datacharacter.ability.GetMonsterHp(ref hp);
                    break;
                case CharacterType.Boss:
                    hpstring = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageMonsterdata.boss_hp;
                    if (hpstring.Contains(","))
                        hpstring = hpstring.Replace(",", "");
                    hp = new BigInteger(hpstring);
                    _Datacharacter.ability.GetBossMonsterHp(ref hp);
                    break;
                case CharacterType.Cow:
                    hpstring = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageMonsterdata.monster_hp;
                    if (hpstring.Contains(","))
                        hpstring = hpstring.Replace(",", "");
                    hp = new BigInteger(hpstring);
                    _Datacharacter.ability.GetMonsterHp(ref hp);
                    break;
                case CharacterType.Cowking:
                    hpstring = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.stageMonsterdata.monster_hp;
                    if (hpstring.Contains(","))
                        hpstring = hpstring.Replace(",", "");
                    hp = new BigInteger(hpstring);
                    _Datacharacter.ability.GetMonsterHp(ref hp);
                    break;
                default:
                    break;
            }
            _character.EnemyOnenableSet();

            _health.SettingHealth(hp);
            obj.transform.position =BTOPsetPosition.Instance.RegenLocation[locationIndex].position;
            if (obj.transform.localScale.x > 0)
            {
                Vector3 scale = obj.transform.localScale;
                if (scale.x > 0)
                    scale.x *= -1;

                obj.transform.localScale = scale;
            }
        }

        
    }
}
