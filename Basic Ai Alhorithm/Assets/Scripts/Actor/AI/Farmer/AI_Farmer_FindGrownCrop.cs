using CropField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_FindGrownCrop : AI_Farmer_BaseState
    {
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "finding crops!", Color.yellow, duration: 1f);
            Transform transform = dependencies.transform;
            var closestCrop = CropField_Manager.instance.GetClosestFullGrownCrop(dependencies.idendity, dependencies.inventory.inventorySlot.currentHoldingCropOrSeed, out int grownCropsCount);
            if (closestCrop == null)
            {
                //there is no crops left, deploy left crops
                dependencies.stateManager.SetState(dependencies.stateManager.state_deployCrops);
            }
            else
            {
                dependencies.idendity.AssignFarmerToHole(closestCrop);
                //move to closest crop
                dependencies.MoveByPathfindingToDestination(destination:closestCrop.worldLocation,
                    OnCompleteMoving:() =>
                    {
                        //gather crop
                        dependencies.stateManager.SetState(dependencies.stateManager.state_gatherCrop);
                    });
            }
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

