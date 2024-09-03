using CropField;
using General.Essencial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_DeployCropsState : AI_Farmer_BaseState
    {
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            Transform transform = dependencies.transform;
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "Deploying crops!", Color.yellow, duration: 1f);
            dependencies.MoveByPathfindingToDestination(destination:LevelManager.instance.deployGatherChest.position,
                OnCompleteMoving:() =>
                {
                    dependencies.StartCoroutine(dependencies.inventory.inventorySlot.DeployCrops(target: transform, onComplete: () =>
                    {
                        int cropEmptyGroundsCount = CropField_Manager.instance.GetEmptyCropGroundsCount();
                        int cropFullyGrownCount = CropField_Manager.instance.GetFullyGrownCropsCount();
                        //think what will be more usefull
                        if (cropFullyGrownCount > cropEmptyGroundsCount)
                        {
                            //deployed everything
                            dependencies.stateManager.SetState(dependencies.stateManager.state_FindGrownCrops);
                        }
                        else
                        {
                            dependencies.stateManager.SetState(dependencies.stateManager.state_gatherSeeds);
                        }
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