using CropField;
using General.Essencial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_DeploySeedsState : AI_Farmer_BaseState
    {
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            Transform transform = dependencies.transform;
            TextPopup(transform.position + new Vector3(0f, 10f, 0f), "Deploying seeds!", Color.green, duration: 1f);
            Debug.Log("Deploy seeds state");
            dependencies.MoveByPathfindingToDestination(destination: LevelManager.instance.deployGatherChest.transform.position,
                OnCompleteMoving: () =>
                {
                    //start deploing
                    dependencies.StartCoroutine(dependencies.inventory.inventorySlot.DeploySeeds(target:transform, onComplete:() =>
                    {
                        int cropEmptyGroundsCount = CropField_Manager.instance.GetEmptyCropGroundsCount();
                        int cropFullyGrownCount = CropField_Manager.instance.GetFullyGrownCropsCount();
                        //think what will be more usefull
                        if(cropFullyGrownCount > cropEmptyGroundsCount)
                        {
                            //deployed everything
                            dependencies.stateManager.SetState(dependencies.stateManager.state_FindGrownCrops);
                        }
                        else
                        {
                            dependencies.stateManager.SetState(dependencies.stateManager.state_gatherSeeds);
                        }
                    },popText:true));
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
