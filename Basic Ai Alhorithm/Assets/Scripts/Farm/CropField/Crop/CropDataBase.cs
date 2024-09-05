using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CropField.Crops
{
    [DefaultExecutionOrder(-1)]
    public class CropDataBase : MonoBehaviour
    {
        public static CropDataBase instance { get; private set; }
        public CropAndSeedBase tomato;
        public CropAndSeedBase cereal;
        public CropAndSeedBase pumpkin;
        public CropAndSeedBase potato;
        public CropAndSeedBase rice;
        private List<CropAndSeedBase> cropAndSeeds = new List<CropAndSeedBase>();
        public GameObject cropGoPatern;
        private void Awake()
        {
            instance = this;
            cropAndSeeds.AddRange(new List<CropAndSeedBase>()
            {
                tomato,
                cereal,
                pumpkin,
                potato,
                rice
            });
        }
        public CropAndSeedBase GetRandomCropAndSeed()
        {
            var randNum = Random.Range(0, cropAndSeeds.Count);
            return cropAndSeeds[randNum];
        }
    }

}
