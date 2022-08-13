using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class ChangeActorStateMachine<T> : Message where T : struct
    {
        public GameObject Target;
        public ActorStateMachine<T> statemachine;
        public StateCallback previousState;
        public StateCallback CurrentState;
        public ChangeActorStateMachine(ActorStateMachine<T> _statemachine)
        {
            statemachine = _statemachine;
            Target = _statemachine.target;
            previousState = _statemachine.PreviousState;
            CurrentState = _statemachine.currentState;
        }
    }

    public class ActorStateMachine<T> : IStateMachine where T:struct
    {
        public bool Triggerevent;
        public GameObject target;

        public StateCallback PreviousState;
        public StateCallback currentState;
        public Dictionary<T, StateCallback> stateLookup;

        MonoBehaviour _component;

        public ActorStateMachineRunner engine;

        public ActorStateMachine(ActorStateMachineRunner _engine,MonoBehaviour component)
        {
            engine = _engine;
            _component = component;
        }

        public StateCallback stateCallback
        {
            get { return currentState; }
        }

        public void RegisterCallback()
        {
            IRegistStateCallback register = _component.GetComponent<IRegistStateCallback>();
            if (register != null)
            {
                register.RegistCallback();
            }
        }

        public void ChangeState(T state)
        {
            currentState?.OnExit?.Invoke();

            PreviousState = currentState;
            if (stateLookup.ContainsKey(state))
            {
                currentState = stateLookup[state];
            }

            currentState.OnEnter?.Invoke();
        }

        public static ActorStateMachine<T> Initialize(MonoBehaviour component)
        {
            var engin = component.GetComponent<ActorStateMachineRunner>();
            if (engin == null)
                engin = component.gameObject.AddComponent<ActorStateMachineRunner>();

            return engin.Initialize<T>(component);
        }
    }

}
