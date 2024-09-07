using CropField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
namespace AI.Farmer
{
    public class AI_Farmer_WaitForNewWork : AI_Farmer_BaseState
    {
        public override string Name()
        {
            return nameof(AI_Farmer_WaitForNewWork);
        }

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
                bool thereIsAnyLeftGrownCrops = fullyGrownCrops > 0;
               // Debug.Log($"waiting {dependencies.transform.name}:grownCropsBetter:{grownCropsSeemsToBeBetterOption} anyLeftSeedHoles:{thereIsAnyLeftSeedHoles} hasSeedsInInv:{inventorySlot.HasAnySeeds}");

                //Gather crops job
                if (thereIsAnyLeftGrownCrops && grownCropsSeemsToBeBetterOption)
                {
                    //safety protocol to be sure is having crops in inventory
                    if(inventorySlot.HasAnySeeds)
                    {
                        stateManager.SetState(stateManager.state_deploySeeds);
                        yield return null;
                        break;
                    }

                    if(!inventorySlot.isFull)
                    {
                        //start/continue planting
                        stateManager.SetState(stateManager.state_FindGrownCrops);
                        yield return null;
                        break;
                    }
                    else
                    {
                        //inventory full
                        stateManager.SetState(stateManager.state_deployCrops);
                        yield return null;
                        break;
                    }
                }
                //Planting seeds job
                else if (thereIsAnyLeftSeedHoles && !grownCropsSeemsToBeBetterOption)
                {
                    //safety protocol to be sure is having seeds in inventory
                    if (inventorySlot.HasAnyCrops)
                    {
                        stateManager.SetState(stateManager.state_deployCrops);
                        yield return null;
                        break;
                    }

                    if (inventorySlot.HasAnySeeds)
                    {
                        //start/continue planting
                        stateManager.SetState(stateManager.state_FindEmptySeedGround);
                        yield return null;
                        break;
                    }
                    else
                    {
                        //there is no more seeds in inventory and there are holes to fill
                        stateManager.SetState(stateManager.state_gatherSeeds);
                        yield return null;
                        break;
                    }
                }

                //if farmer have in inventory any seeds and there are not any left seed holes and want to change job to gather crops
                if (inventorySlot.HasAnySeeds && !thereIsAnyLeftSeedHoles && grownCropsSeemsToBeBetterOption)
                {
                    stateManager.SetState(stateManager.state_deploySeeds);
                    yield return null;
                    break;
                }
                //if farmer have in inventory any crops and there are not any left grown crops and want to change job to plant seeds
                else if (inventorySlot.HasAnyCrops && !thereIsAnyLeftGrownCrops && !grownCropsSeemsToBeBetterOption)
                {
                    stateManager.SetState(stateManager.state_deployCrops);
                    yield return null;
                    break;
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
