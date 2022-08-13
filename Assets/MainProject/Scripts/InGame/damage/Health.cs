using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;
using DLL_Common.Common;
using Spine.Unity;

namespace BlackTree.InGame
{
    public enum HitType
    {
        Normal,
        Critical,
        Skill,
    }
    public class Health : CharacterAbility
    {
        public BigInteger InitHealth;

        public BigInteger CurrentHealth;
        public float Rate;
        public int currenthp;

        [Header("UI오브젝트")]
        public GameObject Healthpanel;
        public GameObject DamagePanel;
        [SerializeField] bool hpbarExist=true;
        [SerializeField] protected Transform UIPosition;
        GameObject hpbar;

        //보상데이터
    
        public bool IsCritical;
        //스킬데미지 들어올때
        public HitType hitType;

        //hpbar 콜백
        public delegate void hpbarEvent(float hp);
        public hpbarEvent hpbarCallback;

        //[BTReadOnly]
        //public string Currenthp { get { return CurrentHealth.ToString(); } }
        protected override void Start()
        {
            base.Start();
            if (hpbarExist)
            {
                hpbar = Instantiate(Healthpanel);
                hpbar.transform.SetParent(this.transform, false);
                hpbar.transform.position = UIPosition.position;
                hpbarCallback+= hpbar.GetComponentInChildren<UI.HealthPanel>().HandleHealthChanged;
            }
           
        }

        private void OnEnable()
        {
            InitHealth = new BigInteger(100);
            CurrentHealth = InitHealth;
        }

        public void SettingHealth(BigInteger hp)
        {
            if (hpbar != null)
                hpbar.SetActive(true);
            InitHealth = hp;
            CurrentHealth = InitHealth;
            Rate = 1.0f;
        }

        protected override void Animating()
        {
            if(_State != null)
            {
              
            }
        }

        protected override void ProcessAbility()
        {
            currenthp = (int)(CurrentHealth.ToFloat());
            if (CurrentHealth<=0)
            {
               
            }
            if(_State != null)
            {
               
            }
        }

        //추후 맞았을때 이펙트 효과 등등 추가(맞을때의 무적처리,빨간색 점멸,파티클 등등) 물론 발동시키는건 얘가하지만 발동 되는 함수는 다른데서 처리하고
        public virtual void Damage(BigInteger damagecaused,bool critical)
        {
            if(critical==false)
                hitType = HitType.Normal;
            else
                hitType = HitType.Critical;

            IsCritical = critical;
            if(this.gameObject.activeInHierarchy)
                Damage(damagecaused);
        }
    

        protected virtual void Damage(BigInteger damagecaused)
        {
            if (CurrentHealth <= 0)
                return;

            CurrentHealth -= damagecaused;

            BigInteger rate = CurrentHealth / InitHealth;
            Rate = rate.ToFloat();
            hpbarCallback(Rate);

            var damageui = Instantiate(DamagePanel);
            damageui.transform.SetParent(UIPosition,false);
            damageui.transform.localPosition = Vector3.zero;

            DamageWorldUI damageuicomponent = damageui.GetComponent<DamageWorldUI>();
            damageuicomponent.Setting(IsCritical, damagecaused);
          

            this.GetComponent<CharacterHit>().HitEffectInstantiate();
            if (CurrentHealth > 0)
            {
                _State.ChangeState(eActorState.Hit);
            }
            else
            {
                if (hpbar != null)
                    hpbar.SetActive(false);
                CurrentHealth = -1;
                //주거씀
                _State.ChangeState(eActorState.Dead);
              
            }
        }

        public float GetPercentageHealth
        {
            get { return (CurrentHealth / InitHealth).ToFloat(); }
        }

    }

}
