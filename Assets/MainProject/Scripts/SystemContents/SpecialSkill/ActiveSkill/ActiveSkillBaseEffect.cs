using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.InGame;

namespace BlackTree
{
    public abstract class ActiveSkillBaseEffect : MonoBehaviour
    {
        public SkillType mySkilltype;
        [HideInInspector] public ActiveBaseSkill baseSkill;
        [HideInInspector] public Character Actor;
        [HideInInspector] public Character TargetActor;

        [Header("적감지")]
        public LayerMask TargetLayer;
        public float distanceTotarget;
        [HideInInspector]public GameObject CurrentEnemy;
        [SerializeField] Spine.Unity.SkeletonAnimation[] spindata;
        [SerializeField] bool[] SpinedataLoop;
        [SerializeField] string[] spineAnimName; 
        // Update is called once per frame
        void Update()
        {
            Process();
        }

        protected abstract void Process();

        public virtual void ActivateEffect(ActiveBaseSkill _skill)
        {
            this.gameObject.SetActive(true);
            for (int i=0; i<spindata.Length; i++)
            {
                spindata[i].AnimationState.SetAnimation(0, spineAnimName[i], SpinedataLoop[i]);
            }
        
            baseSkill = _skill;
            Actor = baseSkill.Actor;
            TargetActor = _skill.Target.GetComponent<Character>();
        }
    }

}
