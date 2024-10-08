using CropField;
using General.Containers;
using General.Essencial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    /// <summary>
    /// Responsible for: move to closest chest and gathering seeds from it
    /// </summary>
    public class AI_Farmer_GatherSeedsState : AI_Farmer_BaseState
    {
        public override Job GetMyJob()
        {
            return Job.SeedJob;
        }

        public override string Name()
        {
            return nameof(AI_Farmer_GatherSeedsState);
        }
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "Gathering seeds!", Color.white, duration: 1f);
            Astar.Brain.AstarPathfinding pathfinding = dependencies.pathfinding;
            Transform transform = dependencies.transform;
            AI_Farmer_StateManager stateManager = dependencies.stateManager;
            Chest closestChest = ChestsManager.instance.FindNearestChest(dependencies.idendity);
            if(closestChest == null)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                yield return null;
            }
            dependencies.MoveByPathfindingToDestination(closestChest.transform.position,
            OnCompleteMoving: () =>
            {
                int cropEmptyGroundsCount = CropField_Manager.instance.GetEmptyCropGroundsCount(dependencies.idendity);
                if (cropEmptyGroundsCount > 0)
                {
                    dependencies.StartCoroutine(closestChest.GiveSeedsToFarmer(dependencies.inventory, onComplete:() =>
                    {
                        stateManager.SetState(stateManager.state_FindEmptySeedGround);
                    }));
                }
                else
                {
                    stateManager.SetState(stateManager.state_waitForNewWork);
                }
            });
            yield return null;
        }

        public override void OnExitState(AI_Farmer_Dependencies dependencies)
        {
           
        }

        public override void OnUpdateState(AI_Farmer_Dependencies dependencies)
        {
            
        }
    }

}
