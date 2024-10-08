using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace General.UI.AI
{
    /// <summary>
    /// UI visualization of AI_Summary.cs
    /// </summary>
    public class UI_AISummary_Visualizer : MonoBehaviour
    {
        [SerializeField] private Slider seedJobVisualizer;
        [SerializeField] private Slider cropJobVisualizer;
        private void Awake()
        {
            Setup();
        }
        private void Setup()
        {
            seedJobVisualizer.minValue = 0;
            seedJobVisualizer.maxValue = AI_Summary.instance.farmer_Summary.ai_farmers.Count;
            cropJobVisualizer.minValue = 0;
            cropJobVisualizer.maxValue = AI_Summary.instance.farmer_Summary.ai_farmers.Count;
            FarmerSummary_OnAnyFarmerJobChange(global::AI.Farmer.AI_Farmer_BaseState.Job.SeedJob, newValue: 0);
            FarmerSummary_OnAnyFarmerJobChange(global::AI.Farmer.AI_Farmer_BaseState.Job.CropJob, newValue: 0);
        }
        private void OnEnable()
        {
            AI_Summary.instance.farmer_Summary.OnAnyFarmerJobChange += FarmerSummary_OnAnyFarmerJobChange;
        }
        private void OnDisable()
        {
            AI_Summary.instance.farmer_Summary.OnAnyFarmerJobChange -= FarmerSummary_OnAnyFarmerJobChange;
        }


        private void FarmerSummary_OnAnyFarmerJobChange(global::AI.Farmer.AI_Farmer_BaseState.Job jobTypes, int newValue)
        {
            switch (jobTypes)
            {
                case global::AI.Farmer.AI_Farmer_BaseState.Job.SeedJob:
                    seedJobVisualizer.DOKill();
                    seedJobVisualizer.DOValue(endValue: newValue, duration: 0.6f).SetEase(Ease.OutQuad);
                    break;
                case global::AI.Farmer.AI_Farmer_BaseState.Job.CropJob:
                    cropJobVisualizer.DOKill();
                    cropJobVisualizer.DOValue(endValue: newValue, duration: 0.6f).SetEase(Ease.OutQuad);
                    break;
                default:
                    break;
            }
        }

    }
}
