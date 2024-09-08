using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CropField.Crops;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using CropField;
public class PlantInstance : MonoBehaviour
{
    public PlantBase plantBase;
    protected bool isGrowing = false;
    public bool isFullyGrown => !isGrowing;
    
    public event Action OnEndedGrowing;
    public void SetupCrop()
    {
        if (!gameObject.TryGetComponent(out MeshRenderer renderer))
            return;
        var startSize = plantBase.startSize;
        var endSize = plantBase.endSize;
        var cropGrowEase = plantBase.cropGrowEase;
        var endCropColor = plantBase.endCropColor;
        var endPositionOffset = plantBase.endPositionOffset;
        var growTimeInS = plantBase.growTimeInS;

        renderer.material.color = plantBase.startCropColor;
        isGrowing = true;
        renderer.transform.localScale = startSize;
        var endSizeWithRandomFactor = endSize + new Vector3(Random.Range(endSize.x / 5, endSize.x / 3), Random.Range(endSize.x / 5, endSize.x / 3), Random.Range(endSize.x / 5, endSize.x / 3));
        float growTimeWithRandomFactor = Random.Range(growTimeInS / 5, growTimeInS / 3);
        renderer.transform.DOScale(endValue: endSizeWithRandomFactor, duration: growTimeWithRandomFactor).SetEase(cropGrowEase);
        renderer.material.DOColor(endValue: endCropColor, duration: growTimeWithRandomFactor).OnComplete(() =>
        {
            AnimateEndGrowing(endSizeWithRandomFactor, renderer);
            isGrowing = false;
            OnEndedGrowing?.Invoke();
        }).SetEase(cropGrowEase);
        renderer.transform.DOLocalMove(renderer.transform.position + endPositionOffset, duration: growTimeWithRandomFactor).SetEase(cropGrowEase);
    }
    private void AnimateEndGrowing(Vector3 endSizeWithRandomFactor, MeshRenderer renderer)
    {
        var endCropColor = plantBase.endCropColor;
        renderer.transform.localScale = new Vector3(endSizeWithRandomFactor.x + endSizeWithRandomFactor.x / 2, endSizeWithRandomFactor.y, endSizeWithRandomFactor.z - endSizeWithRandomFactor.z / 2);
        renderer.transform.DOScale(endSizeWithRandomFactor, duration: 0.3f).SetUpdate(isIndependentUpdate: true);
        renderer.material.color = Color.white;
        renderer.material.DOColor(endCropColor, duration: 0.3f).SetUpdate(isIndependentUpdate: true);
    }
    public void OnHarvested(CropHole cropHole)
    {
        if (isGrowing) return;
    }
    public static PlantInstance CreateCrop(Vector3 worldLocation, PlantBase seed)
    {
        GameObject createdCrop = Instantiate(PlantDataBase.instance.plantGoPatern, worldLocation, Quaternion.identity);
        createdCrop.name = seed.name;
        if (createdCrop.TryGetComponent(out PlantInstance cropInstance))
        {
            cropInstance.plantBase = seed;
            cropInstance.SetupCrop();
            return cropInstance;
        }
        else
        {
            return default;
        }
    }
    public void RemoveThisPlant()
    {
        Destroy(gameObject, t: 0.1f);
    }
}
