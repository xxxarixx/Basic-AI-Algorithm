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
        public AI_Farmer_PatrolState state_patrol { get; private set; } = new AI_Farmer_PatrolState();
        public List<Transform> farmsLocations = new List<Transform>();
        private void Start()
        {
            SetState(state_patrol);
        }
        public void SetState(AI_Farmer_BaseState newState)
        {
            //call last state OnExitMethod
            if(curState != null) 
                curState.OnExitState(stateManager:this);
            //call new state OnEnterMethod
            curState = newState;
            curState.OnEnterState(stateManager:this);
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