using CropField;
using CropField.Crops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_PlantSeedState : AI_Farmer_BaseState
    {
        public override Job GetMyJob()
        {
            return Job.SeedJob;
        }

        public override string Name()
        {
            return nameof(AI_Farmer_PlantSeedState);
        }
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            AI_Farmer_StateManager stateManager = dependencies.stateManager;
            AI_Farmer_Inventory.InventoryItem inventorySlot = dependencies.inventory.inventorySlot;
            
            bool thereIsAnyLeftSeedHoles = true;
            yield return new WaitForSeconds(AI_Farmer_Dependencies.timeBetweenPlant);
            var foundedCropHole = dependencies.idendity.actorAssignToCropHole;
            dependencies.idendity.RemoveAssignFarmerToHole();
            //if was issue with finding free crop hole and there is already plant in it
            if (foundedCropHole == null || foundedCropHole.plant != null || dependencies.inventory.inventorySlot.HasAnyCrops) 
            {
                stateManager.SetState(stateManager.state_waitForNewWork);
                yield return null;
            }
            var currentCountOfEmptyCropHoles = CropField_Manager.instance.GetEmptyCropGroundsCount(dependencies.idendity);
            if (inventorySlot.HasAnySeeds)
            {
                var seed = inventorySlot.GetSeedFromInventory();
                if(seed != null)
                {
                    foundedCropHole.AddSeedToHole(seed,dependencies.inventory);
                    currentCountOfEmptyCropHoles--;
                    TextPopup(dependencies.transform.position + new Vector3(-10f, 10f, 0f),
                        $"planting seed of {foundedCropHole.plant.plantBase.name}, left seeds {inventorySlot.amount} left holes:{currentCountOfEmptyCropHoles}",
                        foundedCropHole.plant.plantBase.startCropColor, 
                        duration: 1.5f);
                }
            }

            thereIsAnyLeftSeedHoles = currentCountOfEmptyCropHoles > 0;

            #region switchState
            //FIRST PRIORITY: if have in inventory any seeds and there are any left seed holes
            if (inventorySlot.HasAnySeeds && thereIsAnyLeftSeedHoles) 
            {
                stateManager.SetState(stateManager.state_FindEmptySeedGround);
                yield return null;
            }
            else
            {
                stateManager.SetState(stateManager.state_waitForNewWork);
            }
            #endregion
        }
        public override void OnExitState(AI_Farmer_Dependencies dependencies)
        {
            
        }

        public override void OnUpdateState(AI_Farmer_Dependencies dependencies)
        {
            
        }
    }

}
