using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlackTree.State;
using System;

namespace BlackTree.InGame
{
    //메인 캐릭터가 움직이는 로직
    public class CharacterController : CharacterAbility, IStateCallback
    {
        [SerializeField] eActorState Mystate;

        Character character;
        [HideInInspector]public bool isMoving;
        Coroutine moveCoroutine;

        //FSM
        public Action OnEnter { get { return onEnter; } }
        public Action OnExit { get { return onExit; } }
        public Action OnUpdate { get { return onUpdate; } }

        [Header("적 감지")]
        public float distanceTotarget;
        public Color GizmoColor;
        public Transform DetectPosition;
        [Header("이속값(기본값은 0.5)=>테스트용")]
        public float SpeedForChange = 0.5f;
        [HideInInspector]public float tempMovespeed;

        //상속용도의 능력치 고유 값
        protected float moveSpeed;

        //메인게임 내의 캐릭터에만 쓰이는 포지션
        CharacterPositionControll MainPositionControll;
        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;
            //Gizmos.DrawWireSphere(this.transform.position, distanceTotarget);
            if (DetectPosition != null)
            {
                Gizmos.DrawLine(DetectPosition.position, (Vector2)DetectPosition.position
            + ((this.transform.localScale.x < 0) ? Vector2.left : Vector2.right)
            * distanceTotarget);
            }

        }
        void Awake()
        {
            isMoving = false;
            MainPositionControll = this.GetComponent<CharacterPositionControll>();
        }

