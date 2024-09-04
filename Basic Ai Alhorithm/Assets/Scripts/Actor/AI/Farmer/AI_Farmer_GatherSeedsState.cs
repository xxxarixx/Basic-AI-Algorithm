using CropField;
using General.Essencial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_GatherSeedsState : AI_Farmer_BaseState
    {
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "Gathering seeds!", Color.white, duration: 1f);
            Astar.Brain.AstarPathfinding pathfinding = dependencies.pathfinding;
            Transform transform = dependencies.transform;
            AI_Farmer_StateManager stateManager = dependencies.stateManager;

            dependencies.MoveByPathfindingToDestination(LevelManager.instance.deployGatherChest.position,
            OnCompleteMoving: () =>
            {
                int cropEmptyGroundsCount = CropField_Manager.instance.GetEmptyCropGroundsCount(dependencies.idendity);
                Debug.Log(cropEmptyGroundsCount);
                if (cropEmptyGroundsCount > 0)
                {
                    dependencies.StartCoroutine(LevelManager.instance.GiveSeedsToFarmer(dependencies.inventory, onComplete:() =>
                    {
                        stateManager.SetState(stateManager.state_FindEmptyCropGround);
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
