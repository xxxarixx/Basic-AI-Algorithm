using System;

namespace AI.Farmer
{
	public abstract class AI_Farmer_BaseState
	{
		public abstract void OnEnterState(AI_Farmer_StateManager stateManager);
        public abstract void OnUpdateState(AI_Farmer_StateManager stateManager);
        public abstract void OnExitState(AI_Farmer_StateManager stateManager);
    }
}
