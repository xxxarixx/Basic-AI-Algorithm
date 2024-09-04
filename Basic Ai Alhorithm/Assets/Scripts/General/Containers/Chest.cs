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
    public class Chest : MonoBehaviour
    {
        public IEnumerator GiveSeedsToFarmer(AI_Farmer_Inventory inventory, Action onComplete)
        {
            var currentHoldingCropOrSeed = CropDataBase.instance.tomato;
            inventory.inventorySlot.ChangeCurrentHoldingSeedOrCrop(currentHoldingCropOrSeed);
            var randAmount = Random.Range(AI_Farmer_Inventory.InventoryItem.maxInventorySize / 2, AI_Farmer_Inventory.InventoryItem.maxInventorySize);
            for (int i = 1; i <= randAmount; i++)
            {
                inventory.inventorySlot.AddAmount(1);
                TextPopup(inventory.transform.position + new Vector3(0f, 10f, 0f),
                    $"Got {currentHoldingCropOrSeed.name} x{inventory.inventorySlot.amount}",
                    currentHoldingCropOrSeed.endCropColor,
                    duration: 0.4f);
                yield return new WaitForSeconds(0.5f);
            }
            onComplete?.Invoke();
        }
    }
}
