using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using DLL_Common.Common;

namespace BlackTree.InGame
{
    public class BTOPsetPosition : MonoSingleton<BTOPsetPosition>
    {
        public int EnemyobjectPoolMaxCount;
        public int BossobjectPoolMaxCount;
        public int MimicPoolMaxCount;
        public Transform[] RegenLocation;
        public Transform[] DungeonRegenLocation;

        public List<CharacterDataBase> NormalCombatCharacterDatabase;
        public List<CharacterDataBase> DungeonCharacterDatabase;
        public List<CharacterDataBase> PetDungeonCharacterDatabase;
        public CharacterDataBase enemyDatabase;
        public CharacterDataBase MimicDatabase;
        public CharacterDataBase BossDatabase;
        public CharacterDataBase DungeonEnemyDatabase;
        public CharacterDataBase DungeonBossEnemyDatabase;
        public CharacterDataBase PetDungeonEnemyDatabase;
        protected List<BTObjectPool<GameObject>> EnemyobjectsParent = new List<BTObjectPool<GameObject>>();
        protected List<BTObjectPool<GameObject>> BossobjectsParent = new List<BTObjectPool<GameObject>>();

        CombatMode NormalMode;
        CombatMode CowRoomMode;
        protected override void Init()
        {

        }

        protected override void Release()
        {
            base.Release();
            NormalMode.Release();
            CowRoomMode.Release();
        }

        /// <summary>
        /// 카우방,전투모두등 세팅
        /// </summary>
        public IEnumerator CreateEnemyObject()
        {
            NormalMode = new MainMode();
            CowRoomMode = new CowRoomMode();

            NormalMode.Init(NormalCombatCharacterDatabase);
            CowRoomMode.Init(DungeonCharacterDatabase);

            yield break;
        }

        void ReleaseCombat()
        {
            Message.Send<UI.Event.EnemyInActive>(new UI.Event.EnemyInActive(CharacterType.Boss));
            Message.Send<UI.Event.EnemyInActive>(new UI.Event.EnemyInActive(CharacterType.Mimic));
            Message.Send<UI.Event.EnemyInActive>(new UI.Event.EnemyInActive(CharacterType.NormalMonster));
            Message.Send<UI.Event.EnemyInActive>(new UI.Event.EnemyInActive(CharacterType.Cow));
            Message.Send<UI.Event.EnemyInActive>(new UI.Event.EnemyInActive(CharacterType.Cowking));
            NormalMode.InActivate();
            CowRoomMode.InActivate();
        }

        public void NormalCombatRegen()
        {
            //게임 시작하면서 적들 다 다시 리젠 할거기때문에 일단 모두 비활성화
            ReleaseCombat();
            NormalMode.EnemyRegen();
        }
        
        public void CowCombatRegen()
        {
            ReleaseCombat();

            CowRoomMode.EnemyRegen(EnemyobjectPoolMaxCount);
     
        }

        public void CreateObject(GameObject Poolprefab,GameObject parent,BTObjectPool<GameObject> pool)
        {
            GameObject obj = Instantiate(Poolprefab);
            obj.transform.SetParent(parent.transform);
            obj.SetActive(false);
            Character character = obj.GetComponent<Character>();

            pool.PoolingObjects.Add(obj);
        }

        public GameObject CreateObject(GameObject poolPrefab)
        {
            GameObject obj = Instantiate(poolPrefab);
            obj.SetActive(false);

            return obj;
        }
    }



}
