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
    /// Responsible for: move to closest chest and deploying crops
    /// </summary>
    public class AI_Farmer_DeployCropsState : AI_Farmer_BaseState
    {
        public override Job GetMyJob()
        {
            return Job.CropJob;
        }

        public override string Name()
        {
            return nameof(AI_Farmer_DeployCropsState);
        }

        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            Transform transform = dependencies.transform;
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "Deploying crops!", Color.yellow, duration: 1f);
            Chest closestChest = ChestsManager.instance.FindNearestChest(dependencies.idendity);
            if (closestChest == null)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                yield return null;
            }
            dependencies.MoveByPathfindingToDestination(destination: closestChest.transform.position,
                OnCompleteMoving:() =>
                {
                    dependencies.StartCoroutine(dependencies.inventory.inventorySlot.DeployCrops(target: transform, onComplete: () =>
                    {
                        dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                    }, popText: true));
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
