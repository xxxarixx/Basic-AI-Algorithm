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
    /// <summary>
    /// Responsible for: finding empty seed ground and moving to it
    /// </summary>
    public class AI_Farmer_FindEmptySeedGroundState : AI_Farmer_BaseState
    {
        public override Job GetMyJob()
        {
            return Job.SeedJob;
        }

        public override string Name()
        {
            return nameof(AI_Farmer_FindEmptySeedGroundState);
        }
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            AI_Farmer_StateManager stateManager = dependencies.stateManager;
            AstarPathfinding pathfinding = dependencies.pathfinding;
            Transform transform = stateManager.transform;
            if (dependencies.inventory.inventorySlot.HasAnyCrops)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_deployCrops);
                yield return null;
            }
            var foundedEmptyGround = FindEmptyCropSpace(dependencies, out bool isNull);
            if(isNull)
            {
                stateManager.SetState(stateManager.state_waitForNewWork);
                yield return null;
            }
            TextPopup(transform.position + new Vector3(0f, 10f, 0f), $"Searching for crop holes", Color.white, duration: 0.5f);
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
        
        private Vector3 FindEmptyCropSpace(AI_Farmer_Dependencies dependencies, out bool isNull)
        {
            isNull = false;
            var transform = dependencies.transform;
            CropHole foundedClosestEmptyCropGround = CropField_Manager.instance.GetClosestEmptyCropGround(dependencies.idendity, out int currentCount);
            if(foundedClosestEmptyCropGround == null)
            {
                isNull = true;
                return default;
            }

            if(!dependencies.idendity.AssignFarmerToHole(ref foundedClosestEmptyCropGround))
            {
                isNull = true;
                return default;
            }
            return foundedClosestEmptyCropGround.worldLocation;
        }
    }
}
