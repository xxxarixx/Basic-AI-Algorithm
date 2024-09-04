using System;
using Unity.VisualScripting;
using UnityEngine;
using DG;
using DG.Tweening;
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
            renderer.transform.DOScale(endValue: endSize, duration: growTimeInS);
            renderer.material.DOColor(endValue: endCropColor, duration: growTimeInS).OnComplete(() =>
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

