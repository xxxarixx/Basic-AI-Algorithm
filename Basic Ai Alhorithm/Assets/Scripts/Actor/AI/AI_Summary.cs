using AI.Farmer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI
{
    [DefaultExecutionOrder(-1)]
    public partial class AI_Summary : MonoBehaviour
    {
        public static AI_Summary instance { get; private set; }
        public AI_Farmer_Summary farmer_Summary;
        public static bool IsParentClass(Type parentType, Type childType)
        {
            return parentType.IsAssignableFrom(childType);
        }
        private void OnEnable()
        {
            farmer_Summary.SubscribeToFarmersStateChange();
        }
        private void OnDisable()
        {
            farmer_Summary.UnsubscribeToFarmersStateChange();
        }
        private void Awake()
        {
            instance = this;
        }

    }
}
