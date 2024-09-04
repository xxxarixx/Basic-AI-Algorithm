using Astar.Brain;
using General.Containers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace General.Essencial
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }
        public Canvas worldSpaceCanv;
        public event Action onEverythingSetupped;
        public bool everythingSettuped { get; private set; } = false;
        private void Awake()
        {
            instance = this;
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => AstarBrain.instance.setupped && ChestsManager.instance.settuped);
            onEverythingSetupped?.Invoke();
            Debug.Log("everything settuped");
        }

    }

}
