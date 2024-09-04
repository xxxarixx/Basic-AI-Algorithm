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
            var foundedEmptyGround = FindEmptyCropSpace(dependencies);
            if(foundedEmptyGround == null)
            {
                stateManager.SetState(stateManager.state_waitForNewWork);
                yield return null;
            }
            TextPopup(transform.position + new Vector3(0f, 10f, 0f), $"Searching for crop holes, founded at:{foundedEmptyGround}", Color.white, duration: 0.5f);
            dependencies.MoveByPathfindingToDestination(foundedEmptyGround,
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
        
        private Vector3 FindEmptyCropSpace(AI_Farmer_Dependencies dependencies)
        {
            var transform = dependencies.transform;
            CropHole foundedClosestEmptyCropGround = CropField_Manager.instance.GetClosestEmptyCropGround(dependencies.idendity, out int currentCount);
            if(foundedClosestEmptyCropGround == null)
                return default;

            dependencies.idendity.AssignFarmerToHole(foundedClosestEmptyCropGround);
            return foundedClosestEmptyCropGround.worldLocation;
        }
    }
}
