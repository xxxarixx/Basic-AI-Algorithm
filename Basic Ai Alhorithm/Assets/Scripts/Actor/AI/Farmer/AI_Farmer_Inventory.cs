using CropField.Crops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.Farmer
{
    /// <summary>
    /// Contains data about farmer inventory, current holding seed or crop
    /// </summary>
    [RequireComponent(typeof(AI_Farmer_Dependencies))]
    public class AI_Farmer_Inventory : MonoBehaviour
    {
        public InventoryItem inventorySlot = new InventoryItem();
        [System.Serializable]
        public class InventoryItem
        {
            private CropAndSeedBase currentHoldingCropOrSeed;
            public int amount { get; private set; } = 0;
            public bool HasAnySeedsOrCrops => amount > 0;
            public CropAndSeedBase GetSeedFromInventory()
            {
                if (!HasAnySeedsOrCrops)
                    return null;
                amount--;
                return currentHoldingCropOrSeed;
            }
            public void ChangeCurrentHoldingSeedOrCrop(CropAndSeedBase newCropOrSeed)
            {
                currentHoldingCropOrSeed = newCropOrSeed;
            }
            public int AddAmount(int toAdd) => amount += toAdd;
        }
        public AI_Farmer_Dependencies dependencies { get; private set; }
        ///<summary>
        /// Decreases amount of seeds each time this is called
        ///</summary>
        /// <returns>CropAndSeedBase but if doesn't have return null</returns>
        
        private void Awake()
        {
            dependencies = GetComponent<AI_Farmer_Dependencies>();
        }
    }
}

