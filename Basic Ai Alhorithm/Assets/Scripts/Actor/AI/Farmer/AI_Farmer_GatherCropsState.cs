using CropField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_GatherCropsState : AI_Farmer_BaseState
    {
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            Transform transform = dependencies.transform;
            CropHole closestCropHole = CropField_Manager.instance.GetClosestFullGrownCrop(dependencies.idendity, dependencies.inventory.inventorySlot.currentHoldingCropOrSeed, out int grownCropsCount);
            if (closestCropHole == null && dependencies.inventory.inventorySlot.HasAnySeedsOrCrops)
            {
                //there was a mistake, shouldnt harvest crop
                dependencies.stateManager.SetState(dependencies.stateManager.state_deployCrops);
                yield return null;
            }
            yield return new WaitForSeconds(AI_Farmer_Dependencies.timeBetweenPlant);
            if(closestCropHole == null)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                yield return null;
            }
            if (dependencies.inventory.inventorySlot.currentHoldingCropOrSeed == null)
                dependencies.inventory.inventorySlot.currentHoldingCropOrSeed = closestCropHole.cropType;
            else if(dependencies.inventory.inventorySlot.currentHoldingCropOrSeed!= null &&
                dependencies.inventory.inventorySlot.currentHoldingCropOrSeed != closestCropHole.cropType)
            {
                //something wrong!, wrong type of crop 
                dependencies.stateManager.SetState(dependencies.stateManager.state_FindGrownCrops);
                yield return null;
            }
            if(!closestCropHole.hasCropOrSeed)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                yield return null;
            }
            dependencies.idendity.RemoveAssignFarmerToHole();
            dependencies.inventory.inventorySlot.AddAmount(1);
            closestCropHole.cropType.OnHarvested(closestCropHole);
            closestCropHole.RemoveCropFromCropHole();
            grownCropsCount--;
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "Start gathering crops", Color.red, duration: 1f);

            if (grownCropsCount > 0 && !dependencies.inventory.inventorySlot.isFull)
            {
                //there are more this type crops to gather
                dependencies.stateManager.SetState(dependencies.stateManager.state_FindGrownCrops);
            }
            else
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_deployCrops);
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
