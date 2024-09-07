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
        #region Empty crop grounds
        public Vector3 GetClosestEmptyCropGroundPosition(Actor_Idendity target)
        {
            List<Vector3> allEmptyCropGrounds = new List<Vector3>();
            cropFields.ForEach(cropField => 
            {
                allEmptyCropGrounds.AddRange(
                    cropField.cropHoleLocations.FindAll(cropHole => 
                    !cropHole.hasPlant && cropHole.CanIAccessThisHole(target))
                    .Select(x => x.worldLocation).ToArray());
            });
            var orderedList = allEmptyCropGrounds.OrderBy(x => Vector3.Distance(x, target.transform.position));
            
            return allEmptyCropGrounds.Count == 0? target.transform.position : orderedList.First();
        }
        public CropHole GetClosestEmptyCropGround(Actor_Idendity target, out int currentCount)
        {
            currentCount = 0;
            if (cropFields.Count == 0)
                return default;
            List<CropHole> allEmptyCropGrounds = new List<CropHole>();
            cropFields.ForEach(cropField =>
            {
                allEmptyCropGrounds.AddRange(cropField.cropHoleLocations.FindAll(c => !c.hasPlant && c.CanIAccessThisHole(target)));
            });
            var ordered = allEmptyCropGrounds.OrderBy(x => Vector3.Distance(x.worldLocation, target.transform.position));
            currentCount = allEmptyCropGrounds.Count;
            if (currentCount == 0)
                return null;
            var closest = ordered.First();
            return closest;
        }
        public int GetEmptyCropGroundsCount(Actor_Idendity target)
        {
            List<CropHole> allEmptyCropGrounds = new List<CropHole>();
            cropFields.ForEach(cropField =>
            {
                allEmptyCropGrounds.AddRange(cropField.cropHoleLocations.FindAll(c => !c.hasPlant && c.CanIAccessThisHole(target)));
            });
            return allEmptyCropGrounds.Count;
        }
        #endregion
        #region Grown crop
        public CropHole GetClosestFullGrownCrop(Actor_Idendity target, CropAndSeedBase targetCropToFind, out int count)
        {
            List<CropHole> cropHolesWithFullyGrownCrop = new List<CropHole>();
            cropFields.ForEach((cropField) =>
            {
                cropHolesWithFullyGrownCrop.AddRange(cropField.cropHoleLocations.FindAll(c => c.CanIGatherThisCrop(targetCropToFind) && c.CanIAccessThisHole(target)));
            });
            var ordered = cropHolesWithFullyGrownCrop.OrderBy(x => Vector3.Distance(x.worldLocation, target.transform.position));
            count = cropHolesWithFullyGrownCrop.Count;
            if (count == 0)
                return null;
            return ordered.First();
        }
        public int GetAllFullyGrownCropsCount(Actor_Idendity target)
        {
            List<CropHole> cropHolesWithFullyGrownCrop = new List<CropHole>();
            cropFields.ForEach((cropField) =>
            {
                cropHolesWithFullyGrownCrop.AddRange(cropField.cropHoleLocations.FindAll(c => c.CanIGatherThisCrop(targetCropToFind:null) && c.CanIAccessThisHole(target)));
            });
            return cropHolesWithFullyGrownCrop.Count;
        }
        #endregion
    }

}
