using BlackTree.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class CharacterStateMachineRunner : MonoBehaviour
    {
		IStateCallbackListener statemachine;
		public void Initialize(IStateCallbackListener _statemachine)
		{
			statemachine = _statemachine;
		}

		private void Update()
		{
			if (statemachine == null)
				return;

			if (Common.InGameManager.Instance.IsMainGameStart == false)
				return;

			statemachine.stateCallback?.OnUpdate?.Invoke();
		}
	}

}
