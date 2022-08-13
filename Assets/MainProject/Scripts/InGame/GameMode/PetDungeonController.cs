using BlackTree.Common;
using BlackTree.InGame;
using DLL_Common.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using DG.Tweening;

namespace BlackTree
{
    public class PetDungeonController : MonoBehaviour
    {
        //enemy
        ObjectPool<GameObject> MovingPetPool = new ObjectPool<GameObject>();
        List<GameObject> MovingPetList = new List<GameObject>();

        public Transform EnemyCreatePosition;
        //player
        public Transform PlayerPosition;
        InGame.Character _character;

        public CharacterDataBase PetDungeonEnemyDatabase;

        [Header("펫 움직이는 연출 관련")]
        public GameObject MovingPetPrefab;
        public Transform WaitPosition;
        public Transform StartPosition;
        public Transform EndPosition;

        List<GameObject> ActiveEnemy = new List<GameObject>();
        [SerializeField]public List<Transform> WaitPetEnemyPositionList = new List<Transform>();
        [SerializeField] public SkeletonAnimation SummonerAnimation;

        public void Init()
        {
            _character = InGameManager.Instance.PetPlayerCharacter;
            
            _character.transform.SetParent(PlayerPosition, false);
            _character.transform.localPosition = Vector3.zero;

            for(int i=0; i< WaitPetEnemyPositionList.Count;i++)
            {
                int index = Random.Range(0, PetDungeonEnemyDatabase.Characters.Length);
                GameObject enemyobj = Instantiate(PetDungeonEnemyDatabase.Characters[index].CharacterPrefab);
                enemyobj.SetActive(false);

                //Health _health = null;
                //Character _character = null;
                //string hpstring = null;

                //_character = enemyobj.GetComponent<Character>();
                //_health = _character._health;
                //_character.playertype = CharacterType.PetEnemy;

                //hpstring = InGameManager.Instance.GetPlayerData.stage_Info.CurrentPetDungeondata.monster_hp;

                //if (hpstring.Contains(","))
                //    hpstring = hpstring.Replace(",", "");
                //BigInteger hp = new BigInteger(hpstring);

                //float hpRate = InGameDataTableManager.DungeonTableList.gain.Find(o => o.stage == InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage).pet_stage_hp_gain_rate;
                //hp = hp * hpRate;

                //_health.SettingHealth(hp);

                enemyobj.transform.SetParent(WaitPetEnemyPositionList[i], false);
                enemyobj.transform.localPosition = Vector3.zero;

                ActiveEnemy.Add(enemyobj);
            }

            Message.AddListener<InGame.Event.EnemyKilled>(EnemyUpdate);
        }

        private void OnApplicationQuit()
        {
            Message.RemoveListener<InGame.Event.EnemyKilled>(EnemyUpdate);
        }

        int petRewardIdx;
        List<PetReward> petRewardList;
        //펫던전 시작
        public void PetDungeonStart()
        {
            for(int i=0; i< ActiveEnemy.Count; i++)
            {
                ActiveEnemy[i].SetActive(true);
                Vector3 _temppos = WaitPetEnemyPositionList[i].transform.position;
                ActiveEnemy[i].transform.position = _temppos;
            }
            CreatePetDungeonEnemy();

            petRewardIdx = Common.InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeondata.reward_pet_group;
            petRewardList = InGameDataTableManager.DungeonTableList.pet_reward.FindAll(o => o.idx == petRewardIdx);
        }

        public void PetDungeonExit()
        {
            for(int i=0; i< MovingPetList.Count; i++)
            {
                MovingPetList[i].transform.position = WaitPosition.position;
                //MovingPetList[i].GetComponent<Character>()._state.ChangeState(eActorState.Idle);
                MovingPetPool.PoolObject(MovingPetList[i]);
            }
            MovingPetList.Clear();
        }
        
