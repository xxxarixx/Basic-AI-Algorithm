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
            bool thereIsAnyCropToGather = true;
            var foundedCropHole = CropField_Manager.instance.GetClosestEmptyCropGround(dependencies.transform);
            yield return new WaitForSeconds(1f);
            //if have in inventory any seeds and there are any left seed holes
            if (inventorySlot.HasAnySeedsOrCrops && thereIsAnyLeftSeedHoles) 
            {
                foundedCropHole.AddSeedToHole(inventorySlot.GetSeedFromInventory());
                TextPopup(dependencies.transform.position + new Vector3(0f, 10f, 0f), $"planting seed, left seeds {inventorySlot.amount}", Color.green, duration: 1.5f);
                stateManager.SetState(stateManager.state_FindEmptyCropGround);
            }
            //if DONT have in inventory any seeds and there ARE any left seed holes
            else if (!inventorySlot.HasAnySeedsOrCrops && thereIsAnyLeftSeedHoles)
            {
                stateManager.SetState(stateManager.state_gatherSeeds);
            }
            //if have in inventory any seeds and there are not any left seed holes
            else if(inventorySlot.HasAnySeedsOrCrops && !thereIsAnyLeftSeedHoles)
            {
                stateManager.SetState(stateManager.state_deploySeeds);
            }
            //gather crops if there are any
            else if(thereIsAnyCropToGather)
            {
                stateManager.SetState(stateManager.state_gatherCrops);
            }
            //wait there is nothing to do
            else
            {
                stateManager.SetState(stateManager.state_waitForNewWork);
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
