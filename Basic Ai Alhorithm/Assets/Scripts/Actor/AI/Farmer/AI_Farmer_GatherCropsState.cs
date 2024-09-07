using CropField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_GatherCropsState : AI_Farmer_BaseState
    {
        public override string Name()
        {
            return nameof(AI_Farmer_GatherCropsState);
        }
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {

            yield return new WaitForSeconds(AI_Farmer_Dependencies.timeBetweenPlant);
            var inventorySlot = dependencies.inventory.inventorySlot;
            //make sure to always remove farmer from hole
            dependencies.idendity.RemoveAssignFarmerToHole();

            Transform transform = dependencies.transform;
            CropHole closestCropHole = CropField_Manager.instance.GetClosestFullGrownCrop(dependencies.idendity, dependencies.inventory.inventorySlot.plantType, out int grownCropsCount);
            if(closestCropHole == null || !closestCropHole.hasPlant || inventorySlot.HasAnySeeds)
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
                yield return null;
            }
            if (!inventorySlot.HasAnyPlant) //make sure to if i dont have any crops to assign my crop
                inventorySlot.ChangePlantType(newPlantType: closestCropHole.plantType, holdingType: AI_Farmer_Inventory.HoldingType.crops);
            else if (inventorySlot.HasAnyPlant && inventorySlot.plantType != closestCropHole.plantType) // check if founded croptype if corrent to holding one
            {
                //something wrong!, wrong type of crop try again find correct one!
                dependencies.stateManager.SetState(dependencies.stateManager.state_FindGrownCrops);
                yield return null;
            }
            TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), $"Gathered {closestCropHole.plantType}", closestCropHole.plantType.endCropColor, duration: 1f);
            inventorySlot.AddAmount(1);
            closestCropHole.plantType.OnHarvested(closestCropHole);
            closestCropHole.RemovePlantFromCropHole();
            grownCropsCount--;

            if (grownCropsCount > 0 && !inventorySlot.isFull)
            {
                //there are more this type crops to gather
                dependencies.stateManager.SetState(dependencies.stateManager.state_FindGrownCrops);
            }
            else
            {
                dependencies.stateManager.SetState(dependencies.stateManager.state_waitForNewWork);
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
