using CropField;
using CropField.Crops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_PlantState : AI_Farmer_BaseState
    {
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            AI_Farmer_StateManager stateManager = dependencies.stateManager;
            AI_Farmer_Inventory.InventoryItem inventorySlot = dependencies.inventory.inventorySlot;
            
            bool thereIsAnyLeftSeedHoles = true;
            yield return new WaitForSeconds(AI_Farmer_Dependencies.timeBetweenPlant);
            var foundedCropHole = CropField_Manager.instance.GetClosestEmptyCropGround(dependencies.idendity, out int currentCountOfEmptyCropHoles);
            dependencies.idendity.RemoveAssignFarmerToHole();
            if (inventorySlot.HasAnySeedsOrCrops && currentCountOfEmptyCropHoles > 0)
            {
                foundedCropHole.AddSeedToHole(inventorySlot.GetSeedFromInventory());
                currentCountOfEmptyCropHoles--;
                TextPopup(dependencies.transform.position + new Vector3(-10f, 10f, 0f),
                    $"planting seed of {foundedCropHole.cropType.name}, left seeds {inventorySlot.amount} left holes:{currentCountOfEmptyCropHoles - 1}",
                    foundedCropHole.cropType.startCropColor, 
                    duration: 1.5f);
                thereIsAnyLeftSeedHoles = currentCountOfEmptyCropHoles > 0;
            }
            else
            {
                thereIsAnyLeftSeedHoles = foundedCropHole != null && currentCountOfEmptyCropHoles > 0;
            }

            int grownCropsCount = 0;
            #region switchState
            //FIRST PRIORITY: if have in inventory any seeds and there are any left seed holes
            if (inventorySlot.HasAnySeedsOrCrops && thereIsAnyLeftSeedHoles) 
            {
                stateManager.SetState(stateManager.state_FindEmptyCropGround);
                yield return null;
            }
            else
            {
                grownCropsCount = CropField_Manager.instance.GetAllFullyGrownCropsCount(dependencies.idendity);
                bool grownCropsSeemsToBeBetterOption = grownCropsCount > currentCountOfEmptyCropHoles;

                //if have in inventory any seeds and there are not any left seed holes
                if (inventorySlot.HasAnySeedsOrCrops && (!thereIsAnyLeftSeedHoles || grownCropsSeemsToBeBetterOption))
                {
                    stateManager.SetState(stateManager.state_deploySeeds);
                }
                //gather crops if there are any
                else if(!inventorySlot.HasAnySeedsOrCrops && grownCropsSeemsToBeBetterOption)
                {
                    stateManager.SetState(stateManager.state_FindGrownCrops);
                }
                //if DONT have in inventory any seeds and there ARE any left seed holes
                else if (!inventorySlot.HasAnySeedsOrCrops && thereIsAnyLeftSeedHoles && !grownCropsSeemsToBeBetterOption)
                {
                    stateManager.SetState(stateManager.state_gatherSeeds);
                }
                //wait there is nothing to do
                else
                {
                    stateManager.SetState(stateManager.state_waitForNewWork);
                }
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
