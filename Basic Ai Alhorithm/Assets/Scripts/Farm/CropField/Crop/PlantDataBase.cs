using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CropField.Crops
{
    /// <summary>
    /// Contains all plant types that exists in game
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class PlantDataBase : MonoBehaviour
    {
        public static PlantDataBase instance { get; private set; }
        private List<PlantBase> cropAndSeeds = new List<PlantBase>();
        public GameObject plantGoPatern;
        private void Awake()
        {
            instance = this;

            cropAndSeeds.AddRange(LoadAll<PlantBase>());
        }
        public PlantBase GetRandomPlantType()
        {
            var randNum = Random.Range(0, cropAndSeeds.Count);
            return cropAndSeeds[randNum];
        }
        private List<T> LoadAll<T>() where T : ScriptableObject
        {
            var allAssets = Resources.LoadAll<T>("");
            return new List<T>(allAssets);
        }
    }

}
