using BlackTree.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public enum CombatType
    {
        Normal,
        Dungeon,
        Pet,
        PVP,
    }
    
    public abstract class CombatMode
    {
        protected Dictionary<CharacterType, Common.ObjectPool<GameObject>> PooledObjectList = new Dictionary<CharacterType, Common.ObjectPool<GameObject>>();
        protected Dictionary<CharacterType, Common.ObjectPool<GameObject>> ActiveObjectList = new Dictionary<CharacterType, Common.ObjectPool<GameObject>>();
        public CombatType combatType=CombatType.Normal;

        List<CharacterDataBase> EnemyDatabaseList=new List<CharacterDataBase>();
        public virtual void Init(List<CharacterDataBase> EnemyDatabaseList)
        {
            for(int i=0; i< EnemyDatabaseList.Count; i++)
            {
                PooledObjectList.Add(EnemyDatabaseList[i].monsterType, new Common.ObjectPool<GameObject>());
            }

            Message.AddListener<InGame.Event.EnemyKilled>(EnemyKilled);
        }

        public virtual void Release()
        {
            Message.RemoveListener<InGame.Event.EnemyKilled>(EnemyKilled);
        }

        void EnemyKilled(InGame.Event.EnemyKilled msg)
        {
            if(PooledObjectList.ContainsKey(msg.charactertype))
            {
                PooledObjectList[msg.charactertype].PoolObject(msg.DeadObj);
            }
        }

        public virtual void InActivate()
        {
            foreach (var data in PooledObjectList)
            {
                foreach(var _qdata in data.Value._pool)
                {
                    _qdata.SetActive(false);
                }
            }
        }

        public virtual GameObject GetEnemyPooledObject(CharacterType mtype)
        {
            CharacterDataBase database = null;
            switch (mtype)
            {
                case CharacterType.Player:
                    break;
                case CharacterType.PetEnemy:
                    database = BTOPsetPosition.Instance.PetDungeonEnemyDatabase;
                    break;
                case CharacterType.NormalMonster:
                    database = BTOPsetPosition.Instance.enemyDatabase;
                    break;
                case CharacterType.Boss:
                    database = BTOPsetPosition.Instance.BossDatabase;
                    break;
                case CharacterType.Mimic:
                    database = BTOPsetPosition.Instance.MimicDatabase;
                    break;
                case CharacterType.Cow:
                    database = BTOPsetPosition.Instance.DungeonEnemyDatabase;
                    break;
                case CharacterType.Cowking:
                    database = BTOPsetPosition.Instance.DungeonBossEnemyDatabase;
                    break;
                case CharacterType.End:
                    break;
                default:
                    break;
            }

            GameObject Pooledobject = null;
            Pooledobject = PooledObjectList[mtype].GetObject();
            if(Pooledobject==null)
            {
                Pooledobject = BTOPsetPosition.Instance.CreateObject(database.Characters[0].CharacterPrefab);
            }

            //PooledObjectList[mtype].PoolObject(Pooledobject);
       
            Pooledobject.SetActive(true);

            return Pooledobject;
        }

        public virtual void CreateObject(GameObject poolPrefab, GameObject parent, BTObjectPool<GameObject> pool)
        {
            BTOPsetPosition.Instance.CreateObject(poolPrefab, parent, pool);
        }

        public abstract void EnemyRegen(int poolMaxcount=-1);
    }
}
