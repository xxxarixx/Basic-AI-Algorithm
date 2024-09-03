using Astar.Brain;
using CropField;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DebugHelper.DebugHelper;
using Random = UnityEngine.Random;
namespace AI.Farmer
{
    public class AI_Farmer_FindEmptyCropGroundState : AI_Farmer_BaseState
    {
       
       
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            AI_Farmer_StateManager stateManager = dependencies.stateManager;
            AstarPathfinding pathfinding = dependencies.pathfinding;
            Transform transform = stateManager.transform;
            TextPopup(transform.position + new Vector3(0f, 10f, 0f), "Searching for crop holes", Color.white, duration: 0.5f);
            dependencies.MoveByPathfindingToDestination(FindEmptyCropSpace(stateManager),
            OnCompleteMoving: () =>
            {
                stateManager.SetState(stateManager.state_plantSeed);
            });
            yield return null;
        }

        public override void OnExitState(AI_Farmer_Dependencies dependencies)
        {
            
        }

        public override void OnUpdateState(AI_Farmer_Dependencies dependencies)
        {
            
        }
        
        private Vector3 FindEmptyCropSpace(AI_Farmer_StateManager stateManager)
        {
            if (CropField_Manager.instance.cropFields.Count == 0)
                return default;
            return CropField_Manager.instance.GetClosestEmptyCropGroundPosition(stateManager.transform);
        }
    }
}
