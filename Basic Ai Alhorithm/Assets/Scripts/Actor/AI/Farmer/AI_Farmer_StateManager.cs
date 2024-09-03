using UnityEngine;
namespace AI.Farmer
{
    [RequireComponent(typeof(AI_Farmer_Dependencies))]
    public class AI_Farmer_StateManager : MonoBehaviour
    {
        public AI_Farmer_Dependencies dependencies { get; private set; }
        public AI_Farmer_BaseState curState { get; private set; }
        public AI_Farmer_FindEmptyCropGroundState state_FindEmptyCropGround { get; private set; } = new AI_Farmer_FindEmptyCropGroundState();
        public AI_Farmer_FindGrownCrop state_FindGrownCrops { get; private set; } = new AI_Farmer_FindGrownCrop();
        public AI_Farmer_PlantState state_plantSeed { get; private set; } = new AI_Farmer_PlantState();
        public AI_Farmer_GatherCropsState state_gatherCrop { get; private set; } = new AI_Farmer_GatherCropsState();
        public AI_Farmer_WaitForNewWork state_waitForNewWork { get; private set; } = new AI_Farmer_WaitForNewWork();
        public AI_Farmer_DeployCropsState state_deployCrops { get; private set; } = new AI_Farmer_DeployCropsState();
        public AI_Farmer_DeploySeedsState state_deploySeeds { get; private set; } = new AI_Farmer_DeploySeedsState();
        public AI_Farmer_GatherSeedsState state_gatherSeeds { get; private set; } = new AI_Farmer_GatherSeedsState();
        private void Start()
        {
            dependencies = GetComponent<AI_Farmer_Dependencies>();
            SetState(state_gatherSeeds);
        }
        public void SetState(AI_Farmer_BaseState newState)
        {
            //call last state OnExitMethod
            if(curState != null)
            {
                curState.OnExitState(dependencies:dependencies);
                StopCoroutine(curState.OnEnterState(dependencies: dependencies));
            }
            //call new state OnEnterMethod
            curState = newState;
            StartCoroutine(curState.OnEnterState(dependencies:dependencies));
        }
        private void Update()
        {
            if (curState == null)
                return;
            curState.OnUpdateState(dependencies: dependencies);
        }
    }
    
}