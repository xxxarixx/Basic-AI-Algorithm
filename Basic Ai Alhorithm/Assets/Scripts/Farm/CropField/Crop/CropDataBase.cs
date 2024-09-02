using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CropField.Crops
{
    [DefaultExecutionOrder(-1)]
    public class CropDataBase : MonoBehaviour
    {
        public static CropDataBase instance { get; private set; }
        public CropBase tomato;
        public GameObject cropGoPatern;
        private void Awake()
        {
            instance = this;
        }
    }

}
