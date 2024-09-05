using CropField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_WaitForNewWork : AI_Farmer_BaseState
    {
        public override IEnumerator OnEnterState(AI_Farmer_Dependencies dependencies)
        {
            AI_Farmer_StateManager stateManager = dependencies.stateManager;
            AI_Farmer_Inventory.InventoryItem inventorySlot = dependencies.inventory.inventorySlot;
            while (true)
            {
                int emptySeedHolesCount = CropField_Manager.instance.GetEmptyCropGroundsCount(dependencies.idendity);
                int fullyGrownCrops = CropField_Manager.instance.GetAllFullyGrownCropsCount(dependencies.idendity);
                int amountOfFarmersInCropJob = AI_Summary.instance.farmer_Summary.farmers_inCropJob.Count;
                int amountOfFarmersInSeedJob = AI_Summary.instance.farmer_Summary.farmers_inSeedJob.Count;
                bool grownCropsSeemsToBeBetterOption = fullyGrownCrops > emptySeedHolesCount && 
                    (amountOfFarmersInCropJob < amountOfFarmersInSeedJob || emptySeedHolesCount == 0);
                bool thereIsAnyLeftSeedHoles = emptySeedHolesCount > 0;
                Debug.Log($"waiting {dependencies.transform.name}:grownCropsBetter:{grownCropsSeemsToBeBetterOption} anyLeftSeedHoles:{thereIsAnyLeftSeedHoles} hasSeedsInInv:{inventorySlot.HasAnySeedsOrCrops}");
                //if have in inventory any seeds and there are not any left seed holes
                if (inventorySlot.HasAnySeedsOrCrops && (!thereIsAnyLeftSeedHoles || grownCropsSeemsToBeBetterOption))
                {
                    stateManager.SetState(stateManager.state_deploySeeds);
                    yield return null;
                }
                //gather crops if there are any
                else if (!inventorySlot.HasAnySeedsOrCrops && grownCropsSeemsToBeBetterOption)
                {
                    stateManager.SetState(stateManager.state_FindGrownCrops);
                    yield return null;
                }
                //if DONT have in inventory any seeds and there ARE any left seed holes
                else if (!inventorySlot.HasAnySeedsOrCrops && thereIsAnyLeftSeedHoles && !grownCropsSeemsToBeBetterOption)
                {
                    stateManager.SetState(stateManager.state_gatherSeeds);
                    yield return null;
                }
                yield return new WaitForSeconds(3f);
                TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), "Waiting for new work...", Color.green, duration: 1f);
            }
        }

        public override void OnExitState(AI_Farmer_Dependencies dependencies)
        {
            
        }

        public override void OnUpdateState(AI_Farmer_Dependencies dependencies)
        {
            
        }
    }
}
