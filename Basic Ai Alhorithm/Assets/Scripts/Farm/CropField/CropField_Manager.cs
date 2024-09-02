using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            Debug.Log($"cropfields:{cropFields.Count}; ");
            cropFields.ForEach(cropField => 
            {
                Debug.Log($"cropfield holes:{cropField.cropHoleLocations.Count}; ");
                allEmptyCropGrounds.AddRange(cropField.cropHoleLocations.FindAll(cropHole => !cropHole.hasCrop).Select(x => x.worldLocation).ToArray());
            });
            var orderedList = allEmptyCropGrounds.OrderBy(x => Vector3.Distance(x, target.position));
            
            return orderedList.Count() == 0? default : orderedList.First();
        }
        public CropHole GetClosestEmptyCropGround(Transform target)
        {
            Dictionary<Vector3, CropHole> allEmptyCropGrounds = new Dictionary<Vector3, CropHole>();
            cropFields.ForEach(cropField =>
            {
                foreach (var cropHole in cropField.cropHoleLocations)
                {
                    allEmptyCropGrounds.Add(cropHole.worldLocation,cropHole);
                }
            });
            var closest = allEmptyCropGrounds.OrderBy(x => Vector3.Distance(x.Key, target.position)).First();
            return closest.Value;
        }
    }

}
