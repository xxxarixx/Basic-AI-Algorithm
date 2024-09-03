using CropField.Crops;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CropField
{
    [DefaultExecutionOrder(-1)]
    public class CropField_Manager : MonoBehaviour
    {
        public static CropField_Manager instance { get; private set; }
        public List<CropField_Single> cropFields = new List<CropField_Single>();
        private void Awake()
        {
            instance = this;
            UpdateCropFieldsList();
        }
        [ContextMenu(nameof(UpdateCropFieldsList))]
        private void UpdateCropFieldsList()
        {
            foreach (Transform child in transform)
            {
                if(child.TryGetComponent<CropField_Single>(out CropField_Single cropField) && !cropFields.Contains(cropField))
                {
                    cropFields.Add(cropField);
                }
            }
        }
        public Vector3 GetClosestEmptyCropGroundPosition(Transform target)
        {
            List<Vector3> allEmptyCropGrounds = new List<Vector3>();
            cropFields.ForEach(cropField => 
            {
                allEmptyCropGrounds.AddRange(cropField.cropHoleLocations.FindAll(cropHole => !cropHole.hasCrop).Select(x => x.worldLocation).ToArray());
            });
            var orderedList = allEmptyCropGrounds.OrderBy(x => Vector3.Distance(x, target.position));
            
            return orderedList.Count() == 0? target.position : orderedList.First();
        }
        public CropHole GetClosestEmptyCropGround(Transform target, out int currentCount)
        {
            List<CropHole> allEmptyCropGrounds = new List<CropHole>();
            cropFields.ForEach(cropField =>
            {
                allEmptyCropGrounds.AddRange(cropField.cropHoleLocations.FindAll(x => !x.hasCrop));
            });
            var ordered = allEmptyCropGrounds.OrderBy(x => Vector3.Distance(x.worldLocation, target.position));
            currentCount = ordered.Count();
            if (ordered.Count() == 0)
                return null;
            var closest = ordered.First();
            return closest;
        }
        public int GetEmptyCropGroundsCount()
        {
            Dictionary<Vector3, CropHole> allEmptyCropGrounds = new Dictionary<Vector3, CropHole>();
            cropFields.ForEach(cropField =>
            {
                foreach (var cropHole in cropField.cropHoleLocations)
                {
                    if (cropHole.hasCrop)
                        continue;
                    if (!allEmptyCropGrounds.ContainsKey(cropHole.worldLocation))
                        allEmptyCropGrounds.Add(cropHole.worldLocation, cropHole);
                }
            });
            return allEmptyCropGrounds.Count();
        }
        public CropHole GetClosestFullGrownCrop(Transform target, CropAndSeedBase targetCropToFind, out int count)
        {
            List<CropHole> cropHolesWithFullyGrownCrop = new List<CropHole>();
            cropFields.ForEach((cropField) =>
            {
                if(targetCropToFind == null)
                {
                    //find any crop
                    cropHolesWithFullyGrownCrop.AddRange(cropField.cropHoleLocations.FindAll(x => x.hasCrop && x.cropType.isFullyGrown));
                }
                else
                {
                    //find this pirticular type
                    cropHolesWithFullyGrownCrop.AddRange(cropField.cropHoleLocations.FindAll(x => x.hasCrop && x.cropType == targetCropToFind && x.cropType.isFullyGrown));
                }
            });
            var ordered = cropHolesWithFullyGrownCrop.OrderBy(x => Vector3.Distance(x.worldLocation, target.position));
            count = ordered.Count();
            if (count == 0)
                return null;
            var closest = ordered.First();
            return closest;
        }
        public int GetFullyGrownCropsCount()
        {
            List<CropHole> cropHolesWithFullyGrownCrop = new List<CropHole>();
            cropFields.ForEach((cropField) =>
            {
                cropHolesWithFullyGrownCrop.AddRange(cropField.cropHoleLocations.FindAll(x => x.hasCrop && x.cropType.isFullyGrown));
            });
            return cropHolesWithFullyGrownCrop.Count;
        }
    }

}
