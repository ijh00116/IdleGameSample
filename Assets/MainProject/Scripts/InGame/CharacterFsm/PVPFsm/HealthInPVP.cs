using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.Common;
using DLL_Common.Common;


namespace BlackTree.InGame
{
    public class HealthInPVP : Health
    {
        [SerializeField] bool hpBarExist;
        CharacterHitInPVP characterhit;


        protected override void Start()
        {
            base.Start();
            characterhit = this.GetComponent<CharacterHitInPVP>();
            _character = this.GetComponent<CharacterInPVP>();

            if (hpBarExist)
            {
                var hpbar = Instantiate(Healthpanel);
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

        //추후 맞았을때 이펙트 효과 등등 추가(맞을때의 무적처리,빨간색 점멸,파티클 등등) 물론 발동시키는건 얘가하지만 발동 되는 함수는 다른데서 처리하고
        public override void Damage(BigInteger damagecaused, bool critical)
        {
            IsCritical = critical;
            Damage(damagecaused);
        }


        protected override void Damage(BigInteger damagecaused)
        {
            if (CurrentHealth <= 0)
                return;

            CurrentHealth -= damagecaused;
            if (characterhit == null)
                return;

            BigInteger rate = CurrentHealth / InitHealth;
            hpbarCallback(rate.ToFloat());

            var damageui = Instantiate(DamagePanel);
            damageui.transform.SetParent(UIPosition);
            damageui.transform.localPosition = Vector3.zero;

            DamageWorldUI damageuicomponent = damageui.GetComponent<DamageWorldUI>();
            damageuicomponent.Setting(IsCritical, damagecaused);

            if (CurrentHealth > 0)
            {
                //_character._state.ChangeState(eActorState.Hit);
                //_character.GetComponent<CharacterHitInPVP>().Hit();
            }
            else
            {
                CurrentHealth = -1;
                //주거씀
                //_character._state.ChangeState(eActorState.Dead);
            }

            float pvpslidervalue = (damagecaused.ToFloat()) / (InitHealth.ToFloat());

            Message.Send<UI.Event.pvpHpDamgaged>(new UI.Event.pvpHpDamgaged(_character.playertype, pvpslidervalue));
        }

    }

}