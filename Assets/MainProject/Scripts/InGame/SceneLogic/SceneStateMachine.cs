using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public interface IRegistStateCallback
    {
        void RegistCallback();
    }
    public class StateCallback
    {
        public Action OnEnter;
        public Action OnExit;
        public Action Update;
    }
    public interface IStateMachine
    {
        StateCallback stateCallback { get; }
    }

    public class ChangeSceneStateMachine<T> : Message where T : struct
    {
        public GameObject Target;
        public SceneStateMachine<T> statemachine;
        public StateCallback previousState;
        public StateCallback CurrentState;
        public ChangeSceneStateMachine(SceneStateMachine<T> _statemachine)
        {
            statemachine = _statemachine;
            Target = _statemachine.target;
            previousState = _statemachine.PreviousState;
            CurrentState = _statemachine.currentState;
        }
    }
    public class SceneStateMachine<T> : IStateMachine where T : struct
    {
        public bool Triggerevent;
        public GameObject target;

        public StateCallback PreviousState;
        public StateCallback currentState;
        public Dictionary<T, StateCallback> stateLookup;

        MonoBehaviour _component;

        public SceneStateMRunner engine;
        public SceneStateMachine(SceneStateMRunner _engine, MonoBehaviour component)
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

        public static SceneStateMachine<T> Initialize(MonoBehaviour component)
        {
            var engin = component.GetComponent<SceneStateMRunner>();
            if (engin == null)
                engin = component.gameObject.AddComponent<SceneStateMRunner>();

            return engin.Initialize<T>(component);
        }

        public bool IsCurrentState(T state)
        {
            if (stateLookup.ContainsKey(state))
            {
                return currentState == stateLookup[state];
            }
            else
                return false;

        }
    }

}
