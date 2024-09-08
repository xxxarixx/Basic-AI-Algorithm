using AI.Farmer;
using CropField.Crops;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
using Random = UnityEngine.Random;

namespace General.Containers
{
    /// <summary>
    /// Used to give player plant/seed or deploy plant/seed/crop, could be used to give player specific plant
    /// </summary>
    public class Chest : MonoBehaviour
    {
        public IEnumerator GiveSeedsToFarmer(AI_Farmer_Inventory inventory, Action onComplete)
        {
            var plantTypeToGive = PlantDataBase.instance.GetRandomPlantType();
            inventory.inventorySlot.ChangePlantType(newPlantType:plantTypeToGive,holdingType:AI_Farmer_Inventory.HoldingType.seeds);
            var randAmount = Random.Range(AI_Farmer_Inventory.InventoryItem.maxInventorySize / 2, AI_Farmer_Inventory.InventoryItem.maxInventorySize);
            for (int i = 1; i <= randAmount; i++)
            {
                inventory.inventorySlot.AddAmount(1);
                TextPopup(inventory.transform.position + new Vector3(0f, 10f, 0f),
                    $"Got {plantTypeToGive.name} x{inventory.inventorySlot.amount}",
                    plantTypeToGive.endCropColor,
                    duration: 0.4f);
                yield return new WaitForSeconds(0.5f);
            }
            onComplete?.Invoke();
        }
    }
}
