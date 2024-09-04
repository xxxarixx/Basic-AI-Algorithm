using CropField.Crops;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
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
            public CropAndSeedBase currentHoldingCropOrSeed;
            public int amount { get; private set; } = 0;
            public const int maxInventorySize = 10;
            public bool isFull => amount >= maxInventorySize;
            public bool HasAnySeedsOrCrops => amount > 0 || currentHoldingCropOrSeed != null;
            public CropAndSeedBase GetSeedFromInventory()
            {
                if (!HasAnySeedsOrCrops)
                    return null;
                amount--;
                amount = Mathf.Clamp(amount, 0, maxInventorySize);
                return currentHoldingCropOrSeed;
            }
            public void ChangeCurrentHoldingSeedOrCrop(CropAndSeedBase newCropOrSeed)
            {
                currentHoldingCropOrSeed = newCropOrSeed;
            }
            public IEnumerator DeploySeeds(Transform target,Action onComplete, bool popText = true)
            {
                for (int i = 0; i <= amount; i++)
                {
                    if(popText && currentHoldingCropOrSeed != null)
                        TextPopup(target.position + new Vector3(0f, 10f, 0f), $"Deployed {currentHoldingCropOrSeed.name}!", Color.red, duration: 1f);
                    AddAmount(-1);
                    yield return new WaitForSeconds(AI_Farmer_Dependencies.timeBetweenDeploy);
                }
                onComplete?.Invoke();
            }
            public IEnumerator DeployCrops(Transform target, Action onComplete, bool popText = true)
            {
                for (int i = 0; i <= amount; i++)
                {
                    if (popText)
                        TextPopup(target.position + new Vector3(0f, 10f, 0f), $"Deployed {currentHoldingCropOrSeed.name}!", Color.red, duration: 1f);
                    AddAmount(-1);
                    yield return new WaitForSeconds(AI_Farmer_Dependencies.timeBetweenDeploy);
                }
                onComplete?.Invoke();
            }
            public void AddAmount(int toAdd)
            {
                amount += toAdd;
                amount = Mathf.Clamp(amount, 0, maxInventorySize);
                if(amount <= 0)
                {
                    ChangeCurrentHoldingSeedOrCrop(null);
                }
            }
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

