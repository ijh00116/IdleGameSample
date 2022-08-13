using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackTree.State;

namespace BlackTree.InGame
{
    public class CharacterAbility : MonoBehaviour
    {
        protected Character _character=null;
        protected StateMachine<eActorState> _State;
        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            _character = GetComponent<Character>();
            _State = _character._state;
        }

        protected virtual void BindAnimator()
        {
      
        }

        protected virtual void InitializeAnimatorParameter()
        {

        }

        protected virtual void RegisterAnimatorParameter(string parametername, AnimatorControllerParameterType paramtype, out int parameter)
        {
            parameter = Animator.StringToHash(parametername);
           
        }

        protected virtual void OnDestroy()
        {

        }
        // Update is called once per frame
        protected void Update()
        {
            Process();
        }

        //Awake animatorsetting
   
        void Process()
        {
            InputHandling();
            Animating();
            ProcessAbility();
        }

        protected virtual void InputHandling()
        {

        }

        protected virtual void Animating()
        {

        }

        protected virtual void ProcessAbility()
        {

        }
    }

}
