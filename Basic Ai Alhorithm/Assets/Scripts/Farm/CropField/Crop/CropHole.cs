using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CropField.Crops;
using Unity.VisualScripting;
using Actor;
using AI.Farmer;
using System;

namespace CropField
{
    public class CropHole : MonoBehaviour
    {
        public Vector3 worldLocation;
        public PlantInstance plant = null;
        public Actor_Idendity actorIdendity_AssignToThisHole = null;
        public bool thereIsActorAssignedToThisHole 
            => actorIdendity_AssignToThisHole != null;

        public bool IsItMeAssignedToThisHole(Actor_Idendity idendity) 
            => thereIsActorAssignedToThisHole && 
            actorIdendity_AssignToThisHole.actorID.Equals(idendity.actorID);

        public bool CanIAccessThisHole(Actor_Idendity idendity)
            => !thereIsActorAssignedToThisHole || 
            IsItMeAssignedToThisHole(idendity);

        public bool hasPlant => plant != null;
        public bool CanIGatherThisCrop(PlantBase targetCropToFind) 
            => hasPlant && plant.isFullyGrown &&
            (targetCropToFind == null|| plant.plantBase.ID == targetCropToFind.ID);
        
        public void AddSeedToHole(PlantBase crop, AI_Farmer_Inventory inv)
        {
            if (hasPlant)
                return;
            inv.inventorySlot.AddAmount(-1);
            plant = PlantInstance.CreateCrop(worldLocation,crop);
        }
        public void RemovePlantFromCropHole()
        {
            if (plant == null || !plant.isFullyGrown)
                return;
            plant.RemoveThisPlant();
            plant = null;
        }
    }
}

