using Astar.Brain;
using General.Essencial;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
namespace AI.Farmer
{
    [RequireComponent(typeof(AI_Farmer_Dependencies))]
    public class AI_Farmer_StateManager : MonoBehaviour
    {
        public event Action<AI_Farmer_BaseState> OnChangeState;
        public AI_Farmer_Dependencies dependencies { get; private set; }
        public AI_Farmer_BaseState curState { get; private set; }
        public AI_Farmer_FindEmptyCropGroundState state_FindEmptyCropGround { get; private set; } = new AI_Farmer_FindEmptyCropGroundState();
        public AI_Farmer_FindGrownCrop state_FindGrownCrops { get; private set; } = new AI_Farmer_FindGrownCrop();
        public AI_Farmer_PlantSeedState state_plantSeed { get; private set; } = new AI_Farmer_PlantSeedState();
        public AI_Farmer_GatherCropsState state_gatherCrop { get; private set; } = new AI_Farmer_GatherCropsState();
        public AI_Farmer_WaitForNewWork state_waitForNewWork { get; private set; } = new AI_Farmer_WaitForNewWork();
        public AI_Farmer_DeployCropsState state_deployCrops { get; private set; } = new AI_Farmer_DeployCropsState();
        public AI_Farmer_DeploySeedsState state_deploySeeds { get; private set; } = new AI_Farmer_DeploySeedsState();
        public AI_Farmer_GatherSeedsState state_gatherSeeds { get; private set; } = new AI_Farmer_GatherSeedsState();
        private Coroutine lastStartState;
        private void Start()
        {
            dependencies = GetComponent<AI_Farmer_Dependencies>();
        }
        private void OnEnable()
        {
            GameManager.instance.onEverythingSetupped += OnGameReady;
        }

        private void OnDisable()
        {
            GameManager.instance.onEverythingSetupped -= OnGameReady;
        }
        private void OnGameReady()
        {
            SetState(state_gatherSeeds);
        }

        public void SetState(AI_Farmer_BaseState newState)
        {
            //call last state OnExitMethod
            if(curState != null)
            {
                curState.OnExitState(dependencies:dependencies);
                
                if(lastStartState != null) 
                    StopCoroutine(lastStartState);
            }
            //call new state OnEnterMethod
            curState = newState;
            OnChangeState?.Invoke(newState);
            lastStartState = StartCoroutine(curState.OnEnterState(dependencies:dependencies));
        }
        private void Update()
        {
            if (curState == null)
                return;
            curState.OnUpdateState(dependencies: dependencies);
        }
    }
    
}