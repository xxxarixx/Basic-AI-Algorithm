using System;
using System.Collections;

namespace AI.Farmer
{
    /// <summary>
    /// basic farmer states
    /// </summary>
	public abstract class AI_Farmer_BaseState
	{
		public abstract IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies);
        public abstract void OnUpdateState(AI_Farmer_Dependencies dependencies);
        public abstract void OnExitState(AI_Farmer_Dependencies dependencies);
        public abstract string Name();
        /// <summary>
        /// To what job is this state closest to
        /// </summary>
        public abstract Job GetMyJob();
        /// <summary>
        /// mainly used to calculate job popularity in AI_Farmer_Summary.cs
        /// </summary>
        public enum Job
        {
            None,
            SeedJob,
            CropJob
        }
    }
}
