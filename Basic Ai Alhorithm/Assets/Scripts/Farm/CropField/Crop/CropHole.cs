using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CropField.Crops;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

namespace CropField
{
    public class CropHole
    {
        public readonly Vector3 worldLocation;
        public CropAndSeedBase plantType;
        public MeshRenderer cropRenderer;
        public Actor_Idendity actorIdendity_AssignToThisHole = null;
        public bool thereIsActorAssignedToThisHole 
            => actorIdendity_AssignToThisHole != null;
        public bool IsItMeAssignedToThisHole(Actor_Idendity idendity) 
            => thereIsActorAssignedToThisHole && 
            actorIdendity_AssignToThisHole == idendity;
        public bool CanIAccessThisHole(Actor_Idendity idendity)
            => !thereIsActorAssignedToThisHole || 
            IsItMeAssignedToThisHole(idendity);
        public bool hasPlant => plantType != null;
        public bool CanIGatherThisCrop(CropAndSeedBase targetCropToFind) 
            => hasPlant && 
            (plantType == null || targetCropToFind == null && plantType.isFullyGrown || plantType == targetCropToFind && plantType.isFullyGrown);
        
        public CropHole(Vector3 worldLocation)
        {
            this.worldLocation = worldLocation;
        }
        public void AddSeedToHole(CropAndSeedBase crop)
        {
            if (hasPlant)
                return;
            this.plantType = crop;
            cropRenderer = CropAndSeedBase.CreateCrop(worldLocation, crop);
        }
        public void RemovePlantFromCropHole()
        {
            if (plantType == null || !plantType.isFullyGrown)
                return;
            cropRenderer.AddComponent<DestroySelf>().StartCounting(durationInSeconds:0.1f);
            cropRenderer = null;
            plantType = null;
        }
    }
}

