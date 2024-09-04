using System;
using System.Collections;

namespace AI.Farmer
{
	public abstract class AI_Farmer_BaseState
	{
		public abstract IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies);
        public abstract void OnUpdateState(AI_Farmer_Dependencies dependencies);
        public abstract void OnExitState(AI_Farmer_Dependencies dependencies);
    }
}
