using AI.Farmer;
using CropField.Crops;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DebugHelper.DebugHelper;
using Random = UnityEngine.Random;

namespace General.Essencial
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance { get; private set; }
        public Transform deployGatherChest;
        private void Awake()
        {
            instance = this;
        }
        public IEnumerator GiveSeedsToFarmer(AI_Farmer_Inventory inventory, Action onComplete)
        {
            var currentHoldingCropOrSeed = CropDataBase.instance.tomato;
            inventory.inventorySlot.ChangeCurrentHoldingSeedOrCrop(currentHoldingCropOrSeed);
            var randAmount = Random.Range(5, 10);
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
