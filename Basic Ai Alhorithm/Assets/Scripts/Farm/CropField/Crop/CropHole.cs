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
        public CropAndSeedBase cropType;
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
        public bool hasCropOrSeed => cropType != null;
        public bool CanIGatherThisCrop(CropAndSeedBase targetCropToFind) 
            => hasCropOrSeed && 
            (cropType == null || targetCropToFind == null && cropType.isFullyGrown || cropType == targetCropToFind && cropType.isFullyGrown);
        
        public CropHole(Vector3 worldLocation)
        {
            this.worldLocation = worldLocation;
        }
        public void AddSeedToHole(CropAndSeedBase crop)
        {
            if (hasCropOrSeed)
                return;
            cropRenderer = CropAndSeedBase.CreateCrop(worldLocation, crop);
            this.cropType = crop;
        }
        public void RemoveCropFromCropHole()
        {
            if (cropType == null || !cropType.isFullyGrown)
                return;
            cropRenderer.AddComponent<DestroySelf>().StartCounting(durationInSeconds:0.1f);
            cropRenderer = null;
            cropType = null;
        }
    }
}

