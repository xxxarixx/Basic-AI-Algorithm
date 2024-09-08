using CropField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_FindGrownCrop : AI_Farmer_BaseState
    {
        public override Job GetMyJob()
        {
            return Job.CropJob;
        }

        public override string Name()
        {
            return nameof(AI_Farmer_FindGrownCrop);
        }

        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            if(dependencies.inventory.inventorySlot.plantType == null)
            {
                TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "Searching for any grown crops", Color.yellow, duration: 1f);
            }
            else
            {
                TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), $"Searching for {dependencies.inventory.inventorySlot.plantType.name} grown crop", dependencies.inventory.inventorySlot.plantType.endCropColor, duration: 1f);
            }
            Transform transform = dependencies.transform;
            if(dependencies.inventory.inventorySlot.HasAnySeeds)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_deploySeeds);
                yield return null;
            }
            var closestCrop = CropField_Manager.instance.GetClosestFullGrownCrop(dependencies.idendity, dependencies.inventory.inventorySlot.plantType, out int grownCropsCount);
            if (closestCrop == null && grownCropsCount == 0)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_deployCrops);
                yield return null;
            }
            else
            {
                dependencies.idendity.AssignFarmerToHole(ref closestCrop);
                //move to closest crop
                dependencies.MoveByPathfindingToDestination(destination:closestCrop.worldLocation,
                OnCompleteMoving:() =>
                {
                    //gather crop
                    dependencies.stateManager.SetState(dependencies.stateManager.state_gatherCrop);
                },
                OnFailed:() =>
                {
                    dependencies.idendity.RemoveAssignFarmerToHole();
                    dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                });
               /* if (dependencies.idendity.AssignFarmerToHole(closestCrop))
                {
                }
                else
                {
                    dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                    yield return null;
                }*/
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

