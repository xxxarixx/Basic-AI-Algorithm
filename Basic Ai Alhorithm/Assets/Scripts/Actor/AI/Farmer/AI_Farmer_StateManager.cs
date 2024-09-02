using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar.Brain;
namespace AI.Farmer
{
    public class AI_Farmer_StateManager : MonoBehaviour
    {
        public ActorStatistics actorStatistics;
        public AstarPathfinding pathfinding;
        public AI_Farmer_BaseState curState { get; private set; }
        public AI_Farmer_FindEmptyCropGroundState state_FindEmptyCropGround { get; private set; } = new AI_Farmer_FindEmptyCropGroundState();
        public AI_Farmer_PlantState state_plantSeed { get; private set; } = new AI_Farmer_PlantState();
        private void Start()
        {
            SetState(state_FindEmptyCropGround);
        }
        public void SetState(AI_Farmer_BaseState newState)
        {
            //call last state OnExitMethod
            if(curState != null)
            {
                curState.OnExitState(stateManager:this);
                StopCoroutine(curState.OnEnterState(stateManager: this));
            }
            //call new state OnEnterMethod
            curState = newState;
            StartCoroutine(curState.OnEnterState(stateManager:this));
        }
        private void Update()
        {
            if (curState == null)
                return;
            curState.OnUpdateState(stateManager: this);
        }
        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {
            
        }
    }
    
}