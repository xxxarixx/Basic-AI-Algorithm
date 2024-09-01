using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Essencial
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }
        public Canvas worldSpaceCanv;
        private void Awake()
        {
            instance = this;
        }

    }

}
