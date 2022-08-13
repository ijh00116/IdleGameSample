using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;
using DLL_Common.Common;

namespace BlackTree
{
    public class LightneingSkillEffect : ActiveSkillBaseEffect
    {
     
        float CurrentTime=0;

        [SerializeField] Transform detectpos;

        [Header("이동속도")]
        [Range(0f,10.0f)]
        [SerializeField] float Speed=0.7f;
        [Header("같은대상에게 데미지 시간 간격(다른대상 나타날경우 보이자마자 첫회 바로 공격 들어감)")]
        [Range(0f, 10.0f)]
        [SerializeField] float DamageTime = 1.0f;
        [Header("사라지는 거리")]
        [Range(0f, 10.0f)]
        [SerializeField] float DistanceToDisappear = 6.0f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 target = new Vector3(detectpos.position.x + distanceTotarget, detectpos.position.y, detectpos.position.z);
            Gizmos.DrawLine(detectpos.position, target);
        }
        protected override void Process()
        {
            //이동중
            transform.Translate(Vector3.right * Time.deltaTime * Speed);

            Vector2 dir = (transform.localScale.x < 0) ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(detectpos.position, dir, distanceTotarget, TargetLayer);

            //
            if(Vector3.Distance(this.transform.position,Actor.transform.position)> DistanceToDisappear)
            {
                this.gameObject.SetActive(false);
            }
            //

            if (hit.collider == null)
            {
                if (CurrentEnemy != null)
                    CurrentEnemy = null;
                return;
            }
            else if (hit.collider.gameObject.GetComponent<Health>().CurrentHealth <= 0)
            {
                if(CurrentEnemy!=null)
                    CurrentEnemy = null;
                return;
            }
            else
            {
                if(CurrentEnemy!=null)
                {
                    if (CurrentEnemy != hit.collider.gameObject)
                    {
                        CurrentEnemy = hit.collider.gameObject;
                        CalculateProjectileData(CurrentEnemy);
                        CurrentTime = 0;
                    }
                    else
                    {
                        if (CurrentTime >= DamageTime)
                        {
                            CalculateProjectileData(CurrentEnemy);
                            CurrentTime = 0;
                        }
                    }
                }
                else
                {
                    CurrentEnemy = hit.collider.gameObject;
                    CalculateProjectileData(CurrentEnemy);
                    CurrentTime = 0;
                }
             
                Debug.Log("데미지 주는중 to " + CurrentEnemy.name);
            }

            if (CurrentEnemy != null)
                CurrentTime += Time.deltaTime;
            
        }

        void CalculateProjectileData(GameObject target)
        {
            BigInteger AtkDmg;

            if (Actor.playertype == CharacterType.Player|| Actor.playertype == CharacterType.PetPlayer)
            {
                AtkDmg = CharacterDataManager.Instance.PlayerCharacterdata.ability.GetAtkDamage()
                    * ((baseSkill.skillInfo.skill_ability + (baseSkill.specialskill.Level * baseSkill.skillInfo.skill_ability_level_up)) / 100.0f);
            }
            else
            {
                AtkDmg = (Actor as CharacterInPVP).userInfo.AbilityList[PVPAbilityType.CHA_SKILL_LIGHTNING_DAMGAGE];
            }
            Health targethealth = target.GetComponent<Health>();
            targethealth.Damage(AtkDmg, true);
        }

        public override void ActivateEffect(ActiveBaseSkill _skill)
        {
            base.ActivateEffect(_skill);
            this.transform.localPosition = new Vector3(0, 1.86f, 0);
            CurrentTime = DamageTime;
        }
    }

}
