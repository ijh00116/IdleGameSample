using BlackTree.Common;
using BlackTree.InGame;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

namespace BlackTree
{
    public class PvpDungeonController : MonoBehaviour
    {
        public Transform PlayerPosition;
        public Transform EnemyPosition;

        [HideInInspector] public CharacterInPVP PlayerCharacter;
        [HideInInspector] public CharacterInPVP EnemyCharacter;

        public void Init()
        {
            var mine = Instantiate(Common.InGameManager.Instance.PVPCharacterPrefab);
            var enemy = Instantiate(Common.InGameManager.Instance.PVPCharacterPrefab);
            
            mine.transform.position = PlayerPosition.position;
            enemy.transform.position = EnemyPosition.position;

            Vector3 enemyscale = enemy.transform.localScale;
            enemyscale.x *= -1;
            enemy.transform.localScale = enemyscale;

            PlayerCharacter = mine.GetComponent<CharacterInPVP>();
            EnemyCharacter = enemy.GetComponent<CharacterInPVP>();

            PlayerCharacter.playertype = CharacterType.PvpPlayer;
            EnemyCharacter.playertype = CharacterType.PvpEnemy;

            PlayerCharacter.enemytype = CharacterType.PvpEnemy;
            EnemyCharacter.enemytype = CharacterType.PvpPlayer;



            PlayerCharacter.gameObject.SetActive(false);
            EnemyCharacter.gameObject.SetActive(false);
        }
        bool InGame=false;
        public void PvpStart()
        {
            PlayerCharacter.gameObject.SetActive(true);
            EnemyCharacter.gameObject.SetActive(true);

            var myinfo = Common.InGameManager.Instance.GetPlayerData.saveData.userinfoPvp;
            var enemyinfo = Common.InGameManager.Instance.EnemyPvpInfo;

            PlayerCharacter.userInfo = myinfo;
            EnemyCharacter.userInfo = enemyinfo;

            BigInteger myFightpower = myinfo.AbilityList[PVPAbilityType.CHA_ATTACK] 
                * (new BigInteger(myinfo.AbilityList[PVPAbilityType.CHA_ATTACK]) / new BigInteger(enemyinfo.AbilityList[PVPAbilityType.CHA_ATTACK]));
            BigInteger enemyFightpower = enemyinfo.AbilityList[PVPAbilityType.CHA_ATTACK] * (new BigInteger(enemyinfo.AbilityList[PVPAbilityType.CHA_ATTACK]) 
                / new BigInteger(myinfo.AbilityList[PVPAbilityType.CHA_ATTACK]));
            PlayerCharacter.GetComponent<HealthInPVP>().SettingHealth(enemyFightpower * 1500 + enemyinfo.AbilityList[PVPAbilityType.CHA_SKILL_LIGHTNING_DAMGAGE]);
            EnemyCharacter.GetComponent<HealthInPVP>().SettingHealth(myFightpower * 1200 + myinfo.AbilityList[PVPAbilityType.CHA_SKILL_LIGHTNING_DAMGAGE]);

            PlayerCharacter.Init();
            EnemyCharacter.Init();
            InGame = true;
        }

        private void Update()
        {
            //체력 연출
            //시간 어느정도 지나면씬 종료하고 
            //씬 대기상태로하고 캐릭 비활성화 하고
            //ui 결과창 띄워주기
            if (InGame == false)
                return;
            if(PlayerCharacter.GetComponent<HealthInPVP>().CurrentHealth<0|| EnemyCharacter.GetComponent<HealthInPVP>().CurrentHealth<0)
            {
                Release();
            }
        }

        public void Release()
        {
            if (InGame == false)
                return;

            InGame = false;
            PlayerCharacter.gameObject.SetActive(false);
            EnemyCharacter.gameObject.SetActive(false);

            bool win = PlayerCharacter.GetComponent<HealthInPVP>().CurrentHealth >= EnemyCharacter.GetComponent<HealthInPVP>().CurrentHealth;
            Message.Send<UI.Event.PVPEnd>(new UI.Event.PVPEnd(win));
        }
    }
}

