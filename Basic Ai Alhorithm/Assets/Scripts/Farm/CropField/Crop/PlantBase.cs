using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
namespace CropField.Crops
{
    [CreateAssetMenu(menuName ="Custom/CreateCrop",fileName ="new Crop")]
    public class PlantBase : ScriptableObject
    {
        public float growTimeInS;
        public Color startCropColor;
        public Color endCropColor;
        public Vector3 startSize;
        public Vector3 endSize;
        public Vector3 endPositionOffset;
        public Ease cropGrowEase = Ease.InQuad;
        public int ID => GetInstanceID();
    }
}

