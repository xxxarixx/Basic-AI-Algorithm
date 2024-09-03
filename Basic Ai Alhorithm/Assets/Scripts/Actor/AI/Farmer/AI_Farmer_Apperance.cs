using AI.Farmer;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Farmer
{
    [RequireComponent(typeof(AI_Farmer_Dependencies))]
    public class AI_Farmer_Apperance : MonoBehaviour
    {
        private AI_Farmer_Dependencies dependencies;
        private Color defaultColor;
        public Material whiteMat;
        public Material greenMat;
        public Material brownMat;
        [SerializeField] private Material defaultMat;
        private void Awake()
        {
            dependencies = GetComponent<AI_Farmer_Dependencies>();
            
        }
        private void Start()
        {
            SetDefaultColor(dependencies.mRenderer.sharedMaterial.color);
        }
        public void SetDefaultColor(Color color)
        {
            defaultColor = color;
            ResetToDefaultColor();
        }
        public void SetTemporaryColor(Material mat)
        {
            UpdateColor(mat);
        }
        public void ResetToDefaultColor() => UpdateColor(defaultMat);
        private void UpdateColor(Material mat)
        {
            dependencies.mRenderer.material = mat;
        }
    }
}
