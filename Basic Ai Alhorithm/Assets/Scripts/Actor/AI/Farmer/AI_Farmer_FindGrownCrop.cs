using CropField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_FindGrownCrop : AI_Farmer_BaseState
    {
        public override string Name()
        {
            return nameof(AI_Farmer_FindGrownCrop);
        }

        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "finding crops!", Color.yellow, duration: 1f);
            Transform transform = dependencies.transform;
            if(dependencies.inventory.inventorySlot.HasAnySeeds)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_deploySeeds);
                yield return null;
            }
            var closestCrop = CropField_Manager.instance.GetClosestFullGrownCrop(dependencies.idendity, dependencies.inventory.inventorySlot.plantType, out int grownCropsCount);
            if (closestCrop == null && grownCropsCount == 0)
            {
                var allCropsCount = CropField_Manager.instance.GetAllFullyGrownCropsCount(dependencies.idendity);
                if(allCropsCount > 0) // there is no more my type of plants in map
                {
                    dependencies.stateManager.SetState(dependencies.stateManager.state_deployCrops);
                    yield return null;
                }
                else
                {
                    //there is no crops left, deploy left crops
                    dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                    yield return null;
                }
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

