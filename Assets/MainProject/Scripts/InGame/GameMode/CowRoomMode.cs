using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using DLL_Common.Common;
using BlackTree.Common;

namespace BlackTree
{
    public class CowRoomMode : CombatMode
    {
        public override void EnemyRegen(int PoolMaxCount)
        {
            for (int i = 0; i < PoolMaxCount; i++)
            {
                GameObject obj = null;
                Health _health = null;
                Character _character = null;
                string hpstring = null;

                float kingrate = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetCowKingRate();
                float randomrate = Random.Range(0, 100);

                if(randomrate<kingrate)
                    obj = GetEnemyPooledObject(CharacterType.Cowking);
                else
                    obj = GetEnemyPooledObject(CharacterType.Cow);

                _character = obj.GetComponent<Character>();
                _health = _character._health;
                if (randomrate < kingrate)
                    _character.playertype = CharacterType.Cowking;
                else
                    _character.playertype = CharacterType.Cow;
                hpstring = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.Dungeondata.monster_hp;
                if (hpstring.Contains(","))
                    hpstring = hpstring.Replace(",", "");

                
                BigInteger hp = new BigInteger(hpstring);
                Data_Character _Datacharacter = CharacterDataManager.Instance.PlayerCharacterdata;
                _Datacharacter.ability.GetMonsterHp(ref hp);
                _character.EnemyOnenableSet();
                _health.SettingHealth(hp);

                obj.transform.position =BTOPsetPosition.Instance.DungeonRegenLocation[i].position;
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

}
