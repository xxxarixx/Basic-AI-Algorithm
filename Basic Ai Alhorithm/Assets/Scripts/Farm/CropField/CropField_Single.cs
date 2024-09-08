using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CropField
{
    public class CropField_Single : MonoBehaviour
    {
        private List<Transform> potencialCropHoleLocations = new List<Transform>();
        public List<CropHole> cropHoleLocations = new List<CropHole>();
        public Vector2Int cropHolePopulationRange = new Vector2Int(6,10);
        [ContextMenu(nameof(RegenerateAllPlantsLocations))]
        public void RegenerateAllPlantsLocations()
        {
            cropHoleLocations = new List<CropHole>();
            if (potencialCropHoleLocations.Count == 0)
                FindPotencialLocations();
            foreach (var cropHoleLocation in potencialCropHoleLocations)
            {
                if (cropHoleLocation.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    meshRenderer.enabled = false;
                }
            }
            cropHolePopulationRange.Clamp(new Vector2Int(1,1), new Vector2Int(potencialCropHoleLocations.Count, potencialCropHoleLocations.Count));
            int amountOfPlantPlaces = Random.Range(cropHolePopulationRange.x, cropHolePopulationRange.y);
            for (int i = 0; i < amountOfPlantPlaces; i++)
            {
                int safetyTrys = 0;
                while (safetyTrys < 50)
                {
                    Transform rand = potencialCropHoleLocations[Random.Range(0, potencialCropHoleLocations.Count)];
                    if (!cropHoleLocations.Exists(x => x.worldLocation == rand.position))
                    {
                        if(rand.TryGetComponent(out MeshRenderer meshRenderer))
                        {
                            meshRenderer.enabled = true;
                        }
                        var cropHole = rand.AddComponent<CropHole>();
                        cropHole.worldLocation = rand.position;
                        cropHoleLocations.Add(cropHole);
                        break;
                    }
                    
                }

            }
        }
        private void FindPotencialLocations()
        {
            potencialCropHoleLocations = new List<Transform>();
            foreach (Transform child in transform)
            {
                potencialCropHoleLocations.Add(child);
            }
        }
        private void Awake()
        {
            FindPotencialLocations();
            RegenerateAllPlantsLocations();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (CropHole cropHole in cropHoleLocations)
            {
                Gizmos.DrawSphere(cropHole.worldLocation, 0.2f);
            }
        }
    }
}