using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;
using Spine.Unity;
using Spine;

namespace BlackTree.InGame
{
    public class CharacterInPVP : Character
    {
        public CharacterType enemytype;

        public UserInfoForPVP userInfo;
        protected override void Awake()
        {
            _health = GetComponent<Health>();
            _state = new StateMachine<eActorState>(this.gameObject, true);
        }

        public void Init()
        {
            this.GetComponent<CharacterShapeInPVP>().ShapeChange(userInfo);

            CharacterUseSkillInPVP pvp = this.GetComponent<CharacterUseSkillInPVP>();
            if(pvp!=null)
            {
                foreach(var _data in pvp.skillEfeectList)
                {
                    if(_data.Value.gameObject!=null)
                        Destroy(_data.Value.gameObject);
                }
                pvp.skillEfeectList.Clear();
            }
        }
    }
}