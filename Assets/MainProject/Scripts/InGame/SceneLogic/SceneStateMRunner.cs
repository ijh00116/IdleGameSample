using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
	public class SceneStateMRunner : MonoBehaviour
	{
		IStateMachine statemachine;
		public SceneStateMachine<T> Initialize<T>(MonoBehaviour component) where T : struct
		{
			var fsm = new SceneStateMachine<T>(this, component);

			statemachine = fsm;

			return fsm;
		}

		private void Update()
		{
			statemachine.stateCallback?.Update?.Invoke();
		}

	}

}
