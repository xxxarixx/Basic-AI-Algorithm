using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CropField.Crops;

namespace CropField
{
    public class CropHole
    {
        public readonly Vector3 worldLocation;
        public CropAndSeedBase crop;
        public bool hasCrop => crop != null;
        public CropHole(Vector3 worldLocation)
        {
            this.worldLocation = worldLocation;
        }
        public void AddSeedToHole(CropAndSeedBase crop)
        {
            CropAndSeedBase.CreateCrop(worldLocation, crop);
            this.crop = crop;
        }
    }
}