        protected override void Start()
        {
            base.Start();
            character = this.GetComponent<Character>();

            Vector2 localscale = transform.localScale;
          
            transform.localScale = localscale;

            tempMovespeed = SpeedForChange;

            _State.AddState(Mystate, this);
        }
 
      
        void onEnter()
        {
            if (isMoving == true)
                return;

                Vector2 dir = (transform.localScale.x < 0) ? Vector2.left : Vector2.right;
                RaycastHit2D hit = Physics2D.Raycast(DetectPosition.position, dir, distanceTotarget, _character.TargetLayer);
                if (hit.collider == null)
                {
                    MoveNextPosition();
                }
                else
                {
                    _character.CurrentEnemy = hit.collider.gameObject;
                    _character.CurrentEnemyCharacter = _character.CurrentEnemy.GetComponent<Character>();

                    if (_character.CurrentEnemyCharacter._health.CurrentHealth <= 0)
                    {
                        MoveNextPosition();
                    }
                    else
                {
                    character._state.ChangeState(eActorState.BaseAttack);
                }
                    
                }

          
        }
        void onExit()
        {
            isMoving = false;
            if(moveCoroutine!=null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
        void onUpdate()
        {
          
        }

        bool NormalMode;
        /// <summary>
        /// normalmode는 메인모드인지 던전모드인지
        /// </summary>
        /// <param name="main=true,dungeon=false"></param>
        public virtual void ResetPosition(bool normalmode)
        {
            NormalMode = normalmode;
            if (character!=null)
                character._state.ChangeState(eActorState.Idle);
            if (NormalMode)
                MainPositionControll.CurrentPlayerPositionList = Common.InGameManager.Instance.PlayerPositionList;
            else
                MainPositionControll.CurrentPlayerPositionList = Common.InGameManager.Instance.DungeonPlayerPositionList;

            Common.InGameManager.Instance.CameraSwitch(false);
            
            transform.position = MainPositionControll.CurrentPlayerPositionList[0].position;

            Common.InGameManager.Instance.BGPositionReset();
        }

        protected virtual void MoveNextPosition()
        {
            if(NormalMode)
            {
                Common.InGameManager.Instance.GetPlayerData.CurrentWave += 1;

                if (Common.InGameManager.Instance.GetPlayerData.CurrentWave > Common.InGameManager.Instance.GetPlayerData.stage_Info.MaxWave())
                {
                    //character._state.ChangeState(eActorState.EventAfterKillBoss);
                    Common.InGameManager.Instance.WaveInfoUpdate();
                    return;
                }
            }
            else
            {
                Common.InGameManager.Instance.GetPlayerData.CowWave += 1;
                

                if (Common.InGameManager.Instance.GetPlayerData.CowWave > Common.InGameManager.Instance.GetPlayerData.stage_Info.MaxWave())
                {
                    character._state.ChangeState(eActorState.Idle);
                    Common.InGameManager.Instance.CowWaveInfoUpdate();
                    return;
                }
            }

            character._Skeletonanimator.state.SetAnimation(0, "run", true);
            character.CharacterPetChangeAnim("run");
            if (CharacterDataManager.Instance.PlayerCharacterdata == null)
                return;

            
            moveSpeed = (float)(CharacterDataManager.Instance.PlayerCharacterdata.ability.GetMoveSpeed());

            if(moveCoroutine==null)
            {
                moveCoroutine = StartCoroutine(WaveMove());
                
            }
                
        }
      
        IEnumerator WaveMove()
        {
            isMoving = true;
            if (tempMovespeed != SpeedForChange)
            {
                tempMovespeed = SpeedForChange;
            }

            character._Skeletonanimator.timeScale = moveSpeed * tempMovespeed;
            //만약 최대웨이브면 다시 리포지셔닝 될떄까지 기다리기
            //카메라 전환될때의 포지션 인덱스
            int positionindex_;
            if(NormalMode)
                positionindex_ = Common.InGameManager.Instance.GetPlayerData.CurrentWave;
            else
                positionindex_ = Common.InGameManager.Instance.GetPlayerData.CowWave;
            bool BossEventScene = true;
           
                //if(Common.InGameManager.Instance.InfiniteMode==false)
                //{
                //    if (positionindex_ == Common.InGameManager.Instance.GetPlayerData.stage_Info.MaxWave() && NormalMode)
                //    {
                //        character._Skeletonanimator.state.SetAnimation(0, "idle_1", true);
                //        Common.InGameManager.Instance.CameraSwitch(true);
                //        Message.Send<UI.Event.MainSceneBossEvent>(new UI.Event.MainSceneBossEvent(() => BossEventScene = true));
                //    }
                //    else
                //        BossEventScene = true;
                //}
                //else
                //{
                //    BossEventScene = true;
                //}

                yield return new WaitUntil(() => BossEventScene == true);

            float Lerptime = 0;
            Vector3 _position = transform.position;
            //이동중
            while (Lerptime<1)
            {
                Lerptime += Time.deltaTime * tempMovespeed * moveSpeed;
                transform.position=Vector3.Lerp(_position, MainPositionControll.CurrentPlayerPositionList[positionindex_].position, Lerptime);
                yield return null;
            }
            if (Common.InGameManager.Instance.InfiniteMode == false)
            {
                if (positionindex_ == Common.InGameManager.Instance.GetPlayerData.stage_Info.MaxWave() && NormalMode)
                {
                    Message.Send<UI.Event.MainSceneBosshpPopupEvent>(new UI.Event.MainSceneBosshpPopupEvent());
                }
            }
            
            
            StopMoving();
        }

        public void StopMoving()
        {
         
            isMoving = false;
            moveCoroutine = null;

            Vector2 dir = (transform.localScale.x < 0) ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(DetectPosition.position, dir, distanceTotarget, _character.TargetLayer);

            if (hit.collider == null)
            {
                MoveNextPosition();
            }
            else
            {
                _character.CurrentEnemy = hit.collider.gameObject;
                _character.CurrentEnemyCharacter = _character.CurrentEnemy.GetComponent<Character>();
                if (_character.CurrentEnemyCharacter._health.CurrentHealth <= 0)
                {
                    MoveNextPosition();
                }
                else
                {
                    character._state.ChangeState(eActorState.BaseAttack);
                }
            }
        }

       

        
    }

}
