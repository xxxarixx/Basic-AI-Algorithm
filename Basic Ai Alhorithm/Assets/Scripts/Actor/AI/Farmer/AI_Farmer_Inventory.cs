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
            public HoldingType holdingType = HoldingType.none;
            public PlantBase plantType;
            public int amount { get; private set; } = 0;
            public const int maxInventorySize = 10;
            public bool isFull => amount >= maxInventorySize;
            public bool HasAnyPlant => plantType != null && amount > 0;
            public bool HasAnySeeds => holdingType == HoldingType.seeds && HasAnyPlant;
            public bool HasAnyCrops => holdingType == HoldingType.crops && HasAnyPlant;
            public PlantBase GetSeedFromInventory()
            {
                if (!HasAnyPlant)
                    return null;
                amount--;
                amount = Mathf.Clamp(amount, 0, maxInventorySize);
                return plantType;
            }
            public void ChangePlantType(PlantBase newPlantType, HoldingType holdingType)
            {
                plantType = newPlantType;
                this.holdingType = holdingType;
            }
            public IEnumerator DeploySeeds(Transform target,Action onComplete, bool popText = true)
            {
                while (amount > 0)
                {
                    if(popText && plantType != null)
                        TextPopup(target.position + new Vector3(0f, 10f, 0f), $"Deployed {plantType.name}!", Color.red, duration: 1f);
                    AddAmount(-1);
                    yield return new WaitForSeconds(AI_Farmer_Dependencies.timeBetweenDeploy);
                }
                onComplete?.Invoke();
            }
            public IEnumerator DeployCrops(Transform target, Action onComplete, bool popText = true)
            {
                while (amount > 0)
                {
                    if (popText)
                        TextPopup(target.position + new Vector3(0f, 10f, 0f), $"Deployed {plantType.name}!", Color.red, duration: 1f);
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
                    ChangePlantType(newPlantType:null,HoldingType.none);
                }
            }
        }
        public enum HoldingType
        {
            none,
            seeds,
            crops
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

