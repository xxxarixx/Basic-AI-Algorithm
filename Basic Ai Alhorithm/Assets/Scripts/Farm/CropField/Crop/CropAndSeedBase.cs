using System;
using Unity.VisualScripting;
using UnityEngine;
using DG;
using DG.Tweening;
using Random = UnityEngine.Random;
namespace CropField.Crops
{
    [CreateAssetMenu(menuName ="Custom/CreateCrop",fileName ="new Crop")]
    public class CropAndSeedBase : ScriptableObject
    {
        public event Action OnEndedGrowing;
        private bool isGrowing = false;
        public bool isFullyGrown => !isGrowing;
        public float growTimeInS;
        public Color startCropColor;
        public Color endCropColor;
        public Vector3 startSize;
        public Vector3 endSize;
        public void SetupCrop(MeshRenderer renderer)
        {
            renderer.material.color = startCropColor;
            isGrowing = true;
            renderer.transform.localScale = startSize;
            var endSizeWithRandomFactor = endSize + new Vector3(Random.Range(endSize.x / 5, endSize.x / 3), Random.Range(endSize.x / 5, endSize.x / 3), Random.Range(endSize.x / 5, endSize.x / 3));
            float growTimeWithRandomFactor = Random.Range(growTimeInS / 5, growTimeInS / 3);
            renderer.transform.DOScale(endValue: endSizeWithRandomFactor, duration: growTimeWithRandomFactor);
            renderer.material.DOColor(endValue: endCropColor, duration: growTimeWithRandomFactor).OnComplete(() =>
            {
                isGrowing = false;
                OnEndedGrowing?.Invoke();
            });
            
        }
        public void OnHarvested(CropHole cropHole)
        {
            if (isGrowing) return;
        }
        public static MeshRenderer CreateCrop(Vector3 worldLocation,CropAndSeedBase crop)
        {
            if (crop == null)
                return default;
           GameObject createdCrop = Instantiate(CropDataBase.instance.cropGoPatern, worldLocation, Quaternion.identity);
           if(createdCrop.TryGetComponent(out MeshRenderer renderer))
           {
                crop.SetupCrop(renderer);
                return renderer;
           }
           else
           {
               return createdCrop.AddComponent<MeshRenderer>();
           }

        }
    }
}

