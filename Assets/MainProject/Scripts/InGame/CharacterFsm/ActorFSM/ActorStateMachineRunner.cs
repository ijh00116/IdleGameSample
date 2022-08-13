using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class ActorStateMachineRunner : MonoBehaviour
    {
		IStateMachine statemachine;
		public ActorStateMachine<T> Initialize<T>(MonoBehaviour component) where T : struct
		{
			var fsm = new ActorStateMachine<T>(this, component);

			statemachine = fsm;

			return fsm;
		}

		private void Update()
		{
			statemachine.stateCallback?.Update?.Invoke();
		}
	}

}