        void EnemyUpdate(InGame.Event.EnemyKilled msg)
        {
            if (msg.charactertype != InGame.CharacterType.PetEnemy)
                return;
            //연출
            //idx찾기
            int rate = 0;
            for (int i = 0; i < petRewardList.Count; i++)
            {
                rate += petRewardList[i].rarity;
            }
            int lotto = UnityEngine.Random.Range(0, rate);
            int index = 0;
            rate = 0;
            for (int i = 0; i < petRewardList.Count; i++)
            {
                rate += petRewardList[i].rarity;
                if (lotto <= rate)
                {
                    index = i;
                    break;
                }
            }
            InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeonRewardPetList.Add(petRewardList[index].pet_idx);
            
            //idx 찾기
            int petidx = petRewardList[index].pet_idx;
            PetInfo info = Common.InGameManager.Instance.petInventory.GetSlots.Find(o => o.pet.idx == petidx).petData.petInfo;
            string spineAssetName = info.SpineName.ToString();

            var obj = MovingPetPool.GetObject();
            if(obj==null)
            {
                obj = Instantiate(MovingPetPrefab);
            }
            MovingPetList.Add(obj);
            obj.SetActive(true);
            SkeletonDataAsset data = null;
            data = Resources.Load<SkeletonDataAsset>(string.Format("Spine/Monster/{0}", spineAssetName));
            obj.transform.localScale = new Vector3((info.Scale) * 2.28f, (info.Scale) * 2.28f, info.Scale);
            obj.GetComponent<PetControllerInPetdungeon>().SetPositionData(StartPosition, EndPosition, data);
            obj.GetComponent<Character>()._state.ChangeState(eActorState.Move);
            StartCoroutine(CreateEnemyAndCreatePet());
        }
        WaitForSeconds waitseconds = new WaitForSeconds(1.0f);
        IEnumerator CreateEnemyAndCreatePet()
        {
            SummonerAnimation.AnimationState.SetAnimation(0, "attack", false);
            Vector3 endEnemyPos= WaitPetEnemyPositionList[WaitPetEnemyPositionList.Count - 1].transform.position;
            GameObject deadenemy = ActiveEnemy[0];
            for (int i = 1; i < ActiveEnemy.Count; i++)
            {
                Vector3 _temppos = WaitPetEnemyPositionList[i - 1].transform.position;
                ActiveEnemy[i].transform.DOMove(_temppos, 1.0f);
            }
            
            GameObject temp = ActiveEnemy[0];
            for (int i = 1; i < ActiveEnemy.Count; i++)
            {
                ActiveEnemy[i - 1] = ActiveEnemy[i];
            }
            ActiveEnemy[ActiveEnemy.Count - 1] = temp;

            yield return waitseconds;
            deadenemy.transform.position = endEnemyPos;
            deadenemy.GetComponent<Character>()._Skeletonanimator.skeleton.A = 1;

            if (Common.InGameManager.Instance._PetsceneFsm._State.IsCurrentState(ePlaySubScene.PetDunGeonUpdate) == false)
                yield break;

            CreatePetDungeonEnemy();
            yield break;
        }

        void CreatePetDungeonEnemy()
        {
            GameObject enemyobj = ActiveEnemy[0];

            Health _health = null;
            Character _character = null;
            string hpstring = null;
            enemyobj.SetActive(true);

            _character = enemyobj.GetComponent<Character>();
            _character._Skeletonanimator.skeleton.A = 1;
            _health = _character._health;
            _character.playertype = CharacterType.PetEnemy;

            hpstring = InGameManager.Instance.GetPlayerData.stage_Info.stagesubinfo.CurrentPetDungeondata.monster_hp;

            if (hpstring.Contains(","))
                hpstring = hpstring.Replace(",", "");
            BigInteger hp = new BigInteger(hpstring);
            hp = hpstring;
            //float hpRate = InGameDataTableManager.DungeonTableList.gain.Find(o => o.stage == InGameManager.Instance.GetPlayerData.stage_Info.Pet_CurrentStage).pet_stage_hp_gain_rate;
            // hp = hp * hpRate;

            _health.SettingHealth(hp);

            enemyobj.transform.SetParent(EnemyCreatePosition, false);
            enemyobj.transform.localPosition = Vector3.zero;
            _character._state.ChangeState(eActorState.Idle);

          
            InGameManager.Instance.PetPlayerCharacter.CurrentEnemy = enemyobj;
            InGameManager.Instance.PetPlayerCharacter.CurrentEnemyCharacter = enemyobj.GetComponent<Character>();
            if (InGameManager.Instance.PetPlayerCharacter.CurrentEnemyCharacter._health.CurrentHealth > 0)
            {
                InGameManager.Instance.PetPlayerCharacter._state.ChangeState(eActorState.BaseAttack);
            }

        }
    }

}
