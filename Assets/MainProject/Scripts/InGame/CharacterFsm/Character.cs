using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using Spine.Unity;
using Spine;

namespace BlackTree.InGame
{
    public enum CharacterType 
    {
        Player,
        Pet,
        PetPlayer,
        PvpPlayer,
        PvpEnemy,

        PetEnemy,

        Cow, Cowking,

        NormalMonster =100,
        Boss,Mimic,

        

        End
    }

    public class Character : MonoBehaviour
    {
        [Header("캐릭터 정보")]
        [SerializeField] public SkeletonAnimation _Skeletonanimator;
        public StateMachine<eActorState> _state;
        public CharacterType playertype;

        CharacterPetControll CharactersPet;

        [Header("적 감지 관련")]
        public LayerMask TargetLayer;
        [HideInInspector]public GameObject CurrentEnemy;
        [HideInInspector] public Character CurrentEnemyCharacter;
        public bool TargetDead 
        {
            get { return CurrentEnemy == null || CurrentEnemyCharacter._health.CurrentHealth < 0; }
        }

        public bool FacingRight;

        [Header("[테스트용입니다 출시때는 제거!]")]
        public bool Skill_Wrath=false;
        public bool Skill_Judge=false;
        public bool Critical = false;

        public Health _health;
        CharacterDead characterdead;
        EnemySkinChanger enemyskinchanger;

        protected virtual void Awake()
        {
            _state = new StateMachine<eActorState>(this.gameObject, true);

            CharactersPet = GetComponent<CharacterPetControll>();
            _health = GetComponent<Health>();
            characterdead = GetComponent<CharacterDead>();
            enemyskinchanger = GetComponent<EnemySkinChanger>();

            Message.AddListener<UI.Event.PetDungeonEnd>(InActivate);
            Message.AddListener<UI.Event.EnemyInActive>(InActiveCharacter);

            FacingRight = true;
            Vector3 localscale = transform.localScale;
            if ((int)playertype >= (int)CharacterType.NormalMonster)
            {
                FacingRight = false;
                if (localscale.x > 0) localscale.x *= -1;
                transform.localScale = localscale;
            }
        }
        protected void OnDestroy()
        {
            Message.RemoveListener<UI.Event.PetDungeonEnd>(InActivate);
            Message.RemoveListener<UI.Event.EnemyInActive>(InActiveCharacter);
        }

        public void CharacterPetChangeAnim(string animName)
        {
            if(CharactersPet!=null)
            {
                CharactersPet.ChangePetAnimation(animName);
            }
        }


        protected void InActivate(UI.Event.PetDungeonEnd msg)
        {
            if(playertype==CharacterType.PetEnemy)
            {
                //Message.Send<InGame.Event.EnemyKilled>(new InGame.Event.EnemyKilled(this.gameObject, playertype));
                this.gameObject.SetActive(false);
            }
        }

        protected void InActiveCharacter(UI.Event.EnemyInActive msg)
        {
            if(playertype==msg.charactertype)
            {
                if(this.gameObject.activeInHierarchy==true)
                {
                   // Message.Send<InGame.Event.EnemyKilled>(new InGame.Event.EnemyKilled(this.gameObject, playertype));
                    this.gameObject.SetActive(false);
                }
            }
        }
        public void EnemyOnenableSet()
        {
            if (enemyskinchanger != null)
                enemyskinchanger.SetSkindata();
            if (characterdead != null)
            {
                if(characterdead.ShadowImage!=null)
                {
                    characterdead.ShadowImage.color = Color.white;
                }
            }
                
        }
    }

}
