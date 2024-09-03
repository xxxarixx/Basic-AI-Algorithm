using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CropField.Crops;
using Unity.VisualScripting;

namespace CropField
{
    public class CropHole
    {
        public readonly Vector3 worldLocation;
        public CropAndSeedBase cropType;
        public MeshRenderer cropRenderer;
        public bool hasCrop => cropType != null;
        public CropHole(Vector3 worldLocation)
        {
            this.worldLocation = worldLocation;
        }
        public void AddSeedToHole(CropAndSeedBase crop)
        {
            cropRenderer = CropAndSeedBase.CreateCrop(worldLocation, crop);
            this.cropType = crop;
        }
        public void RemoveCropFromCropHole()
        {
            cropRenderer.AddComponent<DestroySelf>().StartCounting(durationInSeconds:0.1f);
            cropRenderer = null;
            cropType = null;
        }
    }
}

