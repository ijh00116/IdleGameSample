using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using BlackTree.Common;
using DLL_Common.Common;
using System;
using Spine.Unity;
using Spine;

namespace BlackTree.InGame
{
    public class CharacterUseSkill : CharacterAbility, IStateCallback
    {
        [SerializeField]protected eActorState Mystate;
        [SerializeField] Transform WaitSkillpos;
        public Action OnEnter => onEnter;
        public Action OnExit => onExit;
        public Action OnUpdate => onUpdate;

        protected MeshRenderer meshRenderer;
        protected ActiveBaseSkill CurrentSkill;

        public Dictionary<SkillType, ActiveSkillBaseEffect> skillEfeectList = new Dictionary<SkillType, ActiveSkillBaseEffect>();
        public Dictionary<string, GameObject> skillSubEffectList = new Dictionary<string, GameObject>();


        protected override void Start()
        {
            base.Start();
            if (meshRenderer == null)
                meshRenderer = _character._Skeletonanimator.GetComponent<MeshRenderer>();
            _State.AddState(Mystate, this);

            Message.AddListener<UI.Event.UsingSkillButtonPush>(UseSkillForButtonpush);
            CreateSkillEfeect();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Message.RemoveListener<UI.Event.UsingSkillButtonPush>(UseSkillForButtonpush);
        }

        protected virtual void onEnter()
        {
            UseSkill();
        }
        protected virtual void onUpdate()
        {
            if(CurrentSkill.IsSkillEnd==true)
            {
                _State.ChangeState(eActorState.BaseAttack);
            }
        }

        protected virtual void onExit()
        {

        }

        protected void UseSkill()
        {
            CurrentSkill.UseSkill(_character);
        }

        protected void CreateSkillEfeect()
        {
            List<ActiveSkillInfo> activeSkilltableList;
            activeSkilltableList = InGameDataTableManager.RelicList.active_skill;
            for (int i = 0; i < activeSkilltableList.Count; i++)
            {
                SkillType st = activeSkilltableList[i].skill_type;
                string SkillResourcePath = string.Format("Prefabs/Skills/{0}", st.ToString());
                var obj = Resources.Load<GameObject>(SkillResourcePath);
                if(obj!=null)
                {
                    var _object = Instantiate(obj) as GameObject;
                    _object.SetActive(false);
                    if(st==SkillType.SKILL_USE_LIGHTNING_SLASH ||st==SkillType.SKILL_USE_BUFF_ATK_MOVE_SPEED)
                        _object.transform.SetParent(transform, false);
                    ActiveSkillBaseEffect baseEffect = _object.GetComponent<ActiveSkillBaseEffect>();
                    //baseEffect.ActivateEffect(CurrentSkill);
                    skillEfeectList.Add(st, baseEffect);
                }
            }
        }

        public void InstantiateSkillEffect(string Skillresourcepath,SkillType _type)
        {
            if(skillEfeectList.ContainsKey(_type)==false)
            {
                ActiveSkillBaseEffect obj = Resources.Load<ActiveSkillBaseEffect>(Skillresourcepath);
                ActiveSkillBaseEffect _object = Instantiate(obj);
                _object.transform.SetParent(transform, false);
                _object.ActivateEffect(CurrentSkill);
                skillEfeectList.Add(_type, _object);
            }
            else
            {
                ActiveSkillBaseEffect baseEffect =skillEfeectList[_type];
                baseEffect.gameObject.SetActive(true);
                baseEffect.ActivateEffect(CurrentSkill);
            }
        }

        public void InstantiateSubSkillEffect(string effectkey,string skillResourcepah)
        {
            GameObject effectobj;
            if(skillSubEffectList.ContainsKey(effectkey))
            {
                effectobj = skillSubEffectList[effectkey];
            }
            else
            {
                var obj = Resources.Load<GameObject>(skillResourcepah);
                effectobj = Instantiate(obj);
                skillSubEffectList.Add(effectkey, effectobj);
            }
            effectobj.SetActive(true);
            if (effectkey== "SkillSubResourcePath_0")
            {
                effectobj.transform.position = WaitSkillpos.transform.position;
                effectobj.transform.GetChild(0).transform.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "ready", false);
            }
            else
            {
                skillSubEffectList["SkillSubResourcePath_0"].SetActive(false);
                effectobj.transform.position = _character.CurrentEnemy.transform.position;
                effectobj.transform.GetChild(0).transform.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "explosion", false);
            }
        }

        protected virtual void UseSkillForButtonpush(UI.Event.UsingSkillButtonPush msg)
        {
            if (msg.charactertype != _character.playertype)
                return;
            if (_character.TargetDead == true)
                return;
            

            if (InGameManager.Instance.SpecialSkillInventory.ActiveSkills.ContainsKey(msg.skillType))
            {
                CurrentSkill = InGameManager.Instance.SpecialSkillInventory.ActiveSkills[msg.skillType].specialskilldata;
            }
            if (CurrentSkill == null)
                return;
           
            _character._state.ChangeState(eActorState.Skill);
        }
    }

}
